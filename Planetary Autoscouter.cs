/*
* Autoscouter AI System
* Built by mlq1616
* https://github.com/mlq1819
*/
string Program_Name = "Autoscouter AI";
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);
double Speed_Limit=1000;
double Guest_Mode_Timer=900;
double Acceptable_Angle=1;
bool Control_Gyroscopes=true;
bool Control_Thrusters=true;
int Graph_Length_Seconds=180;

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

enum MenuType{
	Submenu=0,
	Command=1,
	Display=2
}
interface MenuOption{
	string Name();
	MenuType Type();
	bool AutoRefresh();
	int Depth();
	bool Back();
	bool Select();
}

class Menu_Submenu:MenuOption{
	string _Name;
	public string Name(){
		return _Name;
	}
	public MenuType Type(){
		return MenuType.Submenu;
	}
	public bool AutoRefresh(){
		if(Selected){
			return Menu[Selection].AutoRefresh();
		}
		return Last_Count==Count||Count>10;
	}
	public int Depth(){
		if(Selected){
			return 1+Menu[Selection].Depth();
		}
		return 1;
	}
	public bool Selected;
	public int Selection;
	
	int Last_Count;
	public int Display_Count{
		get{
			if(Count>0&&Menu[0].Type()==MenuType.Command){
				for(int i=1;i<Count;i++){
					if(Menu[i].Type()!=MenuType.Display)
						return Count;
				}
				return Count-1;
			}
			return Count;
		}
	}
	public int Count{
		get{
			return Menu.Count;
		}
	}
	
	public List<MenuOption> Menu;
	
	public Menu_Submenu(string name){
		_Name=name.Trim().Substring(0, Math.Min(name.Trim().Length, 24));
		Menu=new List<MenuOption>();
		Selection=0;
		Last_Count=0;
	}
	
	public bool Add(MenuOption new_item){
		Menu.Add(new_item);
		return true;
	}
	
	public bool Back(){
		if(Selected){
			if(Menu[Selection].Back())
				return true;
			Selected=false;
			return true;
		}
		return false;
	}
	
	public bool Select(){
		if(Selected){
			bool output=Menu[Selection].Select();
			if(Menu[Selection].Type()==MenuType.Command)
				Selected=false;
			return output;
		}
		Selected=true;
		return true;
	}
	
	public bool Next(){
		if(Selected){
			if(Menu[Selection].Type()==MenuType.Submenu){
				return ((Menu_Submenu)(Menu[Selection])).Next();
			}
			return false;
		}
		if(Count>0)
			Selection=(Selection+1)%Count;
		return true;
	}
	
	public bool Prev(){
		if(Selected){
			if(Menu[Selection].Type()==MenuType.Submenu){
				return ((Menu_Submenu)(Menu[Selection])).Prev();
			}
			return false;
		}
		if(Count>0)
			Selection=(Selection-1+Count)%Count;
		return true;
	}
	
	public bool Replace(Menu_Submenu Replacement){
		for(int i=0;i<Count;i++){
			if(Menu[i].Name().Equals(Replacement.Name())){
				Menu[i]=Replacement;
				return true;
			}
		}
		return false;
	}
	
	public override string ToString(){
		if(Count>0)
			Selection=Selection%Count;
		if(Selected)
			return Menu[Selection].ToString();
		string output=" -- "+Name()+" -- ";
		if(Count<=7){
			for(int i=0;i<Count;i++){
				output+="\n ";
				switch(Menu[i].Type()){
					case MenuType.Submenu:
						output+="[";
						break;
					case MenuType.Command:
						output+="<";
						break;
					case MenuType.Display:
						output+="(";
						break;
				}
				output+=' ';
				if(Selection==i)
					output+=' '+Menu[i].Name().ToUpper()+' ';
				else 
					output+=Menu[i].Name().ToLower();
				switch(Menu[i].Type()){
					case MenuType.Submenu:
						output+=" ("+(Menu[i] as Menu_Submenu).Display_Count.ToString()+")]";
						break;
					case MenuType.Command:
						output+=">";
						break;
					case MenuType.Display:
						output+=")";
						break;
				}
			}
		}
		else{
			int count=0;
			for(int i=Selection; count<=7 && i<Count;i++){
				count++;
				output+="\n ";
				switch(Menu[i].Type()){
					case MenuType.Submenu:
						output+="[";
						break;
					case MenuType.Command:
						output+="<";
						break;
					case MenuType.Display:
						output+="(";
						break;
				}
				output+=' ';
				if(Selection==i)
					output+=' '+Menu[i].Name().ToUpper()+' ';
				else
					output+=Menu[i].Name().ToLower();
				switch(Menu[i].Type()){
					case MenuType.Submenu:
						output+="]";
						break;
					case MenuType.Command:
						output+=">";
						break;
					case MenuType.Display:
						output+=")";
						break;
				}
			}
		}
		return output;
	}
}

class Menu_Command<T>:MenuOption where T:class{
	string _Name;
	public string Name(){
		return _Name;
	}
	public MenuType Type(){
		return MenuType.Command;
	}
	bool _AutoRefresh;
	public bool AutoRefresh(){
		return _AutoRefresh;
	}
	public int Depth(){
		return 1;
	}
	string Desc;
	T Arg;
	Func<T, bool> Command;
	
	public Menu_Command(string name,Func<T, bool> command,string desc="No description provided",T arg=null,bool autorefresh=false){
		if(name.Trim().Length > 0)
			_Name=name;
		Desc=desc;
		Arg=arg;
		Command=command;
		_AutoRefresh=autorefresh;
	}
	
	public bool Select(){
		return Command(Arg);
	}
	
	public bool Back(){
		return false;
	}
	
	public override string ToString(){
		string output=Name()+'\n';
		string[] words=Desc.Split(' ');
		int length=24;
		foreach(string word in words){
			if(length > 0 && length+word.Length > 24){
				length=0;
				output+='\n';
			}
			else
				output+=' ';
			output+=word;
			if(word.Contains('\n'))
				length=word.Length-word.IndexOf('\n')-1;
			else
				length+=word.Length;
		}
		return output+"\n\nSelect to Execute";
	}
}

class Airlock{
	public IMyDoor Door1;
	public IMyDoor Door2;
	public IMyAirVent Vent;
	public string Name{
		get{
			string name=Door1.CustomName;
			if(name.Contains("Door 1")){
				name=name.Substring(0,name.IndexOf("Door 1"))+name.Substring(name.IndexOf("Door 1")+"Door 1".Length);
			}
			return name.Trim();
		}
	}
	public double AirlockTimer;
	public Airlock(IMyDoor d1,IMyDoor d2,IMyAirVent v=null){
		Door1=d1;
		Door2=d2;
		Vent=v;
		AirlockTimer=10;
	}
	public bool Equals(Airlock o){
		return Door1.Equals(o.Door1)&&Door2.Equals(o.Door2)&&((Vent==null&&o.Vent==null)||Vent.Equals(o.Vent));
	}
	public double Distance(Vector3D Reference){
		double distance_1=(Reference-Door1.GetPosition()).Length();
		double distance_2=(Reference-Door2.GetPosition()).Length();
		return Math.Min(distance_1, distance_2);
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
	if(new_line)
		Me.GetSurface(0).WriteText(text+'\n', append);
	else
		Me.GetSurface(0).WriteText(text, append);
	foreach(CustomPanel Panel in DebugLCDs){
		if(new_line)
			Panel.Display.WriteText(text+'\n', append);
		else
			Panel.Display.WriteText(text, append);
	}
}

string GetRemovedString(string big_string, string small_string){
	string output=big_string;
	if(big_string.Contains(small_string)){
		output=big_string.Substring(0, big_string.IndexOf(small_string))+big_string.Substring(big_string.IndexOf(small_string)+small_string.Length);
	}
	return output;
}

List<List<IMyDoor>> RemoveDoor(List<List<IMyDoor>> list,IMyDoor Door){
	List<List<IMyDoor>> output=new List<List<IMyDoor>>();
	Echo("\tRemoving Door \""+Door.CustomName+"\" from list["+list.Count+"]");
	if(list.Count==0)
		return output;
	string ExampleDoorName="";
	foreach(List<IMyDoor> sublist in list){
		if(sublist.Count>0){
			ExampleDoorName=sublist[0].CustomName;
			break;
		}
	}
	
	bool is_leading_group=(ExampleDoorName.Contains("Door 1")&&Door.CustomName.Contains("Door 1"))||(ExampleDoorName.Contains("Door 2")&&Door.CustomName.Contains("Door 2"));
	for(int i=0;i<list.Count;i++){
		if(list[i].Count>1&&(!is_leading_group||!list[i][0].Equals(Door))){
			if(is_leading_group)
				output.Add(list[i]);
			else{
				List<IMyDoor> pair=new List<IMyDoor>();
				pair.Add(list[i][0]);
				for(int j=1;j<list[i].Count;j++){
					if(!list[i][j].Equals(Door))
						pair.Add(list[i][j]);
				}
				if(pair.Count>1)
					output.Add(pair);
			}
		}
	}
	return output;
}

Color ColorParse(string parse){
	parse=parse.Substring(parse.IndexOf('{')+1);
	parse=parse.Substring(0, parse.IndexOf('}')-1);
	string[] args=parse.Split(' ');
	int r, g, b, a;
	r=Int32.Parse(args[0].Substring(args[0].IndexOf("R:")+2).Trim());
	g=Int32.Parse(args[1].Substring(args[1].IndexOf("G:")+2).Trim());
	b=Int32.Parse(args[2].Substring(args[2].IndexOf("B:")+2).Trim());
	a=Int32.Parse(args[3].Substring(args[3].IndexOf("A:")+2).Trim());
	return new Color(r,g,b,a);
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

struct Altitude_Data{
	public TimeSpan Timestamp;
	public double Sealevel;
	public double Elevation;
	
	public Altitude_Data(double S,double E,TimeSpan start){
		Sealevel=S;
		Elevation=E;
		Timestamp=start;
	}
}

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;
Random Rnd;

IMyShipController Controller;
List<IMyShipController> Controllers;
IMyGyro Gyroscope;

List<CustomPanel> StatusLCDs;
List<CustomPanel> DebugLCDs;
List<CustomPanel> CommandLCDs;
List<CustomPanel> AltitudeLCDs;

Queue<Altitude_Data> Altitude_Graph;
double Altitude_Timer=0;

List<Airlock> Airlocks;

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

List<IMyAirtightSlideDoor> MyDoors;
List<IMyAirtightSlideDoor> OpenDoors;

double Time_To_Crash=double.MaxValue;
double Guest_Timer=double.MaxValue;
Menu_Submenu Command_Menu;

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

bool Guest_Mode=false;

float Mass_Accomodation=0.0f;

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
Vector3D CurrentVelocity;
Vector3D Velocity_Direction{
	get{
		Vector3D VD=CurrentVelocity;
		VD.Normalize();
		return VD;
	}
}
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
	return ctr.IsSameConstructAs(Me)&&ctr.CanControlShip&&ctr.ControlThrusters;
}
void SetupAirlocks(){
	Airlocks=new List<Airlock>();
	List<IMyDoor> AllAirlockDoors=GenericMethods<IMyDoor>.GetAllConstruct("Airlock");
	List<IMyDoor> AllAirlockDoor1s=new List<IMyDoor>();
	List<IMyDoor> AllAirlockDoor2s=new List<IMyDoor>();
	foreach(IMyDoor Door in AllAirlockDoors){
		if(Door.CustomName.Contains("Door 1")){
			AllAirlockDoor1s.Add(Door);
		}
		else if(Door.CustomName.Contains("Door 2")){
			AllAirlockDoor2s.Add(Door);
		}
	}
	List<List<IMyDoor>> PossibleAirlockDoor1Pairs=new List<List<IMyDoor>>();
	foreach(IMyDoor Door1 in AllAirlockDoor1s){
		List<IMyDoor> pair=new List<IMyDoor>();
		pair.Add(Door1);
		List<IMyDoor> Copy=new List<IMyDoor>();
		string name=GetRemovedString(Door1.CustomName,"Door 1");
		foreach(IMyDoor Door2 in AllAirlockDoor2s){
			Copy.Add(Door2);
		}
		foreach(IMyDoor Door2 in GenericMethods<IMyDoor>.SortByDistance(Copy, Door1)){
			if(GetRemovedString(Door2.CustomName,"Door 2").Equals(name))
				pair.Add(Door2);
		}
		if(pair.Count > 1)
			PossibleAirlockDoor1Pairs.Add(pair);
	}
	List<List<IMyDoor>> PossibleAirlockDoor2Pairs=new List<List<IMyDoor>>();
	foreach(IMyDoor Door2 in AllAirlockDoor2s){
		List<IMyDoor> pair=new List<IMyDoor>();
		pair.Add(Door2);
		List<IMyDoor> Copy=new List<IMyDoor>();
		string name=GetRemovedString(Door2.CustomName,"Door 2");
		foreach(IMyDoor Door1 in AllAirlockDoor1s){
			Copy.Add(Door1);
		}
		foreach(IMyDoor Door1 in GenericMethods<IMyDoor>.SortByDistance(Copy, Door2)){
			if(GetRemovedString(Door1.CustomName,"Door 1").Equals(name))
				pair.Add(Door1);
		}
		if(pair.Count>1){
			PossibleAirlockDoor2Pairs.Add(pair);
		}
	}
	int removed=0;
	do{
		removed=0;
		foreach(List<IMyDoor> pair1 in PossibleAirlockDoor1Pairs){
			if(pair1.Count<=1){
				IMyDoor Door=pair1[0];
				PossibleAirlockDoor1Pairs=RemoveDoor(PossibleAirlockDoor1Pairs, Door);
				PossibleAirlockDoor2Pairs=RemoveDoor(PossibleAirlockDoor2Pairs, Door);
				continue;
			}
			foreach(List<IMyDoor> pair2 in PossibleAirlockDoor2Pairs){
				if(pair2.Count<=1){
					IMyDoor Door=pair2[0];
					PossibleAirlockDoor1Pairs=RemoveDoor(PossibleAirlockDoor1Pairs, Door);
					PossibleAirlockDoor2Pairs=RemoveDoor(PossibleAirlockDoor2Pairs, Door);
					continue;
				}
				if(pair2[0].Equals(pair1[1])&&pair1[0].Equals(pair2[1])){
					removed++;
					IMyDoor Door1=pair1[0];
					IMyDoor Door2=pair2[0];
					Airlocks.Add(new Airlock(Door1, Door2));
					PossibleAirlockDoor1Pairs=RemoveDoor(RemoveDoor(PossibleAirlockDoor1Pairs, Door1), Door2);
					PossibleAirlockDoor2Pairs=RemoveDoor(RemoveDoor(PossibleAirlockDoor2Pairs, Door2), Door1);
					break;
				}
			}
		}
	} 
	while(removed>0&&PossibleAirlockDoor1Pairs.Count>0&&PossibleAirlockDoor2Pairs.Count>0);
	foreach(List<IMyDoor> pair1 in PossibleAirlockDoor1Pairs){
		if(pair1.Count<=1){
			IMyDoor Door=pair1[0];
			PossibleAirlockDoor1Pairs=RemoveDoor(PossibleAirlockDoor1Pairs, Door);
			PossibleAirlockDoor2Pairs=RemoveDoor(PossibleAirlockDoor2Pairs, Door);
			continue;
		}
		IMyDoor Door1=pair1[0];
		IMyDoor Door2=pair1[1];
		Airlocks.Add(new Airlock(Door1, Door2));
		PossibleAirlockDoor1Pairs=RemoveDoor(RemoveDoor(PossibleAirlockDoor1Pairs, Door1), Door2);
		PossibleAirlockDoor2Pairs=RemoveDoor(RemoveDoor(PossibleAirlockDoor2Pairs, Door2), Door1);
	}
	for(int i=0;i<Airlocks.Count;i++){
		string name=GetRemovedString(Airlocks[i].Door1.CustomName,"Door 1");
		List<IMyAirVent> Vents=GenericMethods<IMyAirVent>.GetAllConstruct(name+"Air Vent",Airlocks[i].Door1);
		foreach(IMyAirVent vent in Vents){
			if(vent.CustomName.Equals(name+"Air Vent"))
				Airlocks[i].Vent=vent;
		}
	}
}

UpdateFrequency GetUpdateFrequency(){
	return UpdateFrequency.Update1;
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

void Reset(){
	Operational=false;
	if(Do_Breakdown)
		Breakdown();
	Runtime.UpdateFrequency=UpdateFrequency.None;
	Controller=null;
	Gyroscope=null;
	FunctionalBlocks=new List<IMyFunctionalBlock>();
	StatusLCDs=new List<CustomPanel>();
	DebugLCDs=new List<CustomPanel>();
	CommandLCDs=new List<CustomPanel>();
	AltitudeLCDs=new List<CustomPanel>();
	Altitude_Graph=new Queue<Altitude_Data>();
	List<Airlock> Airlocks=new List<Airlock>();
	for(int i=0;i<All_Thrusters.Length;i++)
		All_Thrusters[i]=new List<IMyThrust>();
	MyDoors=new List<IMyAirtightSlideDoor>();
	OpenDoors=new List<IMyAirtightSlideDoor>();
	RestingSpeed=0;
}

bool Setup(){
	Reset();
	GridTerminalSystem.GetBlocksOfType<IMyFunctionalBlock>(FunctionalBlocks);
	List<IMyTextPanel> LCDs=GenericMethods<IMyTextPanel>.GetAllConstruct("Ship Status");
	foreach(IMyTextPanel Panel in LCDs){
		StatusLCDs.Add(new CustomPanel(Panel));
		FunctionalBlocks.Add(Panel);
	}
	LCDs=GenericMethods<IMyTextPanel>.GetAllConstruct("AI Visual Display");
	foreach(IMyTextPanel Panel in LCDs){
		DebugLCDs.Add(new CustomPanel(Panel));
		FunctionalBlocks.Add(Panel);
	}
	LCDs=GenericMethods<IMyTextPanel>.GetAllConstruct("Command Menu Display");
	foreach(IMyTextPanel Panel in LCDs){
		CommandLCDs.Add(new CustomPanel(Panel));
		FunctionalBlocks.Add(Panel);
	}
	LCDs=GenericMethods<IMyTextPanel>.GetAllConstruct("Altitude");
	foreach(IMyTextPanel Panel in LCDs){
		AltitudeLCDs.Add(new CustomPanel(Panel));
		FunctionalBlocks.Add(Panel);
	}
	foreach(CustomPanel Panel in AltitudeLCDs){
		if(Panel.Trans){
			Panel.Display.FontColor=DEFAULT_BACKGROUND_COLOR;
			Panel.Display.BackgroundColor=new Color(0,0,0,0);
		}
		else{
			Panel.Display.FontColor=DEFAULT_TEXT_COLOR;
			Panel.Display.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
		Panel.Display.Font="Monospace";
		Panel.Display.Alignment=TextAlignment.LEFT;
		Panel.Display.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.Display.TextPadding=0;
		Panel.Display.FontSize=0.5f;
	}
	foreach(CustomPanel Panel in DebugLCDs){
		if(Panel.Trans){
			Panel.Display.FontColor=DEFAULT_BACKGROUND_COLOR;
			Panel.Display.BackgroundColor=new Color(0,0,0,0);
		}
		else{
			Panel.Display.FontColor=DEFAULT_TEXT_COLOR;
			Panel.Display.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
		Panel.Display.Alignment=TextAlignment.CENTER;
		Panel.Display.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.Display.TextPadding=10.0f;
		Panel.Display.FontSize=1.0f;
	}
	SetupAirlocks();
	Controller=(IMyShipController)GenericMethods<IMyRemoteControl>.GetContaining("Remote Control");
	Controllers=GenericMethods<IMyShipController>.GetAllFunc(ControllerFunction);
	if(Controller==null){
		Write("Failed to find Controller", false, false);
		return false;
	}
	bool has_main_ctrl=false;
	foreach(IMyShipController Ctrl in Controllers){
		if(Ctrl.CustomName.Equals(Controller.CustomName)){
			has_main_ctrl=true;
			break;
		}
	}
	if(!has_main_ctrl)
		Controllers.Add(Controller);
	Forward=Controller.Orientation.Forward;
	Up=Controller.Orientation.Up;
	Left=Controller.Orientation.Left;
	foreach(IMyShipController Ctrl in Controllers){
		if((Ctrl as IMyTextSurfaceProvider)!=null){
			IMyTextSurfaceProvider Cockpit=Ctrl as IMyTextSurfaceProvider;
			int valid_surface_count=0;
			for(int i=0;i<Cockpit.SurfaceCount;i++){
				Cockpit.GetSurface(i).FontColor=DEFAULT_TEXT_COLOR;
				Cockpit.GetSurface(i).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
				Cockpit.GetSurface(i).Alignment=TextAlignment.CENTER;
				Cockpit.GetSurface(i).ScriptForegroundColor=DEFAULT_TEXT_COLOR;
				Cockpit.GetSurface(i).ScriptBackgroundColor=DEFAULT_BACKGROUND_COLOR;
				if(Cockpit.GetSurface(i).ContentType==ContentType.NONE){
					Cockpit.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
					SetBlockData(Ctrl,"UseSurface"+(i).ToString(),"TRUE");
					switch(valid_surface_count++){
						case 0:
							CommandLCDs.Add(new CustomPanel(Cockpit.GetSurface(i)));
							break;
						case 1:
							StatusLCDs.Add(new CustomPanel(Cockpit.GetSurface(i)));
							break;
						case 2:
							DebugLCDs.Add(new CustomPanel(Cockpit.GetSurface(i)));
							break;
					}
				}
				else if(GetBlockData(Ctrl,"UseSurface"+(i).ToString()).Equals("TRUE")){
					Cockpit.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
					switch(valid_surface_count++){
						case 0:
							CommandLCDs.Add(new CustomPanel(Cockpit.GetSurface(i)));
							break;
						case 1:
							StatusLCDs.Add(new CustomPanel(Cockpit.GetSurface(i)));
							break;
						case 2:
							DebugLCDs.Add(new CustomPanel(Cockpit.GetSurface(i)));
							break;
					}
				}
			}
		}
	}
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
	for(int i=0;i<2;i++){
		bool retry=!Me.CubeGrid.IsStatic;
		foreach(IMyThrust Thruster in MyThrusters){
			/*if(HasBlockData(Thruster, "Owner")){
				long ID=0;
				if(i==0&&!Int64.TryParse(GetBlockData(Thruster, "Owner"),out ID)||(ID!=0&&ID!=Me.CubeGrid.EntityId))
					continue;
			}*/
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
			FunctionalBlocks.Add(Thruster);
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
	MyDoors=GenericMethods<IMyAirtightSlideDoor>.GetAllIncluding("");
	foreach(IMyAirtightSlideDoor Door in MyDoors){
		Door.CloseDoor();
		SetBlockData(Door,"Timer","0");
	}
	
	List<IMyTerminalBlock> AllTerminalBlocks=new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(AllTerminalBlocks);
	MySize=0;
	foreach(IMyTerminalBlock Block in AllTerminalBlocks){
		double distance=(Controller.GetPosition()-Block.GetPosition()).Length();
		MySize=Math.Max(MySize,distance);
	}
	Operational=Me.IsWorking;
	Runtime.UpdateFrequency=GetUpdateFrequency();
	List<IMyPowerProducer> Power=GenericMethods<IMyPowerProducer>.GetAllConstruct("");
	foreach(IMyPowerProducer power in Power)
		FunctionalBlocks.Add(power);
	List<IMyGasTank> Gas=GenericMethods<IMyGasTank>.GetAllConstruct("");
	foreach(IMyGasTank gas in Gas)
		FunctionalBlocks.Add(gas);
	List<IMyLargeTurretBase> Turrets=GenericMethods<IMyLargeTurretBase>.GetAllConstruct("");
	foreach(IMyLargeTurretBase turret in Turrets)
		FunctionalBlocks.Add(turret);
	for(int i=0;i<FunctionalBlocks.Count;i++){
		if(FunctionalBlocks[i].CustomName.Contains("Shell")){
			FunctionalBlocks.RemoveAt(i--);
			continue;
		}
	}
	
	if(Orbiting==false){
		Orbit();
		Orbital_Altitude=1000;
		RestingSpeed=25;
	}
	
	return true;
}

void GetSettings(){
	string[]args=Me.CustomData.Split('\n');
	foreach(string arg in args){
		string[]ags=arg.Split(';');
		if(ags.Length>1){
			switch(ags[0]){
				case "Program_Name":
					Program_Name=ags[1];
					break;
				case "Speed_Limit":
					double.TryParse(ags[1],out Speed_Limit);
					break;
				case "Guest_Mode_Timer":
					double.TryParse(ags[1],out Guest_Mode_Timer);
					break;
				case "Acceptable_Angle":
					double.TryParse(ags[1],out Acceptable_Angle);
					break;
				case "Control_Gyroscopes":
					bool.TryParse(ags[1],out Control_Gyroscopes);
					break;
				case "Control_Thrusters":
					bool.TryParse(ags[1],out Control_Thrusters);
					break;
			}
		}
	}
}

bool Operational=false;
public Program(){
	Prog.P=this;
	GetSettings();
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
	Rnd=new Random();
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		if(!arg.Contains(':'))
			continue;
		int index=arg.IndexOf(':');
		string name=arg.Substring(0,index);
		string data=arg.Substring(index+1);
		switch(name){
			case "Breakdown":
				bool.TryParse(data,out Do_Breakdown);
				break;
			case "BD_Count":
				int.TryParse(data,out BD_Count);
				break;
			case "Orbiting":
				bool.TryParse(data,out Orbiting);
				break;
			case "Orbital_Altitude":
				double.TryParse(data,out Orbital_Altitude);
				break;
			case "RestingSpeed":
				double.TryParse(data,out RestingSpeed);
				break;
			case "Terrain":
				bool.TryParse(data,out Terrain);
				break;
		}
	}
	Setup();
	CreateMenu();
	DisplayMenu();
}

public void Save(){
    this.Storage="Breakdown:"+Do_Breakdown.ToString();
	this.Storage+="•BD_Count:"+BD_Count.ToString();
	this.Storage+="•Orbiting:"+Orbiting.ToString();
	this.Storage+="•Orbital_Altitude:"+Math.Round(Orbital_Altitude,1).ToString();
	this.Storage+="•RestingSpeed:"+Math.Round(RestingSpeed,1).ToString();
	this.Storage+="•Terrain:"+Terrain.ToString();
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	for(int i=0;i<All_Thrusters.Length;i++){
		foreach(IMyThrust Thruster in All_Thrusters[i])
			ResetThruster(Thruster);
	}
	bool ship=!Me.CubeGrid.IsStatic;
	GetSettings();
	Me.CustomData="Program_Name"+';'+Program_Name;
	if(ship)
		Me.CustomData+='\n'+"Speed_Limit"+';'+Speed_Limit.ToString();
	Me.CustomData+='\n'+"Guest_Mode_Timer"+';'+Guest_Mode_Timer.ToString();
	Me.CustomData+='\n'+"Acceptable_Angle"+';'+Acceptable_Angle.ToString();
	if(ship)
		Me.CustomData+='\n'+"Control_Gyroscopes"+';'+Control_Gyroscopes.ToString();
	if(ship)
		Me.CustomData+='\n'+"Control_Thrusters"+';'+Control_Thrusters.ToString();
}

char Bitsplice(char input){
	short shrt=(short)input;
	bool[] bits=new bool[16];
	for(int j=0;j<16;j++){
		bits[j]=((shrt/Math.Pow(2,j))%2)==1;
	}
	int m=Glitch.Next(1,Math.Max(2,BD_Count/20));
	for(int i=0;i<m;i++){
		int k=Glitch.Next(0,16);
		bits[k]=!bits[k];
	}
	shrt=0;
	for(int j=0;j<16;j++){
		if(bits[j])
			shrt+=(short)Math.Pow(2,j);
	}
	return (char)shrt;
}

string Stringsplice(string input){
	string text=input;
	Glitch=new Random(Glitch_Seed);
	for(int i=0;i<text.Length;i++){
		bool valid_char=true;
		char c=text[i];
		if(c=='\n'||c==' '||c=='['||c==']'||c=='<'||c=='>'||c=='-')
			valid_char=false;
		if(valid_char&&Glitch.Next(0,1000)/5.0f<=BD_Percent){
			char output=Bitsplice(text[i]);
			if(i<text.Length-1)
				text=text.Substring(0,i)+output+text.Substring(i+1);
			else
				text=text.Substring(0,text.Length-1)+output;
		}
	}
	return text;
}

enum AlertStatus{
	Green=0,
	Blue=1,
	Yellow=2,
	Orange=3,
	Red=4
}
string Submessage="";
AlertStatus ShipStatus{
	get{
		AlertStatus status=AlertStatus.Green;
		Submessage="";
		if(!Safety){
			AlertStatus nw_sts=AlertStatus.Yellow;
			status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
			Submessage+="\nSafety Protocols disengaged";
		}
		if(Do_Breakdown){
			AlertStatus nw_sts=AlertStatus.Yellow;
			if(BD_Percent>=40)
				nw_sts=AlertStatus.Red;
			else if(BD_Percent>=25)
				nw_sts=AlertStatus.Orange;
			status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
			Submessage+="\nShip is Breaking Down - "+Math.Round(BD_Timer,2)+" s\n"+Math.Round(BD_Percent,1).ToString()+"% Broken Down";
		}
		int Broken=Broken_Blocks;
		if(Broken>0){
			AlertStatus nw_sts=AlertStatus.Yellow;
			if(Broken>=50)
				nw_sts=AlertStatus.Red;
			else if(Broken>=20)
				nw_sts=AlertStatus.Orange;
			status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
			Submessage+="\n"+Broken.ToString()+" detected damaged blocks";
		}
		if(!Me.CubeGrid.IsStatic){
			List<IMyJumpDrive> JumpDrives=GenericMethods<IMyJumpDrive>.GetAllIncluding("");
			foreach(IMyJumpDrive Drive in JumpDrives){
				if(Drive.Status==MyJumpDriveStatus.Jumping){
					AlertStatus nw_sts=AlertStatus.Blue;
					status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
					Submessage+="\nShip is Jumping";
				}
			}
			if(Forward_Thrust==1){
				AlertStatus nw_sts=AlertStatus.Yellow;
				status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				Submessage+="\nNo Forward Thrusters";
			}
			if(Backward_Thrust==1){
				AlertStatus nw_sts=AlertStatus.Yellow;
				status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				Submessage+="\nNo Backward Thrusters";
			}
			if(Up_Thrust==1){
				AlertStatus nw_sts=AlertStatus.Yellow;
				status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				Submessage+="\nNo Up Thrusters";
			}
			if(Down_Thrust==1){
				AlertStatus nw_sts=AlertStatus.Yellow;
				status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				Submessage+="\nNo Down Thrusters";
			}
			if(Left_Thrust==1){
				AlertStatus nw_sts=AlertStatus.Yellow;
				status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				Submessage+="\nNo Left Thrusters";
			}
			if(Right_Thrust==1){
				AlertStatus nw_sts=AlertStatus.Yellow;
				status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				Submessage+="\nNo Right Thrusters";
			}
			if(Gravity.Length()>0){
				if(Up_Gs<Gravity.Length()/10){
					AlertStatus nw_sts=AlertStatus.Yellow;
					if(Forward_Gs<Gravity.Length()/10){
						nw_sts=AlertStatus.Orange;
						double max_Gs=Math.Max(Right_Gs,Left_Gs);
						max_Gs=Math.Max(max_Gs,Down_Gs);
						max_Gs=Math.Max(max_Gs,Backward_Gs);
						if(max_Gs<Gravity.Length()/10){
							nw_sts=AlertStatus.Red;
							Submessage+="\nInsufficient Thrust to liftoff";
						}
						else
							Submessage+="\nInsufficient Vertical and Forward Thrust";
					}
					else
						Submessage+="\nInsufficient Vertical Thrust";
					status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				}
			}
			if(Orbiting){
				AlertStatus nw_sts=AlertStatus.Blue;
				status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
				string altitude=Math.Round(Elevation,0).ToString()+" M";
				string target_altitude=Math.Round(Orbital_Altitude,0).ToString()+" M";
				if(Orbital_Altitude>=1500){
					altitude=Math.Round(Elevation/1000,1).ToString()+" kM";
					target_altitude=Math.Round(Orbital_Altitude/1000,1).ToString()+" kM";
				}
				Submessage+="\nOrbiting at "+altitude+":"+target_altitude;
				double Obt_f=Orbital_Altitude/Elevation;
				string Obt_fs=Math.Round((1-Obt_f)*100,1)+"%";
				double Obt_d=Elevation-Orbital_Altitude;
				if(Obt_f>1.2||Obt_f<0.8||Math.Abs(Obt_d)>250){
					nw_sts=AlertStatus.Red;
					status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
					Submessage+="\nOrbital Decay: "+Math.Round(Obt_d,0).ToString()+" M: "+Obt_fs;
				}
				else if(Obt_f>1.1||Obt_f<0.9||Math.Abs(Obt_d)>100){
					nw_sts=AlertStatus.Orange;
					status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
					Submessage+="\nOrbital Decay: "+Math.Round(Obt_d,0).ToString()+" M: "+Obt_fs;
				}
				else if(Obt_f>1.025||Obt_f<0.975||Math.Abs(Obt_d)>20){
					nw_sts=AlertStatus.Yellow;
					status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
					Submessage+="\nOrbital Decay: "+Math.Round(Obt_d,0).ToString()+" M: "+Obt_fs;
				}
			}
			if(Controller.CalculateShipMass().PhysicalMass>0&&Mass_Accomodation>0){
				if(Controller.GetShipSpeed()>0.01&&Elevation-MySize/2<50){
					AlertStatus nw_sts=AlertStatus.Blue;
					status=(AlertStatus) Math.Max((int)status, (int)nw_sts);
					double psuedo_elevation=Math.Max(Elevation-MySize/2,0);
					double small=Math.Min(psuedo_elevation,Elevation);
					double large=Math.Max(psuedo_elevation,Elevation);
					if(Math.Abs(large-small)<0.5)
						Submessage+="\nShip at low Altitude ("+Math.Round(small,1).ToString()+" meters)";
					else
						Submessage+="\nShip at low Altitude ("+Math.Round(small,1).ToString()+"-"+Math.Round(large,1).ToString()+" meters)";
				}
			}
			if(Time_To_Crash>0){
				if(Time_To_Crash<15 && Controller.GetShipSpeed()>5){
					AlertStatus nw_sts=AlertStatus.Orange;
					status=(AlertStatus) Math.Max((int)status, (int)nw_sts);
					Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
				}
				else if(Time_To_Crash<60 && Controller.GetShipSpeed()>15){
					AlertStatus nw_sts=AlertStatus.Yellow;
					status=(AlertStatus) Math.Max((int)status, (int)nw_sts);
					Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
				}
				else if(Time_To_Crash<180){
					AlertStatus nw_sts=AlertStatus.Blue;
					status=(AlertStatus) Math.Max((int)status, (int)nw_sts);
					Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
				}
			}
			if(_Autoland&&CurrentVelocity.Length()>1){
				AlertStatus nw_sts=AlertStatus.Blue;
				status=(AlertStatus) Math.Max((int)status, (int)nw_sts);
				Submessage+="\nAutoland Enabled";
			}
		}
		if(TurretTimer<30){
			AlertStatus nw_sts=AlertStatus.Red;
			status=(AlertStatus) Math.Max((int)status,(int)nw_sts);
			Submessage+="\nEnemy Detected ("+Math.Round(TurretTimer,1).ToString()+" seconds ago";
		}
		else if(TurretTimer<120){
			AlertStatus nw_sts=AlertStatus.Orange;
			status=(AlertStatus) Math.Max((int)status,(int)nw_sts);
			Submessage+="\nEnemy Detected ("+Math.Round(TurretTimer,1).ToString()+" seconds ago";
		}
		else if(TurretTimer<300){
			AlertStatus nw_sts=AlertStatus.Yellow;
			status=(AlertStatus) Math.Max((int)status,(int)nw_sts);
			Submessage+="\nEnemy Detected ("+Math.Round(TurretTimer,1).ToString()+" seconds ago";
		}
		
		if(Guest_Mode){
			AlertStatus nw_sts=AlertStatus.Blue;
			status=(AlertStatus)Math.Max((int)status,(int)nw_sts);
			Submessage+="\nGuest Mode: "+ToString(FromSeconds(Guest_Mode_Timer-Guest_Timer));
		}
		
		if(Controller.GetShipSpeed()>50){
			AlertStatus nw_sts=AlertStatus.Blue;
			status=(AlertStatus) Math.Max((int)status, (int)nw_sts);
			double Speed=Controller.GetShipSpeed();
			Submessage += "\nHigh Ship Speed [";
			const int SECTIONS=20;
			for(int i=0; i<SECTIONS; i++){
				if(Speed>=((1000.0/SECTIONS)*i)){
					Submessage+='|';
				}
				else {
					Submessage+=' ';
				}
			}
			Submessage+=']';
		}
		if(status==AlertStatus.Green){
			Submessage="\nNo issues";
		}
		return status;
	}
}
void SetStatus(string message, Color TextColor, Color BackgroundColor){
	float padding=40.0f;
	string[] lines=message.Split('\n');
	padding=Math.Max(10.0f, padding-(lines.Length*5.0f));
	string text=message;
	if(Do_Breakdown)
		text=Stringsplice(text);
	foreach(CustomPanel LCD in StatusLCDs){
		LCD.Display.Alignment=TextAlignment.CENTER;
		LCD.Display.FontSize=1.2f;
		LCD.Display.ContentType=ContentType.TEXT_AND_IMAGE;
		LCD.Display.TextPadding=padding;
		LCD.Display.WriteText(text,false);
		if(LCD.Trans){
			LCD.Display.FontColor=BackgroundColor;
			LCD.Display.BackgroundColor=new Color(0,0,0,255);
		}
		else {
			LCD.Display.FontColor=TextColor;
			LCD.Display.BackgroundColor=BackgroundColor;
		}
	}
}

int Last_Status=-1;
void SetAlert(int Status_Num,Color color){
	if(!(Last_Status<0||((Last_Status<=1)==(Status_Num<=1)))){
		Last_Status=Status_Num;
		return;
	}
	List<IMyInteriorLight> Lights=GenericMethods<IMyInteriorLight>.GetAllConstruct("");
	foreach(IMyInteriorLight Light in Lights){
		if(!CanHaveJob(Light,"StatusAlert"))
			continue;
		if(Light.CustomName.Contains("Reactor")||Light.CustomName.Contains("Printer")){
			SetBlockData(Light,"Job","CustomAlert");
			continue;
		}
		if(Status_Num<=1&&Last_Status>1){
			if(HasBlockData(Light,"Job")&&GetBlockData(Light,"Job").Equals("StatusAlert")){
				if(HasBlockData(Light,"DefaultColor")){
					try{
						Light.Color=ColorParse(GetBlockData(Light,"DefaultColor"));
					}
					catch(Exception){
						Echo("Failed to parse color");
					}
				}
				if(HasBlockData(Light,"DefaultBlinkLength")){
					try{
						Light.BlinkLength=float.Parse(GetBlockData(Light,"DefaultBlinkLength"));
					}
					catch(Exception){
						;
					}
				}
				if(HasBlockData(Light,"DefaultBlinkInterval")){
					try{
						Light.BlinkIntervalSeconds=float.Parse(GetBlockData(Light,"DefaultBlinkInterval"));
					}
					catch(Exception){
						;
					}
				}
				if(HasBlockData(Light,"DefaultIntensity")){
					try{
						Light.Intensity=float.Parse(GetBlockData(Light,"DefaultIntensity"));
					}
					catch(Exception){
						;
					}
				}
				SetBlockData(Light,"Job","None");
			}
		}
		else if(Status_Num>1){
			if(!HasBlockData(Light,"DefaultColor"))
				SetBlockData(Light,"DefaultColor",Light.Color.ToString());
			if(!HasBlockData(Light,"DefaultBlinkLength"))
				SetBlockData(Light,"DefaultBlinkLength",Light.BlinkLength.ToString());
			if(!HasBlockData(Light,"DefaultBlinkInterval"))
				SetBlockData(Light,"DefaultBlinkInterval",Light.BlinkIntervalSeconds.ToString());
			if(!HasBlockData(Light,"DefaultIntensity"))
				SetBlockData(Light,"DefaultIntensity",Light.Intensity.ToString());
			SetBlockData(Light,"Job","StatusAlert");
			Light.Color=color;
			Light.BlinkIntervalSeconds=5-Status_Num;
			Light.BlinkLength=100-(25f/Light.BlinkIntervalSeconds);
			Light.Intensity=6+Status_Num;
		}
	}
	Last_Status=Status_Num;
}

void ResetThrusters(){
	for(int i=0;i<All_Thrusters.Length;i++){
		foreach(IMyThrust Thruster in All_Thrusters[i])
			Thruster.ThrustOverridePercentage=0;
	}
}

bool Disable(object obj=null){
	SetStatus("Status LCD\nOffline", new Color(255,255,255,255), new Color(0,0,0,255));
	Operational=false;
	ResetThrusters();
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	foreach(Airlock airlock in Airlocks){
		airlock.Door1.Enabled=true;
		airlock.Door2.Enabled=true;
	}
	Runtime.UpdateFrequency=UpdateFrequency.None;
	Me.Enabled=false;
	return true;
}
bool _Autoland=false;
bool Autoland(object obj=null){
	if((!_Autoland)&&!Control_Thrusters)
		return false;
	if(Orbiting||!Safety)
		return false;
	_Autoland=!_Autoland;
	return true;
}
bool FactoryReset(object obj=null){
	SetStatus("Status LCD\nOffline", DEFAULT_TEXT_COLOR, DEFAULT_BACKGROUND_COLOR);
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	for(int i=0;i<All_Thrusters.Length;i++){
		foreach(IMyThrust Thruster in All_Thrusters[i])
			ResetThruster(Thruster);
	}
	foreach(Airlock airlock in Airlocks){
		airlock.Door1.Enabled=true;
		airlock.Door2.Enabled=true;
	}
	Me.CustomData="";
	this.Storage="";
	Reset();
	Me.Enabled=false;
	return true;
}
bool GuestMode(object obj=null){
	Guest_Mode=!Guest_Mode;
	Guest_Timer=0;
	return true;
}
bool Orbiting=false;
double Orbital_Altitude=1000;
bool Orbit(object obj=null){
	if(RestingSpeed==0){
		//if(Sealevel<100)
			//return false;
		if(!Safety)
			ToggleSafety();
		RestingSpeed=CurrentVelocity.Length();
		Orbiting=true;
		Orbital_Altitude=Elevation;
		Controller.DampenersOverride=true;
	}
	else{
		RestingSpeed=0;
		Orbiting=false;
	}
	_Autoland=false;
	return true;
}
bool Safety=true;
bool ToggleSafety(object obj=null){
	if(Orbiting)
		return false;
	Safety=!Safety;
	if(Safety){
		for(int i=0;i<6;i++){
			foreach(IMyThrust Thrust in All_Thrusters[i])
				Thrust.Enabled=true;
		}
	}
	return true;
}
bool Do_Breakdown;
double BD_Timer=2;
int BD_Count=0;
bool Breakdown(object obj=null){
	if(Do_Breakdown){
		for(int i=0;i<6;i++){
			foreach(IMyThrust Thrust in All_Thrusters[i])
				Thrust.Enabled=true;
		}
		List<IMyGyro> MyGyros=GenericMethods<IMyGyro>.GetAllIncluding("");
		foreach(IMyGyro Gyro in MyGyros){
			Gyro.Yaw=0;
			Gyro.Pitch=0;
			Gyro.Roll=0;
			Gyro.GyroOverride=false;
		}
		Do_Breakdown=false;
	}
	else{
		Do_Breakdown=true;
		BD_Timer=2;
		BD_Count=0;
	}
	return true;
}
bool Toggle_Terrain(object obj=null){
	Terrain=!Terrain;
	MarkAltitude(false);
	return true;
}

bool CreateMenu(object obj=null){
	Command_Menu=new Menu_Submenu("Command Menu");
	//Command_Menu.Add(new Menu_Command<object>("Update Menu", CreateMenu, "Refreshes menu"));
	if(!Me.CubeGrid.IsStatic)
		Command_Menu.Add(new Menu_Command<object>("Toggle Autoland",Autoland,"Toggles On/Off the Autoland feature\nLands at 5 m/s\nDo not use on ships with poor mobility!"));
	Command_Menu.Add(new Menu_Command<object>("Toggle Orbiting", Orbit, "Locks current Speed, allowing the ship to cruise at the current approximate altitude. Will not cruise below 100M."));
	Command_Menu.Add(new Menu_Command<object>("Safety Protocols", ToggleSafety, "Toggles Safety Protocols:\nAnti-Crash, Speed Limit, Auto-Align, Cruise Control"));
	Command_Menu.Add(new Menu_Command<object>("Guest Mode",GuestMode,"Puts the base in Guest Mode for "+Math.Round(Guest_Mode_Timer,0)+" seconds or turns it off"));
	Command_Menu.Add(new Menu_Command<object>("Toggle Terrain", Toggle_Terrain, "Toggles whether Altitude LCDs consider Terrain Elevation."));
	Command_Menu.Add(new Menu_Command<object>("Breakdown", Breakdown, "Disables thrusters randomly one at a time, and randomly sets Gyro Overrides. Gets worse as it progresses. Can randomly reset."));
	Command_Menu.Add(new Menu_Command<object>("Shut Down",Disable,"Resets Thrusters, Gyroscope, and Airlocks, and turns off the program"));
	Command_Menu.Add(new Menu_Command<object>("Factory Reset", FactoryReset, "Resets AI memory and settings, and turns it off"));
	return true;
}
void DisplayMenu(){
	string text=Command_Menu.ToString();
	if(Do_Breakdown)
		text=Stringsplice(text);
	foreach(CustomPanel Panel in CommandLCDs){
		Panel.Display.WriteText(text,false);
		Panel.Display.Alignment=TextAlignment.CENTER;
		Panel.Display.FontSize=1.2f;
		Panel.Display.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.Display.TextPadding=10.0f;
		if(Panel.Trans){
			Panel.Display.FontColor=DEFAULT_BACKGROUND_COLOR;
			Panel.Display.BackgroundColor=new Color(0,0,0,0);
		}
		else{
			Panel.Display.FontColor=DEFAULT_TEXT_COLOR;
			Panel.Display.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
	}
}

double Scan_Time=10;
string ScanString="";
double MySize=0;
double TurretTimer=300;
bool PerformScan(object obj=null){
	Write("Beginning Scan");
	GetSettings();
	ScanString="";
	
	
	List<MyDetectedEntityInfo> DetectedEntities=new List<MyDetectedEntityInfo>();
	List<IMySensorBlock> AllSensors=new List<IMySensorBlock>();
	
	List<IMyLargeTurretBase> AllTurrets=new List<IMyLargeTurretBase>();
	GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(AllTurrets);
	foreach(IMyLargeTurretBase Turret in AllTurrets){
		if(Turret.HasTarget){
			TurretTimer=0;
			break;
		}
	}
	
	foreach(IMyAirtightSlideDoor Door in MyDoors){
		if(Door.Status!=DoorStatus.Closed){
			double timer=0;
			if(HasBlockData(Door,"Timer"))
				double.TryParse(GetBlockData(Door,"Timer"),out timer);
			if(timer<=0){
				timer=10;
				SetBlockData(Door,"Timer",timer.ToString());
				OpenDoors.Add(Door);
			}
		}
	}
	
	if(Command_Menu.AutoRefresh())
		DisplayMenu();
	
	Scan_Time=0;
	return true;
}

void UpdateAirlock(Airlock airlock){
	if(airlock.Door1.Status!=DoorStatus.Closed&&airlock.Door2.Status!=DoorStatus.Closed){
		airlock.Door1.Enabled=true;
		airlock.Door1.CloseDoor();
		airlock.Door2.Enabled=true;
		airlock.Door2.CloseDoor();
	}
	if(!(CanHaveJob(airlock.Door1,"Airlock")&&(CanHaveJob(airlock.Door2,"Airlock"))))
		return;
	bool both_closed=(airlock.Door1.Status==DoorStatus.Closed)&&(airlock.Door2.Status==DoorStatus.Closed);
	
	double wait=1;
	if(airlock.Vent!=null)
		wait=3;
	if(airlock.AirlockTimer<wait||!both_closed){
		SetBlockData(airlock.Door1,"Job","Airlock");
		SetBlockData(airlock.Door2,"Job","Airlock");
		if(!both_closed)
			airlock.AirlockTimer=0;
		airlock.Door1.Enabled=(airlock.Door1.Status!=DoorStatus.Closed);
		airlock.Door2.Enabled=(airlock.Door2.Status!=DoorStatus.Closed);
	}
	else{
		SetBlockData(airlock.Door1,"Job","None");
		SetBlockData(airlock.Door2,"Job","None");
		airlock.Door1.Enabled=true;
		airlock.Door2.Enabled=true;
	}
}

//Sets gyroscope outputs from player input, dampeners, gravity, and autopilot
double Pitch_Time= 1.0f;
double Yaw_Time=1.0f;
double Roll_Time=1.0f;
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
	float gyro_multx=(float)Math.Max(0.1f, Math.Min(1, 1.5f/(Controller.CalculateShipMass().PhysicalMass/gyro_count/1000000)));
	
	float input_pitch=0;
	float input_yaw=0;
	float input_roll=0;
	
	if(Pitch_Time<1)
		Pitch_Time+=seconds_since_last_update;
	if(Yaw_Time<1)
		Yaw_Time+=seconds_since_last_update;
	if(Roll_Time<1)
		Roll_Time+=seconds_since_last_update;
	
	bool Undercontrol=false;
	foreach(IMyShipController Ctrl in Controllers)
		Undercontrol=Undercontrol||Ctrl.IsUnderControl;
	
	foreach(IMyShipController Ctrl in Controllers)
		input_pitch+=Math.Min(Math.Max(Ctrl.RotationIndicator.X/100,-1),1);
	if(Math.Abs(input_pitch)<0.05f){
		input_pitch=current_pitch*0.99f;
		float orbit_multx=1;
		if(Orbiting)
			orbit_multx=50;
		if(Safety){
			if((((Elevation-MySize)<Controller.GetShipSpeed()*2&&(Elevation-MySize)<50)||(Controller.DampenersOverride&&!Controller.IsUnderControl)||Orbiting)&&GetAngle(Gravity,Forward_Vector)<120&&Pitch_Time>=1){
				double difference=Math.Abs(GetAngle(Gravity,Forward_Vector));
				if(Orbiting){
					if(Orbital_Altitude-Elevation>250&&Elevation>500)
						difference=Math.Max(difference-15,0);
					else if(Orbital_Altitude-Elevation>100&&Elevation>250)
						difference=Math.Max(difference-10,0);
					else if(Orbital_Altitude-Elevation>25)
						difference=Math.Max(difference-5,0);
				}
				if(difference<90)
					input_pitch-=10*gyro_multx*((float)Math.Min(Math.Abs((90-difference)/90),1));
			}
			if((Orbiting||(Controller.DampenersOverride&&!Undercontrol))&&(GetAngle(Gravity,Forward_Vector)>(90+Acceptable_Angle/2)||(Orbiting&&GetAngle(Gravity,Forward_Vector)>60))){
				double difference=Math.Abs(GetAngle(Gravity,Forward_Vector));
				if(Orbiting){
					if(Elevation-Orbital_Altitude>250)
						difference=Math.Min(difference+15,180);
					else if(Elevation-Orbital_Altitude>100)
						difference=Math.Min(difference+10,180);
					else if(Elevation-Orbital_Altitude>25)
						difference=Math.Min(difference+5,180);
				}
				if(difference>90+Acceptable_Angle/2||(Orbiting&&difference>90))
					input_pitch+=10*gyro_multx*((float)Math.Min(Math.Abs((difference-90)/90),1))*orbit_multx;
			}
		}
	}
	else{
		Pitch_Time=0;
		input_pitch*=30;
	}
	foreach(IMyShipController Ctrl in Controllers)
		input_yaw+=Math.Min(Math.Max(Ctrl.RotationIndicator.Y/100,-1),1);
	if(Math.Abs(input_yaw)<0.05f){
		input_yaw=current_yaw*0.99f;
	}
	else{
		Yaw_Time=0;
		input_yaw*=30;
	}
	foreach(IMyShipController Ctrl in Controllers)
		input_roll+=Ctrl.RollIndicator;
	if(Math.Abs(input_roll)<0.05f){
		input_roll=current_roll*0.99f;
		if(Safety&&Gravity.Length()>0&&Roll_Time>=1){
			double difference=GetAngle(Left_Vector,Gravity)-GetAngle(Right_Vector,Gravity);
			if(Math.Abs(difference)>Acceptable_Angle){
				input_roll-=(float)Math.Min(Math.Max(difference*5,-5),25)*gyro_multx*5;
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

void SetThrusters(){
	float input_forward=0.0f;
	float input_up=0.0f;
	float input_right=0.0f;
	float damp_multx=0.99f;
	double effective_speed_limit=Speed_Limit;
	
	bool Undercontrol=false;
	foreach(IMyShipController Ctrl in Controllers)
		Undercontrol=Undercontrol||Ctrl.IsUnderControl;
	
	if(Safety){
		if(Elevation<5000)
			effective_speed_limit=Math.Min(effective_speed_limit,(Elevation-30)*10);
		if(Time_To_Crash<60&&Time_To_Crash>=0)
			effective_speed_limit=Math.Min(effective_speed_limit,Time_To_Crash/30*100);
	}
	if(Controller.DampenersOverride){
		Write("Cruise Control: Off");
		Write("Dampeners: On");
		input_right-=(float)((Relative_CurrentVelocity.X-Relative_RestingVelocity.X)*Mass_Accomodation*damp_multx);
		input_up-=(float)((Relative_CurrentVelocity.Y-Relative_RestingVelocity.Y)*Mass_Accomodation*damp_multx);
		input_forward+=(float)((Relative_CurrentVelocity.Z-Relative_RestingVelocity.Z)*Mass_Accomodation*damp_multx);
	}
	else{
		if(Safety&&(Elevation>50||CurrentVelocity.Length()>10)){
			Write("Cruise Control: On");
			Vector3D velocity_direction=CurrentVelocity;
			velocity_direction.Normalize();
			double angle=Math.Min(GetAngle(Forward_Vector, velocity_direction),GetAngle(Backward_Vector, velocity_direction));
			if(angle<=Acceptable_Angle/2){
				input_right-=(float)((Relative_CurrentVelocity.X-Relative_RestingVelocity.X)*Mass_Accomodation*damp_multx);
				input_up-=(float)((Relative_CurrentVelocity.Y-Relative_RestingVelocity.Y)*Mass_Accomodation*damp_multx);
				Write("Stabilizers: On ("+Math.Round(angle, 1)+"° dev)");
			}
			else
				Write("Stabilizers: Off ("+Math.Round(angle, 1)+"° dev)");
		}
		else{
			Write("Cruise Control: Off");
			Write("Dampeners: Off");
		}
	}
	effective_speed_limit=Math.Max(effective_speed_limit,10);
	if(!Safety)
		effective_speed_limit=300000000;
	if(Gravity.Length()>0&&Mass_Accomodation>0&&GetAngle(CurrentVelocity,Gravity)>Acceptable_Angle){
		if(!(_Autoland&&Time_To_Crash>15&&Controller.GetShipSpeed()>5)){
			input_right-=(float)Adjusted_Gravity.X;
			input_up-=(float)Adjusted_Gravity.Y;
			input_forward+=(float)Adjusted_Gravity.Z;
		}
	}
	
	foreach(IMyShipController Ctrl in Controllers){
		if(Ctrl.IsUnderControl&&Math.Abs(Ctrl.MoveIndicator.X)>0.5f){
			if(Ctrl.MoveIndicator.X>0){
				if((!Safety)||(CurrentVelocity+Right_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_right=0.95f*Right_Thrust;
				else
					input_right=Math.Min(input_right,0);
			} else {
				if((!Safety)||(CurrentVelocity+Left_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_right=-0.95f*Left_Thrust;
				else
					input_right=Math.Max(input_right,0);
			}
		}
	}
	
	foreach(IMyShipController Ctrl in Controllers){
		if(Ctrl.IsUnderControl&&Math.Abs(Ctrl.MoveIndicator.Y)>0.5f){
			if(Ctrl.MoveIndicator.Y>0){
				if((!Safety)||(CurrentVelocity+Up_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_up=0.95f*Up_Thrust;
				else
					input_up=Math.Min(input_up,0);
			} else {
				if((!Safety)||(CurrentVelocity+Down_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_up=-0.95f*Down_Thrust;
				else
					input_up=Math.Max(input_up,0);
			}
		}
	}
	
	foreach(IMyShipController Ctrl in Controllers){
		if(Ctrl.IsUnderControl&&Math.Abs(Ctrl.MoveIndicator.Z)>0.5f){
			if(Ctrl.MoveIndicator.Z<0){
				if((!Safety)||(CurrentVelocity+Up_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_forward=0.95f*Forward_Thrust;
				else
					input_forward=Math.Min(input_forward,0);
			} 
			else{
				if((!Safety)||(CurrentVelocity+Down_Vector-RestingVelocity).Length()<=effective_speed_limit)
					input_forward=-0.95f*Backward_Thrust;
				else
					input_forward=Math.Max(input_forward,0);
			}
		}
	}
	
	float output_forward=0.0f;
	float output_backward=0.0f;
	if(input_forward/Forward_Thrust>0.01f)
		output_forward=Math.Min(Math.Abs(input_forward/Forward_Thrust),1);
	else if(input_forward/Backward_Thrust<-0.01f)
		output_backward=Math.Min(Math.Abs(input_forward/Backward_Thrust),1);
	if(Orbiting&&Math.Abs(Orbital_Altitude-Elevation)>1){
		float difference=(float)Math.Abs(Elevation-Orbital_Altitude);
		float up_multx=1;
		if(Elevation<Orbital_Altitude)
			up_multx=(10+difference)/10.0f;
		else
			up_multx=10.0f/(10+difference);
		up_multx=Math.Min(Math.Max(up_multx,0.1f),10);
		if(input_up<0)
			up_multx=1.0f/up_multx;
		if(up_multx<=0.5f){
			input_up*=-1;
			up_multx=0.5f/up_multx;
		}
		input_up*=up_multx;
	}
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
	
	foreach(IMyThrust Thruster in Forward_Thrusters){
		Thruster.ThrustOverridePercentage=output_forward;
		if(!Safety)
			Thruster.Enabled=output_forward>0;
	}
	foreach(IMyThrust Thruster in Backward_Thrusters){
		Thruster.ThrustOverridePercentage=output_backward;
		if(!Safety)
			Thruster.Enabled=output_backward>0;
	}
	foreach(IMyThrust Thruster in Up_Thrusters){
		Thruster.ThrustOverridePercentage=output_up;
		if(!Safety)
			Thruster.Enabled=output_up>0;
	}
	foreach(IMyThrust Thruster in Down_Thrusters){
		Thruster.ThrustOverridePercentage=output_down;
		if(!Safety)
			Thruster.Enabled=output_down>0;
	}
	foreach(IMyThrust Thruster in Right_Thrusters){
		Thruster.ThrustOverridePercentage=output_right;
		if(!Safety)
			Thruster.Enabled=output_right>0;
	}
	foreach(IMyThrust Thruster in Left_Thrusters){
		Thruster.ThrustOverridePercentage=output_left;
		if(!Safety)
			Thruster.Enabled=output_left>0;
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
}

int Glitch_Seed=0;
Random Glitch=new Random(0);
List<IMyFunctionalBlock> FunctionalBlocks;
int Broken_Blocks{
	get{
		int broken=0;
		foreach(IMyFunctionalBlock Block in FunctionalBlocks){
			if(Block==null||(!Block.IsFunctional)||(!Block.IsSameConstructAs(Me)))
				broken++;
		}
		return broken;
	}
}
float BD_Percent{
	get{
		return ((float)BD_Count)/(30+BD_Count)*100;
	}
}
void BD_Cycle(bool try_reset=true){
	BD_Timer=2;
	Glitch_Seed=Rnd.Next();
	int broken=Broken_Blocks;
	if(try_reset){
		int j=Rnd.Next(0,Math.Max(10,BD_Count));
		if(j==0){
			j=Rnd.Next(0,Math.Max(1,Math.Min(BD_Count,5)));
			Breakdown();
			Breakdown();
			for(int k=0;k<j;k++)
				BD_Cycle(false);
		}
	}
	BD_Count++;
	int i=Rnd.Next(0,6);
	if(All_Thrusters[i].Count>0){
		int j=Rnd.Next(0,All_Thrusters[i].Count);
		All_Thrusters[i][j].Enabled=true;
	}
	List<IMyGyro> MyGyros=GenericMethods<IMyGyro>.GetAllConstruct("");
	i=Rnd.Next(0,MyGyros.Count);
	MyGyros[i].Yaw=0;
	MyGyros[i].Pitch=0;
	MyGyros[i].Roll=0;
	MyGyros[i].GyroOverride=false;
	int min_randomize=1;
	if(Time_To_Crash>=0&&Time_To_Crash<=30){
		min_randomize=5;
		int w=Rnd.Next(5,15);
		for(int k=0;k<w;k++){
			i=Rnd.Next(0,6);
			if(All_Thrusters[i].Count>0){
				int j=Rnd.Next(0,All_Thrusters[i].Count);
				All_Thrusters[i][j].Enabled=true;
			}
			i=Rnd.Next(0,MyGyros.Count);
			MyGyros[i].Yaw=0;
			MyGyros[i].Pitch=0;
			MyGyros[i].Roll=0;
			MyGyros[i].GyroOverride=false;
		}
	}
	min_randomize=Math.Max(1,broken/5);
	for(int k=0;k<Rnd.Next(min_randomize,Math.Max(min_randomize+1,BD_Count));k++){
		i=Rnd.Next(0,6);
		if(All_Thrusters[i].Count>0){
			int j=Rnd.Next(0,All_Thrusters[i].Count);
			All_Thrusters[i][j].Enabled=false;
		}
		i=Rnd.Next(0,MyGyros.Count);
		int pn=Rnd.Next(1,Math.Min(11,1+BD_Count/5));
		MyGyros[i].Yaw=(Rnd.Next(-1*pn,pn+1)+Rnd.Next(-1*pn,pn+1))/10.0f;
		MyGyros[i].Pitch=(Rnd.Next(-1*pn,pn+1)+Rnd.Next(-1*pn,pn+1))/10.0f;
		MyGyros[i].Roll=(Rnd.Next(-1*pn,pn+1)+Rnd.Next(-1*pn,pn+1))/10.0f;
		MyGyros[i].GyroOverride=true;
	}
}

bool Terrain=true;

void MarkAltitude(bool do_new=true){
	const int XLEN=48;
	const int XFULL=51;
	if(Gravity.Length()==0)
		return;
	if(do_new)
		Altitude_Timer=((double)Graph_Length_Seconds)/XFULL;
	while(Altitude_Graph.Count>0&&Time_Since_Start.TotalSeconds-Altitude_Graph.Peek().Timestamp.TotalSeconds>Graph_Length_Seconds)
		Altitude_Graph.Dequeue();
	if(do_new&&Altitude_Graph.Count<XLEN&&Gravity.Length()>0)
		Altitude_Graph.Enqueue(new Altitude_Data(Sealevel,Elevation,Time_Since_Start));
	if(Altitude_Graph.Count==0)
		return;
	double max=500;
	double min=double.MaxValue;
	foreach(Altitude_Data Data in Altitude_Graph){
		max=Math.Max(max,Data.Sealevel);
		min=Math.Min(min,Data.Sealevel);
		if(Terrain){
			max=Math.Max(max,Data.Sealevel-Data.Elevation);
			min=Math.Min(min,Data.Sealevel-Data.Elevation);
		}
	}
	if(Terrain)
		max+=100;
	else
		max+=500;
	if(Terrain)
		min=Math.Max(min-100,-1100);
	else
		min=Math.Max(min-500,0);
	max=Math.Ceiling(max/100)*100;
	min=Math.Floor(min/100)*100;
	double interval=(max-min)/34.0;
	//50 wide, 30 tall
	char[][] Graph=new char[35][];
	for(int y=0;y<35;y++){
		Graph[y]=new char[XFULL];
		double altitude=(min+y*interval);
		int alt_num=(int)Math.Floor(altitude/1000);
		int low_alt=(int)Math.Floor((altitude-interval)/1000);
		char alt_10s=((Math.Abs(alt_num)/10)%10).ToString()[0];
		if(alt_num<0)
			alt_10s='-';
		char alt_1s=(Math.Abs(alt_num)%10).ToString()[0];
		for(int x=0;x<XFULL;x++){
			if(min==0&&y==0&&!Terrain)
				Graph[y][x]='-';
			else
				Graph[y][x]=' ';
			if(x<2){
				if(alt_num!=low_alt||(min==0&&y==0)){
					if(x==0)
						Graph[y][x]=alt_10s;
					else
						Graph[y][x]=alt_1s;
				}
				else if(((int)Math.Floor(altitude/500))!=((int)Math.Floor((altitude-interval)/500)))
					Graph[y][x]='-';
				else if(x==1&&((int)Math.Floor(altitude/250))!=((int)Math.Floor((altitude-interval)/250)))
					Graph[y][x]='-';
			}
			else if(x==2){
				Graph[y][x]='|';
			}
		}
	}
	
	if(Orbiting){
		int Y=(int)Math.Round((Orbital_Altitude-min)/interval,0);
		if(Y>=0&&Y<35){
			for(int X=3;X<XFULL;X++){
				if(X%2==0)
					Graph[Y][X]='=';
			}
		}
	}
	
	double time_interval=Graph_Length_Seconds/((double)XLEN);
	double End=Time_Since_Start.TotalSeconds;
	double Start=End-Graph_Length_Seconds;
	
	foreach(Altitude_Data Point in Altitude_Graph){
		int X=(int)Math.Ceiling((Point.Timestamp.TotalSeconds-Start)/time_interval);
		int Y_E=(int)Math.Round((Point.Sealevel-Point.Elevation)/interval,0);
		int Y_S=(int)Math.Round((Point.Sealevel-min)/interval,0);
		if(X>=0&&X<XLEN){
			if(Y_S>=0&&Y_S<35)
				Graph[Y_S][X+3]='○';
			if(Terrain){
				if(Y_E>=0&&Y_E<35)
					Graph[Y_E][X+3]='_';
			}
		}
	}
	if(do_new){
		string time=Math.Round(Altitude_Timer,3).ToString();
		for(int i=1;i<=time.Length;i++)
			Graph[34][XFULL-i]=time[time.Length-i];
	}
	string text="";
	for(int y=34;y>=0;y--){
		if(y<34)
			text+='\n';
		for(int x=0;x<XFULL;x++){
			text+=Graph[y][x];
		}
	}
	if(Do_Breakdown)
		text=Stringsplice(text);
	foreach(CustomPanel Panel in AltitudeLCDs){
		Panel.Display.WriteText(text,false);
	}
	
}

void UpdateTimers(){
	foreach(Airlock airlock in Airlocks){
		if(airlock.AirlockTimer<10)
			airlock.AirlockTimer+=seconds_since_last_update;
		else
			airlock.AirlockTimer=0;
	}
	for(int i=0;i<OpenDoors.Count;i++){
		IMyAirtightSlideDoor Door=OpenDoors[i];
		double timer=10;
		if((!HasBlockData(Door,"Timer"))||!double.TryParse(GetBlockData(Door,"Timer"),out timer))
			SetBlockData(Door,"Timer",timer.ToString());
		timer=Math.Round(Math.Max(0,timer-seconds_since_last_update),3);
		SetBlockData(Door,"Timer",timer.ToString());
		if(timer<=0){
			Door.CloseDoor();
			OpenDoors.RemoveAt(i--);
			continue;
		}
	}
	if(Do_Breakdown){
		BD_Timer-=seconds_since_last_update;
		if(BD_Timer<=0)
			BD_Cycle();
	}
	if(Guest_Mode){
		Guest_Timer+=seconds_since_last_update;
		if(Guest_Timer>=Guest_Mode_Timer)
			Guest_Mode=false;
	}
	if(TurretTimer<300)
		TurretTimer+=seconds_since_last_update;
	if(Altitude_Timer>0)
		Altitude_Timer-=seconds_since_last_update;
	Scan_Time+=seconds_since_last_update;
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
				else if(Elevation<500){
					double terrain_height=(Controller.GetPosition()-PlanetCenter).Length()-Elevation;
					List<IMyLandingGear> AllBlocks=new List<IMyLandingGear>();
					GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(AllBlocks);
					foreach(IMyLandingGear Block in AllBlocks)
						Elevation=Math.Min(Elevation,(Block.GetPosition()-PlanetCenter).Length()-terrain_height);
				}
			}
			else
				Elevation=Sealevel;
			if(!Me.CubeGrid.IsStatic){
				double from_center=(Controller.GetPosition()-PlanetCenter).Length();
				Vector3D next_position=Controller.GetPosition()+1*CurrentVelocity;
				double Elevation_per_second=(from_center-(next_position-PlanetCenter).Length());
				Time_To_Crash=Elevation/Elevation_per_second;
				bool need_print=true;
				if(Time_To_Crash>0){
					if(((!Orbiting)||Orbital_Altitude>500)&&Safety&&Time_To_Crash<15&&Controller.GetShipSpeed()>5){
						Controller.DampenersOverride=true;
						RestingSpeed=0;
						Orbiting=false;
						Write("Crash predicted within 15 seconds; enabling Dampeners");
						need_print=false;
					}
					else if(Time_To_Crash*Math.Max(Elevation,1000)<1800000&&Controller.GetShipSpeed()>1.0f){
						Write(Math.Round(Time_To_Crash, 1).ToString()+" seconds to crash");
						if(_Autoland && Time_To_Crash>30)
							Controller.DampenersOverride=false;
						need_print=false;
					}
					if(Elevation-MySize<5&&_Autoland)
						_Autoland=false;
				}
				if(need_print)
					Write("No crash likely at current velocity");
			}
		}
		else
			PlanetCenter=new Vector3D(0,0,0);
	}
	else
		Sealevel=double.MaxValue;
	Elevation=Math.Max(Elevation,0);
	if(Orbiting&&(Gravity.Length()==0)){
		Orbiting=false;
		RestingSpeed=0;
	}
	Mass_Accomodation=(float)(Controller.CalculateShipMass().PhysicalMass*Gravity.Length());
}

double FromRad(double rad){
	return rad*180/Math.PI;
}

public void Main(string argument, UpdateType updateSource)
{
	try{
		UpdateProgramInfo();
		UpdateSystemData();
		UpdateTimers();
		if(!Me.CubeGrid.IsStatic){
			if(Elevation!=double.MaxValue){
				Write("Elevation: "+Math.Round(Elevation,1).ToString());
				Write("Sealevel: "+Math.Round(Sealevel,1).ToString());
			}
			if(Gravity.Length()>0)
				Write("Gravity:"+Math.Round(Gravity.Length()/9.814,2)+"Gs");
			Write("Maximum Power (Hovering): "+Math.Round(Up_Gs,2)+"Gs");
			Write("Maximum Power (Launching): "+Math.Round(Math.Max(Up_Gs,Forward_Gs),2)+"Gs");
		}
		if(Scan_Time>=5)
			PerformScan();
		else
			Write("Last Scan "+Math.Round(Scan_Time,1).ToString());
		Write(ScanString);
		if(cycle%25==0){
			foreach(Airlock airlock in Airlocks)
				UpdateAirlock(airlock);
		}
		if(AltitudeLCDs.Count>0&&Altitude_Timer<=0)
			MarkAltitude();
		
		if(argument.ToLower().Equals("back")){
			Command_Menu.Back();
			DisplayMenu();
		}
		else if(argument.ToLower().Equals("prev")){
			Command_Menu.Prev();
			DisplayMenu();
		}
		else if(argument.ToLower().Equals("next")){
			Command_Menu.Next();
			DisplayMenu();
		}
		else if(argument.ToLower().Equals("select")){
			Command_Menu.Select();
			DisplayMenu();
		}
		else if(argument.ToLower().Equals("autoland")){
			Autoland();
		}
		else if(argument.ToLower().Equals("factory reset")){
			FactoryReset();
			DisplayMenu();
		}
		else if(argument.ToLower().Equals("guest mode")){
			Guest_Mode=!Guest_Mode;
			Guest_Timer=0;
		}
		else if(argument.ToLower().Equals("breakdown")){
			Breakdown();
		}
		else if(argument.ToLower().Equals("toggle safety")){
			ToggleSafety();
		}
		else if(argument.ToLower().Equals("toggle terrain")){
			Toggle_Terrain();
		}
		if(_Autoland)
			Write("Autoland Enabled");
		
		if(!Me.CubeGrid.IsStatic&&Controller.CalculateShipMass().PhysicalMass>0){
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
		
		
		switch(ShipStatus){
			case AlertStatus.Green:
				SetStatus("Condition "+ShipStatus.ToString()+Submessage, new Color(137, 255, 137, 255), new Color(0, 151, 0, 255));
				SetAlert(0,new Color(0,151,0,255));
				break;
			case AlertStatus.Blue:
				SetStatus("Condition "+ShipStatus.ToString()+Submessage, new Color(137, 239, 255, 255), new Color(0, 88, 151, 255));
				SetAlert(1,new Color(0,88,151,255));
				break;
			case AlertStatus.Yellow:
				SetStatus("Condition "+ShipStatus.ToString()+Submessage, new Color(255, 239, 137, 255), new Color(66, 66, 0, 255));
				SetAlert(2,new Color(66,66,0,255));
				break;
			case AlertStatus.Orange:
				SetStatus("Condition "+ShipStatus.ToString()+Submessage, new Color(255, 197, 0, 255), new Color(88, 44, 0, 255));
				SetAlert(3,new Color(88,44,0,255));
				break;
			case AlertStatus.Red:
				SetStatus("Condition "+ShipStatus.ToString()+Submessage, new Color(255, 137, 137, 255), new Color(151, 0, 0, 255));
				SetAlert(4,new Color(151,0,0,255));
				break;
		}
		
		Runtime.UpdateFrequency=GetUpdateFrequency();
	}
	catch(Exception E){
		Write(E.ToString());
		try{
			SetStatus("Status LCD\nOffline", new Color(255,255,255,255), new Color(0,0,0,255));
		}
		catch(Exception){
			;
		}
		FactoryReset();
		DisplayMenu();
	}
}
