/*
* Battleship --- Vigilance Targeting System 
* Built by mlq1616
* https://github.com/mlq1819
*/
//Name me!
const string Program_Name="Battleship Vigilance AI";
//Sets the maximum firing distance
//Set to the maximum time you expect the cannon to take to aim and print
const double AIM_TIME=15;
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);

Color Green_Text=new Color(137,255,137,255);
Color Green_Back=new Color(0,151,0,255);
Color Blue_Text=new Color(137,239,255,255);
Color Blue_Back=new Color(0,88,151,255);
Color Yellow_Text=new Color(255,239,137,255);
Color Yellow_Back=new Color(66,66,0,255);
Color Orange_Text=new Color(255,197,0,255);
Color Orange_Back=new Color(88,44,0,255);
Color Red_Text=new Color(255,137,137,255);
Color Red_Back=new Color(151,0,0,255);

class Prog{
	public static MyGridProgram P;
	public static TimeSpan FromSeconds(double seconds){
		return (new TimeSpan(0,0,0,(int)seconds,(int)(seconds*1000)%1000));
	}

	public static TimeSpan UpdateTimeSpan(TimeSpan old,double seconds){
		return old+FromSeconds(seconds);
	}
}
class GenericMethods<T> where T : class, IMyTerminalBlock{
	static IMyGridTerminalSystem TerminalSystem{
		get{
			return P.GridTerminalSystem;
		}
	}
	public static MyGridProgram P{
		get{
			return Prog.P;
		}
	}
	
	public static T GetFull(string name,Vector3D Ref,double mx_d=double.MaxValue){
		List<T> AllBlocks=new List<T>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		double min_distance=mx_d;
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Equals(name)){
				double distance=(Ref-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance, distance);
				MyBlocks.Add(Block);
			}
		}
		foreach(T Block in MyBlocks){
			double distance=(Ref-Block.GetPosition()).Length();
			if(distance<=min_distance+0.1)
				return Block;
		}
		return null;
	}
	
	public static T GetFull(string name,IMyTerminalBlock Ref,double mx_d=double.MaxValue){
		return GetFull(name,Ref.GetPosition(),mx_d);
	}
	
	public static T GetFull(string name,double mx_d=double.MaxValue){
		return GetFull(name,P.Me,mx_d);
	}
	
	public static T GetConstruct(string name,IMyTerminalBlock Ref,double mx_d=double.MaxValue){
		List<T> input=GetAllConstruct(name,Ref,mx_d);
		if(input.Count>0)
			return input[0];
		return null;
	}
	
	public static T GetConstruct(string name,double mx_d=double.MaxValue){
		return GetConstruct(name,P.Me,mx_d);
	}
	
	public static List<T> GetAllConstruct(string name,IMyTerminalBlock Ref,double mx_d=double.MaxValue){
		List<T> input=GetAllContaining(name,Ref,mx_d);
		List<T> output=new List<T>();
		foreach(T Block in input){
			if(Ref.IsSameConstructAs(Block))
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> GetAllConstruct(string name){
		return GetAllConstruct(name,P.Me);
	}
	
	public static T GetContaining(string name,Vector3D Ref,double mx_d){
		List<T> AllBlocks=new List<T>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		double min_distance=mx_d;
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Contains(name)){
				double distance=(Ref-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance,distance);
				MyBlocks.Add(Block);
			}
		}
		foreach(T Block in MyBlocks){
			double distance=(Ref-Block.GetPosition()).Length();
			if(distance<=min_distance+0.1)
				return Block;
		}
		return null;
	}
	
	public static T GetContaining(string name,IMyTerminalBlock Ref,double mx_d){
		return GetContaining(name,Ref.GetPosition(),mx_d);
	}
	
	public static T GetContaining(string name,double mx_d=double.MaxValue){
		return GetContaining(name,P.Me,mx_d);
	}
	
	public static List<T> GetAllContaining(string name,Vector3D Ref,double mx_d){
		List<T> AllBlocks=new List<T>();
		List<List<T>> MyLists=new List<List<T>>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Contains(name)){
				bool has_with_name=false;
				for(int i=0;i<MyLists.Count&&!has_with_name;i++){
					if(Block.CustomName.Equals(MyLists[i][0].CustomName)){
						MyLists[i].Add(Block);
						has_with_name=true;
						break;
					}
				}
				if(!has_with_name){
					List<T> new_list=new List<T>();
					new_list.Add(Block);
					MyLists.Add(new_list);
				}
			}
		}
		foreach(List<T> list in MyLists){
			if(list.Count==1){
				MyBlocks.Add(list[0]);
				continue;
			}
			double min_distance=mx_d;
			foreach(T Block in list){
				double distance=(Ref-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance, distance);
			}
			foreach(T Block in list){
				double distance=(Ref-Block.GetPosition()).Length();
				if(distance<=min_distance+0.1){
					MyBlocks.Add(Block);
					break;
				}
			}
		}
		return MyBlocks;
	}
	
	public static List<T> GetAllIncluding(string name,Vector3D Ref,double mx_d=double.MaxValue){
		List<T> AllBlocks=new List<T>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			double distance=(Ref-Block.GetPosition()).Length();
			if(Block.CustomName.Contains(name)&&distance<=mx_d)
				MyBlocks.Add(Block);
		}
		return MyBlocks;
	}
	
	public static List<T> GetAllIncluding(string name,IMyTerminalBlock Ref,double mx_d=double.MaxValue){
		return GetAllIncluding(name,Ref.GetPosition(),mx_d);
	}
	
	public static List<T> GetAllIncluding(string name,double mx_d=double.MaxValue){
		return GetAllIncluding(name,P.Me,mx_d);
	}
	
	public static List<T> GetAllContaining(string name,IMyTerminalBlock Ref,double mx_d=double.MaxValue){
		return GetAllContaining(name,Ref.GetPosition(),mx_d);
	}
	
	public static List<T> GetAllContaining(string name,double mx_d=double.MaxValue){
		return GetAllContaining(name,P.Me,mx_d);
	}
	
	public static List<T> GetAllFunc(Func<T,bool> f){
		List<T> AllBlocks=new List<T>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			if(f(Block))
				MyBlocks.Add(Block);
		}
		return MyBlocks;
	}
	
	public static T GetClosestFunc(Func<T,bool> f,Vector3D Ref,double mx_d=double.MaxValue){
		List<T> MyBlocks=GetAllFunc(f);
		double min_distance=mx_d;
		foreach(T Block in MyBlocks){
			double distance=(Ref-Block.GetPosition()).Length();
			min_distance=Math.Min(min_distance,distance);
		}
		foreach(T Block in MyBlocks){
			double distance=(Ref-Block.GetPosition()).Length();
			if(distance<=min_distance+0.1)
				return Block;
		}
		return null;
	}
	
	public static T GetClosestFunc(Func<T,bool> f,IMyTerminalBlock Ref,double mx_d=double.MaxValue){
		return GetClosestFunc(f,Ref.GetPosition(),mx_d);
	}
	
	public static T GetClosestFunc(Func<T,bool> f,double mx_d=double.MaxValue){
		return GetClosestFunc(f,P.Me,mx_d);
	}
	
	public static T GetGrid(string name,IMyCubeGrid Grid,IMyTerminalBlock Ref,double mx_d=double.MaxValue){
		List<T> input=GetAllGrid(name,Grid,Ref,mx_d);
		if(input.Count>0)
			return input[0];
		return null;
	}
	
	public static T GetGrid(string name,IMyCubeGrid Grid,double mx_d=double.MaxValue){
		return GetGrid(name,Grid,P.Me,mx_d);
	}
	
	public static List<T> GetAllGrid(string name,IMyCubeGrid Grid,IMyTerminalBlock Ref,double mx_d){
		List<T> output=new List<T>();
		List<T> input=GetAllContaining(name,Ref,mx_d);
		foreach(T Block in input){
			if(Block.CubeGrid==Grid)
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> GetAllGrid(string name,IMyCubeGrid Grid,double mx_d=double.MaxValue){
		return GetAllGrid(name,Grid,P.Me,mx_d);
	}
	
	public static List<T> SortByDistance(List<T> unsorted,Vector3D Ref){
		List<T> output=new List<T>();
		while(unsorted.Count>0){
			double min_distance=double.MaxValue;
			foreach(T Block in unsorted){
				double distance=(Ref-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance,distance);
			}
			for(int i=0; i<unsorted.Count; i++){
				double distance=(Ref-unsorted[i].GetPosition()).Length();
				if(distance<=min_distance+0.1){
					output.Add(unsorted[i]);
					unsorted.RemoveAt(i);
					break;
				}
			}
		}
		return output;
	}
	
	public static List<T> SortByDistance(List<T> unsorted,IMyTerminalBlock Ref){
		return SortByDistance(unsorted, Ref.GetPosition());
	}
	
	public static List<T> SortByDistance(List<T> unsorted){
		return SortByDistance(unsorted,P.Me);
	}
	
	public static double GetAngle(Vector3D v1, Vector3D v2){
		v1.Normalize();
		v2.Normalize();
		return Math.Round(Math.Acos(v1.X*v2.X + v1.Y*v2.Y + v1.Z*v2.Z)*57.295755,5);
	}
}

TimeSpan FromSeconds(double seconds){
	return Prog.FromSeconds(seconds);
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

void Write(string text, bool new_line=true, bool append=true){
	Echo(text);
	if(new_line){
		Me.GetSurface(0).WriteText(text+'\n', append);
		foreach(IMyTextPanel Panel in StatusPanels)
			Panel.WriteText(text+'\n', append);
	}
	else{
		Me.GetSurface(0).WriteText(text, append);
		foreach(IMyTextPanel Panel in StatusPanels)
			Panel.WriteText(text, append);
	}
}

bool HasBlockData(IMyTerminalBlock Block, string Name){
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

string GetBlockData(IMyTerminalBlock Block, string Name){
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

bool SetBlockData(IMyTerminalBlock Block, string Name, string Data){
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

enum CannonTask{
	None=0,
	Scan=1,
	Reset=2,
	Search=3,
	Fire=4,
	Manual=5
}

enum FireStatus{
	Idle=0,
	Printing=1,
	Aiming=2,
	WaitingClear=3,
	WaitingTarget=4,
	Firing=5
}

enum PrintStatus{
	StartingPrint=0,
	Printing=1,
	WaitingResources=2,
	EndingPrint=3,
	Ready=4
}

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;
Random Rnd;

IMyRemoteControl Controller;
IMyProjector Projector;
IMyMotorStator YawRotor;
IMyMotorStator PitchRotor;
IMyMotorStator ShellRotor;
IMyShipWelder Welder;
IMySensorBlock Sensor;
IMyShipMergeBlock Merge;
List<IMyGravityGenerator> Generators;


List<IMyInteriorLight> FiringLights;
List<List<IMyInteriorLight>> StatusLights;
List<IMyTextPanel> StatusPanels;

Vector3D Target_Position;

Vector3D Aim_Position=new Vector3D(0,0,0);
Vector3D Aim_Direction{
	get{
		Vector3D output=Aim_Position-Controller.GetPosition();
		output.Normalize();
		return output;
	}
}
double Aim_Distance{
	get{
		return (Aim_Position-Controller.GetPosition()).Length();
	}
}

double Time_To_Hit{
	get{
		double distance=20;
		return (Aim_Distance-distance+distance*1.5657)/104.38;
	}
}
double Time_To_Position{
	get{
		return 0;
	}
}

double Precision{
	get{
		return Math.Min(1,50/Aim_Distance);
	}
}

int Fire_Count=0;

Vector3D Forward_Vector;
Vector3D Backward_Vector{
	get{
		return -1*Forward_Vector;
	}
}
Vector3D Up_Vector;
Vector3D Down_Vector{
	get{
		return -1*Up_Vector;
	}
}
Vector3D Left_Vector;
Vector3D Right_Vector{
	get{
		return -1*Left_Vector;
	}
}

Queue<CannonTask> TaskQueue=new Queue<CannonTask>();
CannonTask CurrentTask{
	get{
		if(Controller.IsUnderControl)
			return CannonTask.Manual;
		if(TaskQueue.Count==0)
			return CannonTask.None;
		return TaskQueue.Peek();
	}
}

PrintStatus CurrentPrintStatus;
FireStatus CurrentFireStatus;

Vector3D GlobalToLocal(Vector3D Global,IMyCubeBlock Ref){
	Vector3D Local=Vector3D.Transform(Global+Ref.GetPosition(), MatrixD.Invert(Ref.WorldMatrix));
	Local.Normalize();
	return Local*Global.Length();
}
Vector3D GlobalToLocalPosition(Vector3D Global,IMyCubeBlock Ref){
	Vector3D Local=Vector3D.Transform(Global, MatrixD.Invert(Ref.WorldMatrix));
	Local.Normalize();
	return Local*(Global-Ref.GetPosition()).Length();
}
Vector3D LocalToGlobal(Vector3D Local,IMyCubeBlock Ref){
	Vector3D Global=Vector3D.Transform(Local, Ref.WorldMatrix)-Ref.GetPosition();
	Global.Normalize();
	return Global*Local.Length();
}
Vector3D LocalToGlobalPosition(Vector3D Local,IMyCubeBlock Ref){
	return Vector3D.Transform(Local,Ref.WorldMatrix);
}

double GetAngle(Vector3D v1,Vector3D v2){
	return GenericMethods<IMyTerminalBlock>.GetAngle(v1,v2);
}

bool Operational=false;
public Program(){
	Rnd=new Random();
	Prog.P=this;
    Me.CustomName=(Program_Name+" Programmable block").Trim();
	for(int i=0;i<Me.SurfaceCount;i++){
		Me.GetSurface(i).FontColor=DEFAULT_TEXT_COLOR;
		Me.GetSurface(i).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		Me.GetSurface(i).Alignment=TextAlignment.CENTER;
		Me.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
	}
	Me.GetSurface(1).FontSize=2.2f;
	Me.GetSurface(1).TextPadding=40.0f;
	Echo("Beginning initialization");
	Me.Enabled=true;
	StatusPanels=GenericMethods<IMyTextPanel>.GetAllContaining("Cannon Status Panel ");
	foreach(IMyTextPanel Panel in StatusPanels){
		Panel.FontColor=DEFAULT_TEXT_COLOR;
		Panel.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		Panel.Alignment=TextAlignment.CENTER;
		Panel.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.FontSize=1.0f;
		Panel.TextPadding=10.0f;
	}
	
	Operational=false;
	
	Controller=GenericMethods<IMyRemoteControl>.GetFull("Driver Controller");
	if(Controller==null)
		return;
	Projector=GenericMethods<IMyProjector>.GetFull("Shell Projector");
	if(Projector==null)
		return;
	YawRotor=GenericMethods<IMyMotorStator>.GetFull("Yaw Rotor");
	if(YawRotor==null)
		return;
	PitchRotor=GenericMethods<IMyMotorStator>.GetFull("Pitch Rotor");
	if(PitchRotor==null)
		return;
	ShellRotor=GenericMethods<IMyMotorStator>.GetFull("Shell Rotor");
	if(ShellRotor==null)
		return;
	Sensor=GenericMethods<IMySensorBlock>.GetFull("Driver Sensor");
	if(Sensor==null)
		return;
	Welder=GenericMethods<IMyShipWelder>.GetFull("Driver Shell Welder");
	if(Welder==null)
		return;
	Merge=GenericMethods<IMyShipMergeBlock>.GetFull("Shell Printer Merge Block");
	if(Merge==null)
		return;
	Generators=GenericMethods<IMyGravityGenerator>.GetAllContaining("Driver Generator ");
	if(Generators.Count==0)
		return;
	
	FiringLights=GenericMethods<IMyInteriorLight>.GetAllContaining("Driver Firing Light ");
	StatusLights=new List<List<IMyInteriorLight>>();
	for(int i=1;i<=3;i++)
		StatusLights.Add(GenericMethods<IMyInteriorLight>.GetAllContaining("Driver Status Light "+i.ToString()));
	
	TaskQueue=new Queue<CannonTask>();
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		try{
			if(arg.IndexOf("Task:")==0){
				string word=arg.Substring("Task:".Length);
				int output=0;
				if(Int32.TryParse(word,out output))
					TaskQueue.Enqueue((CannonTask)output);
			}
			else if(arg.IndexOf("Target:")==0){
				Vector3D.TryParse(arg.Substring("Target:".Length),out Target_Position);
			}
			else if(arg.IndexOf("FireStatus:")==0){
				string word=arg.Substring("Status:".Length);
				int output=0;
				if(Int32.TryParse(word,out output))
					CurrentFireStatus=(FireStatus)output;
			}
			else if(arg.IndexOf("PrintStatus:")==0){
				string word=arg.Substring("Status:".Length);
				int output=0;
				if(Int32.TryParse(word,out output))
					CurrentPrintStatus=(PrintStatus)output;
			}
		}
		catch(Exception){
			continue;
		}
	}
	
	Fire_Count=1;
	
	Operational=true;
	if(((int)CurrentTask)>=((int)CannonTask.Fire))
		Runtime.UpdateFrequency=UpdateFrequency.Update10;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

public void Save(){
    this.Storage="FireStatus:"+((int)CurrentFireStatus).ToString();
	this.Storage+="•PrintStatus:"+((int)CurrentPrintStatus).ToString();
	while(TaskQueue.Count>0){
		this.Storage+="•Task:"+((int)CurrentTask).ToString();
		TaskQueue.Dequeue();
	}
	Me.CustomData=this.Storage;
}

void AddTask(CannonTask Task){
	if(Task==CannonTask.None||TaskQueue.Contains(Task))
		return;
	if(((int)Task)>((int)CurrentTask)){
		Queue<CannonTask> old=new Queue<CannonTask>();
		while(TaskQueue.Count>0)
			old.Enqueue(TaskQueue.Dequeue());
		TaskQueue.Enqueue(Task);
		while(old.Count>0)
			TaskQueue.Enqueue(old.Dequeue());
	}
	else
		TaskQueue.Enqueue(Task);
	if(Task==CannonTask.Fire){
		if(CurrentPrintStatus==PrintStatus.Ready)
			CurrentFireStatus=FireStatus.Aiming;
		else
			CurrentFireStatus=FireStatus.Printing;
		Fire_Count=1;
	}
}

void NextTask(){
	bool remove=true;
	CannonTask last_task=CurrentTask;
	CurrentFireStatus=FireStatus.Idle;
	if(CurrentTask==CannonTask.Fire){
		Called_Next_Fire=true;
		if(Fire_Count>1){
			Fire_Count--;
			remove=false;
		}
		else {
			Target_Position=new Vector3D(0,0,0);
		}
	}
	if(remove){
		if(TaskQueue.Count>0)
			TaskQueue.Dequeue();
		if(TaskQueue.Count==0){
			if(last_task!=CannonTask.Reset)
				TaskQueue.Enqueue(CannonTask.Reset);
		}
	}
	if(TaskQueue.Count==0){
		PitchRotor.RotorLock=true;
		YawRotor.RotorLock=true;
	}
}

void UpdateProgramInfo(){
	cycle=(++cycle)%long.MaxValue;
	switch(loading_char){
		case '|':
			loading_char='\\';
			break;
		case '\\':
			loading_char='-';
			break;
		case '-':
			loading_char='/';
			break;
		case '/':
			loading_char='|';
			break;
	}
	Write("",false,false);
	Echo(Program_Name+" OS Cycle-"+cycle.ToString()+" ("+loading_char+")");
	Me.GetSurface(1).WriteText(Program_Name+" OS Cycle-"+cycle.ToString()+" ("+loading_char+")",false);
	seconds_since_last_update=Runtime.TimeSinceLastRun.TotalSeconds + (Runtime.LastRunTimeMs / 1000);
	Echo(ToString(FromSeconds(seconds_since_last_update))+" since last cycle");
	Time_Since_Start=UpdateTimeSpan(Time_Since_Start,seconds_since_last_update);
	Echo(ToString(Time_Since_Start)+" since last reboot\n");
	Me.GetSurface(1).WriteText("\n"+ToString(Time_Since_Start)+" since last reboot",true);
	foreach(IMyTextPanel Panel in StatusPanels){
		Panel.WriteText("",false);
	}
}

void UpdatePositionalInfo(){
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
}

double Print_Timer=0.0;
void Print(){
	double rotor_angle=ShellRotor.Angle/Math.PI*180;
	bool is_forward=Math.Abs(rotor_angle-180)<1;
	bool is_backward=Math.Abs((rotor_angle+360)%360)<1;
	bool is_printed=Projector.RemainingBlocks==0;
	ShellRotor.RotorLock=false;
	Write("Test 1");
	if(is_printed){
		IMySpaceBall ShellMass=GenericMethods<IMySpaceBall>.GetFull("Shell Mass Block");
		if(ShellMass==null||!ShellMass.IsFunctional)
			is_printed=false;
		Write("Test 2");
		IMyTimerBlock ShellTimer=GenericMethods<IMyTimerBlock>.GetFull("Shell Activation Block");
		if(ShellTimer==null||!ShellTimer.IsFunctional)
			is_printed=false;
		Write("Test 3");
		if(ShellTimer==null||!ShellTimer.IsFunctional)
			is_printed=false;
		Write("Test 4");
		IMyShipMergeBlock ShellMerge=GenericMethods<IMyShipMergeBlock>.GetFull("Shell Merge Block");
		if(ShellMerge==null||!ShellMerge.IsFunctional)
			is_printed=false;
		Write("Test 5");
	}
	if(is_printed){
		Welder.Enabled=false;
		ShellRotor.TargetVelocityRPM=30.0f;
		if(ShellRotor.Angle>1.5*Math.PI)
			ShellRotor.TargetVelocityRPM=1;
		if(is_forward){
			ShellRotor.TargetVelocityRPM=0;
			ShellRotor.RotorLock=true;
			CurrentPrintStatus=PrintStatus.Ready;
		}
		else{
			CurrentPrintStatus=PrintStatus.EndingPrint;
		}
	}
	else{
		Welder.Enabled=true;
		Merge.Enabled=true;
		Projector.Enabled=true;
		if(Fire_Timer>=1)
			ShellRotor.TargetVelocityRPM=-30.0f;
		else if(is_backward){
			ShellRotor.TargetVelocityRPM=0;
			ShellRotor.RotorLock=true;
			if(Print_Timer>30){
				CurrentPrintStatus=PrintStatus.Printing;
			}
			else
				CurrentPrintStatus=PrintStatus.WaitingResources;
		}
		else {
			Print_Timer=0.0;
			CurrentPrintStatus=PrintStatus.StartingPrint;
		}
	}
}

double Aim_Timer=AIM_TIME;
void SetAimed(double time=AIM_TIME){
	Aim_Timer=Math.Min(Math.Max(0,time),AIM_TIME);
	Aim_Position=Target_Position;
}

void ArgumentProcessor(string argument){
	if(argument.ToLower().Equals("dumbfire")){
		Target_Position=Forward_Vector*1000+Controller.GetPosition();
		AddTask(CannonTask.Fire);
		SetAimed();
	}
	else if(argument.ToLower().IndexOf("fire:")==0){
		string[] words=argument.ToLower().Substring("fire:".Length).Split(';');
		if(words.Length>0){
			bool set=false;
			Vector3D Position;
			if(!Vector3D.TryParse(words[0],out Position)){
				MyWaypointInfo Waypoint;
				if(!MyWaypointInfo.TryParse(words[0],out Waypoint)){
					if(MyWaypointInfo.TryParse(words[0].Substring(0,words[0].Length-10),out Waypoint))
						set=true;
				}
				else
					set=true;
				if(set)
					Position=Waypoint.Coords;
			}
			else
				set=true;
			if(set){
				Target_Position=Position;
				AddTask(CannonTask.Fire);
				SetAimed();
			}
		}
	}
	else if(argument.ToLower().Equals("reset")){
		TaskQueue.Clear();
		AddTask(CannonTask.Reset);
	}
}

bool CanAim(Vector3D Direction){
	double Yaw_Difference=Math.Abs(GetAngle(Left_Vector,Direction)-GetAngle(Right_Vector,Direction));
	if(Yaw_Difference>30){
		return true;
	}
	double Pitch_Difference=GetAngle(Down_Vector,Direction)-GetAngle(Up_Vector,Direction);
	double Pitch_Angle=PitchRotor.Angle/Math.PI*180;
	if(Pitch_Angle>180)
		Pitch_Angle-=360;
	double From_Top=Pitch_Angle-Pitch_Difference;
	double Pitch_Range=Math.Abs(PitchRotor.UpperLimitDeg-PitchRotor.LowerLimitDeg);
	Vector3D r_Up=LocalToGlobal(new Vector3D(0,1,0),YawRotor);
	r_Up.Normalize();
	Vector3D r_Down=-1*r_Up;
	double OOB_Range=(90-(Pitch_Range/2))*1.2;
	if(OOB_Range>0){
		if(GetAngle(Direction,r_Down)<=(90+PitchRotor.LowerLimitDeg)*1.2||GetAngle(Direction,r_Up)<=(90-PitchRotor.UpperLimitDeg)*1.2)
			return false;
	}
	return true;
}

void Aim(Vector3D Direction, double precision){
	double max_speed=10;
	double Pitch_Difference=(GetAngle(Up_Vector,Direction)-GetAngle(Down_Vector,Direction));
	PitchRotor.TargetVelocityRPM=(float)Math.Min(Math.Max((Pitch_Difference*Math.Min(1,Math.Max(Math.Abs(Pitch_Difference)/10,precision*10))),-1*max_speed),max_speed);
	if(Math.Abs(Pitch_Difference)<precision/2)
		PitchRotor.TargetVelocityRPM=0;
	double Yaw_Difference=(GetAngle(Left_Vector,Direction)-GetAngle(Right_Vector,Direction));
	YawRotor.TargetVelocityRPM=(float)Math.Min(Math.Max((Yaw_Difference*Math.Min(1,Math.Max(Math.Abs(Yaw_Difference)/10,precision*10))),-1*max_speed),max_speed);
	if(Math.Abs(Yaw_Difference)<precision/2)
		YawRotor.TargetVelocityRPM=0;
}

void Aim(Vector3D Direction){
	Aim(Direction, Precision);
}

void Aim(){
	Aim(Aim_Direction,Precision);
}

public void Reset(){
	bool moving=false;
	double YawAngle=YawRotor.Angle/Math.PI*180;
	if(YawAngle>=180)
		YawAngle=YawAngle-360;
	Write("YawAngle:"+Math.Round(YawAngle,2).ToString()+"°");
	if(Math.Abs(YawAngle)>0.1){
		moving=true;
		YawRotor.RotorLock=false;
		float target_rpm=(float)Math.Max(-20,Math.Min(20, YawAngle*-.1));
		YawRotor.TargetVelocityRPM=target_rpm;
		Write("Yaw Target:"+Math.Round(target_rpm,1).ToString()+"RPM");
	}
	else{
		YawRotor.TargetVelocityRPM=0;
		YawRotor.RotorLock=true;
	}
	double PitchAngle=PitchRotor.Angle/Math.PI*180;
	Write("PitchAngle:"+Math.Round(PitchAngle,2).ToString()+"°");
	if(Math.Abs(PitchAngle)>0.1){
		moving=true;
		PitchRotor.RotorLock=false;
		float target_rpm=(float)Math.Max(-15,Math.Min(15,PitchAngle*-.1));
		PitchRotor.TargetVelocityRPM=target_rpm;
		Write("Pitch Target:"+Math.Round(target_rpm,1).ToString()+"RPM");
	}
	else{
		PitchRotor.TargetVelocityRPM=0;
		PitchRotor.RotorLock=true;
	}
	if(!moving)
		NextTask();
	Target_Position=new Vector3D(0,0,0);
}

List<double> ShellCountdowns=new List<double>();
bool Called_Next_Fire=true;
bool DoFire(){
	IMySpaceBall ShellMass=GenericMethods<IMySpaceBall>.GetFull("Shell Mass Block");
	if(ShellMass==null||!ShellMass.IsFunctional)
		return false;
	IMyTimerBlock ShellTimer=GenericMethods<IMyTimerBlock>.GetFull("Shell Activation Block");
	if(ShellTimer==null||!ShellTimer.IsFunctional)
		return false;
	
	ShellMass.Enabled=true;
	if(!ShellMass.Enabled){
		return false;
	}
	
	
	List<IMyWarhead> Warheads=GenericMethods<IMyWarhead>.GetAllContaining("Shell Warhead ");
	double DetonationTime=(Time_To_Hit+0.05);
	ShellCountdowns.Add(DetonationTime);
	foreach(IMyWarhead Warhead in Warheads){
		Warhead.DetonationTime=(float)DetonationTime;
		Warhead.StartCountdown();
		Warhead.IsArmed=true;
	}
	
	
	ShellTimer.Trigger();
	Merge.Enabled=false;
	Called_Next_Fire=false;
	Fire_Timer=0.0;
	return true;
}

double Fire_Timer=1.0;
public void Fire(){
	if(Aim_Position.Length()<1)
		SetAimed();
	if(Time_To_Position<Time_To_Hit)
		SetAimed(AIM_TIME/2);
	bool can_aim=CanAim(Aim_Direction);
	if(!can_aim){
		NextTask();
		return;
	}
	if(CurrentFireStatus==FireStatus.Firing)
		return;
	bool is_aimed=GetAngle(Forward_Vector,Aim_Direction)<=Precision;
	bool is_clear=(!Sensor.IsActive);
	bool is_printed=CurrentPrintStatus==PrintStatus.Ready&&Merge.Enabled&&Projector.RemainingBlocks==0;
	bool is_ready=true;
	Write("Time_To_Hit:"+Math.Round(Time_To_Hit,2).ToString()+" seconds");
	Write("Time_To_Position:"+Math.Round(Time_To_Position,2).ToString()+" seconds");
	Write("Time to Launch:"+Math.Round(Time_To_Position-Time_To_Hit,2).ToString()+" seconds");
	Write("Aim vs Actual:"+Math.Round((Aim_Position-Target_Position).Length(),1)+"M");
	Write("Target Position:"+Target_Position.ToString());
	if(is_aimed){
		PitchRotor.TargetVelocityRPM=0;
		PitchRotor.RotorLock=true;
		YawRotor.TargetVelocityRPM=0;
		YawRotor.RotorLock=true;
		if(is_printed){
			if(is_clear){
				if(is_ready){
					if(DoFire())
						CurrentFireStatus=FireStatus.Firing;
				}
				else{
					CurrentFireStatus=FireStatus.WaitingTarget;
				}
			}
			else{
				CurrentFireStatus=FireStatus.WaitingClear;
			}
		}
		else {
			CurrentFireStatus=FireStatus.Printing;
		}
	}
	else{
		PitchRotor.RotorLock=false;
		YawRotor.RotorLock=false;
		Aim();
		CurrentFireStatus=FireStatus.Aiming;
	}
}

void Manual(){
	float input_pitch=Math.Min(Math.Max(Controller.RotationIndicator.X/100,-1),1);
	float input_yaw=Math.Min(Math.Max(Controller.RotationIndicator.Y/100,-1),1);
	
	input_pitch=Math.Min(Math.Max(((input_pitch*2)*Math.Abs(input_pitch*2))/2,-1),1);
	input_yaw=Math.Min(Math.Max(((input_yaw*2)*Math.Abs(input_yaw*2))/2,-1),1);
	
	if(Math.Abs(input_pitch)>0.001f){
		PitchRotor.RotorLock=false;
		PitchRotor.TargetVelocityRPM=input_pitch*10;
	}
	else{
		PitchRotor.RotorLock=true;
		PitchRotor.TargetVelocityRPM=0;
	}
	
	if(Math.Abs(input_yaw)>0.001f){
		YawRotor.RotorLock=false;
		YawRotor.TargetVelocityRPM=input_yaw*10;
	}
	else{
		YawRotor.RotorLock=true;
		YawRotor.TargetVelocityRPM=0;
	}
	
	Write("Input_Pitch: "+Math.Round(input_pitch,1).ToString());
	Write("Pitch RPM: "+Math.Round(PitchRotor.TargetVelocityRPM,1).ToString());
	Write("Input_Yaw: "+Math.Round(input_pitch,1).ToString());
	Write("Yaw RPM: "+Math.Round(YawRotor.TargetVelocityRPM,1).ToString());
}

void TimerUpdate(){
	if(Print_Timer<=30&&CurrentPrintStatus!=PrintStatus.Ready){
		Print_Timer+=seconds_since_last_update;
		Write("Print_Timer:"+Math.Round(Print_Timer,2)+" seconds");
	}
	if(Fire_Timer<=1){
		Fire_Timer+=seconds_since_last_update;
		Write("Fire_Timer:"+Math.Round(Fire_Timer,2)+" seconds");
	}
	for(int i=0;i<ShellCountdowns.Count;i++){
		ShellCountdowns[i]-=seconds_since_last_update;
		if(ShellCountdowns[i]<0){
			ShellCountdowns.RemoveAt(i);
			i--;
			continue;
		}
	}
	if(Aim_Timer>0)
		Aim_Timer=Math.Max(0,Aim_Timer-seconds_since_last_update);
}

public void Main(string argument, UpdateType updateSource)
{
	if(!Operational){
		Runtime.UpdateFrequency=UpdateFrequency.None;
		return;
	}
	UpdateProgramInfo();
	UpdatePositionalInfo();
	TimerUpdate();
	if(argument.Length>0){
		ArgumentProcessor(argument);
	}
	Print();
	switch(CurrentTask){
		case CannonTask.None:
			if(Target_Position.Length()>0)
				AddTask(CannonTask.Fire);
			else{
				Write("No current Task");
				PitchRotor.TargetVelocityRPM=0;
				PitchRotor.RotorLock=true;
				YawRotor.TargetVelocityRPM=0;
				YawRotor.RotorLock=true;
			}
			break;
		case CannonTask.Reset:
			Reset();
			break;
		case CannonTask.Scan:
			NextTask();
			break;
		case CannonTask.Search:
			NextTask();
			break;
		case CannonTask.Fire:
			Fire();
			break;
		case CannonTask.Manual:
			Manual();
			break;
	}
	PitchRotor.Enabled=(CurrentTask!=CannonTask.None);
	YawRotor.Enabled=(CurrentTask!=CannonTask.None);
	if(Fire_Timer<1){
		PitchRotor.RotorLock=true;
		YawRotor.RotorLock=true;
	}
	if((!Called_Next_Fire)&&CurrentTask>=CannonTask.Fire&&CurrentFireStatus==FireStatus.Firing&&Fire_Timer>1&&!Sensor.IsActive)
		NextTask();
	
	foreach(IMyInteriorLight Light in FiringLights){
		Light.Enabled=CurrentTask==CannonTask.Fire;
	}
	foreach(IMyGravityGenerator Generator in Generators)
		Generator.Enabled=((CurrentFireStatus==FireStatus.Firing)||(Sensor.IsActive));
	bool active=true;
	foreach(CannonTask task in TaskQueue){
		if(active){
			Write("--"+task.ToString().ToUpper());
			active=false;
		}
		else
			Write(" -"+task.ToString().ToLower());
	}
	
	Write("Current Task:"+CurrentTask.ToString());
	if(CurrentTask==CannonTask.Fire)
		Write("Fire_Count:"+Fire_Count.ToString());
	Write("PrintStatus:"+CurrentPrintStatus.ToString());
	Write("FireStatus:"+CurrentFireStatus.ToString());
	if(Target_Position.Length()>0){
		Write("Distance:"+Math.Round(Aim_Distance/1000,1)+"kM");
		Write("Precision:"+Math.Round(GetAngle(Forward_Vector,Aim_Direction),3)+"°/"+Math.Round(Precision,3).ToString()+"°");
	}
	foreach(double countdown in ShellCountdowns){
		Write("Time to Explosion:"+Math.Round(countdown,1)+" seconds");
	}
	for(int i=0;i<StatusLights.Count;i++){
		if(i==0){
			foreach(IMyInteriorLight Light in StatusLights[i]){
				Light.Enabled=(CurrentTask!=CannonTask.Fire||CurrentFireStatus!=FireStatus.Firing);
			}
		}
		else if(i==1){
			foreach(IMyInteriorLight Light in StatusLights[i]){
				Light.Enabled=(CurrentPrintStatus>=PrintStatus.Printing);
			}
		}
		else if(i==2){
			foreach(IMyInteriorLight Light in StatusLights[i]){
				Light.Enabled=(CurrentPrintStatus>=PrintStatus.Ready);
			}
		}
	}
	if(((int)CurrentTask)>=((int)CannonTask.Scan))
		Runtime.UpdateFrequency=UpdateFrequency.Update1;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}
