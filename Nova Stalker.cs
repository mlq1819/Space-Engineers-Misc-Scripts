/*
* Nova Stalker
* Built by mlq1616
* https://github.com/mlq1819/Space-Engineers-Misc-Scripts
* This script is meant for stalker drones.
*/
string Program_Name="Nova Stalker";
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);
double Speed_Limit=100;
double Acceptable_Angle=5;
bool Control_Gyroscopes=true;
bool Control_Thrusters=true;

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
	
	public static double GetAngle(MatrixD m1,MatrixD v2){
		return Math.Max(GetAngle(m1.Forward,m2.Forward),GetAngle(m1.Up,m2.Up));
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
	
	public static implicit operator T(Roo<T> R){
		return R.Value;
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
bool CanHaveJob(IMyTerminalBlock Block, string JobName){
	return (!HasBlockData(Block,"Job"))||GetBlockData(Block,"Job").Equals("None")||GetBlockData(Block, "Job").Equals(JobName);
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
	IMyTextSurface Surface=Me.GetSurface(0);
	if(new_line){
		Vector2 SurfaceSize=Surface.SurfaceSize;
		string[] Full_Lines=text.Split('\n');
		if(!append)
			Surface.WriteText("",false);
		foreach(string Full_Line in Full_Lines){
			Vector2 StringSize=Surface.MeasureStringInPixels(new StringBuilder(Full_Line),Surface.Font,Surface.FontSize);
			int min_lines=(int)Math.Ceiling(((float)SurfaceSize.X)/StringSize.X);
			string[] words=Full_Line.Split(' ');
			string current_line="";
			for(int i=0;i<words.Length;i++){
				string next_line=current_line;
				if(current_line.Length>0)
					next_line+=' ';
				next_line+=words[i];
				if(current_line.Length>0&&Surface.MeasureStringInPixels(new StringBuilder(next_line),Surface.Font,Surface.FontSize).X>SurfaceSize.X){
					Surface.WriteText(current_line+'\n',true);
					current_line="";
				}
				if(current_line.Length>0)
					current_line+=' ';
				current_line+=words[i];
			}
			if(current_line.Length>0)
				Surface.WriteText(current_line+'\n',true);
		}
	}
	else
		Surface.WriteText(text,append);
}

int Display_Count=1;
int Current_Display=1;
double Display_Timer=5;
void Display(int display_number,string text,bool new_line=true,bool append=true){
	if(display_number==Current_Display)
		Write(text,new_line,append);
	else
		Echo(text);
}
void UpdateMyDisplay(){
	IMyTextSurface Display=Me.GetSurface(0);
	switch(Current_Display){
		case 5:
			Display.FontColor=DEFAULT_TEXT_COLOR;
			Display.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
			Display.Alignment=TextAlignment.LEFT;
			Display.ContentType=ContentType.TEXT_AND_IMAGE;
			Display.Font="Monospace";
			Display.TextPadding=0;
			Display.FontSize=1.25f;
			break;
		default:
			Display.FontColor=DEFAULT_TEXT_COLOR;
			Display.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
			Display.Alignment=TextAlignment.CENTER;
			Display.ContentType=ContentType.TEXT_AND_IMAGE;
			Display.Font="Debug";
			Display.TextPadding=2;
			Display.FontSize=1;
			break;
	}
}

string GetRemovedString(string big_string, string small_string){
	string output=big_string;
	if(big_string.Contains(small_string)){
		output=big_string.Substring(0, big_string.IndexOf(small_string))+big_string.Substring(big_string.IndexOf(small_string)+small_string.Length);
	}
	return output;
}

struct CustomPanel{
	public IMyTextSurface Display;
	public bool Trans;
	public CustomPanel(IMyTextSurface d,bool t=false){
		Display=d;
		Trans=t;
	}
	public CustomPanel(IMyTextPanel p){
		Display=p as IMyTextSurface;
		Trans=p.CustomName.ToLower().Contains("transparent");
	}
}

class Dock{
	private IMyShipConnector _DockingConnector;
	public IMyShipConnector DockingConnector{
		get{
			return _DockingConnector;
		}
		set{
			_DockingConnector=value;
			RefreshDockName();
		}
	}
	public Vector3D DockPosition;
	public double DockDistance{
		get{
			return (DockingConnector.GetPosition()-DockPosition).Length();
		}
	}
	public string PrettyDistance{
		get{
			if(DockDistance/1000>29979.246)
				return Math.Round(DockDistance/299792.46,3).ToString()+"ls";
			else if(DockDistance>1000)
				return Math.Round(DockDistance/1000,1).ToString()+"kM";
			return Math.Round(DockDistance,0).ToString()+"M";
		}
	}
	public Vector3D DockDirection;
	public Vector3D DockForward;
	public Vector3D DockUp;
	public Vector3D DockFinal{
		get{
			return DockPosition+2*DockDirection;
		}
	}
	public Vector3D DockTarget{
		get{
			return DockPosition+3.25*DockDirection;
		}
	}
	public Vector3D DockApproach{
		get{
			return DockPosition+5*DockDirection-25*DockForward+10*DockUp;
		}
	}
	public string DockName;
	public bool Docked{
		get{
			return DockDistance<3.25&&DockingConnector.Status==MyShipConnectorStatus.Connected;
		}
	}
	
	public Dock(IMyShipConnector dockingConnector,Vector3D dockPosition,Vector3D dockDirection,Vector3D dockForward, Vector3D dockUp,string dockName="Unnamed Dock"){
		DockName=dockName;
		DockingConnector=dockingConnector;
		DockPosition=dockPosition;
		DockDirection=dockDirection;
		DockForward=dockForward;
		DockUp=dockUp;
	}
	
	public virtual string PrettyString(){
		return DockName+": "+PrettyDistance;
	}
	
	public override string ToString(){
		return "{"+DockName+";"+DockingConnector.CustomName.ToString()+";"+DockPosition.ToString()+";"+DockDirection.ToString()+";"+DockForward.ToString()+";"+DockUp.ToString()+"}";
	}
	
	public static bool TryParse(string input,out Dock output){
		output=null;
		if(input.IndexOf('{')!=0||input.IndexOf('}')!=input.Length-1)
			return false;
		string[] args=input.Substring(1,input.Length-1).Split(';');
		if(args.Length!=6)
			return false;
		IMyShipConnector dockingConnector=GenericMethods<IMyShipConnector>.GetConstruct(args[0]);
		if(dockingConnector==null)
			return false;
		Vector3D dockPosition,dockDirection,dockForward,dockUp;
		if(!Vector3D.TryParse(args[2],out dockPosition))
			return false;
		if(!Vector3D.TryParse(args[3],out dockDirection))
			return false;
		if(!Vector3D.TryParse(args[4],out dockForward))
			return false;
		if(!Vector3D.TryParse(args[5],out dockUp))
			return false;
		output=new Dock(dockingConnector,dockPosition,dockDirection,dockForward,dockUp,args[0]);
		return true;
	}
	
	public void RefreshDockName(){
		if(DockingConnector!=null&&DockingConnector.Status==MyShipConnectorStatus.Connected){
			IMyShipConnector Other=DockingConnector.OtherConnector;
			if(Other!=null)
				DockName=Other.CubeGrid.CustomName;
		}
	}
}

struct VectorDto{
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
		V1=Box.Min;
		V2=Box.Max;
	}
	
	public Vector3D[2] ToArr(){
		return new Vector3D[2] {V1,V2};
	}
	
	public override string ToString(){
		return "("+V1.ToString()+";"+V2.ToString()+")";
	}
	
	public static VectorDto Parse(string input){
		if(input.IndexOf('(')!=0||input.IndexOf(')')!=input.Length-1)
			throw new ArgumentException("Bad format");
		string[] args=input.Substring(1,input.Length-1).Split(';');
		if(args.Length!=2)
			throw new ArgumentException("Bad format");
		Vector3D v1,v2;
		if(!(Vector3D.TryParse(args[0],out v1)&&Vector3D.TryParse(args[2],out v2)))
			throw new ArgumentException("Bad format");
		return new VetorDto(v1,v2);
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
class Entity{
	public long EntityId;
	public string Name;
	public MyDetectedEntityType Type;
	public MatrixD Orientation;
	public Vector3D Velocity;
	public MyRelationsBetweenPlayerAndBlock Relationship;
	public BoundingBoxD BoundingBox;
	public TimeSpan TimeStamp;
	public Vector3D Position{
		get{
			return BoundingBox.Center;
		}
	}
	
	public Entity(long entityId,string name,MyDetectedEntityType type,MatrixD orientation,Vector3D velocity,MyRelationsBetweenPlayerAndBlock relationship,BoundingBoxD boundingBox,TimeSpan timeStamp){
		EntityId=entityId;
		Name=name;
		Type=type;
		Orientation=orientation;
		Velocity=velocity;
		Relationship=relationship;
		BoundingBox=boundingBox;
		TimeStamp=timeStamp;
	}
	
	public Entity(MyDetectedEntityInfo e){
		EntityId=e.EntityId;
		Name=e.Name;
		Type=e.Type;
		Orientation=e.Orientation;
		Velocity=e.Velocity;
		Relationship=e.Relationship;
		BoundingBox=e.BoundingBox;
		TimeStamp=e.TimeStamp;
	}
	
	public bool Same(Entity o){
		if(Type!=o.Type)
			return false;
		if(EntityId==o.EntityId)
			return true;
		return Name.Equals(o.Name)&&(Position-o.Position).Length()<1&&(Velocity-o.Velocity).Length()<5&&(BoundingBox.Size-o.BoundingBox.Size).Length()<5&&GenericMethods<IMyTerminalBlock>.GetAngle(Orientation,o.Orientation)<10;
	}
	
	public void Update(Entity o){
		Name=o.Name;
		Orientation=o.Orientation;
		Velocity=o.Velocity;
		Relationship=o.Relationship;
		BoundingBox=o.BoundingBox;
		TimeStamp=o.TimeStamp;
	}
	
	public void Update(MyDetectedEntityInfo o){
		Update(new Entity(o));
	}
	
	public override string ToString(){
		return "["+EntityId.ToString()+","+Name+","+Type.ToString()+","+(new VectorDto(Orientation)).ToString()+","+Velocity.ToString()+","+Relationship.ToString()+","+(new VectorDto(BoundingBox)).ToString()+","+TimeStamp.ToString()+"]";
	}
	
	public static Entity Parse(string input){
		if(input[0]!='['||input[input.Length-1]!=']')
			throw new ArgumentException("Bad format");
		string[] args=input.Substring(1,input.Length-1).Split(',');
		if(args.Length!=8)
			throw new ArgumentException("Bad format");
		long entityId=long.Parse(args[0]);
		string name=args[1];
		MyDetectedEntityType type=Enum.Parse(typeof(MyDetectedEntityType),args[2]);
		MatrixD orientation=MatrixD.CreateFromDir(VectorDto.Parse(args[3]));
		Vector3D velocity;
		if(!Vector3D.TryParse(args[4],out velocity))
			throw new ArgumentException("Bad format");
		MyRelationsBetweenPlayerAndBlock relationship=Enum.Parse(typeof(MyRelationsBetweenPlayerAndBlock),args[5]);
		BoundingBoxD boundingBox=BoundingBox.CreateFromPoints(VectorDto.Parse(args[6]).ToArr());
		TimeSpan timeStamp=TimeSpan.Parse(args[7]);
		return new Entity(entityId,name,type,orientation,velocity,relationship,boundingBox,timeStamp);
	}
	
	public static bool TryParse(string input,out Entity output){
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

enum TaskType{
	Idle,
	Transfer,
	Dock,
	Travel,
	Job
}
abstract class Gen_Task{
	public TaskType Type;
	public string Name;
}
abstract class Ship_Task<T>:Gen_Task{
	public T Data;
	public virtual bool Valid{
		get{
			return Type!=TaskType.Idle;
		}
	}
	
	protected Ship_Task(string name,TaskType type,T data){
		Type=type;
		Name=name;
		Data=data;
	}
	
	public override string ToString(){
		return "{("+Name.ToString()+");("+Type.ToString()+");("+Data.ToString()+")}";
	}
	
	public static string[] StringParser(string input){
		if(input.IndexOf("{(")!=0||!input.Substring(input.Length-2).Equals(")}"))
			throw new ArgumentException("Bad format");
		int[] indices={-1,-1};
		int strCount=0;
		for(int i=2;i<input.Length-2;i++){
			if(input.Substring(i,3).Equals(");(")){
				if(strCount>2)
					throw new ArgumentException("Bad format");
				indices[strCount++]=i;
			}
		}
		if(strCount<2)
			throw new ArgumentException("Bad format");
		
		string[] output={input.Substring(2,indices[0]-2),input.Substring(indices[0],indices[1]-indices[0]),input.Substring(indices[2],input.Length-2-indices[2])};
		return output;
	}
	
}
class Task_None:Ship_Task<string>{
	public override bool Valid{
		get{
			return (!base.Valid)||Type!=TaskType.Dock;
		}
	}
	
	public Task_None():base("None",TaskType.Idle,""){
		;
	}
	
	public Task_None(TaskType type):base("None",type,""){
		;
	}
	
	public static Task_None Parse(string input){
		string[] args=Ship_Task<Dock>.StringParser(input);
		TaskType type;
		if((!args[0].Equals("None"))||(!Enum.TryParse(args[1],out type))||args[2].Length>0)
			throw new ArgumentException("Bad Format");
		return new Task_None(type);
	}
	
	public static bool TryParse(string input,out Task_None output){
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
class Task_Refuel:Ship_Task<Dock>{
	public static Task_Refuel Parse(string input){
		string[] args=StringParser(input);
		TaskType type;
		Dock data;
		if((!args[0].Equals("Refuel"))||(!Enum.TryParse(args[1],out type))||(!Dock.TryParse(args[2],out data)))
			throw new ArgumentException("Bad Format");
		return new Task_Refuel(type,data);
	}
	
	public static bool TryParse(string input,out Task_Refuel output){
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
class Task_Stalking:Ship_Task<Entity>{
	public static Task_Stalking Parse(string input){
		string[] args=StringParser(input);
		TaskType type;
		Entity data;
		if(!(args[0].Equals("Stalking")&&Enum.TryParse(args[1],out type)&&Entity.TryParse(args[2],out data)))
			throw new ArgumentException("Bad Format");
		return new Task_Stalking(type,data);
	}
	
	public static bool TryParse(string input,out Task_Stalking output){
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
class Task_Hunting:Ship_Task<Entity>{
	public static Task_Hunting Parse(string input){
		string[] args=StringParser(input);
		TaskType type;
		Entity data;
		if(!(args[0].Equals("Hunting")&&Enum.TryParse(args[1],out type)&&Entity.TryParse(args[2],out data)))
			throw new ArgumentException("Bad Format");
		return new Task_Hunting(type,data);
	}
	
	public static bool TryParse(string input,out Task_Hunting output){
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

Gen_Task ParseTask(string input){
	string name=Ship_Task<int>.StringParser(input)[0];
	switch(name){
		case "None":
			return Task_None.Parse(input);
		case "Refuel":
			return Task_Refuel.Parse(input);
		case "Stalking":
			return Task_Stalking.Parse(input);
		case "Hunting":
			return Task_Hunting.Parse(input);
	}
	return null;
}

struct Planet{
	public Vector3D PlanetCenter;
	public double SealevelRadius;
	public double GravityRadius;
	
	public Planet(Vector3D planetCenter,double sealevelRadius,double gravityRadius){
		PlanetCenter=planetCenter;
		SealevelRadius=sealevelRadius;
		GravityRadius=gravityRadius;
	}
	
	public Planet(Vector3D planetCenter,double sealevel,Vector3D position):this(planetCenter,(position-planetCenter).Length()-sealevel,(position-planetCenter).Length()){
		;
	}
	
	public double DistanceFromGravity(Vector3D Position){
		return (Position-PlanetCenter).Length()-GravityRadius;
	}
	
	public double DistanceFromSealevel(Vector3D Position){
		return (Position-PlanetCenter).Length()-SealevelRadius;
	}
	
	public double GravityDistance(Planet O){
		return Math.Max(0,(PlanetCenter-O.PlanetCenter).Length()-GravityRadius-O.GravityRadius);
	}
	
	public bool Same(Planet O){
		return (PlanetCenter-O.PlanetCenter).Length()<5;
	}
	
	public override string ToString(){
		return "{"+PlanetCenter.ToString()+','+SealevelRadius.ToString()+','+GravityRadius.ToString()+"}";
	}
	
	public static Planet Parse(string input){
		if(input.IndexOf('{'!=0||input.IndexOf('}')!=input.Length-1))
			throw new ArgumentException("Bad format");
		string[] args=input.Substring(1,input.Length-2);
		if(args.Length!=3)
			throw new ArgumentException("Bad format");
		Vector3D planetCenter=Vector3D.Parse(args[0]);
		double sealevelRadius=double.Parse(args[1]);
		double gravityRadius=double.Parse(args[2]);
		return new Planet(planetCenter,sealevelRadius,gravityRadius);
	}
	
	public static bool TryParse(string input,out Planet? output){
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

bool ValidTarget(MyDetectedEntityInfo Entity){
	if(Entity.IsEmpty())
		return false;
	switch(Entity.Relationship){
		case MyRelationsBetweenPlayerAndBlock.Owner:
		case MyRelationsBetweenPlayerAndBlock.FactionShare:
		case MyRelationsBetweenPlayerAndBlock.Friends:
		case MyRelationsBetweenPlayerAndBlock.NoOwnership:
			return false;
		case MyRelationsBetweenPlayerAndBlock.Neutral:
		case MyRelationsBetweenPlayerAndBlock.Enemies:
			break;
	}
	switch(Entity.Type){
		case MyDetectedEntityType.Planet:
		case MyDetectedEntityType.Asteroid:
		case MyDetectedEntityType.None:
		case MyDetectedEntityType.Unknown:
		case MyDetectedEntityType.Meteor:
		case MyDetectedEntityType.Missile:
		case MyDetectedEntityType.FloatingObject:
			return false;
		case MyDetectedEntityType.SmallGrid:
		case MyDetectedEntityType.LargeGrid:
		case MyDetectedEntityType.CharacterHuman
		case MyDetectedEntityType.CharacterOther:
			break;
	}
	return true;
}

bool IsLocked(IMySensorBlock Sensor){
	return !Sensor.Enabled;
}

void UpdateStalkerSensors(){
	List<long> TargetIds=new List<long>();
	List<IMySensorBlock> Locked=LockedSensors();
	for(int i=0;i<Locked.Count;i++){
		MyDetectedEntityInfo Target=Locked[i].LastDetectedEntity;
		if(!ValidTarget(Target))
			Locked[i].Enabled=true;
		else if(TargetIds.Contains(Target.EntityId))
			Locked[i].Enabled=true;
		else
			TargetIds.Add(Target.EntityId);
		if(Locked[i].Enabled)
			SetBlockData(Locked[i],"Tracking",false.ToString());
	}
	List<IMySensorBlock> Unlocked=UnlockedSensors();
	for(int i=0;i<Unlocked.Count;i++){
		MyDetectedEntityInfo Target=Locked[i].LastDetectedEntity;
		if(ValidTarget(Target)&&!TargetIds.Contains(Target.EntityId)){
			Unlocked[i].Enabled=false;
			TargetIds.Add(Target.EntityId);
			SetBlockData(Unlocked[i],"Tracking",true.ToString());
		}
	}
}

void UpdatePlanets(){
	Planet newPlanet=new Planet(PlanetCenter,Sealevel,ShipPosition);
	for(int i=0;i<Planets.Count;i++){
		if(Planets[i].Same(newPlanet)){
			Planets[i]=new Planet(PlanetCenter,Math.Max(Planets[i].SealevelRadius,newPlanet.SealevelRadius),Math.Max(Planets[i].GravityRadius,newPlanet.GravityRadius));
			return;
		}
	}
	Planets.Add(newPlanet);
}

class MyShieldController{
	public IMyTerminalBlock Block;
	public bool Valid{
		get{
			return IsShieldController(Block);
		}
	}
	
	
	public bool ToggleShield{
		get{
			return Get<bool>("DS-C_ToggleShield");
		}
		set{
			Set<bool>("DS-C_ToggleShield",value);
		}
	}
	public long PowerScale{
		get{
			return Get<long>("DS-C_PowerScale");
		}
		set{
			Set<long>("DS-C_PowerScale",value);
		}
	}
	public float PowerWatts{
		get{
			return Get<float>("DS-C_PowerWatts");
		}
		set{
			Set<float>("DS-C_PowerWatts",value);
		}
	}
	public float CFit{
		get{
			return Get<float>("DS-CFit");
		}
		set{
			Set<float>("DS-CFit",value);
		}
	}
	public bool SphereFit{
		get{
			return Get<bool>("DS-C_SphereFit");
		}
		set{
			Set<bool>("DS-C_SphereFit",value);
		}
	}
	public bool ShieldFortify{
		get{
			return Get<bool>("DS-C_ShieldFortify");
		}
		set{
			Set<bool>("DS-C_ShieldFortify",value);
		}
	}
	public float WidthSlider{
		get{
			return Get<float>("DS-C_WidthSlider");
		}
		set{
			Set<float>("DS-C_WidthSlider",value);
		}
	}
	public float HeightSlider{
		get{
			return Get<float>("DS-C_HeightSlider");
		}
		set{
			Set<float>("DS-C_HeightSlider",value);
		}
	}
	public float DepthSlider{
		get{
			return Get<float>("DS-C_DepthSlider");
		}
		set{
			Set<float>("DS-C_DepthSlider",value);
		}
	}
	public float OffsetWidthSlider{
		get{
			return Get<float>("DS-C_OffsetWidthSlider");
		}
		set{
			Set<float>("DS-C_OffsetWidthSlider",value);
		}
	}
	public float OffsetHeightSlider{
		get{
			return Get<float>("DS-C_OffsetHeightSlider");
		}
		set{
			Set<float>("DS-C_OffsetHeightSlider",value);
		}
	}
	public float OffsetDepthSlider{
		get{
			return Get<float>("DS-C_OffsetDepthSlider");
		}
		set{
			Set<float>("DS-C_OffsetDepthSlider",value);
		}
	}
	public bool UseBatteries{
		get{
			return Get<bool>("DS-C_UseBatteries");
		}
		set{
			Set<bool>("DS-C_UseBatteries",value);
		}
	}
	public bool HideIcon{
		get{
			return Get<bool>("DS-C_HideIcon");
		}
		set{
			Set<bool>("DS-C_HideIcon",value);
		}
	}
	public long ShellSelect{
		get{
			return Get<long>("DS-C_ShellSelect");
		}
		set{
			Set<long>("DS-C_ShellSelect",value);
		}
	}
	public bool HideActive{
		get{
			return Get<bool>("DS-C_HideActive");
		}
		set{
			Set<bool>("DS-C_HideActive",value);
		}
	}
	public bool NoWarningSounds{
		get{
			return Get<bool>("DS-C_NoWarningSounds");
		}
		set{
			Set<bool>("DS-C_NoWarningSounds",value);
		}
	}
	public bool DimShieldHits{
		get{
			return Get<bool>("DS-C_DimShieldHits");
		}
		set{
			Set<bool>("DS-C_DimShieldHits",value);
		}
	}
	public bool SideRedirect{
		get{
			return Get<bool>("DS-C_SideRedirect");
		}
		set{
			Set<bool>("DS-C_SideRedirect",value);
		}
	}
	public bool ShowRedirect{
		get{
			return Get<bool>("DS-C_ShowRedirect");
		}
		set{
			Set<bool>("DS-C_ShowRedirect",value);
		}
	}
	public bool TopShield{
		get{
			return Get<bool>("DS-C_TopShield");
		}
		set{
			Set<bool>("DS-C_TopShield",value);
		}
	}
	public bool BottomShield{
		get{
			return Get<bool>("DS-C_BottomShield");
		}
		set{
			Set<bool>("DS-C_BottomShield",value);
		}
	}
	public bool LeftShield{
		get{
			return Get<bool>("DS-C_LeftShield");
		}
		set{
			Set<bool>("DS-C_LeftShield",value);
		}
	}
	public bool RightShield{
		get{
			return Get<bool>("DS-C_RightShield");
		}
		set{
			Set<bool>("DS-C_RightShield",value);
		}
	}
	public bool FrontShield{
		get{
			return Get<bool>("DS-C_FrontShield");
		}
		set{
			Set<bool>("DS-C_FrontShield",value);
		}
	}
	public bool BackShield{
		get{
			return Get<bool>("DS-C_BackShield");
		}
		set{
			Set<bool>("DS-C_BackShield",value);
		}
	}
	
	private T Get<T>(string name){
		return Block.GetValue<T>(name);
	}
	private void Set<T>(string name,T property){
		Block.SetValue<T>(name,property);
	}
	
	
	public PbApiWrapper Api;
	public MyShieldController(IMyTerminalBlock block){
		Block=block;
		Api=new PbApiWrapper(Prog.P.Me);
		Api.SetActiveShield(Block);
	}
	
	public static bool IsShieldController(IMyTerminalBlock Block){
		return Block!=null&&Block.DefinitionDisplayNameText.ToLower().Contains("shield")&&Block.DefinitionDisplayNameText.ToLower().Contains("controller");
	}
}

class MyWeaponCore{
	public IMyTerminalBlock Block;
	
	public static WcPbApi Api;
	public MyWeaponCore(IMyTerminalBlock block){
		Block=block;
	}
	
	public double ExpectCharge(double seconds){
		double LastChargeTime=60;
		if(GenericMethods<IMyTerminalBlock>.HasBlockData(Block,"LastChargeTime"))
			double.TryParse(GenericMethods<IMyTerminalBlock>.GetBlockData(Block,"LastChargeTime"),out LastChargeTime);
		double ChargeTime=0;
		if(GenericMethods<IMyTerminalBlock>.HasBlockData(Block,"ChargeTime"))
			double.TryParse(GenericMethods<IMyTerminalBlock>.GetBlockData(Block,"ChargeTime"),out ChargeTime);
		bool Ready=Api.IsWeaponReadyToFire(Block);
		bool WasCharging=Ready;
		if(GenericMethods<IMyTerminalBlock>.HasBlockData(Block,"WasCharging"))
			bool.TryParse(GenericMethods<IMyTerminalBlock>.GetBlockData(Block,"WasCharging"),out WasCharging);
		if(WasCharging^Ready){
			if(Ready){
				if(LastChargeTime==60)
					LastChargeTime=ChargeTime;
				else
					LastChargeTime=(LastChargeTime+ChargeTime)/2;
				GenericMethods<IMyTerminalBlock>.SetBlockData(Block,"LastChargeTime",Math.Round(LastChargeTime,3).ToString());
			}
			else
				ChargeTime=0;
			GenericMethods<IMyTerminalBlock>.SetBlockData(Block,"WasCharging",Ready.ToString());
		}
		if(!Ready){
			ChargeTime+=seconds;
			GenericMethods<IMyTerminalBlock>.SetBlockData(Block,"ChargeTime",Math.Round(ChargeTime,3).ToString());
			return Math.Max(0,LastChargeTime-ChargeTime);
		}
		return 0;
	}
}

class MyShieldController{
	public IMyTerminalBlock Block;
	public bool Valid{
		get{
			return IsShieldController(Block);
		}
	}
	
	
	public bool ToggleShield{
		get{
			return Get<bool>("DS-C_ToggleShield");
		}
		set{
			Set<bool>("DS-C_ToggleShield",value);
		}
	}
	public long PowerScale{
		get{
			return Get<long>("DS-C_PowerScale");
		}
		set{
			Set<long>("DS-C_PowerScale",value);
		}
	}
	public float PowerWatts{
		get{
			return Get<float>("DS-C_PowerWatts");
		}
		set{
			Set<float>("DS-C_PowerWatts",value);
		}
	}
	public float CFit{
		get{
			return Get<float>("DS-CFit");
		}
		set{
			Set<float>("DS-CFit",value);
		}
	}
	public bool SphereFit{
		get{
			return Get<bool>("DS-C_SphereFit");
		}
		set{
			Set<bool>("DS-C_SphereFit",value);
		}
	}
	public bool ShieldFortify{
		get{
			return Get<bool>("DS-C_ShieldFortify");
		}
		set{
			Set<bool>("DS-C_ShieldFortify",value);
		}
	}
	public float WidthSlider{
		get{
			return Get<float>("DS-C_WidthSlider");
		}
		set{
			Set<float>("DS-C_WidthSlider",value);
		}
	}
	public float HeightSlider{
		get{
			return Get<float>("DS-C_HeightSlider");
		}
		set{
			Set<float>("DS-C_HeightSlider",value);
		}
	}
	public float DepthSlider{
		get{
			return Get<float>("DS-C_DepthSlider");
		}
		set{
			Set<float>("DS-C_DepthSlider",value);
		}
	}
	public float OffsetWidthSlider{
		get{
			return Get<float>("DS-C_OffsetWidthSlider");
		}
		set{
			Set<float>("DS-C_OffsetWidthSlider",value);
		}
	}
	public float OffsetHeightSlider{
		get{
			return Get<float>("DS-C_OffsetHeightSlider");
		}
		set{
			Set<float>("DS-C_OffsetHeightSlider",value);
		}
	}
	public float OffsetDepthSlider{
		get{
			return Get<float>("DS-C_OffsetDepthSlider");
		}
		set{
			Set<float>("DS-C_OffsetDepthSlider",value);
		}
	}
	public bool UseBatteries{
		get{
			return Get<bool>("DS-C_UseBatteries");
		}
		set{
			Set<bool>("DS-C_UseBatteries",value);
		}
	}
	public bool HideIcon{
		get{
			return Get<bool>("DS-C_HideIcon");
		}
		set{
			Set<bool>("DS-C_HideIcon",value);
		}
	}
	public long ShellSelect{
		get{
			return Get<long>("DS-C_ShellSelect");
		}
		set{
			Set<long>("DS-C_ShellSelect",value);
		}
	}
	public bool HideActive{
		get{
			return Get<bool>("DS-C_HideActive");
		}
		set{
			Set<bool>("DS-C_HideActive",value);
		}
	}
	public bool NoWarningSounds{
		get{
			return Get<bool>("DS-C_NoWarningSounds");
		}
		set{
			Set<bool>("DS-C_NoWarningSounds",value);
		}
	}
	public bool DimShieldHits{
		get{
			return Get<bool>("DS-C_DimShieldHits");
		}
		set{
			Set<bool>("DS-C_DimShieldHits",value);
		}
	}
	public bool SideRedirect{
		get{
			return Get<bool>("DS-C_SideRedirect");
		}
		set{
			Set<bool>("DS-C_SideRedirect",value);
		}
	}
	public bool ShowRedirect{
		get{
			return Get<bool>("DS-C_ShowRedirect");
		}
		set{
			Set<bool>("DS-C_ShowRedirect",value);
		}
	}
	public bool TopShield{
		get{
			return Get<bool>("DS-C_TopShield");
		}
		set{
			Set<bool>("DS-C_TopShield",value);
		}
	}
	public bool BottomShield{
		get{
			return Get<bool>("DS-C_BottomShield");
		}
		set{
			Set<bool>("DS-C_BottomShield",value);
		}
	}
	public bool LeftShield{
		get{
			return Get<bool>("DS-C_LeftShield");
		}
		set{
			Set<bool>("DS-C_LeftShield",value);
		}
	}
	public bool RightShield{
		get{
			return Get<bool>("DS-C_RightShield");
		}
		set{
			Set<bool>("DS-C_RightShield",value);
		}
	}
	public bool FrontShield{
		get{
			return Get<bool>("DS-C_FrontShield");
		}
		set{
			Set<bool>("DS-C_FrontShield",value);
		}
	}
	public bool BackShield{
		get{
			return Get<bool>("DS-C_BackShield");
		}
		set{
			Set<bool>("DS-C_BackShield",value);
		}
	}
	
	private T Get<T>(string name){
		return Block.GetValue<T>(name);
	}
	private void Set<T>(string name,T property){
		Block.SetValue<T>(name,property);
	}
	
	public MyShieldController(IMyTerminalBlock block){
		Block=block;
	}
	
	public static bool IsShieldController(IMyTerminalBlock Block){
		return Block!=null&&Block.DefinitionDisplayNameText.ToLower().Contains("shield")&&Block.DefinitionDisplayNameText.ToLower().Contains("controller");
	}
}

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;
Random Rnd;

Gen_Task MyTask;

IMyRemoteControl Controller;
IMyGyro Gyroscope;
List<IMyBatteryBlock> Batteries;
float BatteryPower{
	get{
		float current=0;
		float max=0;
		foreach(IMyBatteryBlock Battery in Batteries){
			current+=Battery.CurrentStoredPower;
			max+=Battery.MaxStoredPower;
		}
		return max>0?current:current/max;
	}
}
List<Dock> FuelingDocks;
IMyShipConnector DockingConnector;
MyShieldController ShieldController;

List<Planet> Planets;

List<IMySensorBlock> StalkerSensors;
List<MyDetectedEntityInfo> StalkerData(){
	List<MyDetectedEntityInfo> output=new List<MyDetectedEntityInfo>();
	foreach(IMySensorBlock Sensor in LockedSensors()){
		MyDetectedEntityInfo Target=Sensor.LastDetectedEntity;
		if(ValidTarget(Target))
			output.Add(Target);
	}
	return output;
}
List<IMySensorBlock> LockedSensors(){
	List<IMySensorBlock> output=new List<IMySensorBlock>();
	for(int i=0;i<StalkerSensors.Count;i++){
		if(IsLocked(StalkerSensors[i]))
			output.Add(StalkerSensors[i]);
	}
	return output;
}
List<IMySensorBlock> UnlockedSensors(){
	List<IMySensorBlock> output=new List<IMySensorBlock>();
	for(int i=0;i<StalkerSensors.Count;i++){
		if(!IsLocked(StalkerSensors[i]))
			output.Add(StalkerSensors[i]);
	}
	return output;
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

float Get_Max_Thrust(){
	float output=Forward_Thrust;
	output=Math.Max(output,Backward_Thrust);
	output=Math.Max(output,Up_Thrust);
	output=Math.Max(output,Down_Thrust);
	output=Math.Max(output,Left_Thrust);
	output=Math.Max(output,Right_Thrust);
	return output;
}
Roo<float> _Max_Thrust=new Roo<float>(Get_Max_Thrust);
float Max_Thrust{
	get{
		return _Max_Thrust;
	}
}

float Get_Forward_Thrust(){
	float total=0;
	foreach(IMyThrust Thruster in Forward_Thrusters){
		if(Thruster.IsWorking)
			total+=Thruster.MaxEffectiveThrust;
	}
	return Math.Max(total,1);
}
Roo<float> Forward_Thrust=new Roo<float>(Get_Forward_Thrust);

float Get_Backward_Thrust(){
	float output=0;
	foreach(IMyThrust Thruster in Backward_Thrusters){
		if(Thruster.IsWorking)
			output+=Thruster.MaxEffectiveThrust;
	}
	return Math.Max(output,1);
}
Roo<float> Backward_Thrust=new Roo<float>(Get_Backward_Thrust);

float Get_Up_Thrust(){
	float total=0;
	foreach(IMyThrust Thruster in Up_Thrusters){
		if(Thruster.IsWorking)
			total+=Thruster.MaxEffectiveThrust;
	}
	return Math.Max(total,1);
}
Roo<float> Up_Thrust=new Roo<float>(Get_Up_Thrust);

float Get_Down_Thrust(){
	float total=0;
	foreach(IMyThrust Thruster in Down_Thrusters){
		if(Thruster.IsWorking)
			total+=Thruster.MaxEffectiveThrust;
	}
	return Math.Max(total,1);
}
Roo<float> Down_Thrust=new Roo<float>(Get_Down_Thrust);

float Get_Left_Thrust{
	float total=0;
	foreach(IMyThrust Thruster in Left_Thrusters){
		if(Thruster.IsWorking)
			total+=Thruster.MaxEffectiveThrust;
	}
	return Math.Max(total,1);
}
Roo<float> Left_Thrust=new Roo<float>(Get_Left_Thrust);

float Get_Right_Thrust{
	float total=0;
	foreach(IMyThrust Thruster in Right_Thrusters){
		if(Thruster.IsWorking)
			total+=Thruster.MaxEffectiveThrust;
	}
	return Math.Max(total,1);
}
Roo<float> Right_Thrust=new Roo<float>(Get_Right_Thrust);

double Forward_Acc{
	get{
		return Forward_Thrust/ShipMass.TotalMass;
	}
}
double Backward_Acc{
	get{
		return Backward_Thrust/ShipMass.TotalMass;
	}
}
double Up_Acc{
	get{
		return Up_Thrust/ShipMass.TotalMass;
	}
}
double Down_Acc{
	get{
		return Down_Thrust/ShipMass.TotalMass;
	}
}
double Left_Acc{
	get{
		return Left_Thrust/ShipMass.TotalMass;
	}
}
double Right_Acc{
	get{
		return Right_Thrust/ShipMass.TotalMass;
	}
}

double Forward_Gs{
	get{
		return Forward_Acc/9.81;
	}
}
double Backward_Gs{
	get{
		return Backward_Acc/9.81;
	}
}
double Up_Gs{
	get{
		return Up_Acc/9.81;
	}
}
double Down_Gs{
	get{
		return Down_Acc/9.81;
	}
}
double Left_Gs{
	get{
		return Left_Acc/9.81;
	}
}
double Right_Gs{
	get{
		return Right_Acc/9.81;
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


Roo<MyShipMass> ShipMass=new Roo<MyShipMass>(Controller.CalculateShipMass());
float Get_Mass_Accomodation(){
	return (float)(Gravity.Length()*ShipMass.PhysicalMass);
}
Roo<float> Mass_Accomodation=new Roo<float>(Get_Mass_Accomodation);
double Time_To_Crash=double.MaxValue;
double Time_To_Stop=0;

Roo<Vector3D> ShipPosition=new Roo<Vector3D>(Controller.GetPosition());

double RestingSpeed=0;
Vector3D RestingVelocity{
	get{
		if(RestingSpeed==0)
			return new Vector3D(0,0,0);
		return RestingSpeed*Forward_Vector;
	}
}
Vector3D Relative_RestingVelocity{
	get{
		return GlobalToLocal(RestingVelocity,Controller);
	}
}
double CurrentSpeed{
	get{
		return CurrentVelocity.Length();
	}
}
Roo<MyShipVelocities> Velocities=new Roo<MyShipVelocities>(Controller.GetShipVelocities());
Vector3D CurrentVelocity{
	get{
		return Velocities.LinearVelocity;
	}
}
Vector3D Velocity_Direction{
	get{
		Vector3D VD=CurrentVelocity;
		VD.Normalize();
		return VD;
	}
}

Vector3D Get_Relative_CurrentVelocity(){
	Vector3D output=Vector3D.Transform(CurrentVelocity+ShipPosition,MatrixD.Invert(Controller.WorldMatrix));
	output.Normalize();
	output*=CurrentVelocity.Length();
	return output;
}
Roo<Vector3D> Relative_CurrentVelocity=new Roo<Vector3D>(Get_Relative_CurrentVelocity);

Roo<Vector3D> Gravity=new Roo<Vector3D>(Controller.GetNaturalGravity());
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
Vector3D AngularVelocity{
	get{
		return Velocities.AngularVelocity;
	}
}
Vector3D Relative_AngularVelocity{
	get{
		return GlobalToLocal(AngularVelocity,Controller);
	}
}

double Get_Elevation(){
	double elevation=double.MaxValue;
	Controller.TryGetPlanetElevation(MyPlanetElevation.Surface,out elevation);
	return elevation;
}
Roo<double> Elevation=new Roo<double>(Get_Elevation);
double Get_Sealevel(){
	double sealevel=double.MaxValue;
	Controller.TryGetPlanetElevation(MyPlanetElevation.Sealevel,out sealevel);
	return sealevel;
}
Roo<double> Sealevel=new Roo<double>(Get_Sealevel);
Vector3D Get_PlanetCenter(){
	Vector3D planetCenter=new Vector3D(0,0,0);
	Controller.TryGetPlanetPosition(out planetCenter);
	return planetCenter;
}
Roo<Vector3D> PlanetCenter=new Roo<double>(Get_Planetcenter);

UpdateFrequency GetUpdateFrequency(){
	if(Running_Thrusters||(Gyroscope!=null&&Gyroscope.GyroOverride))
		return UpdateFrequency.Update1;
	return UpdateFrequency.Update100;
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
	if((!block_type.ToLower().Contains("atmospheric"))&&(!block_type.ToLower().Contains("hydrogen")))
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
void ResetThrusters(){
	for(int i=0;i<All_Thrusters.Length;i++){
		foreach(IMyThrust Thruster in All_Thrusters[i])
			ResetThruster(Thruster);
	}
	Running_Thrusters=false;
}

void Reset(){
	Operational=false;
	Runtime.UpdateFrequency=UpdateFrequency.None;
	Controller=null;
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	Gyroscope=null;
	Batteries=new List<IMyBatteryBlock>();
	FuelingDocks=new List<Dock>();
	DockingConnector=null;
	ShieldController=null;
	
	Planets=new List<Planet>();
	StalkerSensors=new List<IMySensorBlock>();
	for(int i=0;i<All_Thrusters.Length;i++){
		if(All_Thrusters[i]!=null){
			for(int j=0;j<All_Thrusters[i].Count;j++){
				if(All_Thrusters[i][j]!=null)
					ResetThruster(All_Thrusters[i][j]);
			}
		}
		All_Thrusters[i]=new List<IMyThrust>();
	}
	RestingSpeed=0;
	MyTask=new Task_None();
	Notifications=new List<Notification>();
}

double MySize=0;
bool Setup(){
	Reset();
	Controller=GenericMethods<IMyRemoteControl>.GetContaining("");
	if(Controller==null){
		Write("Failed to find Controller", false, false);
		return false;
	}
	Forward=Controller.Orientation.Forward;
	Up=Controller.Orientation.Up;
	Left=Controller.Orientation.Left;
	MySize=Controller.CubeGrid.GridSize;
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
	foreach(IMyThrust Thruster in MyThrusters){
		if(Thruster.CubeGrid!=Controller.CubeGrid)
			continue;
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
	SetThrusterList(Forward_Thrusters,"Forward");
	SetThrusterList(Backward_Thrusters,"Backward");
	SetThrusterList(Up_Thrusters,"Up");
	SetThrusterList(Down_Thrusters,"Down");
	SetThrusterList(Left_Thrusters,"Left");
	SetThrusterList(Right_Thrusters,"Right");
	Batteries=GenericMethods<IMyBatteryBlock>.GetAllIncluding("");
	DockingConnector=GenericMethods<IMyShipConnector>.GetContaining("Stalker Docking Connector");
	ShieldController=new ShieldController(GenericMethods<IMyTerminalBlock>.GetClosestFunc(MyShieldController.IsShieldController));
	
	StalkerSensors=GenericMethods<IMyStalkerSensor>.GetContaining("Stalker Sensor");
	string mode="";
	string[] args=this.Storage.Split('\n');
	foreach(string arg in args){
		switch(arg){
			case "Docks":
			case "Planets":
			case "MyTask":
			case "RestingSpeed":
				mode=arg;
				break;
			default:
				switch(mode){
					case "Docks":
						Dock dock;
						if(Dock.TryParse(arg,out dock))
							FuelingDocks.Add(dock);
						break;
					case "Planets":
						Planet planet;
						if(Planet.TryParse(arg,out planet))
							Planets.Add(planet);
						break;
					case "MyTask":
						try{
							MyTask=ParseTask(arg);
						}
						catch{
							MyTask=new Task_None();
						}
						break;
					case "RestingSpeed":
						double restingSpeed=0;
						if(double.TryParse(arg,out restingSpeed))
							RestingSpeed=restingSpeed;
						break;
				}
				break;
		}
	}
	
	
	Acceptable_Angle=Math.Min(Math.Max(0.5,200/MySize),10);
	Operational=Me.IsWorking;
	Runtime.UpdateFrequency=GetUpdateFrequency();
	return true;
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
	Me.GetSurface(1).TextPadding=30.0f;
	Echo("Beginning initialization");
	Rnd=new Random();
	Notifications=new List<Notification>();
	Task_Queue=new Queue<Task>();
	TaskParser(Me.CustomData);
	Setup();
}

public void Save(){
	this.Storage="\nMyTask\n"+MyTask.ToString();
	this.Storage+="\nRestingSpeed\n"+Math.Round(RestingSpeed,1).ToString();
	this.Storage+="\nDocks";
	foreach(Dock dock in FuelingDocks)
		this.Storage+='\n'+dock.ToString();
	this.Storage+="\nPlanets";
	foreach(Planet planet in Planets)
		this.Storage+='\n'+planet.ToString();
	Me.CustomData="";
	foreach(Task T in Task_Queue){
		Me.CustomData+=T.ToString()+'•';
	}
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	for(int i=0;i<All_Thrusters.Length;i++){
		foreach(IMyThrust Thruster in All_Thrusters[i])
			ResetThruster(Thruster);
	}
}

bool Disable(){
	Operational=false;
	ResetThrusters();
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	Runtime.UpdateFrequency=UpdateFrequency.None;
	Me.Enabled=false;
	return true;
}
bool FactoryReset(){
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	for(int i=0;i<All_Thrusters.Length;i++){
		foreach(IMyThrust Thruster in All_Thrusters[i])
			ResetThruster(Thruster);
	}
	Me.CustomData="";
	this.Storage="";
	Reset();
	Me.CustomData="";
	this.Storage="";
	Me.Enabled=false;
	return true;
}

//Sets gyroscope outputs from player input, dampeners, gravity, and autopilot
double Pitch_Time= 1.0f;
double Yaw_Time=1.0f;
double Roll_Time=1.0f;
bool Do_Direction=false;
Vector3D Target_Direction=new Vector3D(0,0,0);
bool Match_Direction=false;
bool Do_Up=false;
Vector3D Target_Up=new Vector3D(0,0,0);
void SetGyroscopes(){
	Gyroscope.GyroOverride=(AngularVelocity.Length()<3);
	float current_pitch=(float)Relative_AngularVelocity.X;
	float current_yaw=(float)Relative_AngularVelocity.Y;
	float current_roll=(float)Relative_AngularVelocity.Z;
	
	float gyro_count=0;
	List<IMyGyro> AllGyros=new List<IMyGyro>();
	GridTerminalSystem.GetBlocksOfType<IMyGyro>(AllGyros);
	foreach(IMyGyro Gyro in AllGyros){
		if(Gyro.IsWorking)
			gyro_count+=Gyro.GyroPower/100.0f;
	}
	float gyro_multx=(float)Math.Max(0.1f, Math.Min(1, 1.5f/(ShipMass.PhysicalMass/gyro_count/1000000)));
	
	if(Match_Direction&&Do_Position&&Target_Distance>20){
		bool do_Match=true;
		Vector3D target_direction=Target_Position-ShipPosition;
		target_direction.Normalize();
		if(Gravity.Length()>0){
			double Grav_Angle=GetAngle(target_direction,Gravity_Direction);
			if(Grav_Angle<60)
				do_Match=false;
			bool Target_Basically_Up=Grav_Angle>135;
			bool Up_Stronger=Up_Acc>Forward_Acc;
			bool Up_Not_Much_Weaker=Up_Acc>Gravity.Length()&&(Up_Acc-Gravity.Length())/2>(Forward_Acc-Gravity.Length())/3;
			bool Forward_Not_Strong=Forward_Acc<Gravity.Length();
			if(Target_Basically_Up&&(Forward_Not_Strong||Up_Stronger||Up_Not_Much_Weaker))
				do_Match=false;
		}
		if(do_Match){
			Do_Direction=true;
			Target_Direction=target_direction;
		}
	}
	
	float input_pitch=0;
	float input_yaw=0;
	float input_roll=0;
	
	if(Pitch_Time<1)
		Pitch_Time+=seconds_since_last_update;
	if(Yaw_Time<1)
		Yaw_Time+=seconds_since_last_update;
	if(Roll_Time<1)
		Roll_Time+=seconds_since_last_update;
	
	bool Undercontrol=Controller.IsUnderControl;
	
	input_pitch+=Math.Min(Math.Max(Controller.RotationIndicator.X/100,-1),1);
	
	double Direction_Angle=GetAngle(Forward_Vector,Target_Direction);
	
	if(Math.Abs(input_pitch)<0.05f){
		input_pitch=current_pitch*0.99f;
		bool do_adjust_pitch=Do_Direction;
		double v_difference=0;
		if(Do_Direction){
			v_difference=(GetAngle(Up_Vector,Target_Direction)-GetAngle(Down_Vector,Target_Direction))/2;
			if(Gravity.Length()>0){
				double target_grav=Math.Abs(90-GetAngle(Target_Direction,Gravity));
				if(target_grav<45){
					double grav_difference=(GetAngle(Up_Vector,Gravity)-GetAngle(Down_Vector,Gravity))/2;
					if(grav_difference<30&&Math.Abs(v_difference-Direction_Angle)>15)
						do_adjust_pitch=false;
					else if(Direction_Angle>90&&v_difference>90)
						do_adjust_pitch=false;
				}
			}
		}
		if(do_adjust_pitch){
			if(Math.Abs(v_difference)>Math.Min(1,Acceptable_Angle/2))
				input_pitch+=10*gyro_multx*((float)Math.Min(Math.Max(v_difference,-90),90)/90.0f);
		}
		else{
			float orbit_multx=1;
			if(Safety){
				if((((Elevation-MySize)<Controller.GetShipSpeed()*2&&(Elevation-MySize)<50)||Controller.DampenersOverride&&!Controller.IsUnderControl)&&GetAngle(Gravity,Forward_Vector)<120&&Pitch_Time>=1){
					double difference=Math.Abs(GetAngle(Gravity,Forward_Vector));
					if(difference<90)
						input_pitch-=10*gyro_multx*((float)Math.Min(Math.Abs((90-difference)/90),1));
				}
				if((Controller.DampenersOverride&&!Undercontrol)&&(GetAngle(Gravity,Forward_Vector)>(90+Acceptable_Angle/2))){
					double difference=Math.Abs(GetAngle(Gravity,Forward_Vector));
					if(difference>90+Acceptable_Angle/2)
						input_pitch+=10*gyro_multx*((float)Math.Min(Math.Abs((difference-90)/90),1))*orbit_multx;
				}
			}
		}
	}
	else{
		Pitch_Time=0;
		input_pitch*=30;
	}
	input_yaw+=Math.Min(Math.Max(Controller.RotationIndicator.Y/100,-1),1);
	if(Math.Abs(input_yaw)<0.05f){
		input_yaw=current_yaw*0.99f;
		if(Do_Direction){
			double difference=(GetAngle(Left_Vector,Target_Direction)-GetAngle(Right_Vector,Target_Direction))/2;
			if(Direction_Angle>90){
				if(difference<0)
					difference=-180-difference;
				else if(difference>0)
					difference=180-difference;
				else
					difference=180;
			}
			if(Math.Abs(difference)>Math.Min(1,Acceptable_Angle/2))
				input_yaw+=10*gyro_multx*((float)Math.Min(Math.Max(difference,-90),90)/90.0f);
		}
	}
	else{
		Yaw_Time=0;
		input_yaw*=30;
	}
	input_roll+=Controller.RollIndicator;
	if(Math.Abs(input_roll)<0.05f){
		input_roll=current_roll*0.99f;
		if(Do_Up){
			if((!Do_Direction)||GetAngle(Forward_Vector,Target_Direction)<90){
				double difference=(GetAngle(Left_Vector,Target_Up)-GetAngle(Right_Vector,Target_Up))/2;
				if(GetAngle(Up_Vector,Target_Up)>90){
					if(difference<0)
						difference=-180-difference;
					else if(difference>0)
						difference=180-difference;
					else
						difference=180;
				}
				float direction_multx=1.0f;
				if(Do_Direction&&Direction_Angle>Acceptable_Angle)
					direction_multx/=(float)(Direction_Angle/Acceptable_Angle);
				if(Math.Abs(difference)>Math.Min(1,Acceptable_Angle/2))
					input_roll+=30*gyro_multx*direction_multx*((float)Math.Min(Math.Max(difference,-90),90)/90.0f);
			}
		}
		else{ 
			if(Safety&&Gravity.Length()>0&&Roll_Time>=1){
				double difference=GetAngle(Left_Vector,Gravity)-GetAngle(Right_Vector,Gravity);
				if(Math.Abs(difference)>Acceptable_Angle){
					input_roll-=(float)Math.Min(Math.Max(difference*5,-5),25)*gyro_multx*5;
				}
			}
		}
	}
	else{
		Roll_Time=0;
		input_roll*=10;
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

double Distance_Speed_Limit(double distance){
	distance=Math.Abs(distance);
	if(distance<0.5)
		return 4*distance;
	if(distance<1.5)
		return 2;
	if(distance<2.5)
		return 2.5;
	if(distance<5)
		return distance;
	if(distance<25)
		return 10;
	if(distance<50)
		return 20;
	if(distance<100)
		return 25;
	if(distance<250)
		return 40;
	if(distance<500)
		return 50;
	return distance/10;
}
float Match_Thrust(double esl,double Relative_Speed,double Relative_Target_Speed,double Relative_Distance,float T1,float T2,Vector3D V1,Vector3D V2,float Relative_Gravity){
	double R_ESL=Math.Min(Elevation,Math.Min(esl,Distance_Speed_Limit(Relative_Distance)));
	float distance_multx=1.0f;
	double Target_Speed=0;
	double speed_change=Relative_Speed-Relative_Target_Speed;
	double deacceleration=Math.Abs(speed_change*ShipMass.PhysicalMass);
	double time=0;
	//difference is required change in velocity; must be "0" when the target is reached
	if(speed_change>0)
		time=deacceleration/(T1-Relative_Gravity);
	else if(speed_change<0)
		time=deacceleration/(T2+Relative_Gravity);
	//deacceleration is the required change in force; divided by the thruster power, this is now the ammount of time required to make that change
	double acceleration_distance=(Math.Abs(Relative_Speed)+Target_Speed)*time/2;
	//acceleration_distance is the distance that will be covered during that change in speed
	bool increase=acceleration_distance<Math.Abs(Relative_Distance)*1.05+MySize+5;
	//increase determines whether to accelerate or deaccelerate, based on whether the acceleration distance is smaller than the distance to the target (with some wiggle room);
	if(!increase){
		distance_multx*=-1;
	}
	if(Relative_Distance<Relative_Target_Speed){
		if((CurrentVelocity+V1-RestingVelocity).Length()<=R_ESL)
			return -0.95f*T1*distance_multx;
	}
	else if(Relative_Distance>Relative_Target_Speed){
		if((CurrentVelocity+V2-RestingVelocity).Length()<=R_ESL)
			return 0.95f*T2*distance_multx;
	}
	return 0;
}

float SmoothSpeed(IMyThrust Thruster,float input,double effective_speed_limit){
	double timer=0;
	if(HasBlockData(Thruster,"OutputTimer")){
		if(!double.TryParse(GetBlockData(Thruster,"OutputTimer"),out timer))
			timer=0;
	}
	timer=Math.Max(0,timer-seconds_since_last_update);
	float output=input;
	if(HasBlockData(Thruster,"SmoothOverride")){
		if(!float.TryParse(GetBlockData(Thruster,"SmoothOverride"),out output)||output.ToString().Equals("NaN"))
			output=input;
	}
	if(timer<=0){
		output=(2*output+input)/3;
		timer=0.2;
	}
	SetBlockData(Thruster,"OutputTimer",Math.Round(timer,3).ToString());
	SetBlockData(Thruster,"SmoothOverride",output.ToString());
	if(Speed_Deviation>0.5&&input>0&&effective_speed_limit-Speed_Deviation<2.5){
		return (output*99+input)/100;
	}
	return input;
}

bool Safety=true;
bool Do_Position=false;
Vector3D Target_Position=new Vector3D(0,0,0);
Vector3D Relative_Target_Position{
	get{
		return GlobalToLocalPosition(Target_Position,Controller);
	}
}
double Target_Distance{
	get{
		return (Target_Position-ShipPosition).Length();
	}
}
double True_Target_Distance{
	get{
		return (True_Target_Position-ShipPosition).Length();
	}
}
bool Running_Thrusters=false;
void SetThrusters(){
	Running_Thrusters=true;
	float input_forward=0.0f;
	float input_up=0.0f;
	float input_right=0.0f;
	float damp_multx=0.99f;
	double effective_speed_limit=Speed_Limit;
	
	bool Undercontrol=Controller.IsUnderControl;
	
	double Ev_Df=Math.Max(0,Math.Min(20,MySize/4))+10;
	if(Safety){
		if(Elevation<200+Ev_Df)
			effective_speed_limit=Math.Min(effective_speed_limit,Math.Sqrt(Math.Max(Elevation-Ev_Df,0)/200)*Speed_Limit);
		if(Time_To_Crash<30&&Time_To_Crash>=0)
			effective_speed_limit=Math.Min(effective_speed_limit,Math.Sqrt(Time_To_Crash/30)*Speed_Limit);
		if(Do_Position)
			effective_speed_limit=Math.Min(effective_speed_limit,Math.Sqrt(True_Target_Distance/4)*4);
	}
	if(Controller.DampenersOverride){
		Display(3,"Cruise Control: Off");
		Display(3,"Dampeners: On");
		input_right-=(float)((Relative_CurrentVelocity.X-Relative_RestingVelocity.X)*Mass_Accomodation*damp_multx);
		input_up-=(float)((Relative_CurrentVelocity.Y-Relative_RestingVelocity.Y)*Mass_Accomodation*damp_multx);
		input_forward+=(float)((Relative_CurrentVelocity.Z-Relative_RestingVelocity.Z)*Mass_Accomodation*damp_multx);
	}
	else{
		if(Elevation>50||CurrentVelocity.Length()>10){
			Display(3,"Cruise Control: On");
			Vector3D velocity_direction=CurrentVelocity;
			velocity_direction.Normalize();
			double angle=Math.Min(GetAngle(Forward_Vector, velocity_direction),GetAngle(Backward_Vector, velocity_direction));
			if(angle<=Acceptable_Angle/2){
				input_right-=(float)((Relative_CurrentVelocity.X-Relative_RestingVelocity.X)*Mass_Accomodation*damp_multx);
				input_up-=(float)((Relative_CurrentVelocity.Y-Relative_RestingVelocity.Y)*Mass_Accomodation*damp_multx);
				Display(3,"Stabilizers: On ("+Math.Round(angle, 1)+"° dev)");
			}
			else
				Display(3,"Stabilizers: Off ("+Math.Round(angle, 1)+"° dev)");
		}
		else{
			Display(3,"Cruise Control: Off");
			Display(3,"Dampeners: Off");
		}
	}
	
	double ExpectedForwardMovement=CurrentSpeed/60;
	double UpwardMovementLimit=ExpectedForwardMovement*2;
	if((cycle>10&&Runtime.UpdateFrequency==UpdateFrequency.Update1)?LastElevation-Elevation>UpwardMovementLimit:false)
		effective_speed_limit/=2;
	
	effective_speed_limit=Math.Max(effective_speed_limit,5);
	if(!Safety)
		effective_speed_limit=300000000;
	
	Display(3,"Effective Speed Limit:"+Math.Round(effective_speed_limit,1)+"mps");
	float deviation_multx=1;
	if(Target_Distance>25&&effective_speed_limit>5&&Math.Abs(effective_speed_limit-Speed_Deviation)<5)
		deviation_multx=(float)Math.Sqrt(1-Math.Max(Math.Abs(effective_speed_limit-Speed_Deviation),0.1)/5);
	
	
	if(RestingSpeed==0&&Controller.DampenersOverride&&(Speed_Deviation+5)<effective_speed_limit&&!Do_Position){
		for(int i=0;i<All_Thrusters.Length;i++){
			foreach(IMyThrust Thruster in All_Thrusters[i])
				Thruster.ThrustOverride=0;
		}
		Running_Thrusters=false;
		return;
	}
	
	if(Gravity.Length()>0&&Mass_Accomodation>0&&(Controller.GetShipSpeed()<100||GetAngle(CurrentVelocity,Gravity)>Acceptable_Angle)){
		if(!((!Controller.DampenersOverride)&&Elevation<Ev_Df&&CurrentSpeed<1)){
			input_right-=(float)Adjusted_Gravity.X;
			input_up-=(float)Adjusted_Gravity.Y;
			input_forward+=(float)Adjusted_Gravity.Z;
		}
	}
	
	if(Do_Position){
		if(Target_Distance>1500)
			Write("Target Position: "+Math.Round(True_Target_Distance/1000,1)+"kM");
		else
			Write("Target Position: "+Math.Round(True_Target_Distance,0)+"M");
		float thrust_value=Match_Thrust(effective_speed_limit,Relative_CurrentVelocity.X,RestingVelocity.X,Relative_Target_Position.X,Left_Thrust,Right_Thrust,Left_Vector,Right_Vector,-1*(float)Adjusted_Gravity.X);
		if(Math.Abs(thrust_value)>=1)
			input_right=thrust_value*deviation_multx-(float)Adjusted_Gravity.X;
		thrust_value=Match_Thrust(effective_speed_limit,Relative_CurrentVelocity.Y,RestingVelocity.Y,Relative_Target_Position.Y,Down_Thrust,Up_Thrust,Down_Vector,Up_Vector,-1*(float)Adjusted_Gravity.Y);
		if(Math.Abs(thrust_value)>=1)
			input_up=thrust_value*deviation_multx-(float)Adjusted_Gravity.Y;
		thrust_value=-1*Match_Thrust(effective_speed_limit,Relative_CurrentVelocity.Z,RestingVelocity.Z,Relative_Target_Position.Z,Forward_Thrust,Backward_Thrust,Forward_Vector,Backward_Vector,(float)Adjusted_Gravity.Z);
		if(Math.Abs(thrust_value)>=1)
			input_forward=thrust_value*deviation_multx+(float)Adjusted_Gravity.Z;
	}
	else{
		if(Controller.IsUnderControl&&Math.Abs(Controller.MoveIndicator.X)>0.5f){
			if(Controller.MoveIndicator.X>0){
				if((!Safety)||(CurrentVelocity+Right_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_right=0.95f*Right_Thrust*deviation_multx;
				else
					input_right=Math.Min(input_right,0)*deviation_multx;
			} else {
				if((!Safety)||(CurrentVelocity+Left_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_right=-0.95f*Left_Thrust*deviation_multx;
				else
					input_right=Math.Max(input_right,0)*deviation_multx;
			}
		}
		
		if(Controller.IsUnderControl&&Math.Abs(Controller.MoveIndicator.Y)>0.5f){
			if(Controller.MoveIndicator.Y>0){
				bool grav=GetAngle(Up_Vector,Gravity_Direction)>150;
				if((!Safety)||(CurrentVelocity+Up_Vector-RestingVelocity).Length()<=effective_speed_limit||(grav&&(Elevation<100+Ev_Df)))
					input_up=0.95f*Up_Thrust*deviation_multx;
				else
					input_up=Math.Min(input_up,0)*deviation_multx;
			} else {
				if((!Safety)||(CurrentVelocity+Down_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_up=-0.95f*Down_Thrust*deviation_multx;
				else
					input_up=Math.Max(input_up,0)*deviation_multx;
			}
		}
		
		if(Controller.IsUnderControl&&Math.Abs(Controller.MoveIndicator.Z)>0.5f){
			if(Controller.MoveIndicator.Z<0){
				if((!Safety)||(CurrentVelocity+Up_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_forward=0.95f*Forward_Thrust*deviation_multx;
				else
					input_forward=Math.Min(input_forward,0)*deviation_multx;
			} 
			else{
				if((!Safety)||(CurrentVelocity+Down_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_forward=-0.95f*Backward_Thrust*deviation_multx;
				else
					input_forward=Math.Max(input_forward,0)*deviation_multx;
			}
		}
}
	
	
	if(Forward_Thrusters.Count>0)
		input_forward=SmoothSpeed(Forward_Thrusters[0],input_forward,effective_speed_limit);
	else if(Backward_Thrusters.Count>0)
		input_forward=SmoothSpeed(Backward_Thrusters[0],input_forward,effective_speed_limit);
	if(Up_Thrusters.Count>0)
		input_up=SmoothSpeed(Up_Thrusters[0],input_up,effective_speed_limit);
	else if(Down_Thrusters.Count>0)
		input_up=SmoothSpeed(Down_Thrusters[0],input_up,effective_speed_limit);
	if(Right_Thrusters.Count>0)
		input_right=SmoothSpeed(Right_Thrusters[0],input_right,effective_speed_limit);
	else if(Left_Thrusters.Count>0)
		input_right=SmoothSpeed(Left_Thrusters[0],input_right,effective_speed_limit);
	
	float output_forward=0.0f;
	float output_backward=0.0f;
	if(input_forward/Forward_Thrust>0.01f)
		output_forward=Math.Min(Math.Abs(input_forward/Forward_Thrust),1);
	else if(input_forward/Backward_Thrust<-0.01f)
		output_backward=Math.Min(Math.Abs(input_forward/Backward_Thrust),1);
	float output_up=0.0f;
	float output_down=0.0f;
	if(input_up/Up_Thrust>0.01f)
		output_up=Math.Min(Math.Abs(input_up/Up_Thrust), 1);
	else if(input_up/Down_Thrust<-0.01f)
		output_down=Math.Min(Math.Abs(input_up/Down_Thrust), 1);
	float output_right=0.0f;
	float output_left=0.0f;
	if(input_right/Right_Thrust>0.01f)
		output_right=Math.Min(Math.Abs(input_right/Right_Thrust), 1);
	else if(input_right/Left_Thrust<-0.01f)
		output_left=Math.Min(Math.Abs(input_right/Left_Thrust), 1);
	
	const float MIN_THRUST=0.0001f;
	foreach(IMyThrust Thruster in Forward_Thrusters){
		Thruster.ThrustOverridePercentage=output_forward;
		if(output_forward<=0)
			Thruster.ThrustOverride=MIN_THRUST;
	}
	foreach(IMyThrust Thruster in Backward_Thrusters){
		Thruster.ThrustOverridePercentage=output_backward;
		if(output_backward<=0)
			Thruster.ThrustOverride=MIN_THRUST;
	}
	foreach(IMyThrust Thruster in Up_Thrusters){
		Thruster.ThrustOverridePercentage=output_up;
		if(output_up<=0)
			Thruster.ThrustOverride=MIN_THRUST;
	}
	foreach(IMyThrust Thruster in Down_Thrusters){
		Thruster.ThrustOverridePercentage=output_down;
		if(output_down<=0)
			Thruster.ThrustOverride=MIN_THRUST;
	}
	foreach(IMyThrust Thruster in Right_Thrusters){
		Thruster.ThrustOverridePercentage=output_right;
		if(output_right<=0)
			Thruster.ThrustOverride=MIN_THRUST;
	}
	foreach(IMyThrust Thruster in Left_Thrusters){
		Thruster.ThrustOverridePercentage=output_left;
		if(output_left<=0)
			Thruster.ThrustOverride=MIN_THRUST;
	}
	if(output_forward>0)
		Display(4,"Thrust Forwd:"+Math.Round(output_forward,1)+"%");
	if(output_backward>0)
		Display(4,"Thrust Back:"+Math.Round(output_backward,1)+"%");
	if(output_up>0)
		Display(4,"Thrust Up:"+Math.Round(output_up,1)+"%");
	if(output_down>0)
		Display(4,"Thrust Down:"+Math.Round(output_down,1)+"%");
	if(output_left>0)
		Display(4,"Thrust Left:"+Math.Round(output_left,1)+"%");
	if(output_right>0)
		Display(4,"Thrust Right:"+Math.Round(output_right,1)+"%");
}

Vector2I GetSize(IMyTextSurface Display){
	if(Display.Font!="Monospace")
		Display.Font="Monospace";
	Vector2 Size=Display.SurfaceSize;
	Vector2 CharSize=Display.MeasureStringInPixels(new StringBuilder("|"),Display.Font,Display.FontSize);
	float Padding=(100-Display.TextPadding)/100f;
	return new Vector2I((int)(Padding*Size.X/CharSize.X-2),(int)(Padding*Size.Y/CharSize.Y));
}

class Notification{
	public string Text;
	public double Time;
	
	public Notification(string x,double t){
		Text=x;
		Time=t;
	}
}
List<Notification> Notifications;

void UpdateProgramInfo(){
	OneDone.ResetAll();
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
	Echo(Program_Name+" OS\nCycle-"+cycle.ToString()+" ("+loading_char+")");
	Me.GetSurface(1).WriteText(Program_Name+" OS\nCycle-"+cycle.ToString()+" ("+loading_char+")",false);
	seconds_since_last_update=Runtime.TimeSinceLastRun.TotalSeconds + (Runtime.LastRunTimeMs / 1000);
	Display_Timer-=seconds_since_last_update;
	if(Display_Timer<=0){
		Current_Display=(Current_Display%Display_Count)+1;
		Display_Timer=5;
	}
	Write("Display "+Current_Display.ToString()+"/"+Display_Count.ToString());
	Echo(ToString(FromSeconds(seconds_since_last_update))+" since last cycle");
	Time_Since_Start=UpdateTimeSpan(Time_Since_Start,seconds_since_last_update);
	Echo(ToString(Time_Since_Start)+" since last reboot\n");
	Me.GetSurface(1).WriteText("\n"+ToString(Time_Since_Start)+" since last reboot",true);
}

double Target_Elevation=double.MaxValue;
void Crash_Test(){
	double from_center=(ShipPosition-PlanetCenter).Length();
	Vector3D next_position=ShipPosition+1*CurrentVelocity;
	double Elevation_per_second=(from_center-(next_position-PlanetCenter).Length());
	Target_Elevation=Elevation;
	if(Do_Position){
		Vector3D target_direction=Target_Position-PlanetCenter;
		target_direction.Normalize();
		Vector3D me_direction=ShipPosition-PlanetCenter;
		me_direction.Normalize();
		double planet_angle=GetAngle(me_direction,target_direction);
		if(planet_angle<2.5){
			double target_from_center=(Target_Position-PlanetCenter).Length();
			double Target_Elevation=Math.Min(Elevation,target_from_center-(from_center-Elevation));
		}
	}
	Time_To_Crash=(Target_Elevation-MySize/2)/Elevation_per_second;
	bool need_print=true;
	if(Time_To_Crash>0){
		if(Safety&&Time_To_Crash-Time_To_Stop<5&&Controller.GetShipSpeed()>5){
			Controller.DampenersOverride=true;
			RestingSpeed=0;
			for(int i=0;i<Notifications.Count;i++){
				if(Notifications[i].Text.IndexOf("Crash predicted within ")==0&&Notifications[i].Text.Contains(" seconds:\nEnabling Dampeners...")){
					Notifications.RemoveAt(i--);
					continue;
				}
			}
			Notifications.Add(new Notification("Crash predicted within "+Math.Round(5+CurrentSpeed/5,1)+" seconds:\nEnabling Dampeners...",2));
			need_print=false;
		}
		else{
			bool Will_Descend=(ShipPosition-PlanetCenter).Length()>(ShipPosition+CurrentVelocity-PlanetCenter).Length();
			bool Will_Be_Closer=true;
			if(Do_Position){
				Will_Be_Closer=(ShipPosition-Target_Position).Length()<(ShipPosition+CurrentVelocity-Target_Position).Length()||CurrentSpeed<5;
			}
			if(Time_To_Crash*Math.Max(Target_Elevation,1000)<1800000&&CurrentSpeed>1.0f){
				Write(Math.Round(Time_To_Crash,1).ToString()+" seconds to crash");
				need_print=false;
			}
			
		}
	}
	if(need_print)
		Write("No crash likely at current velocity");
}
double LastElevation=double.MaxValue;
void UpdateSystemData(){
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
	
	//Time to Stop: Counter Velocity
	Vector3D Time=new Vector3D(0,0,0);
	if(Relative_CurrentVelocity.X>=0)
		Time.X=Left_Acc-Relative_Gravity.X;
	else
		Time.X=Right_Acc-Relative_Gravity.X;
	Time.X=Math.Abs(Relative_CurrentVelocity.X/Time.X);
	if(Relative_CurrentVelocity.Y>=0)
		Time.Y=Down_Acc-Relative_Gravity.Y;
	else
		Time.Y=Up_Acc-Relative_Gravity.Y;
	Time.Y=Math.Abs(Relative_CurrentVelocity.Y/Time.Y);
	if(Relative_CurrentVelocity.Z>=0)
		Time.Z=Forward_Acc-Relative_Gravity.Z;
	else
		Time.Z=Backward_Acc-Relative_Gravity.Z;
	Time.Z=Math.Abs(Relative_CurrentVelocity.X/Time.Z);
	Time_To_Stop=Math.Max(Math.Max(Time.X,Time.Y),Time.Z);
	
	Time_To_Crash=-1;
	LastElevation=Elevation;
	Elevation=double.MaxValue;
	if(!Me.CubeGrid.IsStatic&&Gravity.Length()>0)
		Crash_Test();
}

void PrintNotifications(){
	if(Notifications.Count>0){
		string written=Me.GetSurface(0).GetText();
		Me.GetSurface(0).WriteText("",false);
		try{
			Write("--Notifications--");
			Dictionary<string,int> N_Counter=new Dictionary<string,int>();
			List<string> Messages=new List<string>();
			for(int i=0;i<Notifications.Count;i++){
				Notifications[i].Time=Math.Max(0,Notifications[i].Time-seconds_since_last_update);
				string text=Notifications[i].Text;
				if(N_Counter.ContainsKey(text))
					N_Counter[text]++;
				else{
					N_Counter.Add(text,1);
					Messages.Add(text);
				}
				if(Notifications[i].Time<=0){
					Notifications.RemoveAt(i--);
					continue;
				}
			}
			foreach(string Text in Messages){
				string str="";
				int count=N_Counter[Text];
				if(count>1)
					str="("+count.ToString()+") ";
				Write("\""+str+Text+"\"");
			}
			Write("--Program--");
		}
		catch(Exception e){
			Me.GetSurface(0).WriteText(written,true);
			throw e;
		}
		Me.GetSurface(0).WriteText(written,true);
	}
}

public void Main(string argument,UpdateType updateSource){
	try{
		UpdateProgramInfo();
		if(updateSource==UpdateType.Script)
			TaskParser(argument);
		else if(updateSource!=UpdateType.Terminal)
			Main_Program(argument);
		else{
			if(argument.ToLower().IndexOf("task:")==0)
				TaskParser(argument.Substring(5));
			else
				Main_Program(argument);
		}
		PrintNotifications();
		if(Current_Display==5)
			Thruster_Graph(new CustomPanel(Me.GetSurface(0)));
	}
	catch(Exception E){
		Write(E.ToString());
		FactoryReset();
	}
}

//Sends an argument to a programmable block
bool Task_Send(Task task){
	IMyProgrammableBlock target=GenericMethods<IMyProgrammableBlock>.GetFull(task.Qualifiers[0]);
	if(target==null)
		return false;
	string arguments="";
	for(int i=1;i<task.Qualifiers.Count;i++){
		if(i!=1)
			arguments+='\n';
		arguments+=task.Qualifiers[i];
	}
	return target.TryRun(arguments);
}

//Tells the ship to orient to a specific forward direction
bool Task_Direction(Task task){
	Vector3D direction=new Vector3D(0,0,0);
	if(task.Qualifiers[0].IndexOf("At ")==0){
		if(Vector3D.TryParse(task.Qualifiers.Last().Substring(3),out direction)){
			Target_Direction=direction-ShipPosition;
			Target_Direction.Normalize();
			Do_Direction=true;
			return true;
		}
	}
	else if(Vector3D.TryParse(task.Qualifiers.Last(),out direction)){
		Target_Direction=direction;
		if(Target_Direction.Length()==0)
			return false;
		Target_Direction.Normalize();
		Do_Direction=true;
		return true;
	}
	return false;
}

//Tells the ship to orient to a specific up direction
bool Task_Up(Task task){
	Vector3D direction=new Vector3D(0,0,0);
	if(Vector3D.TryParse(task.Qualifiers.Last(),out direction)){
		Target_Up=direction;
		Target_Up.Normalize();
		Do_Up=true;
		return true;
	}
	return false;
}

struct Plane{
	public double A;
	public double B;
	public double C;
	public double D;
	
	public Plane(double a,double b,double c,double d){
		A=a;
		B=b;
		C=c;
		D=d;
	}
	
	public Plane(Vector3D a,Vector3D b,Vector3D c){
		A=(b.Y-a.Y)*(c.Z-a.Z)-(c.Y-a.Y)*(b.Z-a.Z);
		B=(b.Z-a.Z)*(c.X-a.X)-(c.Z-a.Z)*(b.X-a.X);
		C=(b.X-a.X)*(c.Y-a.Y)-(c.X-a.X)*(b.Y-a.Y);
		D=-1*(A*a.X+B*a.Y+C*a.Z);
	}
	
	//Creates a Tangent Plane at the given point with the given normal
	public Plane(Vector3D Point,Vector3D Normal){
		A=Normal.X;
		B=Normal.Y;
		C=Normal.Z;
		D=-1*(Normal.X*Point.X+Normal.Y*Point.Y+Normal.Z*Point.Z);
	}
	
	//Creates a Normal Vector from the plane
	public Vector3D Normal(){
		Vector3D output=new Vector3D(A,B,C);
		output.Normalize();
		return output;
	}
}
struct Sphere{
	public Vector3D Center;
	public double Radius;
	
	public Sphere(Vector3D c,double r){
		Center=c;
		Radius=r;
	}
	
	public bool On(Vector3D p){
		return Math.Abs(Math.Pow(p.X-Center.X,2)+Math.Pow(p.Y-Center.Y,2)+Math.Pow(p.Z-Center.Z,2)-Radius)<0.00001;
	}
}
struct Line{
	public Vector3D R;
	public Vector3D V;
	
	//Tangent Line on the Circle, where Vector3D Point is on the circle that results from the intersection of Plane P and Sphere S
	public Line(Plane P,Sphere S,Vector3D Point){
		Vector3D Tangent_Normal=Point-S.Center;
		Tangent_Normal.Normalize();
		V=Vector3D.Cross(P.Normal(),Tangent_Normal);
		R=Point;
	}
}

Vector3D True_Target_Position=new Vector3D(0,0,0);
//Tells the ship to fly to a specific location
bool Task_Go(Vector3D position){
	Target_Position=position;
	True_Target_Position=position;
	Do_Position=true;
	if(Gravity.Length()>0){
		Vector3D MyPosition=ShipPosition;
		double my_radius=(MyPosition-PlanetCenter).Length();
		Vector3D target_direction=Target_Position-PlanetCenter;
		double target_radius=target_direction.Length();
		target_direction.Normalize();
		double planet_radius=my_radius-Target_Elevation;
		double sealevel_radius=my_radius-Sealevel;
		Vector3D me_direction=MyPosition-PlanetCenter;
		me_direction.Normalize();
		double planet_angle=GetAngle(me_direction,target_direction);
		Write("Planetary Angle: "+Math.Round(planet_angle,1).ToString()+"°");
		if(target_radius>sealevel_radius+100){
			if(target_radius>planet_radius/3){
				if(planet_angle>30){
					if(target_radius>my_radius+2000)
						Target_Position=PlanetCenter-(planet_radius*4/3*Gravity_Direction);
					return true;
				}
			}
			else if(my_radius<sealevel_radius+400){
				Target_Position=PlanetCenter-((sealevel_radius+500)*Gravity_Direction);
				return true;
			}
		}
		if((planet_angle>2.5||(planet_angle>5&&target_radius-my_radius>2000))||Target_Elevation-MySize/2<Math.Max(30,Target_Distance/10)){
			while(planet_angle==180){
				//This offsets the angle so we can create a full Plane from the 3 points: Center,Here,Target
				Vector3D offset=new Vector3D(Rnd.Next(0,10)-5,Rnd.Next(0,10)-5,Rnd.Next(0,10)-5);
				offset.Normalize();
				Target_Position+=offset;
				target_direction=Target_Position-PlanetCenter;
				target_direction.Normalize();
				planet_angle=GetAngle(me_direction,target_direction);
			}
			double GoalElevation=500;
			
			if(Target_Elevation<500+MySize){
				//This increases the cruising altitude if the elevation is too low, for collision avoidance
				my_radius+=Math.Max(Math.Min(9.375*2*(GoalElevation-(Target_Elevation-MySize)),5000),400);
				MyPosition=(my_radius)*me_direction+PlanetCenter;
			}
			Target_Position=(my_radius)*target_direction+PlanetCenter;
			//Target is now at same altitude with respect to sealevel
			Plane Bisect=new Plane(Target_Position,MyPosition,PlanetCenter);
			//This plane now bisects the planet along both the current location and target
			Sphere Planet=new Sphere(PlanetCenter,my_radius);
			//This sphere now represents the planet at the current elevation
			Line Tangent=new Line(Bisect,Planet,MyPosition);
			//This line now represents the tangent of the planet in a direction that lines up with the target
			Vector3D Goal_Direction=Tangent.V;
			Vector3D My_Direction=Target_Position-MyPosition;
			My_Direction.Normalize();
			if(GetAngle(Goal_Direction,My_Direction)>GetAngle(-1*Goal_Direction,My_Direction))
				Goal_Direction*=-1;
			Target_Position=Goal_Direction*2500+MyPosition;
		}
	}
	return true;
}

//Tells the ship to match position
bool Task_Match(Task task){
	Match_Direction=true;
	return true;
}

//Main Job Loop
bool PerformTask(){
	Display(1,"Task: "+MyTask.Name+": "+MyTask.Type.ToString());
	switch(MyTask.Name){
		case "None":
			return true;
		case "Refuel":
			return RefuelTask();
		case "Stalking":
			return StalkingTask();
		case "Hunting":
			return HuntingTask();
	}
	return false;
}

void ProcessTasks(){
	Task_Resetter();
	PerformTask();
}

void Task_Resetter(){
	Do_Direction=false;
	Do_Up=false;
	Do_Position=false;
	Match_Direction=false;
}

void Task_Pruner(Task task){
	bool duplicate=false;
	foreach(Task t in Task_Queue){
		if(t.Type==task.Type){
			duplicate=true;
			break;
		}
	}
	if(duplicate){
		Queue<Task> Recycling=new Queue<Task>();
		while(Task_Queue.Count>0){
			Task t=Task_Queue.Dequeue();
			if(!t.Type.Equals(task.Type))
				Recycling.Enqueue(t);
		}
		while(Recycling.Count>0)
			Task_Queue.Enqueue(Recycling.Dequeue());
	}
}

void TaskParser(string argument){
	string[] tasks=argument.Split('•');
	foreach(string task in tasks){
		if(task.Trim().Length==0)
			continue;
		Task t=null;
		if(Task.TryParse(task,out t)){
			if(t.Duration==Quantifier.Stop)
				PerformTask(t);
			else{
				Task_Pruner(t);
				Task_Queue.Enqueue(t);
			}
		}
		else{
			if(t==null)
				Notifications.Add(new Notification("Failed to parse \""+task+"\"",15));
			else{
				Notifications.Add(new Notification("Failed to parse \""+task+"\": Got\""+t.ToString()+"\"",15));
			}
		}
	}
}

void Main_Program(string argument){
	ProcessTasks();
	UpdateSystemData();
	if(!Me.CubeGrid.IsStatic){
		if(Elevation!=double.MaxValue){
			Display(1,"Elevation: "+Math.Round(Elevation,1).ToString());
			Display(1,"Sealevel: "+Math.Round(Sealevel,1).ToString());
		}
		if(Gravity.Length()>0){
			Display(1,"Gravity:"+Math.Round(Gravity.Length()/9.814,2)+"Gs");
			double grav=Relative_Gravity.Y;
			if(Relative_Gravity.Y>0.1)
				Display(1,"Gravity Up:"+Math.Round(Math.Abs(grav)/9.814,2)+"Gs");
			else if(Relative_Gravity.Y<-0.1)
				Display(1,"Gravity Down:"+Math.Round(Math.Abs(grav)/9.814,2)+"Gs");
			grav=Relative_Gravity.Z;
			if(Relative_Gravity.Z>0.1)
				Display(1,"Gravity Back:"+Math.Round(Math.Abs(grav)/9.814,2)+"Gs");
			else if(Relative_Gravity.Z<-0.1)
				Display(1,"Gravity Forwd:"+Math.Round(Math.Abs(grav)/9.814,2)+"Gs");
			grav=Relative_Gravity.X;
			if(Relative_Gravity.X>0.1)
				Display(1,"Gravity Right:"+Math.Round(Math.Abs(grav)/9.814,2)+"Gs");
			else if(Relative_Gravity.X<-0.1)
				Display(1,"Gravity Left:"+Math.Round(Math.Abs(grav)/9.814,2)+"Gs");
		}
		Display(2,"Acceleration Up:"+Math.Round(Up_Gs,2)+"Gs");
		Display(2,"Acceleration Forwd:"+Math.Round(Forward_Gs,2)+"Gs");
		Display(2,"Acceleration Back:"+Math.Round(Backward_Gs,2)+"Gs");
		Display(2,"Acceleration Down:"+Math.Round(Down_Gs,2)+"Gs");
		Display(2,"Acceleration Left:"+Math.Round(Left_Gs,2)+"Gs");
		Display(2,"Acceleration Right:"+Math.Round(Right_Gs,2)+"Gs");
	}
	if(argument.ToLower().Equals("factory reset")){
		FactoryReset();
	}
	
	if(!Me.CubeGrid.IsStatic&&ShipMass.PhysicalMass>0){
		if(Control_Thrusters)
			SetThrusters();
		else
			ResetThrusters();
		if(Control_Gyroscopes)
			SetGyroscopes();
		else
			Gyroscope.GyroOverride=false;
	}
	else
		ResetThrusters();
	Runtime.UpdateFrequency=GetUpdateFrequency();
}