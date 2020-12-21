const string Program_Name = ""; //Name me!
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);

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

class Sector{
	private int _X;
	public int X{
		get{
			return _X;
		}
	}
	private int _Y;
	public int Y{
		get{
			return _Y;
		}
	}
	private int _Z;
	public int Z{
		get{
			return _Z;
		}
	}
	public Vector3D[] Corners;
	public Vector3D Center{
		get{
			return Corners[0]+new Vector3D(2500,2500,2500);
		}
	}
	
	public bool[] subsections;
	
	public Sector(int x,int y,int z){
		_X=x;
		_Y=y;
		_Z=z;
		Corners=new Vector3D[8];
		int small_x=X*5000;
		int large_x=(X+1)*5000;
		int small_y=Y*5000;
		int large_y=(Y+1)*5000;
		int small_z=Z*5000;
		int large_z=(Z+1)*5000;
		for(int i=0;i<8;i++){
			x=small_x;
			if(i%2==1)
				x=large_x;
			z=small_z;
			if(i%4>1)
				z=large_z;
			y=small_y;
			if(i>3)
				y=large_y;
			Corners[i]=new Vector3D(x,y,z);
		}
		subsections=new bool[25];
		for(int i=0;i<25;i++)
			subsections[i]=false;
	}
	
	public Sector(Vector3D Point):this((int)(Point.X/5000),(int)(Point.Y/5000),(int)(Point.Z/5000)){
		;
	}
	
	public Sector(int x,int y,int z,bool[25] subs):this(x,y,z){
		for(int i=0;i<subs.Length;i++)
			subsections[i]=subs[i];
	}
	
	public override string ToString(){
		string output="("+X.ToString()+","+Y.ToString()+","+Z.ToString()+")";
		for(int i=0;i<25;i++){
			if(i>0)
				output+=',';
			output+=subsections[i].ToString();
		}
	}
	
	public int GetSubInt(Vector3D Coords){
		if(Math.Abs(Coords.Y-Corners[0].Y)>25)
			return -1;
		if(Coords.X<Corners[0].X-25)
			return -1;
		if(Coords.X>Corners[1].X+25)
			return -1;
		if(Coords.Z<Corners[0].Z-25)
			return -1;
		if(Coords.Z>Corners[2].Z+25)
			return -1;
		int dx=(int)((Coords.X-Corners[0].X+25)/200);
		int dz=(int)((Coords.Z-Corners[0].Z+25)/200);
		int output=dx+5*dz;
		if(output<0||output>=25)
			return -1;
		return output;
	}
	
	public bool Same(Sector O){
		return X==O.X&&Y==O.Y&&Z==O.Z;
	}
	
	public void Update(Sector O){
		for(int i=0;i<25;i++)
			subsections[i]=subsections[i]||O.subsections[i];
	}
	
	public bool TryParse(string Parse,out Sector output){
		output=null;
		try{
			string[] parts=Parse.Split(')');
			if(parts.Length!=2||parts[0].IndexOf('(')!=0)
				return false;
			string[] coords=parts[0].Split(',');
			int X,Y,Z;
			if(!Int32.TryParse(coords[0],out X))
				return false;
			if(!Int32.TryParse(coords[1],out Y))
				return false;
			if(!Int32.TryParse(coords[2],out Z))
				return false;
			string[] bools=parts[1].Split(',');
			if(bools.Length!=25)
				return false;
			bool[] subsections=new bool[25];
			for(int i=0;i<25;i++){
				if(!bool.TryParse(str,out subsections[i]))
					return false;
			}
			output=new Sector(X,Y,Z,subsections);
			return true;
		}
		catch(Exception){
			return false;
		}
	}
}

class Zone{
	public Vector3D Center;
	public double Radius;
	public bool Outpost=false;
	public bool Gravity=false;
	public bool Explored=false;
	
	public Zone(Vector3D c,double r){
		Center=c;
		Radius=Math.Max(5000,r);
	}
	
	public bool Overlaps(Sector S){
		if((Center-S.Center).Length()<Radius+2500)
			return true;
		foreach(Vector3D Corner in S.Corners){
			if((Corner-Center).Length()<Radius+2500)
				return true;
		}
		return false;
	}
	
	public override string ToString(){
		return'{'+Center.ToString()+';'+Math.Round(Radius,1).ToString()+';'+Outpost.ToString()+';'+Gravity.ToString()+';'+Explored.ToString()+'}';
	}
	
	public static bool TryParse(string Parse,out Zone output){
		output=null;
		if(Parse[0]!='{'||Parse[Parse.Length-1]!='}')
			return false;
		Parse=Parse.Substring(1,Parse.Length-2);
		string[] args=Parse.Split(';');
		if(args.Length!=5)
			return false;
		Vector3D center;
		if(!Vector3D.TryParse(args[0],out center))
			return false;
		double radius;
		if(!double.TryParse(args[1],out radius))
			return false;
		output=new Zone(center,radius);
		bool o,g,x;
		if(!bool.TryParse(args[2],out o))
			return false;
		output.Outpost=o;
		if(!bool.TryParse(args[3],out g))
			return false;
		output.Gravity=g;
		if(!bool.TryParse(args[4],out x))
			return false;
		output.Explored=x;
		return true;
	}
}

class TerrainPoint{
	public Vector3D Point;
	public TimeSpan Age;
	
	public TerrainPoint(Vector3D p,TimeSpan a){
		Point=p;
		Age=a;
	}
	
	public TerrainPoint(Vector3D p):this(p,new TimeSpan(0)){
		;
	}
	
	public double Value(Vector3D R){
		return Age.TotalSeconds/(Math.Max(1,(Point-R).Length()-10));
	}
	
	public override string ToString(){
		return '{'+Point.ToString()+';'+Age.ToString()+'}';
	}
	
	public static bool TryParse(string Parse,out TerrainPoint output){
		output=null;
		if(Parse[0]!='{'||Parse[Parse.Length-1]!='}')
			return false;
		Parse=Parse.Substring(1,Parse.Length-2);
		string[] args=Parse.Split(';');
		if(args.Length!=2)
			return false;
		Vector3D point;
		if(!Vector3D.TryParse(args[0],out point))
			return false;
		TimeSpan age;
		if(!TimeSpan.TryParse(args[1],out age))
			return false;
		output=new TerrainPoint(point,age);
		return true;
	}
}

class TerrainMap{
	public List<TerrainPoint> Points;
	public Vector3D Center;
	public double Size{
		get{
			double size=-1;
			foreach(TerrainPoint Point in Points)
				size=Math.Max(size,(Point.Point-Center).Length());
			return size;
		}
	}
	
	public Vector3D Generated_Center{
		get{
			Vector3D min=Points[0].Point;
			Vector3D max=Points[0].Point;
			foreach(TerrainPoint P in Points){
				min.X=Math.Min(min.X,P.Point.X);
				min.Y=Math.Min(min.Y,P.Point.Y);
				min.Z=Math.Min(min.Z,P.Point.Z);
				max.X=Math.Max(max.X,P.Point.X);
				max.Y=Math.Max(max.Y,P.Point.Y);
				max.Z=Math.Max(max.Z,P.Point.Z);
			}
			return (min+max)/2;
		}
	}
	
	public int Count{
		get{
			return Points.Count;
		}
	}
	
	public TerrainMap(Vector3D c){
		Points=new List<TerrainPoint>();
		Center=c;
	}
	
	public TerrainPoint GetOldest(){
		TimeSpan Oldest=new TimeSpan(0);
		foreach(TerrainPoint P in Points){
			if(TimeSpan.Compare(Oldest,P.Age)==1)
				Oldest=P.Age;
		}
		foreach(TerrainPoint P in Points){
			if(TimeSpan.Compare(Oldest,P.Age)==0)
				return P;
		}
		return null;
	}
	
	public TerrainPoint GetBestToUpdate(Vector3D R){
		double best=0;
		foreach(TerrainPoint P in Points)
			best=Math.Max(best,P.Value(R));
		foreach(TerrainPoint P in Points){
			if(best<P.Value(R)+0.1)
				return P;
		}
		return null;
	}
	
	public TerrainPoint GetOldest(double max,Vector3D R){
		TimeSpan Oldest=new TimeSpan(0);
		foreach(TerrainPoint P in Points){
			if((P.Point-R).Length()<=max&&TimeSpan.Compare(Oldest,P.Age)==1)
				Oldest=P.Age;
		}
		foreach(TerrainPoint P in Points){
			if((P.Point-R).Length()<=max&&TimeSpan.Compare(Oldest,P.Age)==0)
				return P;
		}
		return null;
	}
	
	public TerrainPoint GetOuterMost(){
		double size=Size;
		foreach(TerrainPoint P in Points){
			if(size<=(P.Point-Center).Length()+0.1)
				return P;
		}
		return null;
	}
	
	public TerrainPoint GetOuterMost(Vector3D R){
		double offset=(R-Center).Length()-Size;
		int adder=0;
		double distance;
		if(Count==0)
			return null;
		do{
			distance=offset+Size/8+adder++;
			double outermost=0;
			foreach(TerrainPoint P in Points){
				if((P.Point-R).Length()<=distance)
					outermost=Math.Max(outermost,(P.Point-Center).Length());
			}
			foreach(TerrainPoint P in Points){
				if((P.Point-R).Length()<=distance&&outermost<=(P.Point-Center).Length()+.1)
					return P;
			}
		}
		while(true);
	}
	
	public int GetNeighbors(TerrainPoint Point,double distance=5){
		int count=0;
		foreach(TerrainPoint P in Points)
			if((P.Point-Point.Point).Length()<=distance)
				count++;
		return count;
	}
	
	public void UpdateAges(double seconds){
		for(int i=0;i<Points.Count;i++)
			Points.Age=Prog.UpdateTimeSpan(Points.Age,seconds);
	}
	
	public void Add(Vector3D V){
		Points.Add(new TerrainPoint(V));
	}
	
	public bool Remove(TerrainPoint P){
		return Points.Remove(P);
	}
	
	public bool RemoveAllInArea(Vector3D C,double R){
		for(int i=0;i<Points.Count;i++){
			if((Points[i].Point-C).Length()<=R)
				Points.RemoveAt(i--);
		}
	}
	
	public override string ToString(){
		string output=Center.ToString()+";(";
		foreach(TerrainPoint P in Points)
			output+=P.ToString()+',';
		if(P.Count>0)
			output=output.Substring(0,output.Length-1);
		return output+")";
	}
	
	public static bool TryParse(string Parse,out TerrainMap output){
		output=null;
		try{
			int index=Parse.IndexOf(";(");
			Vector3D center;
			if(!Vector3D.TryParse(Parse.Substring(0,index),out center))
				return false;
			Parse=Parse.Substring(index+1);
			if(Parse[0]!='('||Parse[Parse.Length-1]!=')')
				return false;
			output=new TerrainMap(center);
			Parse=Parse.Substring(1,Parse.Length-2);
			string[] args=Parse.Split(',');
			foreach(string arg in args){
				TerrainPoint P;
				if(TerrainPoint.TryParse(arg,out P))
					output.Add(P);
			}
			return output;
		}catch(Exception){
			return false;
		}
	}
}

TimeSpan FromSeconds(double seconds){
	return Prog.FromSeconds(seconds);
}

TimeSpan UpdateTimeSpan(TimeSpan old,double seconds){
	return Prog.UpdateTimeSpan(old,seconds);
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

TimeSpan Current_Time;

TerrainMap Asteroid;
List<Sector> Sectors;
List<Zone> Zones;

IMyRemoteControl Controller;
IMyGyro Gyroscope;

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
	Sectors=new List<Sector>();
	Zones=new List<Zone>();
	Asteroid=null;
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		if(arg.IndexOf("Sec:")==0){
			Sector sec;
			if(Sector.TryParse(arg.Substring(4),out sec))
				Sectors.Add(sec);
		}
		else if(arg.IndexOf("Zon:")==0){
			Zone zon;
			if(Zone.TryParse(arg.Substring(4),out zon))
				Zones.Add(zon);
		}
		else if(arg.IndexOf("Ast:")==0){
			if(!arg.Substring(4).Equals("null"))
				TerrainMap.TryParse(arg.Substring(4),out Asteroid);
		}
	}
	Controller=GenericMethods<IMyRemoteControl>.GetConstruct("Drone Remote Control");
	Gyroscope=GenericMethods<IMyGyro>.GetConstruct("Control Gyroscope");
	if(Controller==null||Gyroscope==null)
		return;
	
}

public void Save(){
	if(Asteroid==null)
		this.Storage="Ast:null";
	else
		this.Storage="Ast:"+Asteroid.ToString();
	
	foreach(Sector sector in Sectors){
		this.Storage+="•Sec:"+sector.ToString();
	}
	foreach(Zone zone in Zones){
		this.Storage+="•Zon:"+zone.ToString();
	}
}

void UpdateSectors(Sector S){
	for(int i=Sectors.Count-1;i>=0;i--){
		if(Sectors[i].Same(S)){
			Sectors[i].Update(S);
			return;
		}
	}
	Sectors.Add(S);
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
}

void UpdateSystemInfo(){
	Current_Time=DateTime.Now.TimeOfDay;
	if(Asteroid!=null)
		Asteroid.UpdateAges(seconds_since_last_update);
}

public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
}
