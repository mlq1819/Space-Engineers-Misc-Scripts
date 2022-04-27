/*
* Ship AI System 
* Built by mlq1616
* https://github.com/mlq1819
*/
const string ProgramName = "Saturine Ship AI";
Color DefaultTextColor=new Color(197,137,255,255);
Color DefaultBackgroundColor=new Color(44,0,88,255);

// Runtime Logic and Methods
public void Main(string argument,UpdateType updateSource){
	UpdateProgramInfo();
	Write("Connected to "+Ships.Count+" Ships");
	int updateFrequency=100;
	for(int i=0;i<Ships.Count;i++){
		MyShip ship=Ships[i];
		ship.RunSystemUpdate(SecondsSinceLastUpdate);
		if(i==CurrentShip){
			bool runMe=true;
			switch(LastUpdate){
				case UpdateFrequency.Update10:
					runMe=ship.UpdateFrequency<=10||((ShipCycle*10)%ship.UpdateFrequency==0);
					break;
				case UpdateFrequency.Update1:
					runMe=ship.UpdateFrequency<=1||((ShipCycle)%ship.UpdateFrequency==0);
					break;
			}
			if(runMe){
				Write("Performing Operations for "+(i+1).ToString()+"/"+Ships.Count);
				try{
					ship.RunSystemOperations();
				}
				catch(Exception e){
					Write("Encountered error with "+ship.Grid.CustomName);
					throw e;
				}
			}
			else
				CurrentShip++;
		}
		updateFrequency=Math.Min(updateFrequency,ship.UpdateFrequency);
		Write(ship.Status);
	}
	
	if(++CurrentShip>=Ships.Count){
		ShipCycle=(ShipCycle%long.MaxValue)+1;
		CurrentShip=0;
		switch(updateFrequency){
			case 1:
				Runtime.UpdateFrequency=UpdateFrequency.Update1;
				break;
			case 10:
				Runtime.UpdateFrequency=UpdateFrequency.Update10;
				break;
			default:
				Runtime.UpdateFrequency=UpdateFrequency.Update100;
				break;
		}
		LastUpdate=Runtime.UpdateFrequency;
	}
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update1;
}
UpdateFrequency LastUpdate=UpdateFrequency.Update1;


bool TryAddNewShip(IMyShipController controller){
	if(controller==null)
		return false;
	foreach(MyShip ship in Ships){
		if(ship.Grid.IsSameConstructAs(controller.CubeGrid))
			return false;
	}
	var altControllers=CollectionMethods<IMyShipController>.AllByGrid(controller.CubeGrid);
	try{
		MyShip newShip=new MyShip(controller,altControllers);
		if(newShip!=null)
			Ships.Add(newShip);
		if(newShip!=null){
			MyShip.ShipCount++;
		}
		return newShip!=null;
	}
	catch{
		return false;
	}
}

int CurrentShip=0;

// Initialization and Object Definitions
public Program(){
	Write("Beginning initialization.");
	Prog.P=this;
	Me.CustomName=(ProgramName+" Programmable block").Trim();
	for(int i=0;i<Me.SurfaceCount;i++){
		Me.GetSurface(i).FontColor=DefaultTextColor;
		Me.GetSurface(i).BackgroundColor=DefaultBackgroundColor;
		Me.GetSurface(i).Alignment=TextAlignment.CENTER;
		Me.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
		Me.GetSurface(i).Font="Monospace";
		Me.GetSurface(i).WriteText("",false);
	}
	Me.GetSurface(0).FontSize=0.75f;
	Me.GetSurface(1).FontSize=1.5f;
	Me.GetSurface(1).TextPadding=30.0f;
	MyShip.WriteFull=Write;
	
	// Initialize runtime objects
	Write("Initializing objects...");
	Ships=new List<MyShip>();
	
	IMyShipController mainController=CollectionMethods<IMyShipController>.ByGrid(Me.CubeGrid,CollectionMethods<IMyShipController>.AllByName("Main"));
	if(mainController==null){
		Write("Failed to retrieve main controller");
	}
	if(TryAddNewShip(mainController)){
		Write("Added Main Ship\n"+mainController.CustomName);
	}
	List<IMyShipController> mainControllers=CollectionMethods<IMyShipController>.AllByName("Main",CollectionMethods<IMyShipController>.AllBlocks);
	foreach(IMyShipController newController in mainControllers){
		if(TryAddNewShip(newController))
			Write("Added New Ship\n"+newController.CustomName);
	}
	
	// Load runtime variables from CustomData
	Write("Setting variables...");
	
	
	// Load data from Storage
	Write("Loading data...");
	string storageMode="";
	string[] storageArgs=this.Storage.Trim().Split('\n');
	foreach(string line in storageArgs){
		switch(line){
			case "SampleData":
				storageMode=line;
				break;
			default:
				switch(storageMode){
					case "SampleData":
						// Load Data from line
						break;
				}
				break;
		}
	}
	
	Runtime.UpdateFrequency=UpdateFrequency.Once;
	Write("Completed initialization!");
}

List<MyShip> Ships;

public class MyShip{
	public static List<List<MyEntity>> AllEntities;
	public static Action<string,bool,bool> WriteFull;
	public static void Write(string text){
		WriteFull(text,true,true);
	}
	public static int ShipCount=0;
	
	public string Status;
	public int UpdateFrequency=1;
	
	public IMyShipController Controller;
	public List<IMyShipController> AltControllers;
	public IMyGyro Gyroscope;
	List<IMyGyro> AllGyroscopes;
	
	public List<IMyLandingGear> LandingGear;
	
	public List<MyAirlock> Airlocks;
	
	public IMyCubeGrid Grid{
		get{
			return Controller.CubeGrid;
		}
	}
	public double ShipMass{
		get{
			return Controller.CalculateShipMass().TotalMass;
		}
	}
	float MassAccomodation{
		get{
			return (float)(ShipMass*Gravity.Length());
		}
	}
	
	public Vector3D Velocity{
		get{
			return Controller.GetShipVelocities().LinearVelocity;
		}
	}
	public Roo<Vector3D> Position;
	
	public Vector3D DampenVelocity=new Vector3D(0,0,0);
	
	double SecondsSinceLastRun=0;
	
	public double SpeedDeviation{
		get{
			return (Velocity-DampenVelocity).Length();
		}
	}
	
	public Vector3D Relative(Vector3D input){
		Vector3D output=Vector3D.Transform(input+Position,MatrixD.Invert(Controller.WorldMatrix));
		output.Normalize();
		output*=input.Length();
		return output;
	}
	
	public List<IMyThrust>[] AllThrusters=new List<IMyThrust>[6];
	List<IMyThrust> ForwardThrusters{
		set{
			AllThrusters[0]=value;
		}
		get{
			return AllThrusters[0];
		}
	}
	List<IMyThrust> BackwardThrusters{
		set{
			AllThrusters[1]=value;
		}
		get{
			return AllThrusters[1];
		}
	}
	List<IMyThrust> UpThrusters{
		set{
			AllThrusters[2]=value;
		}
		get{
			return AllThrusters[2];
		}
	}
	List<IMyThrust> DownThrusters{
		set{
			AllThrusters[3]=value;
		}
		get{
			return AllThrusters[3];
		}
	}
	List<IMyThrust> LeftThrusters{
		set{
			AllThrusters[4]=value;
		}
		get{
			return AllThrusters[4];
		}
	}
	List<IMyThrust> RightThrusters{
		set{
			AllThrusters[5]=value;
		}
		get{
			return AllThrusters[5];
		}
	}
	
	public Vector3D ForwardVector{
		get{
			return GenMethods.LocalToGlobal(new Vector3D(0,0,-1),Controller);
		}
	}
	public Vector3D BackwardVector{
		get{
			return ForwardVector*-1;
		}
	}
	public Vector3D UpVector{
		get{
			return GenMethods.LocalToGlobal(new Vector3D(0,1,0),Controller);
		}
	}
	public Vector3D DownVector{
		get{
			return UpVector*-1;
		}
	}
	public Vector3D LeftVector{
		get{
			return GenMethods.LocalToGlobal(new Vector3D(-1,0,0),Controller);
		}
	}
	public Vector3D RightVector{
		get{
			return LeftVector*-1;
		}
	}
	
	
	float GetThrust(int i){
		float total=0;
		foreach(IMyThrust Thruster in AllThrusters[i]){
			if(Thruster.IsWorking)
				total+=Thruster.MaxEffectiveThrust;
		}
		return Math.Max(total,1);
	}
	Roo<int,float> ForwardThrust;
	Roo<int,float> BackwardThrust;
	Roo<int,float> UpThrust;
	Roo<int,float> DownThrust;
	Roo<int,float> LeftThrust;
	Roo<int,float> RightThrust;
	
	double ForwardAcc{
		get{
			return ForwardThrust/ShipMass;
		}
	}
	double BackwardAcc{
		get{
			return BackwardThrust/ShipMass;
		}
	}
	double UpAcc{
		get{
			return UpThrust/ShipMass;
		}
	}
	double DownAcc{
		get{
			return DownThrust/ShipMass;
		}
	}
	double LeftAcc{
		get{
			return LeftThrust/ShipMass;
		}
	}
	double RightAcc{
		get{
			return RightThrust/ShipMass;
		}
	}

	double ForwardGs{
		get{
			return ForwardAcc/9.81;
		}
	}
	double BackwardGs{
		get{
			return BackwardAcc/9.81;
		}
	}
	double UpGs{
		get{
			return UpAcc/9.81;
		}
	}
	double DownGs{
		get{
			return DownAcc/9.81;
		}
	}
	double LeftGs{
		get{
			return LeftAcc/9.81;
		}
	}
	double RightGs{
		get{
			return RightAcc/9.81;
		}
	}
	
	Base6Directions.Direction Forward{
		get{
			return Controller.Orientation.Forward;
		}
	}
	Base6Directions.Direction Backward{
		get{
			return Base6Directions.GetOppositeDirection(Forward);
		}
	}
	Base6Directions.Direction Up{
		get{
			return Controller.Orientation.Up;
		}
	}
	Base6Directions.Direction Down{
		get{
			return Base6Directions.GetOppositeDirection(Up);
		}
	}
	Base6Directions.Direction Left{
		get{
			return Controller.Orientation.Left;
		}
	}
	Base6Directions.Direction Right{
		get{
			return Base6Directions.GetOppositeDirection(Left);
		}
	}
	
	double GetElevation(){
		double output=double.MaxValue;
		Controller.TryGetPlanetElevation(MyPlanetElevation.Surface,out output);
		return output;
	}
	public Roo<double> Elevation;
	double GetSealevel(){
		double output=double.MaxValue;
		Controller.TryGetPlanetElevation(MyPlanetElevation.Sealevel,out output);
		return output;
	}
	public Roo<double> Sealevel;
	Vector3D GetPlanetCenter(){
		Vector3D output=new Vector3D(double.MaxValue,double.MaxValue,double.MaxValue);
		Controller.TryGetPlanetPosition(out output);
		return output;
	}
	public Roo<Vector3D> PlanetCenter;
	
	public bool Landed(){
		foreach(IMyLandingGear gear in LandingGear){
			if(gear.IsLocked)
				return true;
		}
		return false;
	}
	
	public Vector3D Gravity{
		get{
			return Controller.GetNaturalGravity();
		}
	}
	
	public MyShip(IMyShipController controller, List<IMyShipController> altControllers){
		Status="Initializing";
		Controller=controller;
		Position=new Roo<Vector3D>(Controller.GetPosition);
		AltControllers=new List<IMyShipController>();
		foreach(var altController in altControllers){
			if(altController!=controller&&altController!=null){
				AltControllers.Add(altController);
			}
		}
		AllGyroscopes=CollectionMethods<IMyGyro>.AllByGrid(Grid);
		if(AllGyroscopes.Count==0)
			throw new ArgumentException("Error initializing ship: No valid gyroscopes");
		Gyroscope=CollectionMethods<IMyGyro>.ByName("Control Gyroscope",AllGyroscopes);
		if(Gyroscope==null)
			Gyroscope=AllGyroscopes[0];
		for(int i=0;i<6;i++)
			AllThrusters[i]=new List<IMyThrust>();
		List<IMyThrust> myThrusters=CollectionMethods<IMyThrust>.AllByGrid(Grid);
		foreach(IMyThrust thrust in myThrusters){
			Base6Directions.Direction direction=thrust.Orientation.Forward;
			if(direction==Backward)
				ForwardThrusters.Add(thrust);
			else if(direction==Forward)
				BackwardThrusters.Add(thrust);
			else if(direction==Down)
				UpThrusters.Add(thrust);
			else if(direction==Up)
				DownThrusters.Add(thrust);
			else if(direction==Right)
				LeftThrusters.Add(thrust);
			else if(direction==Left)
				RightThrusters.Add(thrust);
		}
		ForwardThrust=new Roo<int,float>(GetThrust,0);
		BackwardThrust=new Roo<int,float>(GetThrust,1);
		UpThrust=new Roo<int,float>(GetThrust,2);
		DownThrust=new Roo<int,float>(GetThrust,3);
		LeftThrust=new Roo<int,float>(GetThrust,4);
		RightThrust=new Roo<int,float>(GetThrust,5);
		
		List<IMyLandingGear> allLandingGear=CollectionMethods<IMyLandingGear>.AllByGrid(Grid);
		LandingGear=new List<IMyLandingGear>();
		foreach(IMyLandingGear gear in allLandingGear){
			if(gear.IsParkingEnabled)
				LandingGear.Add(gear);
		}
		
		List<IMyDoor> airlockDoors=CollectionMethods<IMyDoor>.AllByName("Airlock",CollectionMethods<IMyDoor>.AllByConstruct(Grid));
		Dictionary<string,int> airlockNames=new Dictionary<string,int>();
		for(int i=0;i<airlockDoors.Count;i++){
			IMyDoor door=airlockDoors[i];
			string name=door.CustomName;
			if(!name.Contains("Door")){
				airlockDoors.RemoveAt(i--);
				continue;
			}
			name=name.Substring(0,name.IndexOf("Door")).Trim();
			if(airlockNames.ContainsKey(name))
				airlockNames[name]=airlockNames[name]+1;
			else
				airlockNames.Add(name,1);
		}
		Airlocks=new List<MyAirlock>();
		foreach(string airlockName in airlockNames.Keys){
			if(airlockNames[airlockName]<2)
				continue;
			List<List<IMyDoor>> myAirlockDoors=new List<List<IMyDoor>>();
			for(int i=0;i<4;i++)
				myAirlockDoors.Add(new List<IMyDoor>());
			for(int i=0;i<airlockDoors.Count;i++){
				IMyDoor door=airlockDoors[i];
				string name=door.CustomName;
				string numerator=name.Substring(name.IndexOf("Door")).Trim();
				name=name.Substring(0,name.IndexOf("Door")).Trim();
				if(name.Contains(airlockName)&&name.IndexOf(airlockName)==0){
					if(numerator.Contains("1"))
						myAirlockDoors[0].Add(door);
					else if(numerator.Contains("2"))
						myAirlockDoors[1].Add(door);
					else if(numerator.Contains("3"))
						myAirlockDoors[2].Add(door);
					else if(numerator.Contains("4"))
						myAirlockDoors[3].Add(door);
					else
						continue;
					airlockDoors.RemoveAt(i--);
					continue;
				}
			}
			for(int i=0;i<myAirlockDoors.Count;i++){
				if(myAirlockDoors[i].Count==0)
					myAirlockDoors.RemoveAt(i--);
			}
			if(myAirlockDoors.Count>1)
				Airlocks.Add(new MyAirlock(airlockName,myAirlockDoors));
		}
		
		Elevation=new Roo<double>(GetElevation);
		Sealevel=new Roo<double>(GetSealevel);
		PlanetCenter=new Roo<Vector3D>(GetPlanetCenter);
		Status="Initialized";
	}
	
	double LastElevation=0;
	Vector3D LastVelocity=new Vector3D(0,0,0);
	Vector3D Acceleration=new Vector3D(0,0,0);
	public void RunSystemUpdate(double SecondsSinceLastUpdate){
		Status="Updating System Data";
		double elevation=Elevation;
		LastElevation=GenMethods.TryGetBlockData<double>(Controller,"LastElevation",double.Parse,elevation);
		if(!(GenMethods.HasBlockData(Controller,"LastVelocity")&&Vector3D.TryParse(GenMethods.GetBlockData(Controller,"LastVelocity"),out LastVelocity)))
			LastVelocity=Velocity;
		Acceleration=(Velocity-LastVelocity)/SecondsSinceLastUpdate;
		
		
		
		
		GenMethods.SetBlockData(Controller,"LastElevation",Math.Round(Elevation,2).ToString());
		GenMethods.SetBlockData(Controller,"LastVelocity",GenMethods.Round(Velocity,2).ToString());
		SecondsSinceLastRun+=SecondsSinceLastUpdate;
		Status="Ready to Run";
	}
	
	public void RunSystemOperations(){
		Status="Running Operations";
		UpdateFrequency=100;
		
		if(true){
			Write("Thruster Systems");
			ManageThrusters();
			UpdateFrequency=1;
		}
		else
			ResetThrusters();
		
		if(true){
			Write("Gyroscope Systems");
			ManageGyroscopes();
			UpdateFrequency=1;
		}
		else
			Gyroscope.GyroOverride=false;
		
		if(Airlocks.Count>0){
			Write("Airlock Systems");
			foreach(MyAirlock airlock in Airlocks)
				airlock.Update(SecondsSinceLastRun);
		}
		
		
		SecondsSinceLastRun=0;
		Status="Completed Operations";
	}
	
	public void ManageGyroscopes(){
		float gyroCount=0;
		foreach(IMyGyro Gyro in AllGyroscopes){
			if(Gyro.IsWorking)
				gyroCount+=Gyro.GyroPower/100.0f;
		}
		float angularMomentum=(float)(Controller.GetShipVelocities().AngularVelocity.Length());
		float multx=(float)Math.Max(0.1f,Math.Min(1,1.5f/(ShipMass/gyroCount/1000000)))/(Math.Max(1,angularMomentum/1.5f));
		Vector3 input=DampenGyro();
		if(Gravity.Length()>0)
			input+=LevelGyro(5);
		if(Controller.IsUnderControl)
			input=MoveGyro(Controller);
		else if(false){
			//AutoGyro(anoguas);
		}
		
		//SetGyroscopes(input);
	}
	
	Vector3 MoveGyro(IMyShipController controller){
		Vector3 output=new Vector3(0,0,0);
		output.X+=Math.Min(Math.Max(controller.RotationIndicator.X/100,-1),1);
		output.Y+=Math.Min(Math.Max(controller.RotationIndicator.Y/100,-1),1);
		output.Z+=controller.RollIndicator;
		return output;
	}
	
	Vector3 DampenGyro(){
		Vector3D angularVelocity=Controller.GetShipVelocities().AngularVelocity;
		Vector3D relativeAngularVelocity=GenMethods.GlobalToLocal(angularVelocity,Controller);
		return (new Vector3((float)relativeAngularVelocity.X,(float)relativeAngularVelocity.Y,(float)relativeAngularVelocity.Z))*.99f;
	}
	
	Vector3 LevelGyro(float TolerantAngle=1){
		Vector3 output=new Vector3(0,0,0);
		Vector3D gravityDirection=Gravity;
		gravityDirection.Normalize();
		
		double verticalDifference=GenMethods.GetAngle(gravityDirection,ForwardVector)-GenMethods.GetAngle(gravityDirection,BackwardVector);
		if(GenMethods.GetAngle(gravityDirection,ForwardVector)<120&&Math.Abs(verticalDifference)>TolerantAngle)
			output.X-=10*((float)Math.Min(Math.Abs((90-verticalDifference)/90),1));
		
		double horizontalDifference=GenMethods.GetAngle(gravityDirection,LeftVector)-GenMethods.GetAngle(gravityDirection,RightVector);
		if(GenMethods.GetAngle(gravityDirection,ForwardVector)<120&&Math.Abs(horizontalDifference)>TolerantAngle)
			output.Y-=5*((float)Math.Min(Math.Abs((90-horizontalDifference)/90),1));
		
		if(Math.Abs(horizontalDifference)>TolerantAngle)
			output.Z-=10*((float)Math.Min(Math.Abs((90-horizontalDifference)/90),1));
		
		return output;
	}
	
	Vector3 AutoGyro(Vector3D TargetDirection){
		Vector3 output=new Vector3(0,0,0);
		
		
		
		return output;
	}
	
	void SetGyroscopes(Vector3 Input){
		Gyroscope.GyroOverride=true;
		Vector3 global=Vector3D.TransformNormal(Input,Controller.WorldMatrix);
		Vector3 output=Vector3D.TransformNormal(global,MatrixD.Invert(Gyroscope.WorldMatrix));
		output.Normalize();
		output*=Input.Length();
		
		Gyroscope.Pitch=output.X;
		Gyroscope.Yaw=output.Y;
		Gyroscope.Roll=output.Z;
	}
	
	public void ManageThrusters(){
		Vector3 input=DampenThrust();
		var dampen=new Vector3(input.X,input.Y,input.Z);
		
		if(Gravity.Length()>0){
			if(!Landed())
				input+=HoverThrust();
			else if(GenMethods.TryGetBlockData<bool>(Controller,"LandingHoverOverride",bool.Parse,false)){
				if(Velocity.Length()>0.5)
					input+=HoverThrust();
				else if(Velocity.Length()<0.1)
					GenMethods.SetBlockData(Controller,"LandingHoverOverride",false.ToString());
			}
			else if(Velocity.Length()>5){ 
				Vector3D gravityDirection=Gravity;
				gravityDirection.Normalize();
				Vector3D accelerationDirection=Acceleration;
				accelerationDirection.Normalize();
				if(GenMethods.GetAngle(gravityDirection,accelerationDirection)<5&&Acceleration.Length()>Gravity.Length()/2){
					input+=HoverThrust();
					GenMethods.SetBlockData(Controller,"LandingHoverOverride",true.ToString());
				}
			}
		}
		
		var hover=input-dampen;
		Write("1.2Gs: "+Math.Round(1.2*9.81,2).ToString());
		Write("Gravity: "+Math.Round(Gravity.Length(),2).ToString());
		Write("Hover.Length: "+Math.Round(hover.Length()/ShipMass,2).ToString());
		Write("Hover.X: "+Math.Round(hover.X/ShipMass,2).ToString());
		Write("Hover.Y: "+Math.Round(hover.Y/ShipMass,2).ToString());
		Write("Hover.Z: "+Math.Round(hover.Z/ShipMass,2).ToString());
		
		Vector3 moveInput=new Vector3(0,0,0);
		if(Controller.IsUnderControl){
			moveInput=MoveThrust(Controller);
		}
		else {
			foreach(var altController in AltControllers){
				if(altController.IsUnderControl){
					moveInput=MoveThrust(altController);
					if(moveInput.Length()>0.2)
						break;
				}
			}
		}
		
		if(moveInput.Length()>0.2){
			if(Math.Abs(moveInput.X)>0.2f)
				input.X=moveInput.X;
			if(Math.Abs(moveInput.Y)>0.2f)
				input.Y=moveInput.Y;
			if(Math.Abs(moveInput.Z)>0.2f)
				input.Z=moveInput.Z;
		}
		else if(false){
			//input=AutoThrust();
		}
		
		SetThrusters(input);
	}
	
	Vector3 MoveThrust(IMyShipController controller){
		Vector3 output=new Vector3(0,0,0);
		Vector3 input=controller.MoveIndicator;
		Write(controller.MoveIndicator.ToString());
		float multx=(float)(GetDeviationMultx()*0.95f*ShipMass);
		float effectiveSpeedLimit=GetEffectiveSpeedLimit();
		if(Math.Abs(input.X)>0.2f){
			if(input.X<0){
				if((Velocity+RightAcc-DampenVelocity).Length()<effectiveSpeedLimit)
					output.X-=RightThrust*multx;
			}
			else{
				if((Velocity+LeftAcc-DampenVelocity).Length()<effectiveSpeedLimit)
					output.X+=LeftThrust*multx;
			}
		}
		if(Math.Abs(input.Y)>0.2f){
			if(input.Y>0){
				if((Velocity+UpAcc-DampenVelocity).Length()<effectiveSpeedLimit)
					output.Y+=UpThrust*multx;
			}
			else{
				if((Velocity+DownAcc-DampenVelocity).Length()<effectiveSpeedLimit)
					output.Y-=DownThrust*multx;
			}
		}
		if(Math.Abs(input.Z)>0.2f){
			if(input.Z<0){
				if((Velocity+ForwardAcc-DampenVelocity).Length()<effectiveSpeedLimit)
					output.Z+=ForwardThrust*multx;
			}
			else{
				if((Velocity+BackwardAcc-DampenVelocity).Length()<effectiveSpeedLimit)
					output.Z-=BackwardThrust*multx;
			}
		}
		return output;
	}
	
	Vector3 DampenThrust(){
		Vector3 output=new Vector3(0,0,0);
		Vector3D relative=Relative(Velocity-DampenVelocity);
		float dampMultx=(float) Math.Max(0.75f,(100-Math.Pow(ShipCount,2))/100f);
		// relative is in m/s. ShipMass is kg. Looking for Force (kg-ms/s^2). We will assume that we want to stop within 1 second.
		output.X-=(float)(relative.X*ShipMass*dampMultx);
		output.Y-=(float)(relative.Y*ShipMass*dampMultx);
		output.Z+=(float)(relative.Z*ShipMass*dampMultx);
		return output;
	}
	
	Vector3 HoverThrust(){
		Vector3 output=new Vector3(0,0,0);
		if(Gravity.Length()>0){
			Write("Gravity.Length(): "+Gravity.Length());
			Vector3D relativeGravity=GenMethods.GlobalToLocal(Gravity,Controller);
			relativeGravity.Normalize();
			// relative is a directional vector. MassAccomodation is Force (kg-ms/s^2)
			output.X-=(float)relativeGravity.X*MassAccomodation;
			output.Y-=(float)relativeGravity.Y*MassAccomodation;
			output.Z+=(float)relativeGravity.Z*MassAccomodation;
		}
		return output;
	}
	
	Vector3 AutoThrust(Vector3D TargetPosition){
		return AutoThrust(TargetPosition,(TargetPosition-Position).Length());
	}
	
	void SetThrusters(Vector3 Input){
		double effectiveSpeedLimit=GetEffectiveSpeedLimit();
		Input.X=SmoothSpeed("X",Input.X,effectiveSpeedLimit);
		Input.Y=SmoothSpeed("Y",Input.Y,effectiveSpeedLimit);
		Input.Z=SmoothSpeed("Z",Input.Z,effectiveSpeedLimit);
		
		float outputForward=0;
		float outputBackward=0;
		float outputUp=0;
		float outputDown=0;
		float outputRight=0;
		float outputLeft=0;
		
		if(Input.X/RightThrust>0.01f)
			outputRight=Math.Min(Math.Abs(Input.X/RightThrust),1);
		else if(Input.X/LeftThrust<-0.01f)
			outputLeft=Math.Min(Math.Abs(Input.X/LeftThrust),1);
		
		if(Input.Y/UpThrust>0.01f)
			outputUp=Math.Min(Math.Abs(Input.Y/UpThrust),1);
		else if(Input.Y/DownThrust<-0.01f)
			outputDown=Math.Min(Math.Abs(Input.Y/DownThrust),1);
		
		if(Input.Z/ForwardThrust>0.01f)
			outputForward=Math.Min(Math.Abs(Input.Z/ForwardThrust),1);
		else if(Input.Z/BackwardThrust<-0.01f)
			outputBackward=Math.Min(Math.Abs(Input.Z/BackwardThrust),1);
		
		const float MIN_THRUST=0.0001f;
		
		foreach(IMyThrust thrust in ForwardThrusters){
			if(outputForward<=0)
				thrust.ThrustOverride=MIN_THRUST;
			else
				thrust.ThrustOverridePercentage=outputForward;
		}
		foreach(IMyThrust thrust in BackwardThrusters){
			if(outputBackward<=0)
				thrust.ThrustOverride=MIN_THRUST;
			else
				thrust.ThrustOverridePercentage=outputBackward;
		}
		foreach(IMyThrust thrust in UpThrusters){
			if(outputUp<=0)
				thrust.ThrustOverride=MIN_THRUST;
			else
				thrust.ThrustOverridePercentage=outputUp;
		}
		foreach(IMyThrust thrust in DownThrusters){
			if(outputDown<=0)
				thrust.ThrustOverride=MIN_THRUST;
			else
				thrust.ThrustOverridePercentage=outputDown;
		}
		foreach(IMyThrust thrust in RightThrusters){
			if(outputRight<=0)
				thrust.ThrustOverride=MIN_THRUST;
			else
				thrust.ThrustOverridePercentage=outputRight;
		}
		foreach(IMyThrust thrust in LeftThrusters){
			if(outputLeft<=0)
				thrust.ThrustOverride=MIN_THRUST;
			else
				thrust.ThrustOverridePercentage=outputLeft;
		}
		
	}
	
	float GetEffectiveSpeedLimit(){
		float output=100;
		double elevationDeviation=Math.Max(0,Math.Min(20,Grid.GridSize/4))+10;
		if(Elevation<200+elevationDeviation)
			output=(float)Math.Min(output,Math.Sqrt(Math.Max(Elevation-elevationDeviation,0)/200)*100);
		output=Math.Max(output,5);
		return output;
	}
	
	float GetDeviationMultx(){
		float output=1;
		float effectiveSpeedLimit=GetEffectiveSpeedLimit();
		if(Math.Abs(effectiveSpeedLimit-SpeedDeviation)<5)
			output=(float)Math.Sqrt(1-Math.Max(Math.Abs(effectiveSpeedLimit-SpeedDeviation),0.1)/5);
		return output;
	}
	
	double SpeedLimitByDistance(double Distance){
		Distance=Math.Abs(Distance);
		if(Distance<0.5)
			return 4*Distance;
		if(Distance<1.5)
			return 2;
		if(Distance<2.5)
			return 2.5;
		if(Distance<5)
			return Distance;
		if(Distance<25)
			return 10;
		if(Distance<50)
			return 20;
		if(Distance<100)
			return 25;
		if(Distance<250)
			return 40;
		if(Distance<500)
			return 50;
		return Distance/10;
	}
	float MatchThrust(double EffectiveSpeedLimit,double RelativeSpeed,double RelativeTargetSpeed,double RelativeDistance,float T1,float T2,Vector3D V1,Vector3D V2,float RelativeGravity){
		double relativeEffectiveSpeedLimit=Math.Min(Elevation,Math.Min(EffectiveSpeedLimit,SpeedLimitByDistance(RelativeDistance)));
		float distanceMultx=1.0f;
		double targetSpeed=0;
		double speedChange=RelativeSpeed-RelativeTargetSpeed;
		double deacceleration=Math.Abs(speedChange*Controller.CalculateShipMass().PhysicalMass);
		double time=0;
		//difference is required change in velocity; must be "0" when the target is reached
		if(speedChange>0)
			time=deacceleration/(T1-RelativeGravity);
		else if(speedChange<0)
			time=deacceleration/(T2+RelativeGravity);
		//deacceleration is the required change in force; divided by the thruster power, this is now the ammount of time required to make that change
		double acceleration_distance=(Math.Abs(RelativeSpeed)+targetSpeed)*time/2;
		//acceleration_distance is the distance that will be covered during that change in speed
		bool increase=acceleration_distance<Math.Abs(RelativeDistance)*1.05+(Grid.GridSize/2)+5;
		//increase determines whether to accelerate or deaccelerate, based on whether the acceleration distance is smaller than the distance to the target (with some wiggle room);
		if(!increase){
			distanceMultx*=-1;
		}
		if(RelativeDistance<RelativeTargetSpeed){
			if((Velocity+V1-DampenVelocity).Length()<=relativeEffectiveSpeedLimit)
				return -0.95f*T1*distanceMultx;
		}
		else if(RelativeDistance>RelativeTargetSpeed){
			if((Velocity+V2-DampenVelocity).Length()<=relativeEffectiveSpeedLimit)
				return 0.95f*T2*distanceMultx;
		}
		return 0;
	}
	
	Vector3 AutoThrust(Vector3D TargetPosition,double TargetDistance){
		Vector3 output=new Vector3(0,0,0);
		Vector3D relativeTargetPosition=GenMethods.GlobalToLocalPosition(TargetPosition-Position,Controller);
		float effectiveSpeedLimit=GetEffectiveSpeedLimit();
		Vector3D relative=Relative(Velocity-DampenVelocity);
		Vector3D relativeGravity=GenMethods.GlobalToLocal(Gravity,Controller);
		relativeGravity.Normalize();
		relativeGravity*=Gravity.Length();
		
		float multx=GetDeviationMultx();
		
		float thrustValue=MatchThrust(effectiveSpeedLimit,relative.X,DampenVelocity.X,relativeTargetPosition.X,LeftThrust,RightThrust,LeftVector,RightVector,-1*(float)relativeGravity.X);
		if(Math.Abs(thrustValue)>=1)
			output.X-=thrustValue*multx-(float)relativeGravity.X;
		
		thrustValue=MatchThrust(effectiveSpeedLimit,relative.Y,DampenVelocity.Y,relativeTargetPosition.Y,DownThrust,UpThrust,DownVector,UpVector,-1*(float)relativeGravity.Y);
		if(Math.Abs(thrustValue)>=1)
			output.Y+=thrustValue*multx-(float)relativeGravity.Y;
		
		thrustValue=-1*MatchThrust(effectiveSpeedLimit,relative.Z,DampenVelocity.Z,relativeTargetPosition.Z,ForwardThrust,BackwardThrust,ForwardVector,BackwardVector,(float)relativeGravity.Z);
		if(Math.Abs(thrustValue)>=1)
			output.Z+=thrustValue*multx+(float)relativeGravity.Z;
		
		return output;
	}
	
	float SmoothSpeed(string DirectionString,float Input,double EffectiveSpeedLimit){
		double timer=GenMethods.TryGetBlockData<double>(Controller,"OutputTimer-"+DirectionString,double.Parse,0);
		timer=Math.Max(0,timer-SecondsSinceLastRun);
		float output=GenMethods.TryGetBlockData<float>(Controller,"SmootOverride-"+DirectionString,float.Parse,Input);
		if(output.ToString().Equals("NaN"))
			output=Input;
		if(timer<=0){
			output=(2*output+Input)/3;
			timer=0.2;
		}
		GenMethods.SetBlockData(Controller,"OutputTimer-"+DirectionString,Math.Round(timer,3).ToString());
		GenMethods.SetBlockData(Controller,"SmoothOverride-"+DirectionString,output.ToString());
		if(SpeedDeviation>0.5&&Input>0&&EffectiveSpeedLimit-SpeedDeviation<2.5){
			return (output*99+Input)/100;
		}
		return Input;
	}
	
	public void ResetThrusters(){
		if(GenMethods.HasBlockData(Controller,"SmoothOverride-X"))
			GenMethods.WipeBlockData(Controller,"SmoothOverride-X");
		if(GenMethods.HasBlockData(Controller,"SmoothOverride-Y"))
			GenMethods.WipeBlockData(Controller,"SmoothOverride-Y");
		if(GenMethods.HasBlockData(Controller,"SmoothOverride-Z"))
			GenMethods.WipeBlockData(Controller,"SmoothOverride-Z");
		for(int i=0;i<6;i++){
			foreach(IMyThrust thrust in AllThrusters[i]){
				if(thrust.ThrustOverride>0)
					thrust.ThrustOverride=0;
			}
		}
	}
	
	
}

public struct MyDoorSet{
	public List<IMyDoor> Doors;
	public double LastOpened{
		get{
			double output=double.MaxValue;
			foreach(IMyDoor door in Doors){
				double lastOpened=(door.Status==DoorStatus.Open?0:10);
				if(GenMethods.HasBlockData(door,"LastOpened"))
					double.TryParse(GenMethods.GetBlockData(door,"LastOpened"),out lastOpened);
				output=Math.Min(output,lastOpened);
			}
			return output;
		}
	}
	public bool Opened{
		get{
			return LastOpened<=0.2;
		}
	}
	public double LastClosed{
		get{
			double output=double.MaxValue;
			foreach(IMyDoor door in Doors){
				double lastClosed=(door.Status==DoorStatus.Closed?0:10);
				if(GenMethods.HasBlockData(door,"LastClosed"))
					double.TryParse(GenMethods.GetBlockData(door,"LastClosed"),out lastClosed);
				output=Math.Min(output,lastClosed);
			}
			return output;
		}
	}
	public bool FullyClosed{
		get{
			return LastOpened>0.5&&LastClosed<=0.2&&Status==DoorStatus.Closed;
		}
	}
	
	public DoorStatus Status{
		get{
			short status=4;
			foreach(IMyDoor door in Doors)
				status=Math.Min(status,(short)door.Status);
			return (DoorStatus)status;
		}
	}
	
	public MyDoorSet(List<IMyDoor> doors){
		Doors=new List<IMyDoor>();
		foreach(IMyDoor door in doors){
			Doors.Add(door);
			if(!GenMethods.HasBlockData(door,"LastOpened"))
				GenMethods.SetBlockData(door,"LastOpened",(door.Status==DoorStatus.Open?0:10).ToString());
			if(!GenMethods.HasBlockData(door,"LastClosed"))
				GenMethods.SetBlockData(door,"LastClosed",(door.Status==DoorStatus.Closed?0:10).ToString());
		}
	}
	
	public MyDoorSet(IMyDoor door){
		Doors=new List<IMyDoor>();
		Doors.Add(door);
		if(!GenMethods.HasBlockData(door,"LastOpened"))
			GenMethods.SetBlockData(door,"LastOpened",(door.Status==DoorStatus.Open?0:10).ToString());
		if(!GenMethods.HasBlockData(door,"LastClosed"))
			GenMethods.SetBlockData(door,"LastClosed",(door.Status==DoorStatus.Closed?0:10).ToString());
	}
	
	public bool Update(double seconds){
		bool output=false;
		for(int i=0;i<Doors.Count;i++){
			IMyDoor door=Doors[i];
			double lastOpened=(door.Status==DoorStatus.Open?0:10);
			double lastClosed=(door.Status==DoorStatus.Closed?0:10);
			if(door.Status!=DoorStatus.Closed){
				output=true;
				GenMethods.SetBlockData(door,"LastOpened","0");
				if(GenMethods.HasBlockData(door,"LastClosed"))
					double.TryParse(GenMethods.GetBlockData(door,"LastClosed"),out lastClosed);
				if(lastClosed<10){
					lastClosed=Math.Min(lastClosed+seconds,10);
					GenMethods.SetBlockData(door,"LastClosed",Math.Round(lastClosed,3).ToString());
				}
				if(lastClosed+seconds>=10){
					door.CloseDoor();
				}
			}
			else{
				GenMethods.SetBlockData(door,"LastClosed","0");
				if(GenMethods.HasBlockData(door,"LastOpened"))
					double.TryParse(GenMethods.GetBlockData(door,"LastOpened"),out lastOpened);
				if(lastOpened<10){
					output=true;
					lastOpened=Math.Min(lastOpened+seconds,10);
					GenMethods.SetBlockData(door,"LastOpened",Math.Round(lastOpened,3).ToString());
				}
			}
		}
		return output;
	}
	
	public void Lock(){
		foreach(IMyDoor door in Doors){
			if(door.Status!=DoorStatus.Closed){
				door.Enabled=true;
				door.CloseDoor();
			}
			else{
				double lastOpened=0;
				if(!double.TryParse(GenMethods.GetBlockData(door,"LastOpened"),out lastOpened))
					GenMethods.SetBlockData(door,"LastOpened","0");
				if(lastOpened>=0.5)
					door.Enabled=false;
			}
		}
	}
	
	public void Unlock(){
		foreach(IMyDoor door in Doors)
			door.Enabled=true;
	}
	
	public void Open(){
		foreach(IMyDoor Door in Doors){
			if(Door.Status!=DoorStatus.Open){
				Door.Enabled=true;
				Door.OpenDoor();
			}
		}
	}
	
	public void Close(){
		foreach(IMyDoor Door in Doors){
			if(Door.Status!=DoorStatus.Closed){
				Door.Enabled=true;
				Door.CloseDoor();
			}
		}
	}
	
}
public class MyAirlock{
	public string Name;
	public List<MyDoorSet> Sets;
	
	public DoorStatus Status{
		get{
			short status=4;
			foreach(MyDoorSet set in Sets)
				status=Math.Min(status,(short)(set.Status));
			return (DoorStatus)status;
		}
	}
	
	private MyAirlock(string name){
		Name=name;
		Sets=new List<MyDoorSet>();
	}
	
	public MyAirlock(string name,List<IMyDoor> inner,List<IMyDoor> outer):this(name){
		Sets.Add(new MyDoorSet(inner));
		Sets.Add(new MyDoorSet(outer));
		foreach(IMyDoor door in inner)
			GenMethods.SetBlockData(door,"Airlock",name);
		foreach(IMyDoor door in outer)
			GenMethods.SetBlockData(door,"Airlock",name);
	}
	
	public MyAirlock(string name,IMyDoor inner,IMyDoor outer):this(name){
		Sets.Add(new MyDoorSet(inner));
		Sets.Add(new MyDoorSet(outer));
		GenMethods.SetBlockData(inner,"Airlock",name);
		GenMethods.SetBlockData(outer,"Airlock",name);
	}
	
	public MyAirlock(string name,List<List<IMyDoor>> sets):this(name){
		foreach(List<IMyDoor> set in sets){
			if(set.Count>0){
				Sets.Add(new MyDoorSet(set));
				foreach(IMyDoor door in set){
					GenMethods.SetBlockData(door,"Airlock",name);
				}
			}
		}
	}
	
	public bool Update(double seconds){
		bool output=false;
		for(int i=0;i<Sets.Count;i++){
			if(Sets[i].Update(seconds))
				output=true;
		}
		
		List<bool> closed=new List<bool>();
		int notClosed=0;
		double lastOpened=Sets[0].LastOpened;
		foreach(MyDoorSet set in Sets){
			lastOpened=Math.Min(lastOpened,set.LastOpened);
			if(!set.FullyClosed){
				notClosed++;
			}
		}
		if(notClosed==0){
			if(lastOpened>2){
				foreach(MyDoorSet set in Sets)
					set.Unlock();
			}
		}
		else if(notClosed>1){
			foreach(MyDoorSet set in Sets)
				set.Lock();
		}
		else{
			foreach(MyDoorSet set in Sets){
				if(set.FullyClosed){
					set.Lock();
				}
				else if(set.LastClosed>5)
					set.Close();
				else
					set.Unlock();
			}
		}
		return output;
	}
	
}

// Saving and Data Storage Classes
public void Save(){
    // Reset Blocks
	foreach(MyShip ship in Ships){
		ship.Gyroscope.GyroOverride=false;
		ship.ResetThrusters();
	}
	
	// Reset Storage
	this.Storage="";
	
	// Save Data to Storage
	this.Storage+="\n"+"SampleData";
	this.Storage+="\n"+"lorem ipsum";
	
	// Update runtime variables from CustomData
	
	
	// Reset CustomData
	Me.CustomData="";
	
	// Save Runtime Data to CustomData
	
	
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
long ShipCycle=1;
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
		SortHelper(output,Ref,0,output.Count-1);
		return output;
	}
	
	private static void Swap(List<T> list,int i1,int i2){
		T temp=list[i1];
		list[i1]=list[i2];
		list[i2]=temp;
	}
	
	private static int SortPartition(List<T> sorting,Vector3D Ref,int low,int high){
		double pivot=(sorting[high].GetPosition()-Ref).Length();
		int i=low-1;
		for(int j=low;j<high;j++){
			if((sorting[j].GetPosition()-Ref).Length()<pivot)
				Swap(sorting,j,++i);
		}
		Swap(sorting,high,++i);
		return i;
	}
	
	private static void SortHelper(List<T> sorting,Vector3D Ref,int low,int high){
		if(low>=high)
			return;
		int pi=SortPartition(sorting,Ref,low,high);
		SortHelper(sorting,Ref,low,pi-1);
		SortHelper(sorting,Ref,pi+1,high);
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
public class Roo<T>{
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
	
	public static implicit operator T(Roo<T> R){
		return R.Value;
	}
}
class Roo<U,T>{
	// Run Only Once
	private T _Value;
	public T Value{
		get{
			if(!Ran.Value){
				_Value=Updater(UpdateValue);
				Ran.Value=true;
			}
			return _Value;
		}
	}
	private OneDone<bool> Ran;
	private Func<U,T> Updater;
	private U UpdateValue;
	
	public Roo(Func<U,T> updater,U updateValue){
		Ran=new OneDone<bool>(false);
		Updater=updater;
		UpdateValue=updateValue;
	}
	
	public static implicit operator T(Roo<U,T> R){
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
	Echo(ProgramName+" OS\nCycle-"+Cycle.ToString()+"-"+ShipCycle.ToString()+" ("+LoadingChar+")");
	Me.GetSurface(1).WriteText(ProgramName+" OS\nCycle-"+Cycle.ToString()+" ("+LoadingChar+")",false);
	Me.GetSurface(1).WriteText("\nCurrent Ship: "+(CurrentShip+1).ToString()+"/"+Ships.Count.ToString(),true);
	SecondsSinceLastUpdate=Runtime.TimeSinceLastRun.TotalSeconds+(Runtime.LastRunTimeMs/1000);
	Echo(ToString(FromSeconds(SecondsSinceLastUpdate))+" since last Cycle");
	TimeSinceStart=UpdateTimeSpan(TimeSinceStart,SecondsSinceLastUpdate);
	Echo(ToString(TimeSinceStart)+" since last reboot\n");
	Me.GetSurface(1).WriteText("\n"+ToString(TimeSinceStart)+" since last reboot",true);
}

