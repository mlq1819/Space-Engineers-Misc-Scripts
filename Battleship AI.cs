const string Program_Name = "Battleship AI"; //Name me!
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);

class Prog{
	public static MyGridProgram P;
	public static int ShipSize(MyShip ship){
		switch(ship){
			case MyShip.Carrier:
				return 5;
			case MyShip.Frigate:
				return 4;
			case MyShip.Cruiser:
				return 3;
			case MyShip.Prowler:
				return 3;
			case MyShip.Destroyer:
				return 2;
		}
		return 0;
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

enum ShipStatus{
	None=-1,
	SettingUp=0,
	Linking=1,
	Waiting=2,
	Traveling=3,
	InPosition=4,
	Receiving=5,
	Detonating=6,
	Returning=7
}

enum MyShip{
	None=0,
	Carrier=1,
	Frigate=2,
	Cruiser=3,
	Prowler=4,
	Destroyer=5
}

int ShipSize(MyShip s){
	return Prog.ShipSize(s);
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

bool GyroFunc(IMyGyro G){
	return G!=null&&G.IsFunctional;
}

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;
Random Rnd;

IMyRemoteControl Controller;
IMyGyro Gyroscope;
ShipStatus CurrentStatus=ShipStatus.None;
ShipStatus Status{
	get{
		if(Type==MyShip.None||Player_Num<1||Player_Num>2)
			return ShipStatus.SettingUp;
		if(Returning)
			return ShipStatus.Returning;
		if((!Started)||End1.Length()==0||(End1-Controller.GetPosition()).Length()>5000||End2.Length()==0||(End2-Controller.GetPosition()).Length()>5000){
			if(Target_Laser.Length()==0||(Target_Laser-Controller.GetPosition()).Length()>5000)
				return ShipStatus.Linking;
			else
				return ShipStatus.Waiting;
		}
		if((Target_Position-Controller.GetPosition()).Length()>1||GetAngle(Target_Forward,Forward_Vector)>1||GetAngle(Target_Up,Up_Vector)>1)
			return ShipStatus.Traveling;
		return CurrentStatus;
	}
}
IMyRadioAntenna Antenna_R;
IMyLaserAntenna Antenna_L;
int ID;
bool Started=false;
bool Returning=true;
double Fire_Timer=0;



MyShip Type=MyShip.None;
int Player_Num=-1;
string MyListenerString{
	get{
		return Type.ToString()+" "+Player_Num;
	}
}

List<IMyDecoy> Decoys=new List<IMyDecoy>();
Vector3D DecoyPosition(int i){
	if(Decoys.Count>i)
		return new Vector3D(0,0,0);
	if(Decoys[i-1]!=null)
		return Decoys[i-1].GetPosition();
	double sum=0;
	int total=0;
	for(int i=0;i<Decoys.Count;i++){
		for(int j=i+1;j<Decoys.Count;j++){
			if(i!=j&&Decoys[i]!=null&&Decoys[j]!=null){
				double distance=Decoys[j]-Decoys[i];
				sum+=distance/Math.Abs(i-j);
				total++;
			}
		}
	}
	if(total==0)
		return new Vector3D(0,0,0);
	sum=sum/total;
	int ref_num=i;
	int distance=0;
	while(ref_num>0&&ref_num<=Decoys.Count&&Decoys[ref_num-1]==null){
		distance++;
		ref_num=i-distance;
		if(ref_num<0||Decoys[ref_num-1]==null){
			ref_num=i+distance;
		}
	}
	if(ref_num>0&&ref_num<=Decoys.Count&&Decoys[ref_num-1]!=null){
		Vector3D output=Decoys[ref_num-1].GetPosition();
		output+=forward_vector*(ref_num-i)*sum;
		return output;
	}
	return new Vector3D(0,0,0);
}

double ShipMass{
	get{
		return Controller.CalculateShipMass().TotalMass;
	}
}
bool Match_Directions=false;
Vector3D Target_Forward=new Vector3D(0,0,-1);
Vector3D Target_Up=new Vector3D(0,1,0);

Vecto3D Target_Laser=new Vector3D(0,0,0);

Vector3D End1=new Vector3D(0,0,0);
Vector3D End2=new Vector3D(0,0,0);

Vector3D Target_Position=new Vector3D(0,0,0);
Vector3D Get_Position(){
	Vector3D Decoy_First=DecoyPosition(1);
	Vector3D Decoy_Last=DecoyPosition(Decoys.Count);
	Decoy_First=GlobalToLocalPosition(Decoy_First,Controller);
	Decoy_Last=GlobalToLocalPosition(Decoy_Last,Controller);
	Decoy_First.Y=0;
	Decoy_First.X=0;
	Decoy_Last.Y=0;
	Decoy_Last.X=0;
	Decoy_First=LocalToGlobalPosition(Decoy_First,Controller);
	Decoy_Last=LocalToGlobalPosition(Decoy_Last,Controller);
	double d_Decoys=(Decoy_First-Decoy_Last).Length();
	double p_front=(Decoy_First-Controller.GetPosition()).Length()/d_Decoys;
	double p_back=(Decoy_Last-Controller.GetPosition()).Length()/d_Decoys;
	double p_sum=p_front+p_back;
	p_front/=p_sum;
	p_back/=p_sum;
	return p_back*End1+p_front*End2
}

Vector3D AngularVelocity;
Vector3D Relative_AngularVelocity{
	get{
		return GlobalToLocal(AngularVelocity,Controller);
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
	Rnd=new Random();
	ID=Rnd.Next(0,Int32.MaxValue);
	Echo("Beginning initialization");
	Controller=GenericMethods<IMyRemoteControl>.GetContaining("",5);
	Gyroscope=GenericMethods<IMyGyro>.GetClosestFunc(GyroFunc);
	if(Controller==null||Gyroscope==null)
		return;
	Forward=Controller.Orientation.Forward;
	Up=Controller.Orientation.Up;
	Left=Controller.Orientation.Left;
	Antenna_R=GenericMethods<IMyRadioAntenna>.GetContaining("");
	Antenna_L=GenericMethods<IMyLaserAntenna>.GetContaining("");
	if(Antenna_R==null||Antenna_L==null)
		return;
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		int index=arg.IndexOf(':');
		if(index>0){
			string type=arg.Substring(0,index);
			string data=arg.Substring(index+1);
			switch(type){
				case "ID":
					Int32.TryParse(data,out ID);
					break;
				case "CurrentStatus":
					int status;
					if(Int32.TryParse(data,out status)&&status>=-1&&status<=7)
						CurrentStatus=(ShipStatus)status;
					break;
				case "Target_Laser":
					Vector3D.TryParse(data,out Target_Laser);
					break;
				case "End1":
					Vector3D.TryParse(data,out End1);
					break;
				case "End2":
					Vector3D.TryParse(data,out End2);
					break;
				case "Target_Forward":
					Vector3D.TryParse(data,out Target_Forward);
					break;
				case "Target_Up":
					Vector3D.TryParse(data,out Target_Up);
					break;
				case "Target_Position":
					Vector3D.TryParse(data,out Target_Position);
					break;
				case "Started":
					bool.TryParse(data,out Started);
					break;
				case "Returning":
					bool.TryParse(data,out Returning);
					break;
				case "Fire_Timer":
					double.TryParse(data,out Fire_Timer);
					break;
			}
		}
	}
	SetUp();
	
	
	
	Echo("Completed initialization");
	Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

public void Save(){
    this.Storage="ID:"+ID.ToString();
	this.Storage+="•CurrentStatus:"+((int)CurrentStatus).ToString();
	this.Storage+="•Target_Laser:"+Target_Laser.ToString();
	this.Storage+="•End1:"+End1.ToString();
	this.Storage+="•End2:"+End2.ToString();
	this.Storage+="•Target_Forward:"+Target_Forward.ToString();
	this.Storage+="•Target_Up:"+Target_Up.ToString();
	this.Storage+="•Target_Position:"+Target_Position.ToString();
	this.Storage+="•Started:"+Started.ToString();
	this.Storage+="•Returning:"+Returning.ToString();
	this.Storage+="•Fire_Timer:"+Fire_Timer.ToString();
	
}

void UpdateSystemInfo(){
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
	AngularVelocity=Controller.GetShipVelocities().AngularVelocity;
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

void SetUp(){
	string[] args=Me.CustomData.Split('\n');
	Write("Setting Up...");
	foreach(string arg in args){
		int index=arg.IndexOf(":");
		if(index>0){
			string type=arg.Substring(0,index);
			string data=arg.Substring(index+1);
			if(type.Equals("Ship Type")){
				for(int i=1;i<=5;i++){
					if(data.ToLower().Equals(((MyShip)i).ToString().ToLower())){
						Type=((MyShip)i);
						break;
					}
				}
			}
			else if(type.Equals("Player Number")){
				int i=0;
				if(Int32.TryParse(data,out i)){
					if(i>0&&i<=2)
						Player_Num=i;
				}
			}
		}
	}
	if(Status!=ShipStatus.SettingUp){
		IGC.RegisterBroadcastListener(MyListenerString);
		Decoys.Clear();
		for(int i=1;i<=ShipSize(Type);i++)
			Decoys.Add(GenericMethods<IMyDecoy>.GetFull("Decoy "+i.ToString()));
	}
	Me.CustomData="Ship Type:"+Type.ToString()+"\nPlayer Number:"+Player_Num.ToString();
	Write(Me.CustomData);
	Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

void Link(){
	Write("Linking...");
	bool try_connect=false;
	if(Target_Laser.Length()==0||(Target_Laser-Controller.GetPosition()).Length()>5000){
		Antenna_R.Radius=5000;
		Antenna_R.Enabled=true;
	}
	else 
		try_connect=true;
	List<IMyBroadcastListener> listeners=new List<IMyBroadcastListener>();
	IGC.GetBroadcastListeners(listeners);
	foreach(IMyBroadcastListener Listener in listeners){
		if(Listener.Tag.Equals(MyListenerString)){
			while(Listener.HasPendingMessage){
				MyIGCMessage message=Listener.AcceptMessage();
				int index=message.Data.ToString().IndexOf(":");
				if(index>0){
					string target=message.Data.ToString().Substring(0,index);
					int target_id=-1;
					if(Int32.TryParse(target,out target_id)&&target_id==ID){
						string data=message.Data.ToString().Substring(index+1);
						Vector3D.TryParse(data,out Target_Laser);
					}
				}
			}
		}
	}
	if(try_connect){
		Antenna_L.SetTargetCoords((new MyWaypointInfo("Hub "+MyListenerString+" Laser Antenna",Target_Laser.ToString())).ToString());
		Antenna_L.Connect();
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

void Wait(){
	Write("Waiting...");
	List<IMyBroadcastListener> listeners=new List<IMyBroadcastListener>();
	IGC.GetBroadcastListeners(listeners);
	foreach(IMyBroadcastListener Listener in listeners){
		if(Listener.Tag.Equals(MyListenerString)){
			while(Listener.HasPendingMessage){
				MyIGCMessage message=Listener.AcceptMessage();
				string[] args=message.Data.ToString().Split('•');
				if(args.Count==5&&args[0].Equals("Ends")){
					Vector3D.TryParse(args[1],out End1);
					Vector3D.TryParse(args[2],out End2);
					Vector3D.TryParse(args[3],out Target_Forward);
					Vector3D.TryParse(args[4],out Target_Up);
					Target_Position=Get_Position();
				}
			}
		}
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

void Travel(){
	Write("Traveling...");
	if((Target_Position-Controller.GetPosition()).Length()>1){
		MyWaypointInfo Destination=new MyWaypointInfo("Target Position",Target_Position);
		if((!Controller.CurrentWaypoint.Equals(Destination))||!Controller.IsAutoPilotEnabled){
			Controller.ClearWaypoints();
			Controller.AddWaypoint(Destination);
			Controller.SetCollisionAvoidance(true);
			Speed_Limit=50;
			Controller.SpeedLimit=(float)Speed_Limit;
			Controller.SetAutoPilotEnabled(true);
		}
	}
	else {
		Controller.SetAutoPilotEnabled(false);
		if(GetAngle(Forward_Vector,Target_Forward)>1||GetAngle(Up_Vector,Target_Up)>1){
			Gyroscope.GyroOverride=(AngularVelocity.Length()<3);
			float current_pitch=(float) Relative_AngularVelocity.X;
			float current_yaw=(float) Relative_AngularVelocity.Y;
			float current_roll=(float) Relative_AngularVelocity.Z;
			
			float gyro_count=0;
			List<IMyGyro> AllGyros=new List<IMyGyro>();
			GridTerminalSystem.GetBlocksOfType<IMyGyro>(AllGyros);
			foreach(IMyGyro Gyro in AllGyros){
				if(Gyro.IsWorking)
					gyro_count+=Gyro.GyroPower/100.0f;
			}
			float gyro_multx=(float)Math.Max(0.1f,Math.Min(1,1.5f/(ShipMass/gyro_count/1000000)));
			
			float input_pitch=current_pitch*0.99f;
			double difference=GetAngle(Down_Vector,Target_Forward)-GetAngle(Up_Vector,Target_Forward);
			if(Math.Abs(difference)>.1)
				input_pitch-=(float)Math.Min(Math.Max(difference/5,-4),4)*gyro_multx;
			
			float input_yaw=current_yaw*0.99f;
			difference=GetAngle(Left_Vector,Target_Forward)-GetAngle(Right_Vector,Target_Forward);
			if(Math.Abs(difference)>.1)
				input_yaw+=(float)Math.Min(Math.Max(difference/5,-4),4)*gyro_multx;
			
			float input_roll=current_roll*0.99f;
			difference=GetAngle(Left_Vector,Target_Up)-GetAngle(Right_Vector,Target_Up);
			if(Math.Abs(difference)>.1)
				input_roll+=(float)Math.Min(Math.Max(difference/5,-4),1)*gyro_multx;
			Vector3D input=new Vector3D(input_pitch,input_yaw,input_roll);
			
			Vector3D global=Vector3D.TransformNormal(input,Controller.WorldMatrix);
			Vector3D output=Vector3D.TransformNormal(global,MatrixD.Invert(Gyroscope.WorldMatrix));
			output.Normalize();
			output*=input.Length();
			
			Gyroscope.Pitch=(float)output.X;
			Gyroscope.Yaw=(float)output.Y;
			Gyroscope.Roll=(float)output.Z;
		}
		else
			CurrentStatus=ShipStatus.InPosition;
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

IMyDecoy GetNearest(Vector3D near,double max_distance=double.MaxValue){
	double min_distance=max_distance;
	foreach(IMyDecoy Decoy in Decoys){
		if(Decoy!=null&&Decoy.IsWorking)
			min_distance=Math.Min(min_distance,(near-Decoy.GetPosition()).Length());
	}
	foreach(IMyDecoy Decoy in Decoys){
		if(Decoy!=null&&Decoy.IsWorking){
			double distance=(near-Decoy.GetPosition()).Length();
			if(distance>=min_distance-0.1){
				return Decoy;
			}
		}
	}
	return null;
}
void InPosition(){
	Write("In Position, Awaiting Commands...");
	List<IMyBroadcastListener> listeners=new List<IMyBroadcastListener>();
	IGC.GetBroadcastListeners(listeners);
	foreach(IMyBroadcastListener Listener in listeners){
		if(Listener.Tag.Equals(MyListenerString)){
			while(Listener.HasPendingMessage){
				MyIGCMessage message=Listener.AcceptMessage();
				string[] args=message.Data.ToString().Split('•');
				if(args.Count==2&&args[0].Equals("Request")){
					Vector3D near;
					if(Vector3D.TryParse(args[1],out near)){
						IMyDecoy Decoy=GetNearest(near);
						if(Decoy!=null)
							IGC.SendBroadcastMessage(MyListenerString,"Target•"+Decoy.GetPosition().ToString(),TransmissionDistance.TransmissionDistanceMax);
					}
				}
				else if(args.Count==3&&args[0].Equals("Fire")){
					Vector3D near;
					Fire_Timer=4;
					double.TryParse(args[2],out Fire_Timer);
					CurrentStatus=ShipStatus.Receiving;
					if(Vector3D.TryParse(args[1],out near)){
						IMyDecoy Decoy=GetNearest(near,15);
						if(Decoy!=null)
							Decoy.Enabled=false;
					}
				}
				else if(args.Count==2&&args[0].Equals("Return")){
					Vector3D temp;
					if(Vector3D.TryParse(args[1],out temp)){
						Returning=true;
						Target_Position=temp;
					}
				}
			}
		}
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update10;
}

void Receiving(){
	Write("Receiving in "+Math.Round(Fire_Timer,1)+" seconds...");
	if(Fire_Timer<=0){
		int count=0;
		foreach(IMyDecoy Decoy in Decoys){
			if(Decoy!=null){
				if(Decoy.IsWorking&&Decoy.Enabled)
					count++;
			}
		}
		if(count>0)
			CurrentStatus=ShipStatus.InPosition;
		else
			CurrentStatus=ShipStatus.Detonating;
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

bool armed=false;
void Detonate(){
	Write("Detonating...");
	if((!armed)&&Fire_Timer<=0){
		List<IMyWarhead> Unset=new List<IMyWarhead>();
		GridTerminalSystem.GetBlocksOfType<IMyWarhead>(Unset);
		List<IMyWarhead> Set=new List<IMyWarhead>();
		if(Unset.Count>0){
			double Time=2;
			while(Unset.Count>0){
				bool found=false;
				for(int i=0;i<Set.Count;i++){
					IMyWarhead Armed=Set[i];
					for(int j=0;j<Unset.Count;j++){
						IMyWarhead Unarmed=Unset[j];
						if((Armed.GetPosition()-Unarmed.GetPosition()).Length()<12){
							found=true;
							Unarmed.DetonationTime=Armed.DetonationTime;
							Unarmed.StartCountdown();
							Unarmed.IsArmed=true;
							Set.Add(Unarmed);
							Unset.Remove(Unarmed);
							j--;
						}
					}
				}
				if(!found){
					Time+=(Rnd.Next(10,30)+Rnd.Next(10,30))/10.0;
					IMyWarhead Pick=Unset[Rnd.Next(0,Unset.Count-1)];
					Pick.DetonationTime=Time;
					Pick.StartCountdown();
					Pick.IsArmed=true;
					Set.Add(Pick);
					Unset.Remove(Pick);
				}
			}
		}
		armed=true;
	}
	else if(Fire_Timer>0)
		Write(Math.Round(Fire_Timer,1).ToString()+" seconds to Detonation");
	Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

void Return(){
	Write("Game has ended");
	int missing=ShipSize(Type);
	foreach(IMyDecoy Decoy in Decoys){
		if(Decoy!=null){
			if(Decoy.IsWorking&&Decoy.Enabled)
				count--;
		}
	}
	if(count>0){
		Fire_Timer=60;
		CurrentStatus=ShipStatus.Detonating;
		Write("Detonating...");
		return;
	}
	else
		Write("Returning...");
	
	if((Target_Position-Controller.GetPosition()).Length()>1){
		MyWaypointInfo Destination=new MyWaypointInfo("Target Position",Target_Position);
		if((!Controller.CurrentWaypoint.Equals(Destination))||!Controller.IsAutoPilotEnabled){
			Controller.ClearWaypoints();
			Controller.AddWaypoint(Destination);
			Controller.SetCollisionAvoidance(true);
			Speed_Limit=50;
			Controller.SpeedLimit=(float)Speed_Limit;
			Controller.SetAutoPilotEnabled(true);
		}
	}
	else{
		End1=new Vector3D(0,0,0);
		End2=new Vector3D(0,0,0);
		Started=false;
		Returning=false;
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update10;
}

public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	UpdateSystemInfo();
	if(Gyroscope==null||Gyroscope.IsFunctional)
		Gyroscope=GenericMethods<IMyGyro>.GetClosestFunc(GyroFunc);
	Gyro.GyroOverride=false;
	if(Fire_Timer>0)
		Fire_Timer=Math.Max(0,Fire_Timer-seconds_since_last_update);
	if(Status==ShipStatus.SettingUp)
		SetUp();
	if(Status==ShipStatus.Linking)
		Link();
	if(Status==ShipStatus.Waiting)
		Wait();
	if(Status==ShipStatus.Traveling)
		Travel();
	if(Status==ShipStatus.InPosition)
		InPosition();
	if(Status==ShipStatus.Receiving)
		Receiving();
	if(Status==ShipStatus.Detonating)
		Detonate();
	if(Status==ShipStatus.Returning)
		Return();
	Antenna_R.HudText=Status.ToString();
	if(Status!=ShipStatus.SettingUp)
		IGC.SendBroadcastMessage(MyListenerString,"Status•"+Status.ToString(),TransmissionDistance.TransmissionDistanceMax);
}
