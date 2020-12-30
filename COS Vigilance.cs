/*
* Cosmic Engineering Vigilance Targeting System 
* Built by mlq1616
* https://github.com/mlq1819
*/
//Name me!
const string Program_Name="Vigilance AI";
//Sets the maximum firing distance
const double FIRING_DISTANCE=20000; //Recommended between 5k and 20k; works best within range
//The distance the scanners will default to during AutoScan; the lower the faster it scans
const double AUTOSCAN_DISTANCE=20000;//Recommended between 2k and 10k
//Sets whether the AI starts out scanning or whether it has to wait to be told to autoscan
const bool DEFAULT_AUTOSCAN=false;
//Sets whether the AI starts out automatically shooting enemies or whether it has to wait to be told to autofire
const bool DEFAULT_AUTOFIRE=false;
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

class EntityInfo{
	public long ID;
	public string Name;
	public MyDetectedEntityType Type;
	Vector3D? _hitposition;
	public Vector3D? HitPosition{
		get{
			return _hitposition;
		}
		set{
			_hitposition=value;
			if(_hitposition!=null){
				Size=Math.Max(Size, (Position-((Vector3D)_hitposition)).Length());
			}
		}
	}
	Vector3D _velocity;
	public Vector3D Velocity{
		get{
			return _velocity;
		}
		set{
			_velocity=value;
			Age=TimeSpan.Zero;
		}
	}
	public MyRelationsBetweenPlayerAndBlock Relationship;
	public Vector3D Position;
	public double Size=0;
	public TimeSpan Age=TimeSpan.Zero;
	public Vector3D TargetPosition{
		get{
			if(HitPosition!=null)
				return (Vector3D)HitPosition;
			return Position;
		}
	}
	
	public EntityInfo(long id, string name, MyDetectedEntityType type, Vector3D? hitposition, Vector3D velocity, MyRelationsBetweenPlayerAndBlock relationship, Vector3D position){
		ID=id;
		Name=name;
		Type=type;
		HitPosition=hitposition;
		Velocity=velocity;
		Relationship=relationship;
		Position=position;
		Age=TimeSpan.Zero;
	}
	
	public EntityInfo(long id, string name, MyDetectedEntityType type, Vector3D? hitposition, Vector3D velocity, MyRelationsBetweenPlayerAndBlock relationship, Vector3D position, double size) : this(id, name, type, hitposition, velocity, relationship, position){
		this.Size=size;
	}
	
	public EntityInfo(EntityInfo o){
		ID=o.ID;
		Name=o.Name;
		Type=o.Type;
		Position=o.Position;
		HitPosition=o.HitPosition;
		Velocity=o.Velocity;
		Relationship=o.Relationship;
		Size=o.Size;
		Age=o.Age;
	}
	
	public EntityInfo(MyDetectedEntityInfo entity_info){
		ID=entity_info.EntityId;
		Name=entity_info.Name;
		Type=entity_info.Type;
		Position=entity_info.Position;
		if(entity_info.HitPosition!=null){
			HitPosition=entity_info.HitPosition;
		}
		else {
			HitPosition=(Vector3D?) null;
		}
		Velocity=entity_info.Velocity;
		Relationship=entity_info.Relationship;
		Age=TimeSpan.Zero;
	}
	
	public EntityInfo(Vector3D position,Vector3D velocity,long id=-1):this(id,"unknown",MyDetectedEntityType.Unknown,null,velocity,MyRelationsBetweenPlayerAndBlock.Enemies,position){
		;
	}
	
	public static bool TryParse(string Parse, out EntityInfo Entity){
		Entity=new EntityInfo(-1,"bad", MyDetectedEntityType.None, null, new Vector3D(0,0,0), MyRelationsBetweenPlayerAndBlock.NoOwnership, new Vector3D(0,0,0));
		try{
			string[] args=Parse.Split('\n');
			long id;
			if(!Int64.TryParse(args[0], out id)){
				return false;
			}
			string name=args[1];
			MyDetectedEntityType type=(MyDetectedEntityType) Int32.Parse(args[2]);
			Vector3D? hitposition;
			if(args[3].Equals("null")){
				hitposition=(Vector3D?) null;
			}
			else {
				Vector3D temp;
				if(!Vector3D.TryParse(args[3], out temp)){
					return false;
				}
				else {
					hitposition=(Vector3D?) temp;
				}
			}
			Vector3D velocity;
			if(!Vector3D.TryParse(args[4], out velocity)){
				return false;
			}
			MyRelationsBetweenPlayerAndBlock relationship=(MyRelationsBetweenPlayerAndBlock) Int32.Parse(args[5]);
			Vector3D position;
			if(!Vector3D.TryParse(args[6], out position)){
				return false;
			}
			double size=0;
			if(!double.TryParse(args[7], out size)){
				size=0;
			}
			TimeSpan age;
			if(!TimeSpan.TryParse(args[8], out age)){
				return false;
			}
			Entity=new EntityInfo(id, name, type, hitposition, velocity, relationship, position, size);
			Entity.Age=age;
			return true;
		}
		catch(Exception){
			return false;
		}
	}
	
	public override string ToString(){
		string entity_info="";
		entity_info+=ID.ToString()+'\n';
		entity_info+=Name.ToString()+'\n';
		entity_info+=((int)Type).ToString()+'\n';
		if(HitPosition!=null){
			entity_info+=((Vector3D) HitPosition).ToString()+'\n';
		}
		else {
			entity_info+="null"+'\n';
		}
		entity_info+=Velocity.ToString()+'\n';
		entity_info+=((int)Relationship).ToString()+'\n';
		entity_info+=Position.ToString()+'\n';
		entity_info+=Size.ToString()+'\n';
		entity_info+=Age.ToString()+'\n';
		return entity_info;
	}
	
	public string NiceString(){
		string entity_info="";
		entity_info+="ID: "+ID.ToString()+'\n';
		entity_info+="Name: "+Name.ToString()+'\n';
		entity_info+="Type: "+Type.ToString()+'\n';
		if(HitPosition!=null){
			entity_info+="HitPosition: "+NeatVector((Vector3D) HitPosition)+'\n';
		}
		else {
			entity_info+="HitPosition: "+"null"+'\n';
		}
		entity_info+="Velocity: "+NeatVector(Velocity)+'\n';
		entity_info+="Relationship: "+Relationship.ToString()+'\n';
		entity_info+="Position: "+NeatVector(Position)+'\n';
		entity_info+="Size: "+((int)Size).ToString()+'\n';
		entity_info+="Data Age: "+Age.ToString()+'\n';
		return entity_info;
	}
	
	public static string NeatVector(Vector3D vector){
		return "X:"+Math.Round(vector.X,1).ToString()+" Y:"+Math.Round(vector.Y,1).ToString()+" Z:"+Math.Round(vector.Z,1).ToString();
	}
	
	public void Update(double seconds){
		Age=Prog.UpdateTimeSpan(Age,seconds);
	}
	
	public double GetDistance(Vector3D Reference){
		return (TargetPosition-Reference).Length();
	}
}

class EntityList:IEnumerable<EntityInfo>{
	List<EntityInfo> E_List;
	public IEnumerator<EntityInfo> GetEnumerator(){
		return E_List.GetEnumerator();
	}
	IEnumerator IEnumerable.GetEnumerator(){
		return GetEnumerator();
	}
	
	public int Count{
		get{
			return E_List.Count;
		}
	}
	
	public EntityInfo this[int key]{
		get{
			return E_List[key];
		}
		set{
			E_List[key]=value;
		}
	}
	
	public EntityList(){
		E_List=new List<EntityInfo>();
	}
	
	public void UpdatePositions(double seconds){
		foreach(EntityInfo entity in E_List)
			entity.Update(seconds);
	}
	
	public bool UpdateEntry(EntityInfo Entity){
		for(int i=0; i<E_List.Count; i++){
			if(E_List[i].ID==Entity.ID || (Entity.GetDistance(E_List[i].Position)<=0.5f&&Entity.Type==E_List[i].Type)){
				if(E_List[i].Age >= Entity.Age){
					E_List[i]=Entity;
					return true;
				}
				return false;
			}
		}
		E_List.Add(Entity);
		return true;
	}
	
	public bool RemoveEntry(EntityInfo Entity){
		for(int i=0; i<E_List.Count; i++){
			if(E_List[i].ID==Entity.ID || (Entity.GetDistance(E_List[i].Position)<=0.5f&&Entity.Type==E_List[i].Type)){
				E_List.RemoveAt(i);
				return true;
			}
		}
		return false;
	}
	
	public void RemoveAt(int index){
		E_List.RemoveAt(index);
	}
	
	public EntityInfo Get(long ID){
		foreach(EntityInfo entity in E_List){
			if(entity.ID==ID)
				return entity;
		}
		return null;
	}
	
	public double ClosestDistance(MyRelationsBetweenPlayerAndBlock Relationship, double min_size=0){
		double min_distance=double.MaxValue;
		foreach(EntityInfo entity in E_List){
			if(entity.Size >= min_size && entity.Relationship==Relationship)
				min_distance=Math.Min(min_distance, (Prog.P.Me.GetPosition()-entity.Position).Length()-entity.Size);
		}
		return min_distance;
	}
	
	public double ClosestDistance(double min_size=0){
		double min_distance=double.MaxValue;
		foreach(EntityInfo entity in E_List){
			if(entity.Size >= min_size)
				min_distance=Math.Min(min_distance, (Prog.P.Me.GetPosition()-entity.Position).Length()-entity.Size);
		}
		return min_distance;
	}
	
	public void Clear(){
		E_List.Clear();
	}
	
	public void Sort(Vector3D Reference){
		Queue<EntityInfo> Unsorted=new Queue<EntityInfo>();
		double last_distance=0;
		for(int i=0;i<E_List.Count;i++){
			double distance=E_List[i].GetDistance(Reference);
			if(distance<last_distance){
				Unsorted.Enqueue(E_List[i]);
				E_List.RemoveAt(i);
				i--;
				continue;
			}
			last_distance=distance;
		}
		while(Unsorted.Count>0){
			double distance=Unsorted.Peek().GetDistance(Reference);
			int upper=E_List.Count;
			int lower=0;
			int index;
			double down;
			double up;
			do{
				index=(upper-lower)/2+lower;
				down=0;
				if(index>0)
					down=E_List[index-1].GetDistance(Reference);
				up=E_List[index].GetDistance(Reference);
				if(down>distance)
					upper=index-1;
				else
					lower=index-1;
				if(up<distance)
					lower=index;
				else
					upper=index;
			}
			while((down>distance||up<distance)&&upper!=lower);
			E_List.Insert(index,Unsorted.Dequeue());
		}
		if(E_List.Count>128)
			E_List.RemoveRange(128,E_List.Count-128);
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
List<IMyCameraBlock> Cameras;
List<IMyGravityGenerator> Generators;
IMyCockpit Cockpit;


List<IMyInteriorLight> FiringLights;
List<List<IMyInteriorLight>> StatusLights;
List<IMyTextPanel> StatusPanels;

EntityList Targets=new EntityList();

bool AutoFire=DEFAULT_AUTOFIRE;
bool AutoScan=DEFAULT_AUTOSCAN;
EntityInfo Target{
	get{
		if(Targets.Count>0)
			return Targets[0];
		return new EntityInfo(0, "invalid", MyDetectedEntityType.None, null, new Vector3D(0,0,0), MyRelationsBetweenPlayerAndBlock.Neutral, new Vector3D(0,0,0), 0);
	}
}
Vector3D Target_Position{
	get{
		if(Target.HitPosition!=null)
			return(Vector3D)Target.HitPosition;
		return Target.Position;
	}
}

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

Vector3D Suspect_Position=new Vector3D(0,0,0);
Vector3D Suspect_Velocity=new Vector3D(0,0,0);
Vector3D Suspect_Direction{
	get{
		Vector3D output=Suspect_Position-Controller.GetPosition();
		output.Normalize();
		return output;
	}
}
double Suspect_Distance{
	get{
		return (Suspect_Position-Controller.GetPosition()).Length();
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
		return (Target_Position-Aim_Position).Length()/Target.Velocity.Length();
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
	
	Cockpit=GenericMethods<IMyCockpit>.GetFull("Vigilance Control Cockpit");
	if(Cockpit!=null){
		for(int i=0;i<Cockpit.SurfaceCount;i++){
			if(i<3)
				Cockpit.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
			Cockpit.GetSurface(i).FontSize=2.2f;
			Cockpit.GetSurface(i).TextPadding=20;
			Cockpit.GetSurface(i).FontColor=DEFAULT_TEXT_COLOR;
			Cockpit.GetSurface(i).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
			Cockpit.GetSurface(i).Alignment=TextAlignment.CENTER;
			Cockpit.GetSurface(i).ScriptForegroundColor=DEFAULT_TEXT_COLOR;
			Cockpit.GetSurface(i).ScriptBackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
	}
	
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
	Cameras=GenericMethods<IMyCameraBlock>.GetAllContaining("Driver Camera ");
	if(Cameras.Count==0)
		return;
	foreach(IMyCameraBlock Camera in Cameras)
		Camera.EnableRaycast=true;
	
	FiringLights=GenericMethods<IMyInteriorLight>.GetAllContaining("Driver Firing Light ");
	StatusLights=new List<List<IMyInteriorLight>>();
	for(int i=1;i<=3;i++){
		StatusLights.Add(GenericMethods<IMyInteriorLight>.GetAllContaining("Driver Status Light "+i.ToString()));
	}
	
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
				EntityInfo output;
				if(EntityInfo.TryParse(arg.Substring("Target:".Length),out output))
					Targets.UpdateEntry(output);
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
			else if(arg.IndexOf("AutoScan:")==0){
				string word=arg.Substring("AutoScan:".Length);
				bool.TryParse(word,out AutoScan);
			}
			else if(arg.IndexOf("AutoFire:")==0){
				string word=arg.Substring("AutoFire:".Length);
				bool.TryParse(word,out AutoFire);
			}
		}
		catch(Exception){
			continue;
		}
	}
	
	
	if(TaskQueue.Count==0&&AutoScan)
		TaskQueue.Enqueue(CannonTask.Scan);
	Fire_Count=3;
	
	Operational=true;
	IGC.RegisterBroadcastListener("Vigilance AI");
	IGC.RegisterBroadcastListener("Entity Report");
	IGC.RegisterBroadcastListener(Me.CubeGrid.CustomName);
	if(((int)CurrentTask)>=((int)CannonTask.Fire))
		Runtime.UpdateFrequency=UpdateFrequency.Update10;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

public void Save(){
    this.Storage="FireStatus:"+((int)CurrentFireStatus).ToString();
	this.Storage+="•PrintStatus:"+((int)CurrentPrintStatus).ToString();
	this.Storage+="•AutoScan:"+AutoScan.ToString();
	this.Storage+="•AutoFire:"+AutoFire.ToString();
	while(TaskQueue.Count>0){
		this.Storage+="•Task:"+((int)CurrentTask).ToString();
		TaskQueue.Dequeue();
	}
	foreach(EntityInfo Entity in Targets){
		this.Storage+="•Target:"+Entity.ToString();
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
		Fire_Count=3;
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
		else if(Targets.Count>0){
			Targets.RemoveAt(0);
			if(Targets.Count>0){
				Fire_Count=3;
				remove=false;
			}
		}
	}
	if(remove){
		if(TaskQueue.Count>0)
			TaskQueue.Dequeue();
		if(TaskQueue.Count==0){
			if(AutoScan)
				TaskQueue.Enqueue(CannonTask.Scan);
			else if(last_task!=CannonTask.Reset)
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
	
	Targets.UpdatePositions(seconds_since_last_update);
	Suspect_Position+=Suspect_Velocity*seconds_since_last_update;
}

double Print_Timer=0.0;
void Print(){
	double rotor_angle=ShellRotor.Angle/Math.PI*180;
	bool is_forward=Math.Abs((rotor_angle+365)%360)-5<1;
	bool is_backward=Math.Abs(rotor_angle-180)<1;
	bool is_printed=Projector.RemainingBlocks==0;
	
	ShellRotor.RotorLock=false;
	if(is_printed){
		Welder.Enabled=false;
		ShellRotor.TargetVelocityRPM=30.0f;
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
			else{
				CurrentPrintStatus=PrintStatus.WaitingResources;
				string Message="Waiting for resources";
				IGC.SendBroadcastMessage("Vigilance AI", Message, TransmissionDistance.TransmissionDistanceMax);
				IGC.SendBroadcastMessage(Me.CubeGrid.CustomName, Message, TransmissionDistance.TransmissionDistanceMax);
			}
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
	if(Target.Velocity.Length()<0.1)
		return;
	if(Target.Velocity.Length()>100){
		if(Targets.Count>0)
			Targets.RemoveAt(0);
		return;
	}
	double difference=Time_To_Position-(Time_To_Hit+Aim_Timer);
	while(difference<0){
		Aim_Position+=Math.Max(1,difference/2)*Target.Velocity;
		if(Aim_Distance>FIRING_DISTANCE){
			if(Targets.Count>0){
				Targets.RemoveAt(0);
			}
			return;
		}
		difference=Time_To_Position-(Time_To_Hit+Aim_Timer);
	}
}

void SendAllListeners(string Message){
	List<IMyBroadcastListener> listeners=new List<IMyBroadcastListener>();
	IGC.GetBroadcastListeners(listeners);
	foreach(IMyBroadcastListener Listener in listeners){
		IGC.SendBroadcastMessage(Listener.Tag, Message, TransmissionDistance.TransmissionDistanceMax);
	}
}
double Standard_Scan_Time=3;
void Standard_Scan(){
	EntityList DetectedEntities=new EntityList();
	List<IMyLargeTurretBase> AllTurrets=new List<IMyLargeTurretBase>();
	GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(AllTurrets);
	foreach(IMyLargeTurretBase Turret in AllTurrets){
		if(Turret.HasTarget){
			MyDetectedEntityInfo Detected=Turret.GetTargetedEntity();
			if(Detected.Type!=MyDetectedEntityType.None&&Detected.EntityId!=Controller.CubeGrid.EntityId)
				DetectedEntities.UpdateEntry(new EntityInfo(Detected));
		}
	}
	if(Targets.Count>0){
		bool hit=false;
		foreach(IMyCameraBlock Camera in Cameras){
			if(Camera.CanScan(Target_Position)){
				MyDetectedEntityInfo Detected=Camera.Raycast(Target_Position);
				if(Detected.Type!=MyDetectedEntityType.None&&Detected.EntityId!=Controller.CubeGrid.EntityId){
					DetectedEntities.UpdateEntry(new EntityInfo(Detected));
					hit=true;
					break;
				}
			}
		}
		if(!hit){
			foreach(IMyCameraBlock Camera in Cameras){
				double distance=(Target_Position-Controller.GetPosition()).Length()+10;
				if(Camera.AvailableScanRange>=distance){
					for(int i=0;i<4;i++){
						int p_x=1;
						int y_x=1;
						if(i%2==0)
							p_x=-1;
						if(i<2)
							y_x=-1;
						if(Camera.CanScan(distance)){
							MyDetectedEntityInfo Detected=Camera.Raycast(distance,(float)(Precision*10*p_x),(float)(Precision*10*y_x));
							if(Detected.Type!=MyDetectedEntityType.None&&Detected.EntityId!=Controller.CubeGrid.EntityId){
								DetectedEntities.UpdateEntry(new EntityInfo(Detected));
								hit=true;
								break;
							}
						}
					}
				}
			}
		}
	}
	if(AutoFire){
		foreach(EntityInfo Entity in DetectedEntities){
			double distance=(Controller.GetPosition()-Entity.Position).Length();
			if(Entity.Type==MyDetectedEntityType.LargeGrid&&Entity.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies&&distance<FIRING_DISTANCE&&distance>100){
				Targets.UpdateEntry(Entity);
				if(Target.ID==Entity.ID)
					SetAimed(Aim_Timer+0.5);
				if(CurrentTask!=CannonTask.Fire)
					AddTask(CannonTask.Fire);
			}
		}
	}
	EntityInfo Myself = new EntityInfo(Controller.CubeGrid.EntityId,Controller.CubeGrid.CustomName,MyDetectedEntityType.LargeGrid,(Vector3D?)(Controller.GetPosition()),new Vector3D(0,0,0),MyRelationsBetweenPlayerAndBlock.Owner,Controller.CubeGrid.GetPosition());
	SendAllListeners(Myself.ToString());
	foreach(EntityInfo Entity in DetectedEntities){
		SendAllListeners(Entity.ToString());
	}
	
	List<IMyBroadcastListener> listeners=new List<IMyBroadcastListener>();
	IGC.GetBroadcastListeners(listeners);
	foreach(IMyBroadcastListener Listener in listeners){
		while(Listener.HasPendingMessage){
			MyIGCMessage message=Listener.AcceptMessage();
			ArgumentProcessor(message.Data.ToString());
		}
	}
	Standard_Scan_Time=0;
}

void ArgumentProcessor(string argument){
	if(argument.ToLower().Equals("scan")){
		AddTask(CannonTask.Scan);
	}
	else if(argument.ToLower().IndexOf("autoscan")==0){
		string word=argument.ToLower().Substring("autoscan".Length);
		if(word.Length==0||word.Contains("toggle")||word.Contains("switch"))
			AutoScan=!AutoScan;
		else if(word.Contains("on")||word.Contains("enabled")||word.Contains("true"))
			AutoScan=true;
		else if(word.Contains("off")||word.Contains("disabled")||word.Contains("false"))
			AutoScan=false;
		if(AutoScan&&CurrentTask==CannonTask.None)
			AddTask(CannonTask.Scan);
		else if((!AutoScan)&&CurrentTask==CannonTask.Scan)
			NextTask();
	}
	else if(argument.ToLower().IndexOf("autofire")==0){
		string word=argument.ToLower().Substring("autofire".Length);
		if(word.Length==0||word.Contains("toggle")||word.Contains("switch"))
			AutoFire=!AutoFire;
		else if(word.Contains("on")||word.Contains("enabled")||word.Contains("true"))
			AutoFire=true;
		else if(word.Contains("off")||word.Contains("disabled")||word.Contains("false"))
			AutoFire=false;
	}
	else if(argument.ToLower().IndexOf("search:")==0){
		string[] words=argument.ToLower().Substring("search:".Length).Split(';');
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
				if(set){
					Position=Waypoint.Coords;
				}
			}
			else
				set=true;
			if(set){
				Vector3D Velocity=new Vector3D(0,0,0);
				if(words.Length>1){
					Vector3D.TryParse(words[1],out Velocity);
				}
				Targets.UpdateEntry(new EntityInfo(Position,Velocity));
				AddTask(CannonTask.Search);
			}
		}
	}
	else if(argument.ToLower().Equals("dumbfire")){
		Targets.UpdateEntry(new EntityInfo(Forward_Vector*Math.Min(FIRING_DISTANCE,1000)+Controller.GetPosition(),new Vector3D(0,0,0)));
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
				Vector3D Velocity=new Vector3D(0,0,0);
				if(words.Length>1){
					Vector3D.TryParse(words[1],out Velocity);
				}
				Targets.UpdateEntry(new EntityInfo(Position,Velocity));
				AddTask(CannonTask.Fire);
				SetAimed();
			}
		}
	}
	else if(argument.ToLower().Equals("reset")){
		TaskQueue.Clear();
		AddTask(CannonTask.Reset);
	}
	else if(argument.ToLower().IndexOf("suspect:")==0){
		string[] words=argument.ToLower().Substring("suspect:".Length).Split(';');
		if(words.Length>0){
			bool set=false;
			Suspect_Position=new Vector3D(0,0,0);
			if(!Vector3D.TryParse(words[0],out Suspect_Position)){
				MyWaypointInfo Waypoint;
				if(!MyWaypointInfo.TryParse(words[0],out Waypoint)){
					if(MyWaypointInfo.TryParse(words[0].Substring(0,words[0].Length-10),out Waypoint))
						set=true;
				}
				else
					set=true;
				if(set)
					Suspect_Position=Waypoint.Coords;
			}
			else
				set=true;
			if(set){
				Suspect_Velocity=new Vector3D(0,0,0);
				if(words.Length>1){
					Vector3D.TryParse(words[1],out Suspect_Velocity);
				}
				AddTask(CannonTask.Search);
			}
		}
	}
	else{
		if(AutoFire){
			try{
				bool was_empty=(Targets.Count==0);
				EntityInfo Entity;
				if(EntityInfo.TryParse(argument,out Entity)){
					double distance=(Controller.GetPosition()-Entity.Position).Length();
					if(Entity.Type==MyDetectedEntityType.LargeGrid&&Entity.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies&&distance<AUTOSCAN_DISTANCE&&distance>100){
						Targets.UpdateEntry(Entity);
						if(was_empty)
							AddTask(CannonTask.Fire);
						if(Target.ID==Entity.ID){
							SetAimed(Aim_Timer);
						}
					}
				}
			}
			catch(Exception){
				;
			}
		}
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
	if(Math.Abs(From_Top)>45+Yaw_Difference)
		return false;
	return true;
}

void Aim(Vector3D Direction, double precision){
	double Pitch_Difference=GetAngle(Up_Vector,Direction)-GetAngle(Down_Vector,Direction);
	PitchRotor.TargetVelocityRPM=(float)Math.Min(Math.Max((Pitch_Difference*Math.Min(1,Math.Max(Math.Abs(Pitch_Difference)/10,precision*10))),-15),15);
	if(Math.Abs(Pitch_Difference)<precision/2)
		PitchRotor.TargetVelocityRPM=0;
	double Yaw_Difference=GetAngle(Left_Vector,Direction)-GetAngle(Right_Vector,Direction);
	YawRotor.TargetVelocityRPM=(float)Math.Min(Math.Max((Yaw_Difference*Math.Min(1,Math.Max(Math.Abs(Yaw_Difference)/10,precision*10))),-15),15);
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
		float target_rpm=(float)Math.Max(-30,Math.Min(30, YawAngle*-.1));
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
		float target_rpm=(float)Math.Max(-30,Math.Min(30, PitchAngle*-.1));
		PitchRotor.TargetVelocityRPM=target_rpm;
		Write("Pitch Target:"+Math.Round(target_rpm,1).ToString()+"RPM");
	}
	else{
		PitchRotor.TargetVelocityRPM=0;
		PitchRotor.RotorLock=true;
	}
	if(!moving)
		NextTask();
	Targets.Clear();
}

double Scan_Timer=AUTOSCAN_DISTANCE/1000;
double Scan_Aim_Time=10;
bool Has_Done_Scan=false;
int Detection_Count=0;
void Perform_Search_Scan(Vector3D Direction, double Scan_Distance){
	Aim(Direction, 1);
	if(GetAngle(Direction,Forward_Vector)<1){
		PitchRotor.TargetVelocityRPM=0;
		PitchRotor.RotorLock=true;
		YawRotor.TargetVelocityRPM=0;
		YawRotor.RotorLock=true;
	}
	else{
		PitchRotor.RotorLock=false;
		YawRotor.RotorLock=false;
	}
	Write("Scan_Aim_Time:"+Math.Round(Scan_Aim_Time,1)+"/10 seconds");
	if(Has_Done_Scan)
		Write("Scan Status:Done: Found "+Detection_Count+" Entities");
	else
		Write("Scan Status:Aiming");
	foreach(IMyCameraBlock Camera in Cameras){
		double distance=Camera.AvailableScanRange;
		string distance_string=Math.Round(distance,0).ToString()+"M";
		if(distance>=1000)
			distance_string=Math.Round(distance/1000,1).ToString()+"kM";
		Write(Camera.CustomName+":"+distance_string);
	}
	if(!Has_Done_Scan && GetAngle(Forward_Vector,Direction)<1){
		EntityList DetectedEntities=new EntityList();
		for(int i=0;i<Cameras.Count;i++){
			Cameras[i].CustomName="Driver Camera "+(i+1).ToString();
			IMyCameraBlock Camera=Cameras[i];
			if(i==0){
				MyDetectedEntityInfo Entity=Camera.Raycast(Math.Min(Scan_Distance,Camera.AvailableScanRange),0,0);
				if(Entity.Type!=MyDetectedEntityType.None&&Entity.EntityId!=Controller.CubeGrid.EntityId)
					DetectedEntities.UpdateEntry(new EntityInfo(Entity));
				continue;
			}
			double distance=Math.Min(Camera.AvailableScanRange/(4*i),Scan_Distance);
			int degrees=(int)(Camera.RaycastConeLimit/i);
			for(int j=0;j<i;j++){
				float pitch,yaw;
				int lower=degrees*j;
				int upper=degrees*(j+1);
				do{
					pitch=(float)Rnd.Next(lower*10,upper*10)/10.0f;
					yaw=(float)Rnd.Next(lower*10,upper*10)/10.0f;
				}
				while(Math.Sqrt(Math.Pow(pitch,2)+Math.Pow(yaw,2))>Camera.RaycastConeLimit);
				for(int k=0;k<4;k++){
					int p_x=1;
					if(k%2==0)
						p_x=-1;
					int y_x=1;
					if(k<2)
						y_x=-1;
					MyDetectedEntityInfo Entity=Camera.Raycast(Math.Min(distance,Camera.AvailableScanRange),pitch*p_x,yaw*y_x);
					if(Entity.Type!=MyDetectedEntityType.None&&Entity.EntityId!=Controller.CubeGrid.EntityId)
						DetectedEntities.UpdateEntry(new EntityInfo(Entity));
				}
			}
		}
		if(AutoFire){
			foreach(EntityInfo Entity in DetectedEntities){
				double distance=(Controller.GetPosition()-Entity.Position).Length();
				if(Entity.Type==MyDetectedEntityType.LargeGrid&&Entity.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies&&distance<FIRING_DISTANCE&&distance>100){
					Targets.UpdateEntry(Entity);
					if(Target.ID==Entity.ID)
						SetAimed(Aim_Timer);
					if(CurrentTask!=CannonTask.Fire)
					AddTask(CannonTask.Fire);
				}
			}
		}
		foreach(EntityInfo Entity in DetectedEntities){
			SendAllListeners(Entity.ToString());
		}
		Detection_Count=DetectedEntities.Count;
		Has_Done_Scan=true;
	}
}

Vector3D Scan_Direction=new Vector3D(0,0,0);
public void Scan(){
	Write("Scan_Timer:"+Math.Round(Scan_Timer,1)+"/"+Math.Round(AUTOSCAN_DISTANCE/1000,1)+" seconds");
	double Angle=GetAngle(Scan_Direction,Forward_Vector);
	Write(Math.Round(Angle,1).ToString()+"°");
	if(Scan_Timer>=AUTOSCAN_DISTANCE/1000||!CanAim(Scan_Direction)||Scan_Aim_Time>=5){
		Has_Done_Scan=false;
		Scan_Aim_Time=0;
		Scan_Timer=0;
		Detection_Count=0;
		do{
			double Pitch_Angle=PitchRotor.Angle/Math.PI*180;
			if(Pitch_Angle>180)
				Pitch_Angle-=360;
			Scan_Direction=new Vector3D(0,0,0);
			Scan_Direction+=Rnd.Next(0,36)*Forward_Vector;
			Scan_Direction+=Rnd.Next(-36,36)*Left_Vector;
			if(Pitch_Angle>=0)
				Scan_Direction+=Rnd.Next((int)(Pitch_Angle-30),30)*Up_Vector;
			else
				Scan_Direction+=Rnd.Next(-30,(int)(30+Pitch_Angle))*Up_Vector;
			if(Scan_Direction.Length()==0)
				Scan_Direction=new Vector3D(1,1,1);
			Scan_Direction.Normalize();
		}
		while(!CanAim(Scan_Direction));
	}
	Perform_Search_Scan(Scan_Direction,AUTOSCAN_DISTANCE);
}

Vector3D Search_Direction=new Vector3D(0,0,0);
double Search_Distance=0;
double Search_Timer=0;
public void Search(){
	Write("Scan_Timer:"+Math.Round(Scan_Timer,1)+"/"+Math.Round(AUTOSCAN_DISTANCE/1000,1)+" seconds");
	if(!CanAim(Suspect_Direction)||Suspect_Distance>AUTOSCAN_DISTANCE||Search_Timer>180){
		NextTask();
		return;
	}
	if(Scan_Timer>=AUTOSCAN_DISTANCE/1000||!CanAim(Search_Direction)||Scan_Aim_Time>=5){
		Has_Done_Scan=false;
		Scan_Aim_Time=0;
		Scan_Timer=0;
		Detection_Count=0;
		do{
			int x=Rnd.Next(-36,36);
			int y=Rnd.Next(-36,36);
			int z=Rnd.Next(-36,36);
			Vector3D Random_Direction=new Vector3D(x,y,z);
			if(Random_Direction.Length()==0)
				Random_Direction=new Vector3D(1,1,1);
			Random_Direction.Normalize();
			Random_Direction*=Rnd.Next(600,800);
			Random_Direction+=Suspect_Position+AIM_TIME*Suspect_Velocity;
			Search_Direction=Random_Direction-Controller.GetPosition();
			Search_Distance=Search_Direction.Length();
			Search_Direction.Normalize();
		}
		while(!CanAim(Search_Direction));
	}
	Perform_Search_Scan(Search_Direction,Search_Distance+100);
}

List<double> ShellCountdowns=new List<double>();
bool Called_Next_Fire=true;
double Fire_Scan_Timer=AUTOSCAN_DISTANCE/1000;
bool DoFire(){
	foreach(IMyCameraBlock Camera in Cameras){
		double distance=Camera.AvailableScanRange;
		string distance_string=Math.Round(distance,0).ToString()+"M";
		if(distance>=1000)
			distance_string=Math.Round(distance/1000,1).ToString()+"kM";
		Write(Camera.CustomName+":"+distance_string);
	}
	if(Fire_Scan_Timer>=AUTOSCAN_DISTANCE/1000){
		Fire_Scan_Timer=0;
		foreach(IMyCameraBlock Camera in Cameras){
			MyDetectedEntityInfo Entity=Camera.Raycast(Math.Min(AUTOSCAN_DISTANCE,Math.Max(50,Aim_Distance/10*9)),0,0);
			if(Entity.Type!=MyDetectedEntityType.None&&Entity.Relationship!=MyRelationsBetweenPlayerAndBlock.Enemies&&(((Vector3D) Target.HitPosition)-Aim_Position).Length()>5)
				return false;
		}
	}
	else
		return false;
	
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
	if(Aim_Distance>FIRING_DISTANCE||Target.ID==0||!can_aim){
		Write("Aim_Distance:"+Math.Round(Aim_Distance/1000,2).ToString()+"kM");
		Write("Target.ID:"+Target.ID);
		Write("can_aim:"+can_aim.ToString());
		NextTask();
		return;
	}
	if(CurrentFireStatus==FireStatus.Firing)
		return;
	bool is_aimed=GetAngle(Forward_Vector,Aim_Direction)<=Precision;
	bool is_clear=(!Sensor.IsActive);
	bool is_printed=CurrentPrintStatus==PrintStatus.Ready&&Merge.Enabled&&Projector.RemainingBlocks==0;
	bool is_ready=(Target.Velocity.Length()<0.1)||(Math.Abs(Time_To_Hit-Time_To_Position)<1.2);
	Write("Time_To_Hit:"+Math.Round(Time_To_Hit,2).ToString()+" seconds");
	Write("Time_To_Position:"+Math.Round(Time_To_Position,2).ToString()+" seconds");
	Write("Time to Launch:"+Math.Round(Time_To_Position-Time_To_Hit,2).ToString()+" seconds");
	Write("Aim vs Actual:"+Math.Round((Aim_Position-Target_Position).Length(),1)+"M");
	Write("Target Position:"+EntityInfo.NeatVector(Target_Position));
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
		PitchRotor.TargetVelocityRPM=input_pitch*-10;
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
	if(Standard_Scan_Time<1)
		Standard_Scan_Time+=seconds_since_last_update;
	if(Aim_Timer>0)
		Aim_Timer=Math.Max(0,Aim_Timer-seconds_since_last_update);
	if(Scan_Timer<AUTOSCAN_DISTANCE/1000&&GetAngle(Forward_Vector,Scan_Direction)<1)
		Scan_Timer+=seconds_since_last_update;
	if(Scan_Aim_Time<5&&GetAngle(Forward_Vector,Scan_Direction)>20)
		Scan_Aim_Time+=seconds_since_last_update;
	if(Fire_Scan_Timer<AUTOSCAN_DISTANCE/1000)
		Fire_Scan_Timer+=seconds_since_last_update;
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
	if(Standard_Scan_Time>=1)
		Standard_Scan();
	if(AutoScan&&CurrentTask==CannonTask.None)
		NextTask();
	Print();
	switch(CurrentTask){
		case CannonTask.None:
			if(Targets.Count>0)
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
			Scan();
			break;
		case CannonTask.Search:
			Search();
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
	foreach(IMyGravityGenerator Generator in Generators){
		Generator.Enabled=((CurrentFireStatus==FireStatus.Firing)||(Sensor.IsActive));
	}
	if(AutoScan)
		Write("AutoScan:On");
	else
		Write("AutoScan:Off");
	if(AutoFire)
		Write("AutoFire:On");
	else
		Write("AutoFire:Off");
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
	if(Cockpit!=null){
		if(Cockpit.SurfaceCount>=1){
			Color Text_Color=Green_Text;
			Color Back_Color=Green_Back;
			Cockpit.GetSurface(0).WriteText("Current Task\n"+CurrentTask.ToString(),false);
			switch(CurrentTask){
				case CannonTask.Scan:
					Text_Color=Blue_Text;
					Back_Color=Blue_Back;
					break;
				case CannonTask.Reset:
					Text_Color=Yellow_Text;
					Back_Color=Yellow_Back;
					break;
				case CannonTask.Search:
					Text_Color=Orange_Text;
					Back_Color=Orange_Back;
					break;
				case CannonTask.Fire:
					Text_Color=Red_Text;
					Back_Color=Red_Back;
					break;
				case CannonTask.Manual:
					Text_Color=Blue_Text;
					Back_Color=Blue_Back;
					break;
			}
			Cockpit.GetSurface(0).FontColor=Text_Color;
			Cockpit.GetSurface(0).BackgroundColor=Back_Color;
		}
		if(Cockpit.SurfaceCount>=2){
			Color Text_Color=Green_Text;
			Color Back_Color=Green_Back;
			Cockpit.GetSurface(1).WriteText("Firing Status\n"+CurrentFireStatus.ToString(),false);
			switch(CurrentFireStatus){
				case FireStatus.WaitingClear:
					Text_Color=Blue_Text;
					Back_Color=Blue_Back;
					break;
				case FireStatus.Printing:
					Text_Color=Yellow_Text;
					Back_Color=Yellow_Back;
					break;
				case FireStatus.Aiming:
					Text_Color=Orange_Text;
					Back_Color=Orange_Back;
					break;
				case FireStatus.WaitingTarget:
					Text_Color=Orange_Text;
					Back_Color=Orange_Back;
					break;
				case FireStatus.Firing:
					Text_Color=Red_Text;
					Back_Color=Red_Back;
					break;
			}
			Cockpit.GetSurface(1).FontColor=Text_Color;
			Cockpit.GetSurface(1).BackgroundColor=Back_Color;
		}
		if(Cockpit.SurfaceCount>=3){
			Color Text_Color=Green_Text;
			Color Back_Color=Green_Back;
			Cockpit.GetSurface(2).WriteText("Print Status\n"+CurrentPrintStatus.ToString(),false);
			switch(CurrentPrintStatus){
				case PrintStatus.StartingPrint:
					Text_Color=Blue_Text;
					Back_Color=Blue_Back;
					break;
				case PrintStatus.EndingPrint:
					Text_Color=Blue_Text;
					Back_Color=Blue_Back;
					break;
				case PrintStatus.Printing:
					Text_Color=Yellow_Text;
					Back_Color=Yellow_Back;
					break;
				case PrintStatus.WaitingResources:
					Text_Color=Red_Text;
					Back_Color=Red_Back;
					break;
			}
			Cockpit.GetSurface(2).FontColor=Text_Color;
			Cockpit.GetSurface(2).BackgroundColor=Back_Color;
		}
	}
	if(CurrentTask==CannonTask.Fire)
		Write("Fire_Count:"+Fire_Count.ToString());
	Write("PrintStatus:"+CurrentPrintStatus.ToString());
	Write("FireStatus:"+CurrentFireStatus.ToString());
	if(Targets.Count>0){
		Write("Distance:"+Math.Round(Aim_Distance/1000,1)+"kM");
		Write("Precision:"+Math.Round(GetAngle(Forward_Vector,Aim_Direction),3)+"°/"+Math.Round(Precision,3).ToString()+"°");
		Write(Targets.Count.ToString()+" Targets");
		for(int i=0;i<Targets.Count;i++){
			double distance=(Targets[i].Position-Controller.GetPosition()).Length();
			Write("  Target "+(i+1).ToString()+":"+Targets[i].ID.ToString()+':'+Math.Round(distance/1000,2)+"kM");
		}
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
		Runtime.UpdateFrequency=UpdateFrequency.Update10;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}
