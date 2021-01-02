const string Program_Name = "Battleship HUB AI"; //Name me!
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

class DisplayArray{
	public List<List<IMyTextPanel>> Panels;
	public string Name;
	
	DisplayArray(string name,List<List<IMyTextPanel>> panels){
		Name=name;
		Panels=panels;
	}
	
	static IMyTextPanel GetFromList(List<IMyTextPanel> list,string name){
		for(int i=0;i<list.Count;i++){
			if(list[i].CustomName.Equals(name)){
				IMyTextPanel panel=list[i];
				list.RemoveAt(i);
				return panel;
			}
		}
		return null;
	}
	
	public static bool GetArray(string name,out DisplayArray output){
		output=null;
		List<List<IMyTextPanel>> panels=new List<List<IMyTextPanel>>();
		List<IMyTextPanel> all_panels=GenericMethods<IMyTextPanel>.GetAllContaining(name);
		for(int i=0;i<8;i++){
			char s='A';
			s=(char)(((int)s)+i);
			List<IMyTextPanel> input=new List<IMyTextPanel>();
			for(int j=1;j<=8;j++){
				IMyTextPanel panel=GetFromList(all_panels,name.Trim()+" "+s+j.ToString());
				if(panel==null)
					return false;
				input.Add(panel);
			}
			panels.Add(input);
		}
		output=new DisplayArray(name.Trim(),panels);
		return true;
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

string GetCommand(string parser){
	int index=parser.IndexOf(':');
	if(index<0)
		return "";
	return parser.Substring(0,index);
}
string GetData(string parser){
	int index=parser.IndexOf(':');
	if(index<0)
		return "";
	return parser.Substring(index+1);
}

enum GameStatus{
	Waiting=0,
	Ready=1,
	Awaiting=2,
	InProgress=3,
	Paused=4
}

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;

Random Rnd;

int Player_Count=1;
double Turn_Timer=90;
bool Allow_Pause=true;
bool Use_Real_Ships=false;
bool Destroy_Ships=false;
bool See_Opponent_Choice=false;
GameStatus Status=GameStatus.Ready;

bool Player_1_Ready=false;
bool Player_2_Ready=false;

DisplayArray Player1Enemy;
DisplayArray Player1Own;
DisplayArray Player2Enemy;
DisplayArray Player2Own;
List<IMyTextPanel> Player1StatusPanels;
List<IMyTextPanel> Player2StatusPanels;
List<IMyTextPanel> HubStatusPanels;

List<IMyDoor> Room1Doors;
List<IMyDoor> Room2Doors;


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
	
	if(!DisplayArray.GetArray("Room 1 Enemy LCD",out Player1Enemy)){
		Write("Failed to get R1E");
		return;
	}
	if(!DisplayArray.GetArray("Room 1 Own LCD",out Player1Own)){
		Write("Failed to get R1O");
		return;
	}
	if(!DisplayArray.GetArray("Room 2 Enemy LCD",out Player2Enemy)){
		Write("Failed to get R2E");
		return;
	}
	if(!DisplayArray.GetArray("Room 2 Own LCD",out Player2Own)){
		Write("Failed to get R2O");
		return;
	}
	Player1StatusPanels=GenericMethods<IMyTextPanel>.GetAllContaining("Room 1 Game Status Panel");
	foreach(IMyTextPanel Panel in Player1StatusPanels){
		Panel.FontColor=new Color(255,137,137,255);
		Panel.BackgroundColor=new Color(151,0,0,255);
		Panel.Alignment=TextAlignment.CENTER;
		Panel.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.FontSize=1.2f;
		Panel.TextPadding=10;
	}
	Player2StatusPanels=GenericMethods<IMyTextPanel>.GetAllContaining("Room 2 Game Status Panel");
	foreach(IMyTextPanel Panel in Player2StatusPanels){
		Panel.FontColor=new Color(137,137,255,255);
		Panel.BackgroundColor=new Color(0,0,151,255);
		Panel.Alignment=TextAlignment.CENTER;
		Panel.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.FontSize=1.2f;
		Panel.TextPadding=10;
	}
	HubStatusPanels=GenericMethods<IMyTextPanel>.GetAllContaining("Hub Game Status Panel");
	foreach(IMyTextPanel Panel in HubStatusPanels){
		Panel.FontColor=new Color(255,255,255,255);
		Panel.BackgroundColor=new Color(0,0,0,0);
		Panel.Alignment=TextAlignment.CENTER;
		Panel.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.FontSize=1.2f;
		Panel.TextPadding=10;
	}
	Rnd=new Random();
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		string command=GetCommand(arg);
		string data=GetData(arg);
		switch(command){
			case "Player_Count":
				Int32.TryParse(data,out Player_Count);
				break;
			case "Turn_Timer":
				double.TryParse(data,out Turn_Timer);
				break;
			case "Allow_Pause":
				bool.TryParse(data,out Allow_Pause);
				break;
			case "Use_Real_Ships":
				bool.TryParse(data,out Use_Real_Ships);
				break;
			case "Destroy_Ships":
				bool.TryParse(data,out Destroy_Ships);
				break;
			case "See_Opponent_Choice":
				bool.TryParse(data,out See_Opponent_Choice);
				break;
			case "Status":
				int o;
				if(Int32.TryParse(data,out o))
					Status=(GameStatus)o;
				break;
		}
	}
	Room1Doors=GenericMethods<IMyDoor>.GetAllContaining("Room 1 Door");
	Room2Doors=GenericMethods<IMyDoor>.GetAllContaining("Room 2 Door");
	
	// The constructor, called only once every session and
    // always before any other method is called. Use it to
    // initialize your script. 
    //     
    // The constructor is optional and can be removed if not
    // needed.
    // 
    // It's recommended to set RuntimeInfo.UpdateFrequency 
    // here, which will allow your script to run itself without a 
    // timer block.
	Write("Completed Initialization");
	Runtime.UpdateFrequency=UpdateFrequency.Update10;//60tps
}

public void Save(){
    this.Storage="Status:"+((int)Status).ToString();
	this.Storage+="•Player_Count:"+Player_Count.ToString();
	this.Storage+="•Turn_Timer:"+Turn_Timer.ToString();
	this.Storage+="•Allow_Pause"+Allow_Pause.ToString();
	this.Storage+="•Use_Real_Ships:"+Use_Real_Ships.ToString();
	this.Storage+="•Destroy_Ships:"+Destroy_Ships.ToString();
	this.Storage+="•See_Opponent_Choice:"+See_Opponent_Choice.ToString();
	
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

Vector2 Target=new Vector2(0,0);
List<Vector2> Parts=new List<Vector2>();
Color Worm=new Color(0,0,0,255);
void DisplayCheck(DisplayArray Da){
	if(Parts.Count==0){
		Parts.Add(new Vector2(0,0));
		Parts.Add(new Vector2(0,0));
		Parts.Add(new Vector2(0,0));
		Target=new Vector2(Rnd.Next(0,8),Rnd.Next(0,8));
		Worm=new Color(Rnd.Next(100,255),Rnd.Next(100,255),Rnd.Next(100,255),255);
	}
	if(DisplayIdleTimer>0.1+0.01*Parts.Count){
		DisplayIdleTimer=0;
		bool Can_Left=true;
		bool Can_Right=true;
		bool Can_Up=true;
		bool Can_Down=true;
		Vector2 Left=Parts[0]+(new Vector2(-1,0));
		if(Left.X<0)
			Can_Left=false;
		Vector2 Right=Parts[0]+(new Vector2(1,0));
		if(Right.X>7)
			Can_Right=false;
		Vector2 Up=Parts[0]+(new Vector2(0,-1));
		if(Up.Y<0)
			Can_Up=false;
		Vector2 Down=Parts[0]+(new Vector2(0,1));
		if(Down.Y>7)
			Can_Down=false;
		if(Parts[0]==Target){
			Parts.Add(Parts[Parts.Count-1]);
			Target=new Vector2(Rnd.Next(0,8),Rnd.Next(0,8));
			Worm=new Color(Rnd.Next(100,255),Rnd.Next(100,255),Rnd.Next(100,255),255);
		}
		foreach(Vector2 part in Parts){
			if(part==Left)
				Can_Left=false;
			if(part==Right)
				Can_Right=false;
			if(part==Up)
				Can_Up=false;
			if(part==Down)
				Can_Down=false;
		}
		List<Vector2> Options=new List<Vector2>();
		if(Can_Left)
			Options.Add(Left);
		if(Can_Right)
			Options.Add(Right);
		if(Can_Up)
			Options.Add(Up);
		if(Can_Down)
			Options.Add(Down);
		if(Options.Count==0){
			Parts.Clear();
		}
		else{
			Vector2 Last_V=Parts[0];
			if(Options.Count==1){
				Parts[0]=Options[0];
			}
			else{
				int pick=Rnd.Next(0,Options.Count);
				int chance=Rnd.Next(0,2);
				while(pick<Options.Count&&chance>0&&Vector2.Distance(Parts[0],Target)>3){
					pick=Rnd.Next(0,Options.Count);
					chance=Rnd.Next(0,2);
				}
				if(pick<Options.Count){
					Parts[0]=Options[pick];
				}
				else{
					double min_distance=double.MaxValue;
					foreach(Vector2 v in Options)
						min_distance=Math.Min(min_distance,Vector2.Distance(Parts[0],v));
					foreach(Vector2 v in Options){
						if(min_distance>=Vector2.Distance(Parts[0],v)-0.1){
							Parts[0]=v;
							break;
						}
					}
				}
			}
			for(int i=1;i<Parts.Count;i++){
				if(Parts[i]!=Parts[i-1]){
					Vector2 temp=Parts[i];
					Parts[i]=Last_V;
					Last_V=temp;
				}
			}
		}
	}
	if(Parts.Count==0){
		Parts.Add(new Vector2(0,0));
		Parts.Add(new Vector2(0,0));
		Parts.Add(new Vector2(0,0));
		Target=new Vector2(Rnd.Next(0,8),Rnd.Next(0,8));
		Worm=new Color(Rnd.Next(100,255),Rnd.Next(100,255),Rnd.Next(100,255),255);
	}
	for(int i=0;i<Da.Panels.Count;i++){
		for(int j=0;j<Da.Panels[i].Count;j++){
			if((int)Target.Y==i&&(int)Target.X==j){
				Da.Panels[i][j].BackgroundColor=new Color(Rnd.Next(25,75),Rnd.Next(25,75),Rnd.Next(25,75),255);
			}
			else{
				int overlapped=-1;
				for(int k=0;k<Parts.Count;k++){
					Vector2 Part=Parts[k];
					if((int)Part.Y==i&&(int)Part.X==j){
						overlapped=k;
						break;
					}
				}
				if(overlapped>=0){
					float multx=1-(overlapped/(2.0f*(Parts.Count-1)));
					Da.Panels[i][j].BackgroundColor=Color.Multiply(Worm,multx);
				}
				else
					Da.Panels[i][j].BackgroundColor=new Color(10,10,10,255);
			}
			
		}
	}
}

double DisplayIdleTimer=0;
int Selection=0;
void Argument_Processor(string argument){
	if(((int)Status)<((int)GameStatus.Awaiting)){
		if(argument.ToLower().Equals("prev")){
			do{
				Selection=(Selection-1+7)%7;
			}
			while((Selection==5&&!Use_Real_Ships)||(Selection==0&&Status!=GameStatus.Ready)||(Selection==3&&Turn_Timer<30));
		}
		else if(argument.ToLower().Equals("next")){
			do{
				Selection=(Selection+1)%7;
			}
			while((Selection==5&&!Use_Real_Ships)||(Selection==0&&Status!=GameStatus.Ready)||(Selection==3&&Turn_Timer<30));
		}
		else if(argument.ToLower().Equals("down")){
			switch(Selection){
				case 1:
					Player_Count=Math.Max(0,Player_Count-1);
					break;
				case 2:
					Turn_Timer-=10;
					if(Turn_Timer<30)
						Turn_Timer=0;
					break;
				case 3:
					Allow_Pause=false;
					break;
				case 4:
					Use_Real_Ships=false;
					Destroy_Ships=false;
					break;
				case 5:
					Destroy_Ships=false;
					break;
				case 6:
					See_Opponent_Choice=false;
					break;
			}
		}
		else if(argument.ToLower().Equals("up")){
			switch(Selection){
				case 0:
					if(Status==GameStatus.Ready){
						Player_1_Ready=Player_Count<1;
						Player_2_Ready=Player_Count<2;
						Status=GameStatus.Awaiting;
					}
					break;
				case 1:
					Player_Count=Math.Min(2,Player_Count+1);
					break;
				case 2:
					if(Turn_Timer<30)
						Turn_Timer=30;
					else
						Turn_Timer=Math.Min(300,Turn_Timer+10);
					break;
				case 3:
					Allow_Pause=true;
					break;
				case 4:
					Use_Real_Ships=true;
					break;
				case 5:
					Destroy_Ships=true;
					break;
				case 6:
					See_Opponent_Choice=true;
					break;
			}
		}
	}
	else if(argument.ToLower().Equals("down")&&Status==GameStatus.Awaiting){
		Status=GameStatus.Ready;
	}
	if(argument.IndexOf("Player ")==0){
		int player_num=-1;
		string command=GetCommand(argument);
		string data=GetData(argument);
		if(command.Equals("Player 1"))
			player_num=1;
		else if(command.Equals("Player 2"))
			player_num=2;
		if(player_num>0){
			switch(data){
				case "Left":
					;
					break;
				case "Up":
					;
					break;
				case "Down":
					;
					break;
				case "Right":
					;
					break;
				case "Confirm":
					;
					break;
				case "Pause":
					if(Status==GameStatus.InProgress&&Allow_Pause)
						Status=GameStatus.Paused;
					else if(Status==GameStatus.Paused)
						Status=GameStatus.InProgress;
					break;
				case "Cancel":
					;
					break;
				case "Forfeit":
					;
					break;
			}
		}
	}
	
}

public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	if(DisplayIdleTimer<5)
		DisplayIdleTimer+=seconds_since_last_update;
	DisplayCheck(Player1Enemy);
	DisplayCheck(Player1Own);
	DisplayCheck(Player2Enemy);
	DisplayCheck(Player2Own);
	if(argument.Length>0){
		Argument_Processor(argument);
		while((Selection==5&&!Use_Real_Ships)||(Selection==0&&Status!=GameStatus.Ready)||(Selection==3&&Turn_Timer<30)){
			Selection=(Selection+1)%7;
		}
	}
	
	string HubText="";
	string Player1Text="";
	string Player2Text="";
	if(((int)Status)<((int)GameStatus.Awaiting)){
		Player1Text="Start game in Lobby";
		Player2Text="Start game in Lobby";
		HubText="Start game in Lobby\n";
		foreach(IMyDoor Door in Room1Doors){
			double Timer=5;
			if(HasBlockData(Door,"LastOpened")){
				double.TryParse(GetBlockData(Door,"LastOpened"),out Timer);
				if(Timer<7.5)
					Timer+=seconds_since_last_update;
				if(Door.Status==DoorStatus.Opening||(Timer>7.5&&Door.Status==DoorStatus.Open))
					Timer=0;
			}
			if(Timer>5)
				Door.CloseDoor();
			SetBlockData(Door,"LastOpened",Timer.ToString());
		}
		foreach(IMyDoor Door in Room2Doors){
			double Timer=5;
			if(HasBlockData(Door,"LastOpened")){
				double.TryParse(GetBlockData(Door,"LastOpened"),out Timer);
				if(Timer<7.5)
					Timer+=seconds_since_last_update;
				if(Door.Status==DoorStatus.Opening||(Timer>7.5&&Door.Status==DoorStatus.Open))
					Timer=0;
			}
			if(Timer>5)
				Door.CloseDoor();
			SetBlockData(Door,"LastOpened",Timer.ToString());
		}
		string s="";
		if(Selection==0)
			s="> ";
		else
			s="";
		if(Status==GameStatus.Ready)
			Write(s+"Start Game");
		if(Selection==1)
			s="> ";
		else
			s="";
		Write(s+"Player Count: "+Player_Count.ToString());
		if(Selection==2)
			s="> ";
		else
			s="";
		if(Turn_Timer<30)
			Write(s+"Turn Timer: Off");
		else
			Write(s+"Turn Timer: "+Math.Round(Turn_Timer,0).ToString()+"s");
		if(Turn_Timer>=30){
			if(Selection==3)
				s="> ";
			else
				s="";
			Write(s+"Allow Pause: "+Allow_Pause.ToString());
		}
		if(Selection==4)
			s="> ";
		else
			s="";
		Write(s+"Use Real Ships: "+Use_Real_Ships.ToString());
		if(Selection==5)
			s="> ";
		else
			s="";
		if(Use_Real_Ships)
			Write(s+"Destroy Ships: "+Destroy_Ships.ToString());
		if(Selection==6)
			s="> ";
		else
			s="";
		Write(s+"See Opponent's Choice: "+See_Opponent_Choice.ToString());
	}
	if(Status==GameStatus.Awaiting){
		HubText=Player_Count+" human players\n";
		HubText+="Player 1: ";
		if(!Player_1_Ready){
			foreach(IMyDoor Door in Room1Doors){
				Door.Enabled=true;
				Door.OpenDoor();
			}
			Player1Text="Press Confirm to ready-up\n";
			HubText+="Waiting...";
		}
		else{
			foreach(IMyDoor Door in Room1Doors){
				Door.Enabled=Door.Status!=DoorStatus.Closed;
				Door.CloseDoor();
			}
			Player1Text="Waiting for Player 2...\n";
			HubText+="Ready!";
		}
		HubText+="\nPlayer 2: ";
		if(!Player_2_Ready){
			foreach(IMyDoor Door in Room2Doors){
				Door.Enabled=true;
				Door.OpenDoor();
			}
			Player2Text="Press Confirm to ready-up\n";
			HubText+="Waiting...";
		}
		else{
			foreach(IMyDoor Door in Room2Doors){
				Door.Enabled=Door.Status!=DoorStatus.Closed;
				Door.CloseDoor();
			}
			Player2Text="Waiting for Player 1...\n";
			HubText+="Ready!";
		}
		if(Player_1_Ready&&Player_2_Ready){
			Status=GameStatus.InProgress;
		}
	}
	
	
	
	foreach(IMyTextPanel Panel in Player1StatusPanels)
		Panel.WriteText(Player1Text,false);
	foreach(IMyTextPanel Panel in Player2StatusPanels)
		Panel.WriteText(Player2Text,false);
	foreach(IMyTextPanel Panel in HubStatusPanels)
		Panel.WriteText(HubText,false);
}
