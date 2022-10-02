const string ProgramName = "Nova Skylance OS"; 
Color DefaultTextColor=new Color(197,137,255,255);
Color DefaultBackgroundColor=new Color(44,0,88,255);
const float MaxCannonSpeed=3f;

// Runtime Logic and Methods
public void Main(string argument,UpdateType updateSource){
	UpdateProgramInfo();
	
	Forward=GenMethods.LocalToGlobal(new Vector3D(0,0,-1),Projector);
	Forward.Normalize();
	Up=GenMethods.LocalToGlobal(new Vector3D(0,1,0),Projector);
	Up.Normalize();
	Left=GenMethods.LocalToGlobal(new Vector3D(-1,0,0),Projector);
	Left.Normalize();
	
	ScanCamera=GetScanningCamera();
	if(ScanningCameras.Count==0){
		Write("Warning! No available Scanning Cameras!");
	}
	else{
		if(ScanCamera!=null)
			Write(Math.Round(ScanCamera.AvailableScanRange/1000,2).ToString()+"kM Scanning");
	}
	
	
	if(CurrentTarget!=null)
		CurrentTarget.UpdateVectors(SecondsSinceLastUpdate);
	foreach(var target in KnownTargets){
		target.UpdateVectors(SecondsSinceLastUpdate);
	}
	
	TargetingLCD.ScriptForegroundColor=new Color(125,0,255,255);
	
	if(Status==CannonStatus.Idle&&argument.ToLower().Equals("scan")){
		StatusTimer=0;
		if(PerformBlindScan()&&CurrentTarget!=null){
			SwitchStatus(CannonStatus.Tracking);
		}
	}
	else if((int)Status<=(int)CannonStatus.Aiming&&argument.ToLower().Equals("change")){
		Write("Changing Targets");
		if(ChangeTarget()){
			if(Status==CannonStatus.Idle)
				SwitchStatus(CannonStatus.Tracking);
		}
		else if((int)Status>=(int)CannonStatus.Tracking)
			SwitchStatus(CannonStatus.Idle);
	}
	
	if(LastScanTime<double.MaxValue)
		LastScanTime=Math.Min(double.MaxValue,LastScanTime+SecondsSinceLastUpdate);
	
	double statusProgress=0;
	int statusSwitchRedundancyCounter=0;
	Runtime.UpdateFrequency=UpdateFrequency.Update10;
	do{
		switch(Status){
			case CannonStatus.Broken:
				ResetCannon();
				Runtime.UpdateFrequency=UpdateFrequency.Update100;
				break;
			case CannonStatus.Reloading:
				statusProgress=Reloading();
				if(CompletedStatus(statusProgress)){
					if(CurrentTarget!=null||ChangeTarget())
						SwitchStatus(CannonStatus.Tracking);
					else
						SwitchStatus(CannonStatus.Idle);
				}
				break;
			case CannonStatus.Idle:
				statusProgress=Idle();
				break;
			case CannonStatus.Tracking:
				statusProgress=Tracking();
				if(CompletedStatus(statusProgress))
					SwitchStatus(CannonStatus.Aiming);
				break;
			case CannonStatus.Aiming:
				statusProgress=Aiming();
				if(CompletedStatus(statusProgress))
					SwitchStatus(CannonStatus.Firing);
				break;
			case CannonStatus.Firing:
				statusProgress=Firing();
				if(CompletedStatus(statusProgress))
					SwitchStatus(CannonStatus.Reloading);
				break;
		}
	}
	while(CompletedStatus(statusProgress)&&(++statusSwitchRedundancyCounter)<10);
	
	Controller.Enabled=(int)Status<=(int)CannonStatus.Idle;
	
	if(Status!=CannonStatus.Firing){
		Merge.Enabled=true;
		Projector.Enabled=true;
		foreach(var generator in Generators){
			generator.Enabled=false;
		}
	}
	
	Write(Status.ToString());
	Write("SubStatus: "+SubStatus.ToString());
	Write("StatusTimer: "+Math.Round(StatusTimer,2).ToString());
	
	if(KnownTargets.Count>0){
		Write("Aware of "+KnownTargets.Count.ToString()+"Enemy Targets in Range");
		foreach(var target in KnownTargets){
			string timeStr;
			var age=DateTime.Now-target.Entity.TimeStamp;
			if(age.TotalSeconds<60){
				var msStr=DigitInt(age.Milliseconds,3);
				var sStr=DigitInt(age.Seconds,2);
				timeStr=sStr+'.'+msStr+"s";
			}
			else if(age.TotalMinutes<60){
				var sStr=DigitInt(age.Seconds,2);
				var mStr=DigitInt(age.Minutes,2);
				timeStr=mStr+'.'+sStr+"min";
			}
			else if(age.TotalHours<24){
				var mStr=DigitInt(age.Minutes,2);
				var hStr=DigitInt(age.Hours,2);
				timeStr=hStr+'.'+mStr+"hr";
			}
			else{
				var hStr=DigitInt(age.Hours,2);
				var dStr=Math.Round(age.TotalDays,0).ToString();
				timeStr=dStr+'.'+hStr+"D";
			}
			Write(target.GetSize().ToString()+" - "+Math.Round(target.GetDistance(Projector.GetPosition())-50,2)+"kM"+" - "+timeStr);
		}
	}
	
	switch(Status){
		case CannonStatus.Broken:
			SetStatusLights(Color.Black,Color.Blue,statusProgress);
			break;
		case CannonStatus.Reloading:
			SetStatusLights(Color.Yellow,Color.Blue,statusProgress);
			break;
		case CannonStatus.Idle:
			SetStatusLights(Color.Blue,Color.Cyan,statusProgress);
			break;
		case CannonStatus.Tracking:
			SetStatusLights(Color.Cyan,Color.Green,statusProgress);
			break;
		case CannonStatus.Aiming:
			SetStatusLights(Color.Green,Color.Yellow,statusProgress);
			break;
		case CannonStatus.Firing:
			if(StatusTimer<=1)
				SetStatusLights(Color.Red,Color.Red,1,10);
			else
				SetStatusLights(Color.Red,Color.Yellow,statusProgress,10-(StatusTimer-1));
			break;
	}
	
	Write(ScanningCameras.Count.ToString()+" Scanning Cameras");
}

bool ChangeTarget(){
	CurrentTarget=null;
	for(int i=0;i<KnownTargets.Count;i++){
		var target=KnownTargets[i];
		var distance=Math.Min((target.Entity.Position-Projector.GetPosition()).Length(),(target.Entity.Position-(Projector.GetPosition()+Forward*150)).Length());
		if(distance<=50000&&distance>=2500){
			if(target.Entity.BoundingBox.Size.Length()>=25){
				CurrentTarget=target;
				KnownTargets.RemoveAt(i);
				return true;
			}
		}
	}
	return false;
}

bool CompletedStatus(double statusProgress){
	return Math.Abs(statusProgress-1)<=Math.Pow(0.1,3);
}

void SwitchStatus(CannonStatus newStatus){
	bool useVents=false;
	switch(newStatus){
		case CannonStatus.Reloading:
		case CannonStatus.Firing:
			useVents=true;
			break;
	}
	
	foreach(IMyHeatVent vent in HeatVents){
		vent.Enabled=useVents;
	}
	
	StatusTimer=0;
	
	Status=newStatus;
	SubStatus=0;
}

double Reloading(){
	ResetCannon();
	Projector.Enabled=true;
	foreach(var welder in AssistWelders){
		welder.Enabled=true;
	}
	
	if(Projector.TotalBlocks!=18)
		Write("Warning! Projector Block Count doesn't match expected.");
	
	if(Projector.RemainingBlocks>0){
		SubStatus=0;
		return (.7*(Projector.TotalBlocks-Projector.RemainingBlocks))/Projector.TotalBlocks;
	}
	else{
		if(Shell==null){
			if(!MyCannonShell.TryGet(out Shell)){
				Write("Error! Projection does not match expected.");
				SubStatus=-1;
				return 0;
			}
		}
		if(Shell.Ready()){
			StatusTimer+=SecondsSinceLastUpdate;
			if(StatusTimer<2){
				SubStatus=2;
				return .9+StatusTimer/20;
			}
			else{
				SubStatus=3;
				return 1;
			}
		}
		else{
			SubStatus=1;
			StatusTimer=0;
			return Shell.Completion()*.2+.7;
		}
	}
}

double Idle(){
	if(Controller.IsUnderControl){
		AzimuthRotor.RotorLock=false;
		ElevationRotor.RotorLock=false;
		SubStatus=2;
		if(StatusTimer<=4){
			TargetingLCD.ContentType=ContentType.TEXT_AND_IMAGE;
			TargetingLCD.Alignment=TextAlignment.CENTER;
			TargetingLCD.FontColor=DefaultTextColor;
			TargetingLCD.FontSize=1.6f;
		}
		else
			TargetingLCD.ContentType=ContentType.SCRIPT;
		StatusTimer=Math.Min(5,StatusTimer+SecondsSinceLastUpdate);
		return 1;
	}
	else if(SubStatus==2){
		StatusTimer+=SecondsSinceLastUpdate;
		if(StatusTimer>=35){
			SubStatus=0;
			StatusTimer-=35;
		}
		else
			return (30-StatusTimer)/30.0;
	}
	
	if(SubStatus!=2) {
		if(SubStatus<=0){
			StatusTimer=Math.Min(32,StatusTimer+SecondsSinceLastUpdate);
			if(StatusTimer>=31)
				SubStatus=1;
		}
		else{
			StatusTimer=Math.Max(0,StatusTimer-SecondsSinceLastUpdate);
			if(StatusTimer<=1)
				SubStatus=0;
		}
		if(ResetCannon())
			Runtime.UpdateFrequency=UpdateFrequency.Update100;
	}
	
	return Math.Max(Math.Min((StatusTimer-1)/30.0,1),0);
}

bool PerformBlindScan(){
	if(ScanCamera==null){
		TargetingLCD.WriteText("Failed to Ping Target - "+Math.Round(StatusTimer,1).ToString(),false);
		TargetingLCD.WriteText("\nNo Scan Camera",true);
		return false;
	}
	const double MinScanDistance=5000;
	
	
	TargetingLCD.ContentType=ContentType.TEXT_AND_IMAGE;
	TargetingLCD.Alignment=TextAlignment.CENTER;
	TargetingLCD.FontColor=DefaultTextColor;
	TargetingLCD.FontSize=1.6f;
	
	for(int i=0;i<6;i++){
		if((ScanCamera?.AvailableScanRange??0)<MinScanDistance){
			TargetingLCD.WriteText("Failed to Ping Target - "+Math.Round(StatusTimer,1).ToString(),false);
			TargetingLCD.WriteText("\nInsufficient Range - "+Math.Round(ScanCamera?.AvailableScanRange??0,0).ToString(),true);
			return false;
		}
		double scanDistance=Math.Max(ScanCamera.AvailableScanRange,MinScanDistance);
		double raycastTargetDistance=(scanDistance+MinScanDistance)/2;
		var raycastTarget=TargetingCamera.GetPosition()+(raycastTargetDistance+(ScanCamera.GetPosition()-TargetingCamera.GetPosition()).Length())*Forward;
		var raycastDirection=raycastTarget-ScanCamera.GetPosition();
		raycastDirection.Normalize();
		var raycast=raycastDirection*scanDistance+ScanCamera.GetPosition();
		if(!ScanCamera.CanScan(raycast)){
			TargetingLCD.WriteText("Failed to Ping Target - "+Math.Round(StatusTimer,1).ToString(),false);
			TargetingLCD.WriteText("\nInvalid Angle or Range",true);
			return false;
		}
		var lastScanCameraPosition=ScanCamera.GetPosition();
		var detected=ScanCamera.Raycast(raycast);
		ScanCamera=GetScanningCamera();
		LastScanTime=0;
		TargetingLCD.CustomData=(new MyWaypointInfo("Latest Raycast",raycast)).ToString();
		if(!detected.IsEmpty()){
			if(detected.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies){
				var target=new MyTarget(detected);
				var distance=Math.Min((target.Entity.Position-Projector.GetPosition()).Length(),(target.Entity.Position-lastScanCameraPosition).Length());
				Write("Target Detected - "+Math.Round(distance,0).ToString()+"M");
				if(distance<=50000&&distance>=2500){
					if(target.Entity.BoundingBox.Size.Length()>=25)
						CurrentTarget=target;
				}
				if(CurrentTarget==null)
					KnownTargets.Add(target);
				return true;
			}
		}
	}
	TargetingLCD.WriteText("Failed to Ping Target - "+Math.Round(StatusTimer,1).ToString(),false);
	TargetingLCD.WriteText("\nDid not ping valid target",true);
	return false;
}

bool PerformScan(Vector3D Target){
	var distance=(Target-Projector.GetPosition()).Length()+500;
	
	var timeBetweenScans=Math.Max(1,(distance+500)/28000);
	
	if(LastScanTime<timeBetweenScans)
		return false;
	
	if(!ScanCamera.CanScan(Target))
		return false;
	var lastScanCameraPosition=ScanCamera.GetPosition();
	var detected=ScanCamera.Raycast(Target);
	ScanCamera=GetScanningCamera();
	LastScanTime=0;
	
	if(!detected.IsEmpty()){
		if(detected.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies){
			if(CurrentTarget.Entity.Same(detected)){
				Write("Locked onto Target");
				CurrentTarget.Update(detected);
				return true;
			}
			else{
				foreach(var target in KnownTargets){
					if(target.Entity.Same(detected)){
						Write("Udpating known Target");
						target.Update(detected);
						return false;
					}
				}
				var newTarget=new MyTarget(detected);
				KnownTargets.Add(newTarget);
				distance=Math.Min((newTarget.Entity.Position-Projector.GetPosition()).Length(),(newTarget.Entity.Position-lastScanCameraPosition).Length());
				Write("Alternate Target Detected - "+Math.Round(distance,0).ToString()+"M");
				return false;
			}
		}
	}
	return false;
}

double Tracking(){
	var distance=CurrentTarget.GetDistance(Projector.GetPosition())-50;
	
	TargetingLCD.ContentType=ContentType.SCRIPT;
	TargetingLCD.ScriptForegroundColor=new Color(0,255,255,255);
	
	Write("Tracking "+CurrentTarget.GetSize().ToString()+" Enemy Ship at "+Math.Round(distance,2).ToString()+"kM");
	
	var angle=PointCannonTowards(CurrentTarget.ExpectedPosition);
	var timeBetweenScans=Math.Max(1,(distance+500)/28000);
	
	var angleProgress=(180-angle)/180;
	var timerProgress=Math.Min(1,LastScanTime/timeBetweenScans);
	var cameraProgress=Math.Min(1,ScanCamera.AvailableScanRange/(distance+500));
	var totalProgress=.3*angleProgress+.3*timerProgress+.3*cameraProgress;
	
	if(angle<5){
		SubStatus=1;
		if(ScanCamera?.Closed??true){
			Write("Warning! No ScanCamera detected");
			return totalProgress;
		}
		
		if(ScanCamera.AvailableScanRange<distance+500)
			return totalProgress;
		
		SubStatus=2;
		
		if(LastScanTime<timeBetweenScans)
			return totalProgress;
		
		var direction=CurrentTarget.ExpectedPosition-ScanCamera.GetPosition();
		direction.Normalize();
		var raycast=ScanCamera.GetPosition()+(distance+500)*direction;
		
		if(!ScanCamera.CanScan(raycast))
			return totalProgress;
		var lastScanCameraPosition=ScanCamera.GetPosition();
		var detected=ScanCamera.Raycast(raycast);
		ScanCamera=GetScanningCamera();
		LastScanTime=0;
		
		if(!detected.IsEmpty()){
			if(detected.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies){
				if(CurrentTarget.Entity.Same(detected)){
					Write("Locked onto Target");
					CurrentTarget.Update(detected);
					return 1;
				}
				else{
					foreach(var target in KnownTargets){
						if(target.Entity.Same(detected)){
							Write("Udpating known Target");
							target.Update(detected);
							return totalProgress;
						}
					}
					var newTarget=new MyTarget(detected);
					KnownTargets.Add(newTarget);
					distance=Math.Min((newTarget.Entity.Position-Projector.GetPosition()).Length(),(newTarget.Entity.Position-lastScanCameraPosition).Length());
					Write("Alternate Target Detected - "+Math.Round(distance,0).ToString()+"M");
				}
			}
		}
		
	}
	else
		SubStatus=0;
	return totalProgress;
}

double Aiming(){
	var baseDistance=CurrentTarget.GetDistance(Projector.GetPosition())-50;
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
	Write("Aiming at "+CurrentTarget.GetSize().ToString()+" Enemy Ship at "+Math.Round(baseDistance,2).ToString()+"kM");
	
	TargetingLCD.ScriptForegroundColor=new Color(255,255,0,255);
	
	var maxSpeed=CurrentTarget.Large?300:500;
	
	var projectileLocation=Projector.GetPosition()+Forward*150;
	
	double timeToNextWindow;
	
	//Let's go with a 20-second window for aiming
	if(NextLaunch==null||((DateTime)NextLaunch?.LaunchTime)<=DateTime.Now-new TimeSpan(0,0,0,0,200)){
		SubStatus=0;
		var nextWindow=RoundDateTime(DateTime.Now);
		timeToNextWindow=(nextWindow-DateTime.Now).TotalSeconds;
		
		var predictedTarget=PredictPosition(CurrentTarget,timeToNextWindow+1);
		
		var targetDistance=(predictedTarget-projectileLocation).Length();
		var timeToHit=targetDistance/300+1;
		var lastTimeToHit=double.MaxValue;
		
		do{
			predictedTarget=PredictPosition(CurrentTarget,timeToHit);
			lastTimeToHit=timeToHit;
			targetDistance=(predictedTarget-projectileLocation).Length();
			timeToHit=targetDistance/300+1;
		}
		while(Math.Abs(timeToHit-lastTimeToHit)>0.01);
		
		NextLaunch=new LaunchInstructions(predictedTarget,nextWindow);
	}
	
	var angle=PointCannonTowards(NextLaunch.PredictedTarget);
	var acceptableAngle=Math.Atan(5/baseDistance)*180/Math.PI;
	timeToNextWindow=(NextLaunch.LaunchTime-DateTime.Now).TotalSeconds;
	
	
	var angleProgress=Math.Min(1,acceptableAngle/angle);
	var timerProgress=Math.Min(1,(20-timeToNextWindow)/20);
	var lockProgress=Math.Min(1,StatusTimer);
	
	if(AzimuthRotor.RotorLock&&ElevationRotor.RotorLock)
		StatusTimer+=SecondsSinceLastUpdate;
	else
		StatusTimer=0;
	
	if(angle<=acceptableAngle){
		SubStatus=1;
		if(Math.Abs(timeToNextWindow)<=.2){
			SubStatus=2;
			if(StatusTimer>=1){
				SubStatus=3;
				if(Shell.Ready())
					return 1;
			}
		}
	}
	
	return .4*angleProgress+.4*timerProgress+.1*lockProgress+(Shell.Ready()?.1:0);
}

double Firing(){
	TargetingLCD.ScriptForegroundColor=new Color(255,0,0,255);
	if(SubStatus==0){
		Shell.Mass.Enabled=true;
		Shell.Battery.Enabled=true;
		Shell.Generator.Enabled=true;
		Shell.Gyroscope.Enabled=true;
		Shell.Gyroscope.GyroOverride=false;
		Shell.Light1.Enabled=true;
		Shell.Light2.Enabled=true;
		
		Projector.Enabled=false;
		foreach(var welder in AssistWelders){
			welder.Enabled=false;
		}
		foreach(var generator in Generators){
			generator.Enabled=true;
		}
		
		Merge.Enabled=false;
		Shell.Merge.Enabled=false;
		
		SubStatus=1;
	}
	else
		StatusTimer+=SecondsSinceLastUpdate;
	
	var position=Shell.Gyroscope.GetPosition();
	
	var shellDistance=Math.Min((position-Projector.GetPosition()).Length(),(position-Projector.GetPosition()+Forward*150).Length());
	var targetDistance=(position-NextLaunch.PredictedTarget).Length();
	var timeToContact=targetDistance/300;
	
	
	if(StatusTimer>=1&&shellDistance>500){
		SubStatus=2;
		
		PerformScan(CurrentTarget.ExpectedPosition);
		PointCannonTowards(CurrentTarget.ExpectedPosition);
		
		NextLaunch.PredictedTarget=PredictPosition(CurrentTarget,timeToContact);
		targetDistance=(position-NextLaunch.PredictedTarget).Length();
		timeToContact=targetDistance/300;
		
		if(!Shell.Warhead.IsCountingDown)
			Shell.Warhead.StartCountdown();
		if(!Shell.Warhead.IsArmed&&(shellDistance>1000&&targetDistance<2500)||(shellDistance>300&&targetDistance<1000))
			Shell.Warhead.IsArmed=true;
		Shell.Aim(PredictPosition(CurrentTarget,timeToContact));
		
		if(shellDistance>300&&CurrentTarget.GetDistance(position)<10){
			Shell.Warhead.IsArmed=true;
			Shell.Warhead.Detonate();
			Shell=null;
			CurrentTarget=null;
			return 1;
		}
		
		Merge.Enabled=true;
		Projector.Enabled=true;
		foreach(var welder in AssistWelders){
			welder.Enabled=true;
		}
		foreach(var generator in Generators){
			generator.Enabled=false;
		}
	}
	
	var launchProgress=Math.Min(1,StatusTimer);
	var escapeProgress=Math.Min(1,shellDistance/500);
	var contactProgress=shellDistance/(shellDistance+targetDistance);
	
	if(Shell.Completion()<.5){
		Shell=null;
		CurrentTarget=null;
		return 1;
	}
	
	return Math.Min(1,.1*launchProgress+.1*escapeProgress+.8*contactProgress);
}

DateTime RoundDateTime(DateTime input){
	double milliseconds=input.Millisecond;
	var seconds=input.Second%20+milliseconds/1000;
	
	return input.AddSeconds(20-seconds);
	
}

Vector3D PredictPosition(MyTarget Target,double Time){
	var currentPosition=Target.ExpectedPosition;
	var currentVelocity=Target.ExpectedVelocity;
	var currentAcceleration=Target.ExpectedAcceleration;
	var time=Time;
	
	var maxSpeed=Target.Large?300:500;
	
	while(time>0.01){
		var slice=time;
		var relativeVelocity=GenMethods.LocalToGlobal(currentVelocity,Target.Entity.Orientation,currentPosition);
		var relativeAcceleration=GenMethods.LocalToGlobal(currentAcceleration,Target.Entity.Orientation,currentPosition);
		if((relativeAcceleration.X>0)^(relativeVelocity.X>0))
			slice=Math.Min(slice,Math.Abs(relativeVelocity.X/relativeAcceleration.X));
		else
			slice=Math.Min(slice,(maxSpeed-Math.Abs(relativeVelocity.X))/Math.Abs(relativeAcceleration.X)/2);
		if((relativeAcceleration.Y>0)^(relativeVelocity.Y>0))
			slice=Math.Min(slice,Math.Abs(relativeVelocity.Y/relativeAcceleration.Y));
		else
			slice=Math.Min(slice,(maxSpeed-Math.Abs(relativeVelocity.Y))/Math.Abs(relativeAcceleration.Y)/2);
		if((relativeAcceleration.Z>0)^(relativeVelocity.Z>0))
			slice=Math.Min(slice,Math.Abs(relativeVelocity.Z/relativeAcceleration.Z));
		else
			slice=Math.Min(slice,(maxSpeed-Math.Abs(relativeVelocity.Z))/Math.Abs(relativeAcceleration.Z)/2);
		
		slice=Math.Min(time,Math.Max(slice,1));
		
		var newVelocity=currentVelocity;
		var newAcceleration=currentAcceleration;
		if((relativeVelocity.X+slice*relativeAcceleration.X>0)^(relativeVelocity.X>0)){
			relativeVelocity.X=0;
			relativeAcceleration.X=0;
		}
		if((relativeVelocity.Y+slice*relativeAcceleration.Y>0)^(relativeVelocity.Y>0)){
			relativeVelocity.Y=0;
			relativeAcceleration.Y=0;
		}
		if((relativeVelocity.Z+slice*relativeAcceleration.Z>0)^(relativeVelocity.Z>0)){
			relativeVelocity.Z=0;
			relativeAcceleration.Z=0;
		}
		relativeVelocity+=slice*relativeAcceleration;
		newVelocity=GenMethods.GlobalToLocal(relativeVelocity,Target.Entity.Orientation,currentPosition);
		newAcceleration=GenMethods.GlobalToLocal(relativeAcceleration,Target.Entity.Orientation,currentPosition);
		
		var velDir=newVelocity;
		velDir.Normalize();
		newVelocity=velDir*Math.Min(maxSpeed,newVelocity.Length());
		
		currentPosition=slice*(currentVelocity+newVelocity)/2+currentPosition;
		currentVelocity=newVelocity;
		currentAcceleration=newAcceleration;
		time-=slice;
	}
	
	return currentPosition;
}


void LockCannon(){
	ElevationRotor.TargetVelocityRPM=0;
	AzimuthRotor.TargetVelocityRPM=0;
	ElevationRotor.RotorLock=true;
	AzimuthRotor.RotorLock=true;
}

bool ResetCannon(){
	var azimuthAngle=AzimuthRotor.Angle*180/Math.PI;
	if(azimuthAngle>180)
		azimuthAngle-=360;
	
	if(Math.Abs(azimuthAngle)<=0.1){
		AzimuthRotor.RotorLock=true;
		AzimuthRotor.TargetVelocityRPM=0;
	}
	else{
		AzimuthRotor.RotorLock=false;
		AzimuthRotor.TargetVelocityRPM=(float)Math.Max(Math.Min(azimuthAngle/-5,MaxCannonSpeed/2),MaxCannonSpeed/-2);
	}
	
	var elevationAngle=ElevationRotor.Angle*180/Math.PI;
	if(elevationAngle>180)
		elevationAngle-=360;
	if(Math.Abs(elevationAngle)<=0.1){
		ElevationRotor.RotorLock=true;
		ElevationRotor.TargetVelocityRPM=0;
	}
	else{
		ElevationRotor.RotorLock=false;
		ElevationRotor.TargetVelocityRPM=(float)Math.Max(Math.Min(elevationAngle/-5,MaxCannonSpeed/2),MaxCannonSpeed/-2);
	}
	
	return AzimuthRotor.RotorLock&&ElevationRotor.RotorLock;
}

double PointCannonTowards(Vector3D target){
	var targetDir=target-Projector.GetPosition();
	targetDir.Normalize();
	var currentAngle=GenMethods.GetAngle(targetDir,Forward);
	
	const double Acceptable=0.1;
	
	// If target is right, expect angle from right smaller than angle from left
	var azimuthAngle=(GenMethods.GetAngle(Left,targetDir)-GenMethods.GetAngle(Right,targetDir))/2;
	// If target is above, expect angle from up smaller than angle from below
	var elevationAngle=(GenMethods.GetAngle(Down,targetDir)-GenMethods.GetAngle(Up,targetDir))/2;
	
	// Positive Azimuth is turning to the right - already checked rotor
	var azimuthSpeed=Math.Max(Math.Min(Math.Abs(azimuthAngle)/10,2*MaxCannonSpeed),0);
	if(azimuthAngle>0+Acceptable){
		AzimuthRotor.RotorLock=false;
		AzimuthRotor.TargetVelocityRPM=(float)azimuthSpeed;
	}
	else if(azimuthAngle<0-Acceptable){
		AzimuthRotor.RotorLock=false;
		AzimuthRotor.TargetVelocityRPM=(float)(-1*azimuthSpeed);
	}
	else{
		AzimuthRotor.RotorLock=true;
		AzimuthRotor.TargetVelocityRPM=0;
	}
	
	// Positive Elevation is turning up - already checked rotorvar elevationSpeed=Math.Max(Math.Min(Math.Abs(Elevation)/5,6),0.2);
	var elevationSpeed=Math.Max(Math.Min(Math.Abs(elevationAngle)/10,MaxCannonSpeed),0.2);
	if(elevationAngle>0+Acceptable){
		ElevationRotor.RotorLock=false;
		ElevationRotor.TargetVelocityRPM=(float)elevationSpeed;
	}
	else if(elevationAngle<0-Acceptable){
		ElevationRotor.RotorLock=false;
		ElevationRotor.TargetVelocityRPM=(float)(-1*elevationSpeed);
	}
	else{
		ElevationRotor.RotorLock=true;
		ElevationRotor.TargetVelocityRPM=0;
	}
	
	return currentAngle;
}

Color PercentColor(Color low, Color high, double percent){
	return new Color(
	(int)(((high.R-low.R)*percent)+low.R),
	(int)(((high.G-low.G)*percent)+low.G),
	(int)(((high.B-low.B)*percent)+low.B)
	);
}

void SetStatusLights(Color min, Color max, double current, double intensity=5){
	Color highest,lowest;
	
	if(current<.5){
		lowest=min;
		highest=PercentColor(min,max,current*2);
	}
	else{
		lowest=PercentColor(min,max,current*2-1);
		highest=max;
	}
	
	for(int j=0;j<StatusLights.Count;j++){
		var set=StatusLights[j];
		for(int i=0; i<set.Count;i++){
			double percent=(set.Count-(i+1.0))/set.Count;
			set[i].Color=PercentColor(lowest,highest,percent);
			if(intensity<=5)
				set[i].Radius=(float)intensity;
			else
				set[i].Radius=(float)(intensity*2-5);
			set[i].Intensity=(float)intensity;
		}
	}
}

string DigitInt(int num,int digits){
	string output=num.ToString();
	while(output.Length<digits)
		output='0'+output;
	return output;
}

// Initialization and Object Definitions
public Program(){
	Echo("Beginning initialization.");
	Prog.P=this;
	Me.CustomName=(ProgramName+" Programmable block").Trim();
	for(int i=0;i<Me.SurfaceCount;i++){
		Me.GetSurface(i).FontColor=DefaultTextColor;
		Me.GetSurface(i).BackgroundColor=DefaultBackgroundColor;
		Me.GetSurface(i).Alignment=TextAlignment.CENTER;
		Me.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
		Me.GetSurface(i).WriteText("",false);
	}
	Me.GetSurface(1).FontSize=2.2f;
	Me.GetSurface(1).TextPadding=40.0f;
	
	// Initialize runtime objects
	Echo("Initializing objects...");
	
	AzimuthRotor=CollectionMethods<IMyMotorStator>.ByFullName("Skylance-Cannon Azimuth Rotor");
	ElevationRotor=CollectionMethods<IMyMotorStator>.ByFullName("Skylance-Cannon Elevation Rotor");
	SupportRotor=CollectionMethods<IMyMotorStator>.ByFullName("Skylance-Cannon Support Rotor");
	
	ScanningCameras=CollectionMethods<IMyCameraBlock>.AllByName("Skyguard-Cannon Scanning Camera");
	foreach(IMyCameraBlock camera in ScanningCameras){
		camera.EnableRaycast=true;
	}
	
	StatusLights=new List<List<IMyInteriorLight>>();
	int groupNum=0;
	var lights=CollectionMethods<IMyInteriorLight>.AllByName("Skylance-Cannon Status Light Panel A-");
	while(lights.Count>0){
		lights=CollectionMethods<IMyInteriorLight>.SortByName(lights);
		StatusLights.Add(lights);
		char groupChar=(char)('A'+(++groupNum));
		lights=CollectionMethods<IMyInteriorLight>.AllByName("Skylance-Cannon Status Light Panel "+groupChar+"-");
	}
	lights=CollectionMethods<IMyInteriorLight>.AllByName("Skylance-Base Status Light Panel A-");
	groupNum=0;
	while(lights.Count>0){
		lights=CollectionMethods<IMyInteriorLight>.SortByName(lights);
		StatusLights.Add(lights);
		char groupChar=(char)('A'+(++groupNum));
		lights=CollectionMethods<IMyInteriorLight>.AllByName("Skylance-Base Status Light Panel "+groupChar+"-");
	}
	
	AssistWelders=CollectionMethods<IMyShipWelder>.AllByName("Skylance-Cannon Assist Welder");
	Projector=CollectionMethods<IMyProjector>.ByFullName("Skylance-Cannon Projector");
	Merge=CollectionMethods<IMyShipMergeBlock>.ByFullName("Skylance-Cannon Merge Block");
	Generators=CollectionMethods<IMyGravityGenerator>.AllByName("Skylance-Cannon Gravity Generator");
	TargetingCamera=CollectionMethods<IMyCameraBlock>.ByFullName("Skylance-Cannon Targeting Camera");
	TargetingLCD=CollectionMethods<IMyTextPanel>.ByFullName("Skylance-Cannon Targeting LCD");
	
	Controller=CollectionMethods<IMyTurretControlBlock>.ByFullName("Skylance Controller");
	
	HeatVents=CollectionMethods<IMyHeatVent>.AllByName("Skylance-Cannon Heat Vent");
	
	MyCannonShell shell=null;
	if(MyCannonShell.TryGet(out shell))
		Shell=shell;
	
	Status=CannonStatus.Idle;
	StatusTimer=0;
	KnownTargets=new List<MyTarget>();
	NextLaunch=null;
	
	// Load runtime variables from CustomData
	Echo("Setting variables...");
	
	
	// Load data from Storage
	Echo("Loading data...");
	string storageMode="";
	string[] storageArgs=this.Storage.Trim().Split('\n');
	foreach(string line in storageArgs){
		switch(line){
			case "Status":
			case "SubStatus":
			case "StatusTimer":
			case "CurrentTarget":
			case "KnownTargets":
				storageMode=line;
				break;
			default:
				switch(storageMode){
					case "Status":
						int status=0;
						if(int.TryParse(line,out status)){
							Status=(CannonStatus)status;
							if(Status==CannonStatus.Firing)
								SwitchStatus(CannonStatus.Reloading);
						}
						break;
					case "SubStatus":
						short substatus=0;
						if(short.TryParse(line,out substatus))
							SubStatus=substatus;
						break;
					case "StatusTimer":
						double statusTimer=0;
						if(double.TryParse(line,out statusTimer))
							StatusTimer=statusTimer;
						break;
					case "CurrentTarget":
						MyTarget currenttarget;
						if(MyTarget.TryParse(line,out currenttarget))
							CurrentTarget=currenttarget;
						break;
					case "KnownTargets":
						MyTarget target;
						if(MyTarget.TryParse(line,out target))
							KnownTargets.Add(target);
						break;
				}
				break;
		}
	}
	
	Runtime.UpdateFrequency=UpdateFrequency.Once;
	Echo("Completed initialization!");
}

CannonStatus Status;
double StatusTimer;
short SubStatus=0;
double LastScanTime=double.MaxValue;

MyTarget CurrentTarget;
List<MyTarget> KnownTargets;
LaunchInstructions NextLaunch;

IMyMotorStator AzimuthRotor;
IMyMotorStator ElevationRotor;
IMyMotorStator SupportRotor;

List<IMyCameraBlock> ScanningCameras;
IMyCameraBlock ScanCamera;
IMyCameraBlock GetScanningCamera(){
	if((ScanningCameras?.Count??0)==0)
		return null;
	double maxScanRange=0;
	foreach(var camera in ScanningCameras){
		camera.Enabled=true;
		if(camera.IsWorking)
			maxScanRange=Math.Max(maxScanRange,camera.AvailableScanRange);
	}
	for(int i=0;i<ScanningCameras.Count;i++){
		if(ScanningCameras[i].IsWorking&&ScanningCameras[i].AvailableScanRange+1>=maxScanRange)
			return ScanningCameras[i];
	}
	return null;
}

IMyCameraBlock TargetingCamera;
IMyTextPanel TargetingLCD;

List<List<IMyInteriorLight>> StatusLights;
List<IMyHeatVent> HeatVents;

List<IMyShipWelder> AssistWelders;
IMyProjector Projector;
List<IMyGravityGenerator> Generators;

IMyShipMergeBlock Merge;
IMyTurretControlBlock Controller;

MyCannonShell Shell;

Vector3D Forward;
Vector3D Backward{
	get{
		return -1*Forward;
	}
}
Vector3D Up;
Vector3D Down{
	get{
		return -1*Up;
	}
}
Vector3D Left;
Vector3D Right{
	get{
		return -1*Left;
	}
}

// Saving and Data Storage Classes
public void Save(){
    // Reset Storage
	this.Storage="";
	
	// Save Data to Storage
	this.Storage+="\n"+"Status";
	this.Storage+="\n"+((int)Status).ToString();
	this.Storage+="\n"+"SubStatus";
	this.Storage+="\n"+SubStatus.ToString();
	this.Storage+="\n"+"StatusTimer";
	this.Storage+="\n"+Math.Round(StatusTimer,3).ToString();
	if(CurrentTarget!=null){
		this.Storage+="\n"+"CurrentTarget";
		this.Storage+="\n"+CurrentTarget.ToString();
	}
	if(KnownTargets.Count>0){
		this.Storage+="\n"+"KnownTargets";
		foreach(var target in KnownTargets){
			this.Storage+="\n"+target.ToString();
		}
	}
	
	// Update runtime variables from CustomData
	
	
	// Reset CustomData
	Me.CustomData="";
	
	// Save Runtime Data to CustomData
	
	
}

enum CannonStatus{
	Broken=-1,
	Reloading=0,
	Idle=1,
	Tracking=2,
	Aiming=3,
	Firing=4
}

class MyCannonShell{
	public IMyArtificialMassBlock Mass;
	public IMyBatteryBlock Battery;
	public IMyGravityGenerator Generator;
	public IMyGyro Gyroscope;
	public IMyInteriorLight Light1;
	public IMyInteriorLight Light2;
	public IMyShipMergeBlock Merge;
	public IMyWarhead Warhead;
	
	private Vector3D Forward{
		get{
			var forward=GenMethods.LocalToGlobal(new Vector3D(0,1,0),Gyroscope);
			forward.Normalize();
			return forward;
		}
	}
	private Vector3D Up{
		get{
			var up=GenMethods.LocalToGlobal(new Vector3D(0,0,1),Gyroscope);
			up.Normalize();
			return up;
		}
	}
	private Vector3D Left{
		get{
			var left=GenMethods.LocalToGlobal(new Vector3D(-1,0,0),Gyroscope);
			left.Normalize();
			return left;
		}
	}
	private MatrixD LastOrientation;
	private Vector3D AngularVelocity;
	
	private MyCannonShell(IMyArtificialMassBlock mass, IMyBatteryBlock battery,IMyGravityGenerator generator,IMyGyro gyroscope,IMyInteriorLight l1,IMyInteriorLight l2,IMyShipMergeBlock merge,IMyWarhead warhead){
		Mass=mass;
		Battery=battery;
		Generator=generator;
		Gyroscope=gyroscope;
		Light1=l1;
		Light2=l2;
		Merge=merge;
		Warhead=warhead;
		LastOrientation=MatrixD.CreateFromDir(Forward,Up);
		AngularVelocity=new Vector3D(0,0,0);
	}
	
	public static bool TryGet(out MyCannonShell output){
		output=null;
		var mass=CollectionMethods<IMyArtificialMassBlock>.ByFullName("Skylance-Shell Artificial Mass");
		var battery=CollectionMethods<IMyBatteryBlock>.ByFullName("Skylance-Shell Battery");
		var generator=CollectionMethods<IMyGravityGenerator>.ByFullName("Skylance-Shell Gravity Generator");
		var gyroscope=CollectionMethods<IMyGyro>.ByFullName("Skylance-Shell Gyroscope");
		var l1=CollectionMethods<IMyInteriorLight>.ByFullName("Skylance-Shell Light 1");
		var l2=CollectionMethods<IMyInteriorLight>.ByFullName("Skylance-Shell Light 2");
		var merge=CollectionMethods<IMyShipMergeBlock>.ByFullName("Skylance-Shell Merge Block");
		var warhead=CollectionMethods<IMyWarhead>.ByFullName("Skylance-Shell Warhead");
		if(gyroscope==null||battery==null||mass==null||generator==null||l1==null||l2==null||warhead==null||merge==null)
			return false;
		battery.Enabled=false;
		output=new MyCannonShell(mass,battery,generator,gyroscope,l1,l2,merge,warhead);
		return output!=null;
	}
	
	public void Update(double SecondsSinceLastUpdate){
		var forward=Forward;
		var backward=-1*forward; 
		var up=Up;
		var down=-1*up;
		var left=Left;
		var right=-1*left;
		
		// These are in deg/update, but *2
		var pitch=GenMethods.GetAngle(LastOrientation.Forward,down)-GenMethods.GetAngle(LastOrientation.Forward,up);
		var yaw=GenMethods.GetAngle(LastOrientation.Forward,left)-GenMethods.GetAngle(LastOrientation.Forward,right);
		var roll=GenMethods.GetAngle(LastOrientation.Up,left)-GenMethods.GetAngle(LastOrientation.Up,right);
		
		// This is in deg/min
		AngularVelocity=(new Vector3D(pitch,yaw,roll))/(SecondsSinceLastUpdate*2)/60;
		LastOrientation=MatrixD.CreateFromDir(forward,up);
	}
	
	public double Aim(Vector3D target){
		var forward=Forward;
		var backward=-1*forward; 
		var up=Up;
		var down=-1*up;
		var left=Left;
		var right=-1*left;
		
		
		var targetDir=target-Gyroscope.GetPosition();
		targetDir.Normalize();
		
		var currentAngle=GenMethods.GetAngle(targetDir,forward);
		
		const double Acceptable=1;
		
		// If target is right, expect angle from right smaller than angle from left
		var azimuthAngle=(GenMethods.GetAngle(left,targetDir)-GenMethods.GetAngle(right,targetDir))/2;
		
		if(currentAngle>120&&Math.Abs(azimuthAngle)<90)
			azimuthAngle=180-azimuthAngle;
		
		// If target is above, expect angle from up smaller than angle from below
		var elevationAngle=(GenMethods.GetAngle(down,targetDir)-GenMethods.GetAngle(up,targetDir))/2;
		
		var relativeAngularVelocity=GenMethods.GlobalToLocal(AngularVelocity,Gyroscope);
		
		var pitch=-.99*relativeAngularVelocity.X;
		var yaw=-.99*relativeAngularVelocity.Z;
		var roll=-.99*relativeAngularVelocity.Y;
		
		const double MaxSpeed=.3;
		
		// Negative to right, Positive to Left
		var targetAzimuthVelocity=Math.Min(Math.Max(.1*azimuthAngle/30,-1*MaxSpeed),MaxSpeed);
		if(Math.Abs(azimuthAngle)>Acceptable)
			yaw=targetAzimuthVelocity-relativeAngularVelocity.Z;
		
		// Positive to up, Negative to down
		var targetElevationVelocity=Math.Min(Math.Max(-.1*elevationAngle/30,-1*MaxSpeed),MaxSpeed);
		if(Math.Abs(elevationAngle)>Acceptable)
			pitch=targetElevationVelocity-relativeAngularVelocity.X;
		
		// Prog.P.Echo("AzimuthVelocity:"+Math.Round(relativeAngularVelocity.Z,3).ToString());
		// Prog.P.Echo("targetAzimuthVelocity:"+Math.Round(targetAzimuthVelocity,3).ToString());
		// Prog.P.Echo("ElevationVelocity:"+Math.Round(relativeAngularVelocity.X,3).ToString());
		// Prog.P.Echo("targetElevationVelocity:"+Math.Round(targetElevationVelocity,3).ToString());
		// Prog.P.Echo("Trying to turn "+(targetElevationVelocity>0?"Down":"Up")+"-"+(targetAzimuthVelocity>0?"Right":"Left"));
		
		// Prog.P.Echo("Deviation = "+Math.Round(currentAngle,1).ToString()+'°');
		// Prog.P.Echo("Azimuth = "+Math.Round(azimuthAngle,1).ToString()+'°');
		// Prog.P.Echo("Elevation = "+Math.Round(elevationAngle,1).ToString()+'°');
	
		Gyroscope.Pitch=(float)pitch;
		Gyroscope.Yaw=-1*(float)roll;
		Gyroscope.Roll=(float)yaw;
		
		Gyroscope.GyroOverride=true;
		
		return currentAngle;
	}
	
	public bool Ready(){
		if(Gyroscope==null||Battery==null||Mass==null||Generator==null||Light1==null||Light2==null||Warhead==null||Merge==null)
			return false;
		if(Gyroscope.Closed||Battery.Closed||Mass.Closed||Generator.Closed||Light1.Closed||Light2.Closed||Warhead.Closed||Merge.Closed)
			return false;
		if(!Gyroscope.IsFunctional||!Battery.IsFunctional||!Mass.IsFunctional||!Generator.IsFunctional||!Light1.IsFunctional||!Light2.IsFunctional||!Warhead.IsFunctional||!Merge.IsFunctional)
			return false;
		return Battery.HasCapacityRemaining;
	}
	
	public double Completion(){
		int completion=0;
		if(Gyroscope?.IsFunctional??false)
			completion++;
		if(Battery?.IsFunctional??false)
			completion++;
		if(Mass?.IsFunctional??false)
			completion++;
		if(Generator?.IsFunctional??false)
			completion++;
		if(Light1?.IsFunctional??false)
			completion++;
		if(Light2?.IsFunctional??false)
			completion++;
		if(Warhead?.IsFunctional??false)
			completion++;
		if(Merge?.IsFunctional??false)
			completion++;
		if(Battery?.HasCapacityRemaining??false)
			completion++;
		return completion/9.0;
	}
	
}

class LaunchInstructions{
	public Vector3D PredictedTarget;
	public DateTime LaunchTime;
	
	public LaunchInstructions(Vector3D t,DateTime l){
		PredictedTarget=t;
		LaunchTime=l;
	}
}

public enum ShipSize{
	Tiny=1,
	Small=2,
	Medium=3,
	Large=4,
	Huge=5,
	Gargantuan=6
}
public class MyTarget{
	public MyEntity Entity;
	public Vector3D ExpectedPosition;
	public Vector3D ExpectedVelocity;
	public Vector3D ExpectedAcceleration;
	public double TimeSinceLastUpdate;
	public bool Large{
		get{
			return Entity.Type==MyDetectedEntityType.LargeGrid;
		}
	}
	public bool Small{
		get{
			return Entity.Type==MyDetectedEntityType.SmallGrid;
		}
	}
	public bool Voxel{
		get{
			return Entity.Type==MyDetectedEntityType.Planet||Entity.Type==MyDetectedEntityType.Asteroid;
		}
	}
	
	public ShipSize GetSize(){
		double size=Entity.BoundingBox.Size.Length();
		if(size<10)
			return ShipSize.Tiny;
		if(size<25)
			return ShipSize.Small;
		if(size<75)
			return ShipSize.Medium;
		if(size<150)
			return ShipSize.Large;
		if(size<300)
			return ShipSize.Huge;
		return ShipSize.Gargantuan;
	}
	
	public double GetDistance(Vector3D from){
		return (ExpectedPosition-from).Length()-2*Entity.BoundingBox.Size.Length()/3;
	}
	
	public MyTarget(MyDetectedEntityInfo e){
		Entity=new MyEntity(e);
		ExpectedPosition=Entity.Position;
		ExpectedVelocity=Entity.Velocity;
		ExpectedAcceleration=new Vector3D(0,0,0);
		TimeSinceLastUpdate=0;
	}
	
	private MyTarget(MyEntity e,Vector3D pos,Vector3D vel,Vector3D acc,double time){
		Entity=e;
		ExpectedPosition=pos;
		ExpectedVelocity=vel;
		ExpectedAcceleration=acc;
		TimeSinceLastUpdate=time;
	}
	
	public void Update(MyDetectedEntityInfo e){
		Entity.Update(e);
		ExpectedAcceleration=(Entity.Velocity-ExpectedVelocity)/TimeSinceLastUpdate;
		ExpectedVelocity=Entity.Velocity;
		ExpectedPosition=Entity.Position;
		TimeSinceLastUpdate=0;
	}
	
	public override string ToString(){
		return "{["+ExpectedPosition.ToString()+","+ExpectedVelocity.ToString()+","+ExpectedAcceleration.ToString()+","+Math.Round(TimeSinceLastUpdate,3).ToString()+"],"+Entity.ToString()+"}";
	}
	
	public static MyTarget Parse(string input){
		if(input[0]!='{'||input[1]!='['||input[input.Length-2]!=']'||input[input.Length-1]!='}')
			throw new ArgumentException("Bad format");
		int index=input.IndexOf(']')+1;
		if(input[index]!=','||input[index+1]!='[')
			throw new ArgumentException("Bad format");
		string trackingSubstr=input.Substring(2,index-3);
		string entitySubstr=input.Substring(index+1,input.Length-trackingSubstr.Length-5);
		string[] args=trackingSubstr.Split(',');
		if(args.Length!=4)
			throw new ArgumentException("Bad format");
		Vector3D expectedPosition,expectedVelocity,expectedAcceleration;
		if(!Vector3D.TryParse(args[0],out expectedPosition)||!Vector3D.TryParse(args[1],out expectedVelocity)||!Vector3D.TryParse(args[2],out expectedAcceleration))
			throw new ArgumentException("Bad format");
		double timeSinceLastUpdate=double.Parse(args[3]);
		MyEntity entity=MyEntity.Parse(entitySubstr);
		return new MyTarget(entity,expectedPosition,expectedVelocity,expectedAcceleration,timeSinceLastUpdate);
	}
	
	public static bool TryParse(string input,out MyTarget output){
		output=null;
		try{
			output=Parse(input);
			return output!=null;
		}
		catch(Exception){
			return false;
		}
	}
	
	public void UpdateVectors(double SecondsSinceLastUpdate){
		TimeSinceLastUpdate+=SecondsSinceLastUpdate;
		if(Voxel)
			return;
		var lastVelDir=ExpectedVelocity;
		lastVelDir.Normalize();
		var lastAccDir=ExpectedAcceleration;
		lastAccDir.Normalize();
		
		var newExpectedVelocity=ExpectedAcceleration*SecondsSinceLastUpdate+ExpectedVelocity;
		var newVelDir=newExpectedVelocity;
		newVelDir.Normalize();
		
		double maxSpeed=Large?300:500;
		
		if(GenMethods.GetAngle(lastVelDir,lastAccDir)>=150&&GenMethods.GetAngle(lastVelDir,newVelDir)>=150){
			var newExpectedAcceleration=ExpectedAcceleration;
			if(ExpectedVelocity.X>0^newExpectedVelocity.X>0){
				newExpectedVelocity.X=0;
				newExpectedAcceleration.X=0;
			}
			if(ExpectedVelocity.Y>0^newExpectedVelocity.Y>0){
				newExpectedVelocity.Y=0;
				newExpectedAcceleration.Y=0;
			}
			if(ExpectedVelocity.Z>0^newExpectedVelocity.Z>0){
				newExpectedVelocity.Z=0;
				newExpectedAcceleration.Z=0;
			}
			var newAccDir=newExpectedAcceleration;
			newAccDir.Normalize();
			lastAccDir=ExpectedAcceleration;
			lastAccDir.Normalize();
			if(GenMethods.GetAngle(lastAccDir,newAccDir)>0.1){
				ExpectedAcceleration=newExpectedAcceleration;
			}
		}
		
		newVelDir=newExpectedVelocity;
		newVelDir.Normalize();
		newExpectedVelocity=newVelDir*Math.Min(newExpectedVelocity.Length(),maxSpeed);
		
		var avgVelocity=(newExpectedVelocity+ExpectedVelocity)/2;
		var newExpectedPosition=ExpectedPosition+SecondsSinceLastUpdate*avgVelocity;
		
		ExpectedVelocity=newExpectedVelocity;
		ExpectedPosition=newExpectedPosition;
	}
}

public struct VectorDto{
	public Vector3D V1;
	public Vector3D V2;
	
	public VectorDto(Vector3D v1,Vector3D v2){
		V1=v1;
		V2=v2;
	}
	
	public VectorDto(MatrixD Orientation){
		V1=Orientation.Forward;
		V2=Orientation.Up;
	}
	
	public VectorDto(BoundingBox Box){
		V1=Box.Min+Box.Center;
		V2=Box.Max+Box.Center;
	}
	
	public Vector3D[] ToArr(){
		return new Vector3D[] {V1,V2};
	}
	
	public override string ToString(){
		return "("+V1.ToString()+";"+V2.ToString()+")";
	}
	
	public static VectorDto Parse(string input){
		if(input.IndexOf('(')!=0||input.IndexOf(')')!=input.Length-1)
			throw new ArgumentException("Bad format");
		string[] args=input.Substring(1,input.Length-2).Split(';');
		if(args.Length!=2)
			throw new ArgumentException("Bad format");
		Vector3D v1,v2;
		if(!(Vector3D.TryParse(args[0],out v1)&&Vector3D.TryParse(args[1],out v2)))
			throw new ArgumentException("Bad format");
		return new VectorDto(v1,v2);
	}
	
	public static bool TryParse(string input,out VectorDto? output){
		output=null;
		try{
			output=Parse(input);
			return output!=null;
		}
		catch{
			return false;
		}
	}
}
public class MyEntity{
	public long EntityId;
	public string Name;
	public MyDetectedEntityType Type;
	public MatrixD Orientation;
	public Vector3D Velocity;
	public MyRelationsBetweenPlayerAndBlock Relationship;
	public BoundingBoxD BoundingBox;
	public DateTime TimeStamp;
	public TimeSpan Age{
		get{
			return DateTime.Now.Subtract(TimeStamp);
		}
	}
	public Vector3D Position{
		get{
			return BoundingBox.Center;
		}
	}
	
	public MyEntity(long entityId,string name,MyDetectedEntityType type,MatrixD orientation,Vector3D velocity,MyRelationsBetweenPlayerAndBlock relationship,BoundingBoxD boundingBox,DateTime timeStamp){
		EntityId=entityId;
		Name=name;
		Type=type;
		Orientation=orientation;
		Velocity=velocity;
		Relationship=relationship;
		BoundingBox=boundingBox;
		TimeStamp=timeStamp;
	}
	
	public MyEntity(MyDetectedEntityInfo e){
		EntityId=e.EntityId;
		Name=e.Name;
		Type=e.Type;
		Orientation=e.Orientation;
		Velocity=e.Velocity;
		Relationship=e.Relationship;
		BoundingBox=e.BoundingBox;
		TimeStamp=DateTime.Now;
	}
	
	public bool Same(MyEntity o){
		if(Type!=o.Type)
			return false;
		if(EntityId==o.EntityId)
			return true;
		return Relationship==o.Relationship&&Name.Equals(o.Name);
	}
	
	public bool Same(MyDetectedEntityInfo o){
		if(Type!=o.Type)
			return false;
		if(EntityId==o.EntityId)
			return true;
		return Relationship==o.Relationship&&Name.Equals(o.Name);
	}
	
	public void Update(MyEntity o){
		Name=o.Name;
		Orientation=o.Orientation;
		Velocity=o.Velocity;
		Relationship=o.Relationship;
		BoundingBox=o.BoundingBox;
		TimeStamp=o.TimeStamp;
	}
	
	public void Update(MyDetectedEntityInfo o){
		Update(new MyEntity(o));
	}
	
	public override string ToString(){
		return "["+EntityId.ToString()+","+Name+","+Type.ToString()+","+(new VectorDto(Orientation)).ToString()+","+Velocity.ToString()+","+Relationship.ToString()+","+(new VectorDto(BoundingBox.Min,BoundingBox.Max)).ToString()+","+TimeStamp.ToString()+"]";
	}
	
	public static MyEntity Parse(string input){
		if(input[0]!='['||input[input.Length-1]!=']')
			throw new ArgumentException("Bad format");
		string[] args=input.Substring(1,input.Length-2).Split(',');
		if(args.Length!=8)
			throw new ArgumentException("Bad format");
		long entityId=long.Parse(args[0]);
		string name=args[1];
		MyDetectedEntityType type=(MyDetectedEntityType)Enum.Parse(typeof(MyDetectedEntityType),args[2]);
		VectorDto orientationDto=VectorDto.Parse(args[3]);
		MatrixD orientation=MatrixD.CreateFromDir(orientationDto.V1,orientationDto.V2);
		Vector3D velocity;
		if(!Vector3D.TryParse(args[4],out velocity))
			throw new ArgumentException("Bad format");
		MyRelationsBetweenPlayerAndBlock relationship=(MyRelationsBetweenPlayerAndBlock)Enum.Parse(typeof(MyRelationsBetweenPlayerAndBlock),args[5]);
		BoundingBoxD boundingBox=BoundingBoxD.CreateFromPoints(VectorDto.Parse(args[6]).ToArr());
		DateTime timeStamp=DateTime.Parse(args[7]);
		return new MyEntity(entityId,name,type,orientation,velocity,relationship,boundingBox,timeStamp);
	}
	
	public static bool TryParse(string input,out MyEntity output){
		output=null;
		try{
			output=Parse(input);
			return output!=null;
		}
		catch{
			return false;
		}
	}
}


// Core Components
TimeSpan TimeSinceStart=new TimeSpan(0);
long Cycle=0;
char LoadingChar='|';
double SecondsSinceLastUpdate=0;

static class Prog{
	public static MyGridProgram P;
}

static class GenMethods{
	private static double GetAngle(Vector3D v1,Vector3D v2,int i){
		v1.Normalize();
		v2.Normalize();
		double output=Math.Round(Math.Acos(v1.X*v2.X+v1.Y*v2.Y+v1.Z*v2.Z)*180/Math.PI,5);
		if(i>0&&output.ToString().Equals("NaN")){
			Random Rnd=new Random();
			Vector3D v3=new Vector3D(Rnd.Next(0,10)-5,Rnd.Next(0,10)-5,Rnd.Next(0,10)-5);
			v3.Normalize();
			if(Rnd.Next(0,1)==1)
				output=GetAngle(v1+v3/360,v2,i-1);
			else
				output=GetAngle(v1,v2+v3/360,i-1);
		}
		return output;
	}
	
	public static double GetAngle(Vector3D v1, Vector3D v2){
		return GetAngle(v1,v2,10);
	}
	
	public static bool HasBlockData(IMyTerminalBlock Block, string Name){
		if(Name.Contains(':'))
			return false;
		string[] args=Block.CustomData.Split('•');
		foreach(string argument in args){
			if(argument.IndexOf(Name+':')==0){
				return true;
			}
		}
		return false;
	}

	public static string GetBlockData(IMyTerminalBlock Block, string Name){
		if(Name.Contains(':'))
			return "";
		string[] args=Block.CustomData.Split('•');
		foreach(string argument in args){
			if(argument.IndexOf(Name+':')==0){
				return argument.Substring((Name+':').Length);
			}
		}
		return "";
	}
	
	public static T GetBlockData<T>(IMyTerminalBlock Block,string Name,Func<string,T> F){
		return F(GetBlockData(Block,Name));
	}
	
	public static T TryGetBlockData<T>(IMyTerminalBlock Block,string Name,Func<string,T> F,T DefaultValue){;
		if(!HasBlockData(Block,Name))
			return DefaultValue;
		try{
			return F(GetBlockData(Block,Name));
		}
		catch{
			return DefaultValue;
		}
	}
	
	public static bool SetBlockData(IMyTerminalBlock Block, string Name, string Data){
		if(Name.Contains(':'))
			return false;
		string[] args=Block.CustomData.Split('•');
		for(int i=0; i<args.Count(); i++){
			if(args[i].IndexOf(Name+':')==0){
				Block.CustomData=Name+':'+Data;
				for(int j=0; j<args.Count(); j++){
					if(j!=i){
						Block.CustomData+='•'+args[j];
					}
				}
				return true;
			}
		}
		Block.CustomData+='•'+Name+':'+Data;
		return true;
	}
	
	public static bool WipeBlockData(IMyTerminalBlock Block,string Name){
		if(Name.Contains(':'))
			return false;
		string[] args=Block.CustomData.Split('•');
		for(int i=0; i<args.Count(); i++){
			if(args[i].IndexOf(Name+':')==0){
				Block.CustomData="";
				for(int j=0; j<args.Count(); j++){
					if(j!=i)
						Block.CustomData+='•'+args[j];
				}
				return true;
			}
		}
		return false;
	}

	public static Vector3D GlobalToLocal(Vector3D Global,MatrixD Ref,Vector3D Pos){
		Vector3D Local=Vector3D.Transform(Global+Pos,MatrixD.Invert(Ref));
		Local.Normalize();
		return Local*Global.Length();
	}

	public static Vector3D GlobalToLocal(Vector3D Global,IMyCubeBlock Ref){
		return GlobalToLocal(Global,Ref.WorldMatrix,Ref.GetPosition());
	}

	public static Vector3D GlobalToLocalPosition(Vector3D Global,IMyCubeBlock Ref){
		Vector3D Local=Vector3D.Transform(Global,MatrixD.Invert(Ref.WorldMatrix));
		Local.Normalize();
		return Local*(Global-Ref.GetPosition()).Length();
	}

	public static Vector3D LocalToGlobal(Vector3D Local,MatrixD Ref,Vector3D Pos){
		Vector3D Global=Vector3D.Transform(Local,Ref)-Pos;
		Global.Normalize();
		return Global*Local.Length();
	}

	public static Vector3D LocalToGlobal(Vector3D Local,IMyCubeBlock Ref){
		return LocalToGlobal(Local,Ref.WorldMatrix,Ref.GetPosition());
	}

	public static Vector3D LocalToGlobalPosition(Vector3D Local,IMyCubeBlock Ref){
		return Vector3D.Transform(Local,Ref.WorldMatrix);
	}

	public static List<T> Merge<T>(List<T> L1,List<T> L2){
		return L1.Concat(L2).ToList();
	}
	
	public static string GetRemovedString(string bigString,string smallString){
		string output=bigString;
		if(bigString.Contains(smallString)){
			output=bigString.Substring(0, bigString.IndexOf(smallString))+bigString.Substring(bigString.IndexOf(smallString)+smallString.Length);
		}
		return output;
	}
	
	public static string Round(Vector3D Vector,int Places){
		return "X:"+Math.Round(Vector.X,Places).ToString()+" Y:"+Math.Round(Vector.Y,Places).ToString()+" Z:"+Math.Round(Vector.Z,Places).ToString();
	}
	
}

static class CollectionMethods<T> where T:class,IMyTerminalBlock{
	public static MyGridProgram P{
		get{
			return Prog.P;
		}
	}
	
	private static List<T> Get_AllBlocks(){
		List<T> output=new List<T>();
		P.GridTerminalSystem.GetBlocksOfType<T>(output);
		return output;
	}
	public static Rool<T> AllBlocks=new Rool<T>(Get_AllBlocks);
	private static List<T> Get_AllConstruct(){
		List<T> output=new List<T>();
		foreach(T Block in AllBlocks){
			if(Block.IsSameConstructAs(P.Me))
				output.Add(Block);
		}
		return output;
	}
	public static Rool<T> AllConstruct=new Rool<T>(Get_AllConstruct);
	
	public static T ByFullName(string name,List<T> blocks){
		foreach(T Block in blocks){
			if(Block?.CustomName.Equals(name)??false)
				return Block;
		}
		return null;
	}
	
	public static T ByFullName(string name){
		return ByName(name,AllConstruct);
	}
	
	public static T ByName(string name,List<T> blocks){
		foreach(T Block in blocks){
			if(Block?.CustomName.Contains(name)??false)
				return Block;
		}
		return null;
	}
	
	public static T ByName(string name){
		return ByName(name,AllConstruct);
	}
	
	public static List<T> AllByName(string name,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block?.CustomName.Contains(name)??false)
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByName(string name){
		return AllByName(name,AllConstruct);
	}
	
	public static T ByDistance(Vector3D Ref,double distance,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&(Block.GetPosition()-Ref).Length()<=distance)
				return Block;
		}
		return null;
	}
	
	public static T ByDistance(Vector3D Ref,double distance){
		return ByDistance(Ref,distance,AllConstruct);
	}
	
	public static List<T> AllByDistance(Vector3D Ref,double distance,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&(Block.GetPosition()-Ref).Length()<=distance)
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByDistance(Vector3D Ref,double distance){
		return AllByDistance(Ref,distance,AllConstruct);
	}
	
	public static T ByClosest(Vector3D Ref,List<T> blocks){
		return (blocks.Count>0?SortByDistance(blocks,Ref)[0]:null);
	}
	
	public static T ByClosest(Vector3D Ref){
		return ByClosest(Ref,AllConstruct);
	}
	
	public static T ByFunc(Func<T,bool> f,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&f(Block))
				return Block;
		}
		return null;
	}
	
	public static T ByFunc(Func<T,bool> f){
		return ByFunc(f,AllConstruct);
	}
	
	public static List<T> AllByFunc(Func<T,bool> f,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&f(Block))
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByFunc(Func<T,bool> f){
		return AllByFunc(f,AllConstruct);
	}
	
	public static T ByFunc<U>(Func<T,U,bool> f,U param,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&f(Block,param))
				return Block;
		}
		return null;
	}
	
	public static T ByFunc<U>(Func<T,U,bool> f,U param){
		return ByFunc<U>(f,param,AllConstruct);
	}
	
	public static List<T> AllByFunc<U>(Func<T,U,bool> f,U param,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&f(Block,param))
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByFunc<U>(Func<T,U,bool> f,U param){
		return AllByFunc(f,param,AllConstruct);
	}
	
	public static T ByGrid(IMyCubeGrid Grid,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&Block.CubeGrid==Grid)
				return Block;
		}
		return null;
	}
	
	public static T ByGrid(IMyCubeGrid Grid){
		return ByGrid(Grid,AllBlocks);
	}
	
	public static List<T> AllByGrid(IMyCubeGrid Grid,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&Block.CubeGrid==Grid)
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByGrid(IMyCubeGrid Grid){
		return AllByGrid(Grid,AllBlocks);
	}
	
	public static T ByConstruct(IMyCubeGrid Grid,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&Block.CubeGrid.IsSameConstructAs(Grid))
				return Block;
		}
		return null;
	}
	
	public static T ByConstruct(IMyCubeGrid Grid){
		return ByConstruct(Grid,AllBlocks);
	}
	
	public static List<T> AllByConstruct(IMyCubeGrid Grid,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&Block.CubeGrid.IsSameConstructAs(Grid))
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByConstruct(IMyCubeGrid Grid){
		return AllByConstruct(Grid,AllBlocks);
	}
	
	public static T ByDefinitionString(string def,List<T> blocks){
		if(def.ToLower().Equals(def)){
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.ToLower().Contains(def)??false)
					return Block;
			}
		}
		else{
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.Contains(def)??false)
					return Block;
			}
		}
		return null;
	}
	
	public static T ByDefinitionString(string def){
		return ByDefinitionString(def,AllConstruct);
	}
	
	public static List<T> AllByDefinitionString(string def,List<T> blocks){
		List<T> output=new List<T>();
		if(def.ToLower().Equals(def)){
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.ToLower().Contains(def)??false)
					output.Add(Block);
			}
		}
		else{
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.Contains(def)??false)
					output.Add(Block);
			}
		}
		return output;
	}
	
	public static List<T> AllByDefinitionString(string def){
		return AllByDefinitionString(def,AllConstruct);
	}
	
	public static List<T> SortByDistance(List<T> input,Vector3D Ref){
		if(input.Count<=1)
			return input;
		List<T> output=new List<T>();
		foreach(T block in input)
			output.Add(block);
		SortHelperD(output,Ref,0,output.Count-1);
		return output;
	}
	
	private static void Swap(List<T> list,int i1,int i2){
		T temp=list[i1];
		list[i1]=list[i2];
		list[i2]=temp;
	}
	
	private static int SortPartitionD(List<T> sorting,Vector3D Ref,int low,int high){
		double pivot=(sorting[high].GetPosition()-Ref).Length();
		int i=low-1;
		for(int j=low;j<high;j++){
			if((sorting[j].GetPosition()-Ref).Length()<pivot)
				Swap(sorting,j,++i);
		}
		Swap(sorting,high,++i);
		return i;
	}
	
	private static void SortHelperD(List<T> sorting,Vector3D Ref,int low,int high){
		if(low>=high)
			return;
		int pi=SortPartitionD(sorting,Ref,low,high);
		SortHelperD(sorting,Ref,low,pi-1);
		SortHelperD(sorting,Ref,pi+1,high);
	}
	
	public static List<T> SortByName(List<T> input){
		if(input.Count<=1)
			return input;
		List<T> output=new List<T>();
		foreach(T block in input)
			output.Add(block);
		SortHelperN(output,0,output.Count-1);
		return output;
	}
	
	private static int SortPartitionN(List<T> sorting,int low,int high){
		string pivot=sorting[high].CustomName;
		int i=low-1;
		for(int j=low;j<high;j++){
			if(String.Compare(sorting[j].CustomName,pivot)<0)
				Swap(sorting,j,++i);
		}
		Swap(sorting,high,++i);
		return i;
	}
	
	private static void SortHelperN(List<T> sorting,int low,int high){
		if(low>=high)
			return;
		int pi=SortPartitionN(sorting,low,high);
		SortHelperN(sorting,low,pi-1);
		SortHelperN(sorting,pi+1,high);
	}
	
	
}

abstract class OneDone{
	public static List<OneDone> All;
	
	protected OneDone(){
		if(All==null)
			All=new List<OneDone>();
		All.Add(this);
	}
	
	public static void ResetAll(){
		if(All==null)
			return;
		for(int i=0;i<All.Count;i++)
			All[i].Reset();
	}
	
	public abstract void Reset();
}
class OneDone<T>:OneDone{
	private T Default;
	public T Value;
	
	public OneDone(T value):base(){
		Default=value;
		Value=value;
	}
	
	public override void Reset(){
		Value=Default;
	}
	
	public static implicit operator T(OneDone<T> O){
		return O.Value;
	}
}
class Rool<T>:IEnumerable<T>{
	// Run Only Once
	private List<T> _Value;
	public List<T> Value{
		get{
			if(!Ran.Value){
				_Value=Updater();
				Ran.Value=true;
			}
			return _Value;
		}
	}
	private OneDone<bool> Ran;
	private Func<List<T>> Updater;
	
	public Rool(Func<List<T>> updater){
		Ran=new OneDone<bool>(false);
		Updater=updater;
	}
	
	public IEnumerator<T> GetEnumerator(){
		return Value.GetEnumerator();
	}
	
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
	
	public static implicit operator List<T>(Rool<T> R){
		return R.Value;
	}
}
class Roo<T>{
	// Run Only Once
	private T _Value;
	public T Value{
		get{
			if(!Ran.Value){
				_Value=Updater();
				Ran.Value=true;
			}
			return _Value;
		}
	}
	private OneDone<bool> Ran;
	private Func<T> Updater;
	
	public Roo(Func<T> updater){
		Ran=new OneDone<bool>(false);
		Updater=updater;
	}
	
	public void Reset(){
		Ran.Reset();
	}
	
	public static implicit operator T(Roo<T> R){
		return R.Value;
	}
}

bool GameTick(int divisor=100){
	int tickTime=100;
	if(Runtime.UpdateFrequency==UpdateFrequency.Update1)
		tickTime=1;
	else if(Runtime.UpdateFrequency==UpdateFrequency.Update10)
		tickTime=10;
	return (Cycle*tickTime)%divisor==0;
}

TimeSpan FromSeconds(double seconds){
	return (new TimeSpan(0,0,0,(int)seconds,(int)(seconds*1000)%1000));
}

TimeSpan UpdateTimeSpan(TimeSpan old,double seconds){
	return old+FromSeconds(seconds);
}

string ToString(TimeSpan ts){
	if(ts.TotalDays>=1)
		return Math.Round(ts.TotalDays,2).ToString()+" days";
	else if(ts.TotalHours>=1)
		return Math.Round(ts.TotalHours,2).ToString()+" hours";
	else if(ts.TotalMinutes>=1)
		return Math.Round(ts.TotalMinutes,2).ToString()+" minutes";
	else if(ts.TotalSeconds>=1)
		return Math.Round(ts.TotalSeconds,3).ToString()+" seconds";
	else 
		return Math.Round(ts.TotalMilliseconds,0).ToString()+" milliseconds";
}

void Write(string text,bool new_line=true,bool append=true){
	Echo(text);
	if(new_line)
		Me.GetSurface(0).WriteText(text+'\n', append);
	else
		Me.GetSurface(0).WriteText(text, append);
}

void UpdateProgramInfo(){
	OneDone.ResetAll();
	Cycle=(++Cycle)%long.MaxValue;
	switch(LoadingChar){
		case '|':
			LoadingChar='\\';
			break;
		case '\\':
			LoadingChar='-';
			break;
		case '-':
			LoadingChar='/';
			break;
		case '/':
			LoadingChar='|';
			break;
	}
	Write("",false,false);
	Echo(ProgramName+" OS\nCycle-"+Cycle.ToString()+" ("+LoadingChar+")");
	Me.GetSurface(1).WriteText(ProgramName+" OS\nCycle-"+Cycle.ToString()+" ("+LoadingChar+")",false);
	SecondsSinceLastUpdate=Runtime.TimeSinceLastRun.TotalSeconds+(Runtime.LastRunTimeMs/1000);
	Echo(ToString(FromSeconds(SecondsSinceLastUpdate))+" since last Cycle");
	TimeSinceStart=UpdateTimeSpan(TimeSinceStart,SecondsSinceLastUpdate);
	Echo(ToString(TimeSinceStart)+" since last reboot\n");
	Me.GetSurface(1).WriteText("\n"+ToString(TimeSinceStart)+" since last reboot",true);
}

