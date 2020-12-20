const string Program_Name = ""; //Name me!
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

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;

IMyShipController Controller;
IMyGyro Gyroscope;

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

float Forward_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Forward_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return Math.Max(total,1);
	}
}
float Backward_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Backward_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return Math.Max(total,1);
	}
}
float Up_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Up_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return Math.Max(total,1);
	}
}
float Down_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Down_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return Math.Max(total,1);
	}
}
float Left_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Left_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return Math.Max(total,1);
	}
}
float Right_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Right_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return Math.Max(total,1);
	}
}

double Forward_Gs{
	get{
		return Forward_Thrust/Controller.CalculateShipMass().TotalMass/9.81;
	}
}
double Backward_Gs{
	get{
		return Backward_Thrust/Controller.CalculateShipMass().TotalMass/9.81;
	}
}
double Up_Gs{
	get{
		return Up_Thrust/Controller.CalculateShipMass().TotalMass/9.81;
	}
}
double Down_Gs{
	get{
		return Down_Thrust/Controller.CalculateShipMass().TotalMass/9.81;
	}
}
double Left_Gs{
	get{
		return Left_Thrust/Controller.CalculateShipMass().TotalMass/9.81;
	}
}
double Right_Gs{
	get{
		return Right_Thrust/Controller.CalculateShipMass().TotalMass/9.81;
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

float Mass_Accomodation=0.0f;

Vector3D RestingVelocity;
Vector3D Relative_RestingVelocity{
	get{
		return GlobalToLocal(RestingVelocity,Controller);
	}
}
Vector3D CurrentVelocity;
Vector3D Relative_CurrentVelocity{
	get{
		Vector3D output=Vector3D.Transform(CurrentVelocity+Controller.GetPosition(),MatrixD.Invert(Controller.WorldMatrix));
		output.Normalize();
		output*=CurrentVelocity.Length();
		return output;
	}
}
Vector3D Gravity;
Vector3D Relative_Gravity{
	get{
		return GlobalToLocal(Gravity,Controller);
	}
}
Vector3D Adjusted_Gravity{
	get{
		Vector3D temp=GlobalToLocal(Gravity,Controller);
		temp.Normalize();
		return temp*Mass_Accomodation;
	}
}
Vector3D Gravity_Direction{
	get{
		Vector3D direction=Gravity;
		direction.Normalize();
		return direction;
	}
}
double Speed_Deviation{
	get{
		return (CurrentVelocity-RestingVelocity).Length();
	}
}
Vector3D AngularVelocity;
Vector3D Relative_AngularVelocity{
	get{
		return GlobalToLocal(AngularVelocity,Controller);
	}
}

double Elevation;
double Sealevel;
Vector3D PlanetCenter;

bool ControllerFunction(IMyShipController ctr){
	IMyRemoteControl Remote=ctr as IMyRemoteControl;
	return ctr.IsSameConstructAs(Me)&&ctr.CanControlShip&&ctr.ControlThrusters&&(ctr.IsMainCockpit||Remote!=null);
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

bool Operational=false;
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
	Controller=GenericMethods<IMyShipController>.GetClosestFunc(ControllerFunction);
	if(Controller==null){
		Write("Failed to find Controller", false, false);
		return;
	}
	Forward=Controller.Orientation.Forward;
	Up=Controller.Orientation.Up;
	Left=Controller.Orientation.Left;
	Gyroscope=GenericMethods<IMyGyro>.GetConstruct("Control Gyroscope");
	if(Gyroscope==null){
		Gyroscope=GenericMethods<IMyGyro>.GetConstruct("");
		if(Gyroscope==null&&!Me.CubeGrid.IsStatic)
			return false;
	}
	if(Gyroscope!=null){
		Gyroscope.CustomName="Control Gyroscope";
		Gyroscope.GyroOverride=Controller.IsUnderControl;
	}
	List<IMyThrust> MyThrusters=GenericMethods<IMyThrust>.GetAllConstruct("");
	for(int i=0;i<2;i++){
		bool retry=!Me.CubeGrid.IsStatic;
		foreach(IMyThrust Thruster in MyThrusters){
			if(HasBlockData(Thruster, "Owner")){
				long ID=0;
				if(i==0&&!Int64.TryParse(GetBlockData(Thruster, "Owner"),out ID)||(ID!=0&&ID!=Me.CubeGrid.EntityId))
					continue;
			}
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
	Operational=Me.IsWorking;
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

public void Save(){
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
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

void UpdateSystemData(){
	Write("", false, false);
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
	Gravity=Controller.GetNaturalGravity();
	CurrentVelocity=Controller.GetShipVelocities().LinearVelocity;
	AngularVelocity=Controller.GetShipVelocities().AngularVelocity;
	Time_To_Crash=-1;
	Elevation=double.MaxValue;
	if(Controller.TryGetPlanetElevation(MyPlanetElevation.Sealevel,out Sealevel)){
		if(Controller.TryGetPlanetPosition(out PlanetCenter)){
			if(Sealevel<6000&&Controller.TryGetPlanetElevation(MyPlanetElevation.Surface,out Elevation)){
				if(Sealevel>5000){
					double difference=Sealevel-5000;
					Elevation=((Elevation*(1000-difference))+(Sealevel*difference))/1000;
				}
				else if(Elevation<50){
					double terrain_height=(Controller.GetPosition()-PlanetCenter).Length()-Elevation;
					List<IMyTerminalBlock> AllBlocks=new List<IMyTerminalBlock>();
					GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(AllBlocks);
					foreach(IMyTerminalBlock Block in AllBlocks)
						Elevation=Math.Min(Elevation, (Block.GetPosition()-PlanetCenter).Length()-terrain_height);
				}
			}
			else
				Elevation=Sealevel;
		}
		else
			PlanetCenter=new Vector3D(0,0,0);
	}
	else
		Sealevel=double.MaxValue;
	Mass_Accomodation=(float)(Controller.CalculateShipMass().PhysicalMass*Gravity.Length());
}

public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	UpdateSystemData();
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
}
