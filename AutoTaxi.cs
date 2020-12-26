const string Program_Name = "AutoTaxi Drone AI"; //Name me!
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);

class Prog{
	public static MyGridProgram P;
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

class Dock{
	public Vector3D Position;
	public Vector3D Orientation;
	public Vector3D Return;
	public bool DoUp;
	public Vector3D Up;
	
	public Dock(Vector3D p,Vector3D o,Vector3D r){
		Position=p;
		Orientation=o;
		Orientation.Normalize();
		Return=r;
		Up=new Vector3D(0,0,1);
		DoUp=false;
	}
	
	public Dock(Vector3D p,Vector3D o,Vector3D r,Vector3D u):this(p,o,r){
		Up=u;
		DoUp=true;
	}
	
	public override string ToString(){
		if(DoUp)
			return '('+Position.ToString()+';'+Orientation.ToString()+';'+Return.ToString()+';'+Up.ToString()+')';
		else
			return '('+Position.ToString()+';'+Orientation.ToString()+';'+Return.ToString()+')';
	}
	
	public static bool TryParse(string Parse,out Dock output){
		output=null;
		if(Parse[0]!='('||Parse[Parse.Length-1]!=')')
			return false;
		Parse=Parse.Substring(1,Parse.Length-2);
		string[] args=Parse.Split(';');
		if(args.Length!=3&&args.Length!=4)
			return false;
		Vector3D p;
		if(!Vector3D.TryParse(args[0],out p))
			return false;
		Vector3D o;
		if((!Vector3D.TryParse(args[1],out o))||o.Length()==0)
			return false;
		Vector3D r;
		if(!Vector3D.TryParse(args[2],out r))
			return false;
		output=new Dock(p,o,r);
		Vector3D u;
		if(args.Length==4&&Vector3D.TryParse(args[3],out u)){
			output.Up=u;
			output.DoUp=true;
		}
		return true;
	}
}

enum DroneTask{
	None=0,
	Docked=1,
	Docking=2,
	Traveling=3
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

void Write(string text,bool new_line=true,bool append=true){
	Echo(text);
	if(new_line)
		Me.GetSurface(0).WriteText(text+'\n', append);
	else
		Me.GetSurface(0).WriteText(text, append);
}

string GetRemovedString(string big_string, string small_string){
	string output=big_string;
	if(big_string.Contains(small_string)){
		output=big_string.Substring(0, big_string.IndexOf(small_string))+big_string.Substring(big_string.IndexOf(small_string)+small_string.Length);
	}
	return output;
}

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;

TimeSpan Current_Time;
double Cycle_Time{
	get{
		return Current_Time.TotalSeconds%600;
	}
}

Stack<DroneTask> Tasks;

Queue<Dock> Docks;
Dock MyDock{
	get{
		if(Docks.Count>0)
			return Docks.Peek();
		return null;
	}
}

IMyRemoteControl Controller;
IMyGyro Gyroscope;
IMyRadioAntenna Antenna;
IMyShipConnector Connector;
List<IMyBatteryBlock> Batteries;
List<IMyTextPanel> LCDs;
List<IMyDoor> Doors;
IMyBeacon Beacon;
IMySoundBlock Sound;

bool Running=false;

float Charge{
	get{
		float current=0,max=0;
		foreach(IMyBatteryBlock B in Batteries){
			current+=B.CurrentStoredPower;
			max+=B.MaxStoredPower;
		}
		return current/max;
	}
}

double ShipMass{
	get{
		return Controller.CalculateShipMass().TotalMass;
	}
}

bool Match_Direction=false;
bool Slow_Down=true;
Vector3D Target_Direction=new Vector3D(0,1,0);
bool Match_Position=false;
Vector3D Pseudo_Target=new Vector3D(0,0,0);
Vector3D Relative_Pseudo_Target{
	get{
		return GlobalToLocalPosition(Pseudo_Target,Controller);
	}
}
Vector3D Target_Position=new Vector3D(0,0,0);
double Target_Distance{
	get{
		return (Target_Position-Controller.GetPosition()).Length();
	}
}

Vector3D RestingVelocity=new Vector3D(0,0,0);
Vector3D Relative_RestingVelocity{
	get{
		return GlobalToLocal(RestingVelocity,Controller);
	}
}
Vector3D LinearVelocity;
Vector3D Relative_LinearVelocity{
	get{
		Vector3D output=Vector3D.Transform(LinearVelocity+Controller.GetPosition(),MatrixD.Invert(Controller.WorldMatrix));
		output.Normalize();
		output*=LinearVelocity.Length();
		return output;
	}
}
double Speed_Deviation{
	get{
		return (LinearVelocity-RestingVelocity).Length();
	}
}
double Acceleration{
	get{
		return (Forward_Thrust+Backward_Thrust)/(2*ShipMass);
	}
}
double Time_To_Resting{
	get{
		return (RestingVelocity-LinearVelocity).Length()/Acceleration;
	}
}
double Distance_To_Resting{
	get{
		return Acceleration*Math.Pow(Time_To_Resting,2)/2;
	}
}

double Distance_To_Base{
	get{
		return (Controller.GetPosition()-MyDock.Return).Length();
	}
}

double Speed_Limit=100;

Vector3D AngularVelocity;
Vector3D Relative_AngularVelocity{
	get{
		return GlobalToLocal(AngularVelocity,Controller);
	}
}

List<IMyThrust>[] All_Thrusters=new List<IMyThrust>[6];
List<IMyThrust> Forward_Thrusters{
	set{
		All_Thrusters[0]=value;
	}
	get{
		return All_Thrusters[0];
	}
}
List<IMyThrust> Backward_Thrusters{
	set{
		All_Thrusters[1]=value;
	}
	get{
		return All_Thrusters[1];
	}
}
List<IMyThrust> Up_Thrusters{
	set{
		All_Thrusters[2]=value;
	}
	get{
		return All_Thrusters[2];
	}
}
List<IMyThrust> Down_Thrusters{
	set{
		All_Thrusters[3]=value;
	}
	get{
		return All_Thrusters[3];
	}
}
List<IMyThrust> Left_Thrusters{
	set{
		All_Thrusters[4]=value;
	}
	get{
		return All_Thrusters[4];
	}
}
List<IMyThrust> Right_Thrusters{
	set{
		All_Thrusters[5]=value;
	}
	get{
		return All_Thrusters[5];
	}
}

Base6Directions.Direction Forward;
Base6Directions.Direction Backward{
	get{
		return Base6Directions.GetOppositeDirection(Forward);
	}
}
Base6Directions.Direction Up;
Base6Directions.Direction Down{
	get{
		return Base6Directions.GetOppositeDirection(Up);
	}
}
Base6Directions.Direction Left;
Base6Directions.Direction Right{
	get{
		return Base6Directions.GetOppositeDirection(Left);
	}
}

float GetThrust(int i){
	float total=0;
	foreach(IMyThrust T in All_Thrusters[i])
		total+=T.MaxEffectiveThrust;
	return Math.Max(total,1);
}
float Forward_Thrust{
	get{
		return GetThrust(0);
	}
}
float Backward_Thrust{
	get{
		return GetThrust(1);
	}
}
float Up_Thrust{
	get{
		return GetThrust(2);
	}
}
float Down_Thrust{
	get{
		return GetThrust(3);
	}
}
float Left_Thrust{
	get{
		return GetThrust(4);
	}
}
float Right_Thrust{
	get{
		return GetThrust(5);
	}
}

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

string GetThrustTypeName(IMyThrust Thruster){
	string block_type=Thruster.BlockDefinition.SubtypeName;
	if(block_type.Contains("LargeBlock"))
		block_type=GetRemovedString(block_type,"LargeBlock");
	else if(block_type.Contains("SmallBlock"))
		block_type=GetRemovedString(block_type,"SmallBlock");
	if(block_type.Contains("Thrust"))
		block_type=GetRemovedString(block_type,"Thrust");
	string size="";
	if(block_type.Contains("Small")){
		size="Small";
		block_type=GetRemovedString(block_type,size);
	}
	else if(block_type.Contains("Large")){
		size="Large";
		block_type=GetRemovedString(block_type,size);
	}
	if((!block_type.ToLower().Contains("atmospheric"))||(!block_type.ToLower().Contains("hydrogen")))
		block_type+="Ion";
	return (size+" "+block_type).Trim();
}
struct NameTuple{
	public string Name;
	public int Count;
	
	public NameTuple(string n,int c=0){
		Name=n;
		Count=c;
	}
}
void SetThrusterList(List<IMyThrust> Thrusters,string Direction){
	List<NameTuple> Thruster_Types=new List<NameTuple>();
	foreach(IMyThrust Thruster in Thrusters){
		if(!HasBlockData(Thruster,"DefaultOverride"))
			SetBlockData(Thruster,"DefaultOverride",Thruster.ThrustOverridePercentage.ToString());
		SetBlockData(Thruster,"Owner",Me.CubeGrid.EntityId.ToString());
		SetBlockData(Thruster,"DefaultName",Thruster.CustomName);
		string name=GetThrustTypeName(Thruster);
		bool found=false;
		for(int i=0;i<Thruster_Types.Count;i++){
			if(name.Equals(Thruster_Types[i].Name)){
				found=true;
				Thruster_Types[i]=new NameTuple(name,Thruster_Types[i].Count+1);
				break;
			}
		}
		if(!found)
			Thruster_Types.Add(new NameTuple(name,1));
	}
	foreach(IMyThrust Thruster in Thrusters){
		string name=GetThrustTypeName(Thruster);
		for(int i=0;i<Thruster_Types.Count;i++){
			if(name.Equals(Thruster_Types[i].Name)){
				Thruster.CustomName=(Direction+" "+name+" Thruster "+(Thruster_Types[i].Count).ToString()).Trim();
				Thruster_Types[i]=new NameTuple(name,Thruster_Types[i].Count-1);
				break;
			}
		}
	}
}
void ResetThruster(IMyThrust Thruster){
	if(HasBlockData(Thruster,"DefaultOverride")){
		float ThrustOverride=0.0f;
		if(float.TryParse(GetBlockData(Thruster,"DefaultOverride"),out ThrustOverride))
			Thruster.ThrustOverridePercentage=ThrustOverride;
		else
			Thruster.ThrustOverridePercentage=0.0f;
	}
	if(HasBlockData(Thruster,"DefaultName")){
		Thruster.CustomName=GetBlockData(Thruster,"DefaultName");
	}
	SetBlockData(Thruster,"Owner","0");
}

public Program(){
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
	Docks=new Queue<Dock>();
	Tasks=new Stack<DroneTask>();
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		if(arg.IndexOf("Tas:")==0){
			int t;
			if(Int32.TryParse(arg.Substring(4),out t))
				Tasks.Push((DroneTask)t);
		}
		else if(arg.IndexOf("Doc:")==0){
			Dock D;
			if(!arg.Substring(4).Equals("null")){
				Dock.TryParse(arg.Substring(4),out D);
				Docks.Enqueue(D);
			}
		}
		else if(arg.IndexOf("Run:")==0){
			bool.TryParse(arg.Substring(4),out Running);
		}
	}
	Controller=GenericMethods<IMyRemoteControl>.GetConstruct("Taxi Remote Control");
	Gyroscope=GenericMethods<IMyGyro>.GetConstruct("Control Gyroscope");
	Connector=GenericMethods<IMyShipConnector>.GetConstruct("Taxi Connector");
	if(Controller==null||Gyroscope==null||Connector==null)
		return;
	Forward=Controller.Orientation.Forward;
	Up=Controller.Orientation.Up;
	Left=Controller.Orientation.Left;
	List<IMyThrust> MyThrusters=GenericMethods<IMyThrust>.GetAllConstruct("");
	for(int i=0;i<6;i++)
		All_Thrusters[i]=new List<IMyThrust>();
	for(int i=0;i<2;i++){
		bool retry=!Me.CubeGrid.IsStatic;
		foreach(IMyThrust Thruster in MyThrusters){
			if(Thruster.CubeGrid!=Controller.CubeGrid)
				continue;
			retry=false;
			Base6Directions.Direction ThrustDirection=Thruster.Orientation.Forward;
			if(ThrustDirection==Backward)
				Forward_Thrusters.Add(Thruster);
			else if(ThrustDirection==Forward)
				Backward_Thrusters.Add(Thruster);
			else if(ThrustDirection==Down)
				Up_Thrusters.Add(Thruster);
			else if(ThrustDirection==Up)
				Down_Thrusters.Add(Thruster);
			else if(ThrustDirection==Right)
				Left_Thrusters.Add(Thruster);
			else if(ThrustDirection==Left)
				Right_Thrusters.Add(Thruster);
		}
		if(!retry)
			break;
	}
	SetThrusterList(Forward_Thrusters,"Forward");
	SetThrusterList(Backward_Thrusters,"Backward");
	SetThrusterList(Up_Thrusters,"Up");
	SetThrusterList(Down_Thrusters,"Down");
	SetThrusterList(Left_Thrusters,"Left");
	SetThrusterList(Right_Thrusters,"Right");
	Sound=GenericMethods<IMySoundBlock>.GetConstruct("Taxi Sound Block");
	Beacon=GenericMethods<IMyBeacon>.GetConstruct("Taxi Beacon");
	Doors=GenericMethods<IMyDoor>.GetAllConstruct("Taxi Door");
	Antenna=GenericMethods<IMyRadioAntenna>.GetConstruct("Taxi Antenna");
	Batteries=GenericMethods<IMyBatteryBlock>.GetAllConstruct("Taxi");
	LCDs=GenericMethods<IMyTextPanel>.GetAllConstruct("Taxi LCD");
	foreach(IMyTextPanel Panel in LCDs){
		Panel.FontColor=DEFAULT_TEXT_COLOR;
		Panel.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		Panel.Alignment=TextAlignment.CENTER;
		Panel.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.WriteText("Taxi Service",false);
	}
	if(Batteries.Count==0)
		return;
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

public void Save(){
	this.Storage="•Run:"+Running.ToString();
	foreach(Dock dock in Docks)
		this.Storage+="•Doc:"+dock.ToString();
	Stack<DroneTask> temp=new Stack<DroneTask>();
    foreach(DroneTask T in Tasks)
		temp.Push(T);
	foreach(DroneTask T in temp)
		this.Storage+="•Tas:"+((int)T).ToString();
}

void EndTask(bool do_pop=true){
	DroneTask Last=DroneTask.None;
	if(Tasks.Count>0){
		Last=Tasks.Peek();
		if(do_pop)
			Tasks.Pop();
	}
	if(Tasks.Count==0)
		Tasks.Push(DroneTask.None);
	switch(Last){
		case DroneTask.Docked:
			foreach(IMyBatteryBlock B in Batteries)
				B.ChargeMode=ChargeMode.Auto;
			Connector.Disconnect();
			foreach(IMyDoor Door in Doors)
				Door.CloseDoor();
			break;
		case DroneTask.Docking:
			Match_Position=false;
			Match_Direction=false;
			break;
		case DroneTask.Traveling:
			Match_Position=false;
			Match_Direction=false;
			Controller.ClearWaypoints();
			Controller.SetAutoPilotEnabled(false);
			Sound.Play();
			break;
	}
}

void Docked(){
	if(MyDock==null){
		EndTask();
		return;
	}
	foreach(IMyBatteryBlock B in Batteries)
		B.ChargeMode=ChargeMode.Recharge;
	foreach(IMyDoor Door in Doors)
		Door.OpenDoor();
	Runtime.UpdateFrequency=UpdateFrequency.Update10;
}

void Docking(){
	if(MyDock==null){
		EndTask();
		return;
	}
	Target_Direction=MyDock.Orientation;
	Match_Direction=true;
	Target_Position=MyDock.Position+10*MyDock.Orientation;
	Match_Position=true;
	Speed_Limit=5;
	Vector3D angle=Controller.GetPosition()-MyDock.Position;
	angle.Normalize();
	if((Controller.GetPosition()-MyDock.Position).Length()<12&&GetAngle(MyDock.Orientation,angle)<5){
		Target_Position=Controller.GetPosition()-Connector.GetPosition();
		Target_Position=MyDock.Orientation*1.5+MyDock.Position+Target_Position;
		Speed_Limit=2.5;
	}
	if(Connector.Status!=MyShipConnectorStatus.Unconnected)
		Connector.Connect();
	if(Connector.Status==MyShipConnectorStatus.Connected){
		EndTask();
		Tasks.Push(DroneTask.Docked);
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

void Traveling(){
	Match_Position=false;
	if(MyDock==null){
		EndTask();
		return;
	}
	MyWaypointInfo Destination=new MyWaypointInfo("Return to Base",MyDock.Return);
	if((!Controller.CurrentWaypoint.Equals(Destination))||!Controller.IsAutoPilotEnabled){
		Controller.ClearWaypoints();
		Controller.AddWaypoint(Destination);
		Controller.SetCollisionAvoidance(true);
		Speed_Limit=Math.Max(5,Math.Min(100,Distance_To_Base/15));
		Controller.SpeedLimit=(float)Speed_Limit;
		Controller.SetAutoPilotEnabled(true);
	}
	if((Controller.GetPosition()-MyDock.Return).Length()<2.5){
		EndTask();
		Tasks.Push(DroneTask.Docking);
	}
	if(Match_Position)
		Runtime.UpdateFrequency=UpdateFrequency.Update1;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update10;
}

void SetGyroscopes(){
	if((!Match_Direction)||Controller.IsUnderControl||Controller.IsAutoPilotEnabled){
		Gyroscope.GyroOverride=false;
		Write("Gyroscope Controls:Off");
		return;
	}
	Write("Gyroscope Controls:On");
	if(Match_Direction)
		Write("Match_Direction:"+Math.Round(GetAngle(Target_Direction,Forward_Vector),1).ToString()+'°');
	Gyroscope.GyroOverride=(AngularVelocity.Length()<3);
	float current_pitch=(float) Relative_AngularVelocity.X;
	float current_yaw=(float) Relative_AngularVelocity.Y;
	float current_roll=(float) Relative_AngularVelocity.Z;
	
	float gyro_count = 0;
	List<IMyGyro> AllGyros=new List<IMyGyro>();
	GridTerminalSystem.GetBlocksOfType<IMyGyro>(AllGyros);
	foreach(IMyGyro Gyro in AllGyros){
		if(Gyro.IsWorking)
			gyro_count+=Gyro.GyroPower/100.0f;
	}
	float gyro_multx=(float)Math.Max(0.1f,Math.Min(1,1.5f/(ShipMass/gyro_count/1000000)));
	
	float input_pitch=current_pitch*0.99f;
	double difference=GetAngle(Down_Vector,Target_Direction)-GetAngle(Up_Vector,Target_Direction);
	if(Math.Abs(difference)>.1)
		input_pitch-=(float)Math.Min(Math.Max(difference/5,-4),4)*gyro_multx;
	
	float input_yaw=current_yaw*0.99f;
	difference=GetAngle(Left_Vector,Target_Direction)-GetAngle(Right_Vector,Target_Direction);
	if(Math.Abs(difference)>.1)
		input_yaw+=(float)Math.Min(Math.Max(difference/5,-4),4)*gyro_multx;
	
	float input_roll=current_roll*0.99f;
	if(MyDock!=null&&MyDock.DoUp&&Tasks.Peek()==DroneTask.Docking){
		difference=GetAngle(Left_Vector,MyDock.Up)-GetAngle(Right_Vector,MyDock.Up);
		if(Math.Abs(difference)>.1){
			if(GetAngle(Down_Vector,MyDock.Up)>GetAngle(Down_Vector,MyDock.Up))
				input_roll+=(float)Math.Min(Math.Max(difference/5,-4),1)*gyro_multx;
			else
				input_roll-=(float)Math.Min(Math.Max(difference/5,-4),1)*gyro_multx;
		}
	}
	
	Vector3D input=new Vector3D(input_pitch,input_yaw,input_roll);
	
	Vector3D global=Vector3D.TransformNormal(input,Controller.WorldMatrix);
	Vector3D output=Vector3D.TransformNormal(global,MatrixD.Invert(Gyroscope.WorldMatrix));
	output.Normalize();
	output*=input.Length();
	
	Gyroscope.Pitch=(float)output.X;
	Gyroscope.Yaw=(float)output.Y;
	Gyroscope.Roll=(float)output.Z;
}

void SetThrusters(){
	if((RestingVelocity.Length()==0&&!Match_Position)||Controller.IsUnderControl||Controller.IsAutoPilotEnabled||Relative_LinearVelocity.Length()>Speed_Limit){
		for(int i=0;i<6;i++){
			foreach(IMyThrust T in All_Thrusters[i])
				T.ThrustOverridePercentage=0;
		}
		Write("Thruster Controls:Off");
		if(Controller.IsAutoPilotEnabled){
			Write("   AutoPilot: "+Math.Round((Controller.GetPosition()-Controller.CurrentWaypoint.Coords).Length(),1).ToString()+" meters");
		}
		return;
	}
	Write("Thruster Controls:On");
	if(Match_Position)
		Write("Match_Position:"+Math.Round((Target_Position-Controller.GetPosition()).Length(),1).ToString()+"meters\n   (X:"+Math.Round(Relative_Pseudo_Target.X,1).ToString()+" Y:"+Math.Round(Relative_Pseudo_Target.Y,1).ToString()+" Z:"+Math.Round(Relative_Pseudo_Target.Z,1).ToString()+")");
	if(RestingVelocity.Length()>0)
		Write("RestingVelocity:"+Math.Round(RestingVelocity.Length(),1).ToString()+"mps");
	float damp_multx=0.99f;
	double ESL=Speed_Limit;
	if(Slow_Down)
		ESL=Math.Min(ESL,Speed_Limit*(Target_Distance-Distance_To_Resting*1.2));
	if(Speed_Limit<5)
		ESL=Math.Max(ESL,2.5);
	else
		ESL=Math.Max(ESL,5);
	
	float input_right=-1*(float)((Relative_LinearVelocity.X-Relative_RestingVelocity.X)*ShipMass*damp_multx);
	float input_up=-1*(float)((Relative_LinearVelocity.Y-Relative_RestingVelocity.Y)*ShipMass*damp_multx);
	float input_forward=(float)((Relative_LinearVelocity.Z-Relative_RestingVelocity.Z)*ShipMass*damp_multx);
	
	bool matched_direction=!Match_Direction;
	if(Match_Direction)
		matched_direction=Math.Abs(GetAngle(Target_Direction,Forward_Vector))<=5;
	
	Vector3D Movement_Direction=Relative_Pseudo_Target;
	Movement_Direction.Normalize();
	
	if(Match_Position){
		double Relative_Distance=Relative_Pseudo_Target.X;
		double Target_Speed=Math.Abs(Movement_Direction.X*ESL);
		if(matched_direction||!Match_Direction){
			if(Relative_Distance>0){
				if(Math.Abs((Relative_LinearVelocity+Left_Vector-RestingVelocity).X)<=Target_Speed)
					input_right=0.95f*Left_Thrust;
				else
					input_right=0;
			}
			else{
				if(Math.Abs((Relative_LinearVelocity+Right_Vector-RestingVelocity).X)<=Target_Speed)
					input_right=-0.95f*Right_Thrust;
				else
					input_right=0;
			}
		}
	}
	if(Match_Position){
		double Relative_Distance=Relative_Pseudo_Target.Y;
		double Target_Speed=Math.Abs(Movement_Direction.Y*ESL);
		if(matched_direction||!Match_Direction){
			if(Relative_Distance>0){
				if(Math.Abs((Relative_LinearVelocity+Down_Vector-RestingVelocity).Y)<=Target_Speed)
					input_up=0.95f*Down_Thrust;
				else
					input_up=0;
			}
			else{
				if(Math.Abs((Relative_LinearVelocity+Up_Vector-RestingVelocity).Y)<=Target_Speed)
					input_up=-0.95f*Up_Thrust;
				else
					input_up=0;
			}
		}
	}
	if(Match_Position){
		double Relative_Distance=Relative_Pseudo_Target.Z;
		double Target_Speed=Math.Abs(Movement_Direction.Z*ESL);
		if(matched_direction||!Match_Direction){
			if(Relative_Distance>0){
				if(Math.Abs((Relative_LinearVelocity+Backward_Vector-RestingVelocity).Z)<=Target_Speed)
					input_forward=-0.95f*Backward_Thrust;
				else
					input_forward=0;
			}
			else{
				if(Math.Abs((Relative_LinearVelocity+Forward_Vector-RestingVelocity).Z)<=Target_Speed)
					input_forward=0.95f*Forward_Thrust;
				else
					input_forward=0;
			}
		}
	}
	
	float output_forward=0.0f;
	float output_backward=0.0f;
	if(input_forward/Forward_Thrust>0.05f)
		output_forward=Math.Min(Math.Abs(input_forward/Forward_Thrust),1);
	else if(input_forward/Backward_Thrust<-0.05f)
		output_backward=Math.Min(Math.Abs(input_forward/Backward_Thrust),1);
	float output_up=0.0f;
	float output_down=0.0f;
	if(input_up/Up_Thrust > 0.05f)
		output_up=Math.Min(Math.Abs(input_up/Up_Thrust),1);
	else if(input_up/Down_Thrust<-0.05f)
		output_down=Math.Min(Math.Abs(input_up/Down_Thrust),1);
	float output_right=0.0f;
	float output_left=0.0f;
	if(input_right/Right_Thrust>0.05f)
		output_right=Math.Min(Math.Abs(input_right/Right_Thrust),1);
	else if(input_right/Left_Thrust<-0.05f)
		output_left=Math.Min(Math.Abs(input_right/Left_Thrust),1);
	
	foreach(IMyThrust Thruster in Forward_Thrusters)
		Thruster.ThrustOverridePercentage=output_forward;
	foreach(IMyThrust Thruster in Backward_Thrusters)
		Thruster.ThrustOverridePercentage=output_backward;
	foreach(IMyThrust Thruster in Up_Thrusters)
		Thruster.ThrustOverridePercentage=output_up;
	foreach(IMyThrust Thruster in Down_Thrusters)
		Thruster.ThrustOverridePercentage=output_down;
	foreach(IMyThrust Thruster in Right_Thrusters)
		Thruster.ThrustOverridePercentage=output_right;
	foreach(IMyThrust Thruster in Left_Thrusters)
		Thruster.ThrustOverridePercentage=output_left;
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
}

void UpdateSystemInfo(){
	Current_Time=DateTime.Now.TimeOfDay;
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
	LinearVelocity=Controller.GetShipVelocities().LinearVelocity;
	AngularVelocity=Controller.GetShipVelocities().AngularVelocity;
	Pseudo_Target=Target_Position;
}

bool ProcessArgument(string argument){
	LastArgument=argument;
	if(argument.ToLower().IndexOf("dock:")==0){
		Dock D=null;
		Vector3D p,o,r;
		string[] args=argument.Substring(5).Split('•');
		if(args.Length!=3&&args.Length!=4)
			return false;
		if(!Vector3D.TryParse(args[0],out p))
			return false;
		if(!Vector3D.TryParse(args[1],out o))
			return false;
		if(!Vector3D.TryParse(args[2],out r))
			return false;
		D=new Dock(p,o,r);
		Vector3D u;
		if(args.Length==4&&Vector3D.TryParse(args[3],out u)){
			D.Up=u;
			D.DoUp=true;
		}
		Docks.Enqueue(D);
		if(Docks.Count>2)
			Docks.Dequeue();
		return true;
	}
	else if(argument.ToLower().Equals("running")){
		Running=!Running;
		return true;
	}
	else if(argument.ToLower().Equals("wipe")){
		Docks.Clear();
		return true;
	}
	return false;
}

bool ArgumentError=false;
string ArgumentError_Message="";
string LastArgument="";
bool switched=false;
public void Main(string argument, UpdateType updateSource)
{
	if(Tasks.Count==0)
		Tasks.Push(DroneTask.None);
	UpdateProgramInfo();
	UpdateSystemInfo();
	double Time_To_Embark=600-Cycle_Time;
	Write("Running: "+Running.ToString());
	if(MyDock!=null&&Running){
		if(switched&&Cycle_Time>300){
			switched=false;
		}
		else if((!switched)&&Cycle_Time<150){
			if(Charge>=0.95f){
				Docks.Enqueue(Docks.Dequeue());
				Tasks.Clear();
				Tasks.Push(DroneTask.Traveling);
				switched=true;
			}
		}
		if(Distance_To_Base>1000)
			Write(Math.Round(Distance_To_Base/1000,1)+"kM from Destination");
		else
			Write(Math.Round(Distance_To_Base,0)+" meters from Destination");
		if(Time_To_Embark>60)
			Write(Math.Round(Time_To_Embark/60,1)+" minutes to departure");
		else
			Write(Math.Round(Time_To_Embark,0)+" seconds to departure");
		string minutes="";
		string seconds="";
		int min=(int)(Time_To_Embark/60);
		int sec=(int)(Time_To_Embark%60);
		if(min<10)
			minutes+="0";
		minutes+=min.ToString();
		if(sec<10)
			seconds+="0";
		seconds+=sec.ToString();
		foreach(IMyTextPanel Panel in LCDs){
			Panel.WriteText(minutes+":"+seconds,false);
		}
		if(Beacon!=null){
			Beacon.HudText=minutes+":"+seconds;
			Beacon.Radius=100;
		}
	}
	else {
		foreach(IMyTextPanel Panel in LCDs)
			Panel.WriteText("Out of Service",false);
		if(Beacon!=null){
			Beacon.HudText="Out of Service";
			Beacon.Radius=25;
		}
	}
	if(MyDock!=null)
		Antenna.Radius=Math.Min(5000,500+(float)Distance_To_Base);
	else
		Antenna.Radius=500;
	if(argument.Length>0)
		ArgumentError=!ProcessArgument(argument);
	if(LastArgument.Length>0)
		Write("Last Argument: "+LastArgument);
	if(ArgumentError){
		if(ArgumentError_Message.Length>0)
			Write("Invalid Argument: "+ArgumentError_Message);
		else
			Write("Invalid Argument");
	}
	else
		ArgumentError_Message="";
	Write("Speed: "+Math.Round(LinearVelocity.Length(),1).ToString()+"mps");
	bool active=true;
	Write("Tasks");
	foreach(DroneTask Task in Tasks){
		if(active)
			Write(" "+Task.ToString().ToUpper());
		else
			Write("  "+Task.ToString().ToLower());
		active=false;
	}
	switch(Tasks.Peek()){
		case DroneTask.None:
			Runtime.UpdateFrequency=UpdateFrequency.Update100;
			if(MyDock!=null){
				EndTask();
				Tasks.Push(DroneTask.Traveling);
			}
			break;
		case DroneTask.Docked:
			Docked();
			break;
		case DroneTask.Docking:
			Docking();
			break;
		case DroneTask.Traveling:
			Traveling();
			break;
	}
	SetGyroscopes();
	SetThrusters();
}
