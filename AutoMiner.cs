const string Program_Name = "AutoMiner Drone AI"; //Name me!
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
	public bool Complete{
		get{
			foreach(bool b in subsections){
				if(!b)
					return false;
			}
			return true;
		}
	}
	
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
		subsections=new bool[625];
		for(int i=0;i<625;i++)
			subsections[i]=false;
	}
	
	public static Vector3D GetStart(Vector3D input){
		Vector3D output=input;
		output.X=((int)(input.X/5000))*5000;
		output.Y=((int)(input.Y/5000))*5000;
		output.Z=((int)(input.Z/5000))*5000;
		return output;
	}
	
	public Sector(Vector3D Point):this((int)(Point.X/5000),(int)(Point.Y/5000),(int)(Point.Z/5000)){
		;
	}
	
	public Sector(int x,int y,int z,bool[] subs):this(x,y,z){
		for(int i=0;i<subs.Length;i++)
			subsections[i]=subs[i];
	}
	
	public override string ToString(){
		string output="("+X.ToString()+","+Y.ToString()+","+Z.ToString()+")";
		for(int i=0;i<625;i++){
			if(i>0)
				output+=',';
			output+=subsections[i].ToString();
		}
		return output;
	}
	
	public string NiceString(Vector3D Reference){
		Vector3D R=Sector.GetStart(Reference);
		R/=5000;
		int count=0;
		int total=0;
		foreach(bool b in subsections){
			total++;
			if(b)
				count++;
		}
		return "Sector ("+(X-((int)R.X)).ToString()+","+(Y-((int)R.Y)).ToString()+","+(Z-((int)R.Z)).ToString()+") "+count+"/"+total;
	}
	
	public string NiceString(){
		return NiceString(new Vector3D(0,0,0));
	}
	
	public int Distance(Vector3D Reference){
		Vector3D R=Sector.GetStart(Reference);
		R/=5000;
		return Math.Abs(X-((int)R.X))+Math.Abs(Y-((int)R.Y))+Math.Abs(Z-((int)R.Z));
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
	
	public bool Same(Vector3D I){
		Vector3D O=Sector.GetStart(I);
		return X==((int)O.X)&&Y==((int)O.Y)&&Z==((int)O.Z);
	}
	
	public bool Same(Sector O){
		return X==O.X&&Y==O.Y&&Z==O.Z;
	}
	
	public void Update(Sector O){
		for(int i=0;i<625;i++)
			subsections[i]=subsections[i]||O.subsections[i];
	}
	
	public static bool TryParse(string Parse,out Sector output){
		output=null;
		try{
			string[] parts=Parse.Split(')');
			if(parts.Length!=2||parts[0].IndexOf('(')!=0)
				return false;
			parts[0]=parts[0].Substring(1);
			string[] coords=parts[0].Split(',');
			int X,Y,Z;
			if(!Int32.TryParse(coords[0],out X))
				return false;
			if(!Int32.TryParse(coords[1],out Y))
				return false;
			if(!Int32.TryParse(coords[2],out Z))
				return false;
			string[] bools=parts[1].Split(',');
			if(bools.Length!=625)
				return false;
			bool[] subsections=new bool[625];
			for(int i=0;i<625;i++){
				if(!bool.TryParse(bools[i],out subsections[i]))
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
	
	public bool Overlaps(Vector3D S){
		Vector3D center=S+2500*(new Vector3D(1,1,1));
		if((Center-center).Length()<Radius+2500)
			return true;
		for(int i=0;i<8;i++){
			Vector3D Tweak=new Vector3D(0,0,0);
			if(i%2==1)
				Tweak.X=1;
			if((i/2)%2==1)
				Tweak.Z=1;
			if(i>=4)
				Tweak.Y=1;
			Tweak*=5000;
			if(((S+Tweak)-Center).Length()<Radius+2500)
				return true;
		}
		return false;
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
	
	public string NiceString(){
		string output="";
		if(Outpost)
			output+="Outpost ";
		else if(Gravity)
			output+="Gravity ";
		else if(Explored)
			output+="Explored ";
		output+="Zone with Radius "+Math.Round(Radius/1000,1)+"kM";
		return output;
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
	public static bool Update_Size=true;
	private double _Size;
	public double Size{
		get{
			if(Update_Size){
				_Size=0;
				foreach(TerrainPoint Point in Points)
					_Size=Math.Max(_Size,(Point.Point-Center).Length());
				Update_Size=false;
			}
			return _Size;
		}
	}
	
	public static Func<string,string,bool> Broadcast;
	public static bool DoBroadcast=false;
	
	public double Distance_From_Angle(double angle){
		return 2*Math.Sin(angle*Math.PI/360)*Size;
	}
	
	public double Distance(Vector3D v1,Vector3D v2){
		Vector3D d1=v1-Center;
		Vector3D d2=v2-Center;
		d1.Normalize();
		d2.Normalize();
		return Distance_From_Angle(GenericMethods<IMyTerminalBlock>.GetAngle(d1,d2));
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
		while(((double)adder)<=Size/7);
		return null;
	}
	
	public List<TerrainPoint> GetNeighbors(TerrainPoint Point,double distance=7.5){
		List<TerrainPoint> Output=new List<TerrainPoint>();
		foreach(TerrainPoint P in Points){
			double dif=Math.Min((P.Point-Point.Point).Length(),Distance(P.Point,Point.Point));
			if(dif<=distance&&dif>0.1)
				Output.Add(P);
		}
		return Output;
	}
	
	public int CountNeighbors(TerrainPoint Point,double distance=7.5){
		int count=0;
		foreach(TerrainPoint P in Points){
			double dif=Math.Min((P.Point-Point.Point).Length(),Distance(P.Point,Point.Point));
			if(dif<=distance&&dif>0.1)
				count++;
		}
		return count;
	}
	
	public void UpdateAges(double seconds){
		for(int i=0;i<Points.Count;i++)
			Points[i].Age=Prog.UpdateTimeSpan(Points[i].Age,seconds);
	}
	
	public void Add(TerrainPoint P){
		for(int i=0;i<Points.Count;i++){
			if((Points[i].Point-P.Point).Length()<0.1){
				Points[i]=P;
				return;
			}
		}
		Points.Add(P);
	}
	
	public void Add(Vector3D V){
		if(DoBroadcast)
			Broadcast("Ast-Add",(new TerrainPoint(V)).ToString());
		for(int i=0;i<Points.Count;i++){
			if((Points[i].Point-V).Length()<0.1){
				Points[i]=new TerrainPoint(V);
				return;
			}
		}
		Points.Add(new TerrainPoint(V));
	}
	
	public bool Remove(TerrainPoint P){
		if(DoBroadcast)
			Broadcast("Ast-Rem",P.ToString());
		foreach(TerrainPoint P2 in Points){
			if(P2.Point==P.Point)
				return Points.Remove(P2);
		}
		return false;
	}
	
	public int RemoveAllInArea(Vector3D C,double R){
		int count=0;
		for(int i=0;i<Points.Count;i++){
			if((Points[i].Point-C).Length()<=R){
				count++;
				if(DoBroadcast)
					Broadcast("Ast-Rem",Points[i].ToString());
				Points.RemoveAt(i--);
			}
		}
		return count;
	}
	
	public override string ToString(){
		string output=Center.ToString()+";(";
		foreach(TerrainPoint P in Points)
			output+=P.ToString()+',';
		if(Points.Count>0)
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
			return true;
		}catch(Exception){
			return false;
		}
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
	Charging=3,
	Returning=4,
	Traveling=5,
	Exploring=6,
	Scanning=7,
	Ejecting=8,
	Mining=9
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
		return Current_Time.TotalSeconds%10800;
	}
}

Stack<DroneTask> Tasks;
bool AutoUndock=true;

Dock MyDock;
TerrainMap Asteroid;
List<Sector> Sectors;
int Last_Sector=-1;
List<Zone> Zones;

IMyRemoteControl Controller;
IMyGyro Gyroscope;
IMyRadioAntenna Antenna;
List<IMyShipDrill> Drills;
List<IMyConveyorSorter> Sorters;
List<IMyShipConnector> Connectors;
IMyShipConnector Docking_Connector;
List<IMyBatteryBlock> Batteries;
List<IMyCargoContainer> Cargo;
IMySensorBlock Sensor;

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

IMyCameraBlock[] All_Cameras=new IMyCameraBlock[5];
IMyCameraBlock Forward_Camera{
	set{
		All_Cameras[0]=value;
	}
	get{
		return All_Cameras[0];
	}
}
IMyCameraBlock Top_Camera{
	set{
		All_Cameras[1]=value;
	}
	get{
		return All_Cameras[1];
	}
}
IMyCameraBlock Bottom_Camera{
	set{
		All_Cameras[2]=value;
	}
	get{
		return All_Cameras[2];
	}
}
IMyCameraBlock Left_Camera{
	set{
		All_Cameras[3]=value;
	}
	get{
		return All_Cameras[3];
	}
}
IMyCameraBlock Right_Camera{
	set{
		All_Cameras[4]=value;
	}
	get{
		return All_Cameras[4];
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
	Sectors=new List<Sector>();
	Zones=new List<Zone>();
	Asteroid=null;
	MyDock=null;
	Tasks=new Stack<DroneTask>();
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
		else if(arg.IndexOf("Tas:")==0){
			int t;
			if(Int32.TryParse(arg.Substring(4),out t))
				Tasks.Push((DroneTask)t);
		}
		else if(arg.IndexOf("Ast:")==0){
			if(!arg.Substring(4).Equals("null"))
				TerrainMap.TryParse(arg.Substring(4),out Asteroid);
		}
		else if(arg.IndexOf("Doc:")==0){
			if(!arg.Substring(4).Equals("null"))
				Dock.TryParse(arg.Substring(4),out MyDock);
		}
		else if(arg.IndexOf("LSc:")==0){
			Int32.TryParse(arg.Substring(4),out Last_Sector);
		}
		else if(arg.IndexOf("Und:")==0){
			bool.TryParse(arg.Substring(4),out AutoUndock);
		}
	}
	TerrainMap.Broadcast=Broadcast;
	TerrainMap.DoBroadcast=true;
	Controller=GenericMethods<IMyRemoteControl>.GetConstruct("Drone Remote Control");
	Gyroscope=GenericMethods<IMyGyro>.GetConstruct("Control Gyroscope");
	if(Controller==null||Gyroscope==null)
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
	Antenna=GenericMethods<IMyRadioAntenna>.GetConstruct("Drone Antenna");
	if(Antenna==null)
		return;
	Drills=GenericMethods<IMyShipDrill>.GetAllConstruct("");
	Sorters=GenericMethods<IMyConveyorSorter>.GetAllConstruct("");
	Connectors=GenericMethods<IMyShipConnector>.GetAllConstruct("Drone Connector");
	Docking_Connector=GenericMethods<IMyShipConnector>.GetConstruct("Drone Connector (Back)");
	if(Docking_Connector==null)
		return;
	Batteries=GenericMethods<IMyBatteryBlock>.GetAllConstruct("Drone");
	Cargo=GenericMethods<IMyCargoContainer>.GetAllConstruct("Drone Cargo Container");
	if(Drills.Count==0||Sorters.Count==0||Connectors.Count==0||Batteries.Count==0||Cargo.Count==0)
		return;
	Sensor=GenericMethods<IMySensorBlock>.GetConstruct("Drone Sensor");
	if(Sensor==null)
		return;
	Forward_Camera=GenericMethods<IMyCameraBlock>.GetConstruct("Drone Camera (Front Center)");
	Top_Camera=GenericMethods<IMyCameraBlock>.GetConstruct("Drone Camera (Front Top)");
	Bottom_Camera=GenericMethods<IMyCameraBlock>.GetConstruct("Drone Camera (Front Bottom)");
	Left_Camera=GenericMethods<IMyCameraBlock>.GetConstruct("Drone Camera (Front Left)");
	Right_Camera=GenericMethods<IMyCameraBlock>.GetConstruct("Drone Camera (Front Right)");
	for(int i=0;i<All_Cameras.Length;i++){
		if(All_Cameras[i]==null)
			return;
		All_Cameras[i].EnableRaycast=true;
	}
	
	IGC.RegisterBroadcastListener("AutoMiner");
	IGC.RegisterBroadcastListener("AutoMiner Base AI");
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

bool Factory_Reset=false;
public void Save(){
	if(Factory_Reset)
		return;
	if(Asteroid==null)
		this.Storage="Ast:null";
	else
		this.Storage="Ast:"+Asteroid.ToString();
	this.Storage+="•Doc:";
	if(MyDock==null)
		this.Storage+="null";
	else
		this.Storage+=MyDock.ToString();
	this.Storage+="•Und:"+AutoUndock.ToString();
	if(Last_Sector!=-1)
		this.Storage+="•LSc:"+Last_Sector.ToString();
	foreach(Sector sector in Sectors)
		this.Storage+="•Sec:"+sector.ToString();
	foreach(Zone zone in Zones)
		this.Storage+="•Zon:"+zone.ToString();
	Stack<DroneTask> temp=new Stack<DroneTask>();
	foreach(DroneTask T in Tasks)
		temp.Push(T);
	foreach(DroneTask T in temp)
		this.Storage+="•Tas:"+((int)T).ToString();
}

bool InGravityZone(Vector3D pos,out Zone GZ){
	GZ=null;
	for(int i=0;i<Zones.Count;i++){
		if(Zones[i].Gravity&&(Zones[i].Center-pos).Length()<Zones[i].Radius){
			GZ=Zones[i];
			return true;
		}
	}
	return false;
}

bool IntersectsGravityZone(int count,Vector3D A,Vector3D B,out Zone GZ){
	Vector3D middle=(A+B)/2;
	Zone z;
	GZ=null;
	if(InGravityZone(middle,out z)){
		GZ=z;
		return true;
	}
	if(count==0)
		return false;
	if(IntersectsGravityZone(count-1,A,middle,out z)){
		GZ=z;
		return true;
	}
	if(IntersectsGravityZone(count-1,middle,B,out z)){
		GZ=z;
		return true;
	}
	return false;
}

bool IntersectsGravityZone(out Zone GZ){
	if(InGravityZone(Controller.GetPosition(),out GZ))
		return true;
	if(InGravityZone(Pseudo_Target,out GZ))
		return true;
	return IntersectsGravityZone(3,Controller.GetPosition(),Pseudo_Target,out GZ);
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

bool Broadcast(string Command,string Subdata){
	IGC.SendBroadcastMessage("AutoMiner",Command+":"+Subdata,TransmissionDistance.TransmissionDistanceMax);
	return true;
}

void SendUpdate(bool UpdateSectors=true){
	if(UpdateSectors){
		foreach(Sector S in Sectors)
			Broadcast("Sector",S.ToString());
	}
	foreach(Zone Z in Zones){
		Broadcast("Zone",Z.ToString());
	}
	if(Asteroid!=null)
		Broadcast("Asteroid",Asteroid.ToString());
}

int GetUpdates(){
	int count=0;
	List<IMyBroadcastListener> listeners=new List<IMyBroadcastListener>();
	IGC.GetBroadcastListeners(listeners);
	foreach(IMyBroadcastListener Listener in listeners){
		while(Listener.HasPendingMessage){
			MyIGCMessage message=Listener.AcceptMessage();
			count++;
			string Data=message.Data.ToString();
			int index=Data.IndexOf(":");
			if(index!=-1){
				string Command=Data.Substring(0,index);
				string Subdata=Data.Substring(index+1);
				if(Command.Equals("Sector")){
					Sector S=null;
					if(Sector.TryParse(Subdata,out S))
						UpdateSectors(S);
				}
				else if(Command.Equals("Zone")){
					Zone Z=null;
					if(Zone.TryParse(Subdata,out Z)){
						bool has=false;
						for(int i=0;i<Zones.Count;i++){
							if((Zones[i].Center-Z.Center).Length()<5){
								has=true;
								Zones[i].Radius=Math.Max(Zones[i].Radius,Z.Radius);
								break;
							}
						}
						if(!has)
							Zones.Add(Z);
					}
				}
				else if(Command.Contains("Ast-")&&Asteroid!=null){
					TerrainPoint P=null;
					if(TerrainPoint.TryParse(Subdata,out P)){
						if((P.Point-Asteroid.Center).Length()<2500){
							if(Command.Equals("Ast-Add"))
								Asteroid.Add(P);
							else if(Command.Equals("Ast-Rem"))
								Asteroid.Remove(P);
						}
					}
				}
				else if(Command.Equals("Asteroid")&&Asteroid==null){
					TerrainMap T=null;
					if(TerrainMap.TryParse(Subdata,out T))
						Asteroid=T;
				}
				else if(Command.Equals("AutoUndock")&&Listener.Tag.Equals("AutoMiner Base AI")){
					if(Me.CustomData.Length==0)
						bool.TryParse(Subdata,out AutoUndock);
				}
				else if(Command.Equals("Return")){
					EndTask();
					Tasks.Clear();
					AutoUndock=false;
					Tasks.Push(DroneTask.Returning);
				}
			}
		}
	}
	return count;
}

Sector FindOneSector(Vector3D starting_point,Vector3D current_point,int depth){
	Vector3D attempt=Sector.GetStart(current_point);
	string dep="  ";
	for(int i=0;i<depth;i++)
		dep+=" ";
	Write(dep+"FindOneSector");
	bool found=false;
	foreach(Zone Z in Zones){
		if(Z.Overlaps(attempt)){
			Write(dep+" Zone Overlap");
			return null;
		}
	}
	bool incomplete=false;
	for(int i=0;i<Sectors.Count;i++){
		Sector S=Sectors[i];
		if(S.Same(attempt)){
			found=true;
			incomplete=!S.Complete;
			if(incomplete)
				Last_Sector=i;
			else
				Write(dep+" Completed");
			break;
		}
	}
	if((!found)||incomplete){
		Sector output=new Sector(current_point);
		if(!found){
			Sectors.Add(output);
			Last_Sector=Sectors.Count-1;
		}
		Write(dep+" Valid");
		return output;
	}
	return null;
}

List<Sector> FindSector(int distance_goal,Vector3D starting_point,Vector3D current_point,int depth=0){
	string dep="";
	for(int i=0;i<depth;i++)
		dep+=" ";
	Write(dep+"Start FindSector("+distance_goal.ToString()+","+depth.ToString()+")");
	List<Sector> output=new List<Sector>();
	if(distance_goal>17)
		return output;
	if((current_point-MyDock.Return).Length()>87500)
		return output;
	if(distance_goal==0){
		Sector attempt=FindOneSector(starting_point,current_point,depth);
		if(attempt!=null)
			output.Add(attempt);
		Write(dep+"End FindSector("+distance_goal.ToString()+","+depth.ToString()+")");
		return output;
	}
	for(int i=0;i<6;i++){
		Vector3D Tweak=new Vector3D(0,0,0);
		switch(i){
			case 0:
				Tweak.X=-1;
				break;
			case 1:
				Tweak.X=1;
				break;
			case 2:
				Tweak.Y=-1;
				break;
			case 3:
				Tweak.Y=1;
				break;
			case 4:
				Tweak.Z=-1;
				break;
			case 5:
				Tweak.Z=1;
				break;
		}
		Tweak*=5000;
		if((Sector.GetStart(current_point+Tweak)-starting_point).Length()>(Sector.GetStart(current_point)-starting_point).Length()){
			List<Sector> input=FindSector(distance_goal-1,starting_point,current_point+Tweak,depth+1);
			foreach(Sector S in input){
				if(S!=null)
					output.Add(S);
			}
		}
	}
	Write(dep+"End FindSector("+distance_goal.ToString()+","+depth.ToString()+")");
	return output;
}

Sector NextSector(){
	Vector3D Coords_Start=MyDock.Return;
	if(Last_Sector!=-1){
		if(!Sectors[Last_Sector].Complete)
			return Sectors[Last_Sector];
		//Coords_Start=(Sectors[Last_Sector].Center-MyDock.Return)/2+MyDock.Return;
	}
	for(int i=0;i<Sectors.Count;i++){
		if(!Sectors[i].Complete){
			Last_Sector=i;
			return Sectors[i];
		}
	}
	int distance_count=0;
	List<Sector> output=new List<Sector>();
	do{
		List<Sector> input=FindSector(distance_count++,Coords_Start,Coords_Start);
		foreach(Sector S in input)
			output.Add(S);
	}
	while(output.Count==0&&distance_count<=17);
	double min=double.MaxValue;
	foreach(Sector S in output){
		if(S!=null)
			min=Math.Min(min,(S.Center-MyDock.Return).Length());
	}
	foreach(Sector S in output){
		if(S!=null&&(S.Center-MyDock.Return).Length()<min+0.1)
			return S;
	}
	return null;
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
			foreach(IMyConveyorSorter S in Sorters){
				S.Enabled=false;
				S.SetFilter(MyConveyorSorterMode.Whitelist,new List<MyInventoryItemFilter>(){new MyInventoryItemFilter("MyObjectBuilder_Ore/Stone",false)});
			}
			foreach(IMyShipConnector Connector in Connectors)
				Connector.Disconnect();
			break;
		case DroneTask.Docking:
			Match_Position=false;
			Match_Direction=false;
			break;
		case DroneTask.Charging:
			Match_Position=false;
			Match_Direction=false;
			break;
		case DroneTask.Returning:
			Match_Position=false;
			Match_Direction=false;
			Controller.ClearWaypoints();
			Controller.SetAutoPilotEnabled(false);
			break;
		case DroneTask.Traveling:
			Controller.ClearWaypoints();
			Controller.SetAutoPilotEnabled(false);
			break;
		case DroneTask.Exploring:
			Match_Position=false;
			Match_Direction=false;
			Speed_Limit=100;
			Slow_Down=true;
			break;
		case DroneTask.Scanning:
			Match_Position=false;
			Match_Direction=false;
			last_scan_index=0;
			break;
		case DroneTask.Ejecting:
			Match_Position=false;
			Match_Direction=false;
			foreach(IMyConveyorSorter S in Sorters)
				S.Enabled=false;
			foreach(IMyShipConnector Connector in Connectors)
				Connector.ThrowOut=false;
			break;
		case DroneTask.Mining:
			foreach(IMyShipDrill D in Drills)
				D.Enabled=false;
			Match_Position=false;
			Match_Direction=false;
			RestingVelocity=new Vector3D(0,0,0);
			SetRange(50);
			break;
	}
}

void Docked(){
	if(MyDock==null){
		EndTask();
		return;
	}
	foreach(IMyConveyorSorter S in Sorters){
		if(S.CustomName.Contains("Back")){
			S.Enabled=true;
			S.DrainAll=true;
			S.SetFilter(MyConveyorSorterMode.Whitelist,new List<MyInventoryItemFilter>(){new MyInventoryItemFilter("MyObjectBuilder_Ore/(null)",true)});
		}
	}
	bool Continue=!AutoUndock;
	foreach(IMyShipConnector Connector in Connectors){
		Connector.ThrowOut=false;
		if(!Continue)
			Continue=Connector.GetInventory().CurrentVolume>0;
	}
	foreach(IMyBatteryBlock B in Batteries){
		B.ChargeMode=ChargeMode.Recharge;
		if(!Continue)
			Continue=B.CurrentStoredPower<B.MaxStoredPower;
	}
	if(!Continue){
		foreach(IMyCargoContainer C in Cargo){
			if(C.GetInventory().CurrentVolume>0){
				Continue=true;
				break;
			}
		}
	}
	if(!Continue)
		EndTask();
	Runtime.UpdateFrequency=UpdateFrequency.Update100;
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
	Speed_Limit=2.5;
	Vector3D angle=Controller.GetPosition()-MyDock.Position;
	angle.Normalize();
	if((Controller.GetPosition()-MyDock.Position).Length()<12&&GetAngle(MyDock.Orientation,angle)<5){
		Target_Position=Controller.GetPosition()-Docking_Connector.GetPosition();
		Target_Position=MyDock.Orientation*1.5+MyDock.Position+Target_Position;
		Speed_Limit=1;
	}
	if(Docking_Connector.Status!=MyShipConnectorStatus.Unconnected)
		Docking_Connector.Connect();
	if(Docking_Connector.Status==MyShipConnectorStatus.Connected){
		EndTask();
		Tasks.Push(DroneTask.Docked);
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

void Charging(){
	Match_Position=false;
	if(Asteroid!=null){
		if((Controller.GetPosition()-Asteroid.Center).Length()<Asteroid.Size+200){
			Match_Position=true;
			Speed_Limit=20;
			Target_Position=Controller.GetPosition()-Asteroid.Center;
			Target_Position.Normalize();
			Target_Position=(Asteroid.Size+400)*Target_Position+Asteroid.Center;
		}
	}
	if(!Match_Position){
		Match_Direction=true;
		Target_Direction=new Vector3D(0,1,0);
	}
	if(Charge>0.75f)
		EndTask();
	if(Match_Position)
		Runtime.UpdateFrequency=UpdateFrequency.Update1;
	else if(Match_Direction)
		Runtime.UpdateFrequency=UpdateFrequency.Update10;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

void Returning(){
	Match_Position=false;
	if(MyDock==null){
		EndTask();
		return;
	}
	if(Asteroid!=null){
		if((Controller.GetPosition()-Asteroid.Center).Length()<Asteroid.Size){
			Match_Position=true;
			Speed_Limit=20;
			Target_Position=Controller.GetPosition()-Asteroid.Center;
			Target_Position.Normalize();
			Target_Position=Target_Position*(10+Asteroid.Size)+Asteroid.Center;
		}
	}
	if(!Match_Position){
		MyWaypointInfo Destination=new MyWaypointInfo("Return to Base",MyDock.Return);
		if((!Controller.CurrentWaypoint.Equals(Destination))||!Controller.IsAutoPilotEnabled){
			Controller.ClearWaypoints();
			Controller.AddWaypoint(Destination);
			Controller.SetCollisionAvoidance(true);
			Speed_Limit=Math.Max(5,Math.Min(100,Distance_To_Base/15));
			Controller.SpeedLimit=(float)Speed_Limit;
			Controller.SetAutoPilotEnabled(true);
		}
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

void Traveling(){
	MyWaypointInfo Destination=new MyWaypointInfo("Base",MyDock.Return);
	if(Asteroid!=null){
		Target_Position=Controller.GetPosition()-Asteroid.Center;
		Target_Position.Normalize();
		Target_Position=(200+Asteroid.Size)*Target_Position+Asteroid.Center;
		Destination=new MyWaypointInfo("Traveling to Asteroid",Target_Position);
	}
	else{
		Sector S=NextSector();
		if(S!=null){
			double distance=double.MaxValue;
			for(int i=0;i<4;i++)
				distance=Math.Min(distance,(Controller.GetPosition()-S.Corners[i]).Length());
			for(int i=0;i<4;i++){
				if(distance>(Controller.GetPosition()-S.Corners[i]).Length()-.1){
					Destination=new MyWaypointInfo("Traveling to Sector",S.Corners[i]);
					Write("Traveling: "+S.NiceString(MyDock.Return));
					break;
				}
			}
		}
	}
	if(Destination.Name.Equals("Base")){
		AutoUndock=false;
		EndTask();
		Tasks.Push(DroneTask.Returning);
		Runtime.UpdateFrequency=UpdateFrequency.Update1;
		return;
	}
	if((!Controller.CurrentWaypoint.Equals(Destination))||!Controller.IsAutoPilotEnabled){
		Controller.ClearWaypoints();
		Controller.AddWaypoint(Destination);
		Controller.SetCollisionAvoidance(true);
		Speed_Limit=Math.Min(100,(Controller.GetPosition()-Destination.Coords).Length()/15);
		Controller.SpeedLimit=(float)Speed_Limit;
		Controller.SetAutoPilotEnabled(true);
	}
	if((Controller.GetPosition()-Destination.Coords).Length()<2.5){
		EndTask();
		if(Asteroid!=null){
			if(Tasks.Peek()!=DroneTask.Scanning)
				Tasks.Push(DroneTask.Scanning);
		} 
		else{
			if(Tasks.Peek()!=DroneTask.Exploring)
				Tasks.Push(DroneTask.Exploring);
		}
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update10;
}

bool RaycastCheck(MyDetectedEntityInfo e){
	return e.Type==MyDetectedEntityType.Asteroid;
}
void Exploring(){
	Sector S=NextSector();
	if(S==null){
		EndTask();
		return;
	}
	Write("Exploring: "+S.NiceString(MyDock.Return));
	bool incomplete=false;
	Vector3D Start_Position=new Vector3D(0,0,0);
	Vector3D End_Position=new Vector3D(0,0,0);
	double min_distance=double.MaxValue;
	for(int i=0;i<S.subsections.Length;i++){
		if(!S.subsections[i]){
			incomplete=true;
			int x=i%25;
			int z=i/25;
			Target_Position=S.Corners[0]+new Vector3D(x*200,0,z*200);
			min_distance=Math.Min(min_distance,(Controller.GetPosition()-Target_Position).Length());
		}
	}
	for(int i=0;i<S.subsections.Length;i++){
		if(!S.subsections[i]){
			incomplete=true;
			int x=i%25;
			int z=i/25;
			Target_Position=S.Corners[0]+new Vector3D(x*200,0,z*200);
			if((Controller.GetPosition()-Target_Position).Length()<min_distance+0.1){
				Write("Next Section: subsections["+i+"]");
				break;
			}
		}
	}
	Speed_Limit=34;
	Match_Position=false;
	Slow_Down=true;
	if(incomplete&&Asteroid==null){
		if((Controller.GetPosition()-Target_Position).Length()>7500){
			EndTask(false);
			Tasks.Push(DroneTask.Traveling);
		}
		Match_Position=true;
		Slow_Down=false;
		Write("Target: "+Math.Round((Controller.GetPosition()-Target_Position).Length(),0).ToString()+"m");
		Match_Direction=true;
		Target_Direction=new Vector3D(0,1,0);
		if(GetAngle(Forward_Vector,Target_Direction)<1){
			MyDetectedEntityInfo A=new MyDetectedEntityInfo(-1,"null",MyDetectedEntityType.None,null,new MatrixD(0,0,0,0,0,0,0,0,0),new Vector3D(0,0,0),MyRelationsBetweenPlayerAndBlock.NoOwnership,new BoundingBoxD(new Vector3D(0,0,0),new Vector3D(0,0,0)),0);
			for(int i=0;i<S.subsections.Length;i++){
				if(!S.subsections[i]){
					int x=i%25;
					int z=i/25;
					Vector3D Coordinates=S.Corners[0]+(new Vector3D(x*200,0,z*200));
					double distance=(Controller.GetPosition()-Coordinates).Length();
					if(distance<250)
						Write("Subsections["+i+"]:"+Math.Round((Controller.GetPosition()-Coordinates).Length(),0)+"m");
					if(distance<5){
						MyDetectedEntityInfo O;
						Vector3D Target=Coordinates+(new Vector3D(0,5000,0));
						O=Forward_Camera.Raycast(Target);
						if(RaycastCheck(O))
							A=O;
						O=Top_Camera.Raycast(Target+(new Vector3D(0,0,200)));
						if(RaycastCheck(O))
							A=O;
						O=Bottom_Camera.Raycast(Target+(new Vector3D(0,0,-200)));
						if(RaycastCheck(O))
							A=O;
						O=Left_Camera.Raycast(Target+(new Vector3D(-200,0,0)));
						if(RaycastCheck(O))
							A=O;
						O=Right_Camera.Raycast(Target+(new Vector3D(200,0,0)));
						if(RaycastCheck(O))
							A=O;
						S.subsections[i]=true;
					}
					if(RaycastCheck(A))
						break;
				}
			}
			if(RaycastCheck(A)){
				bool valid=true;
				foreach(Zone z in Zones){
					if((A.Position-z.Center).Length()<z.Radius+400){
						valid=false;
						break;
					}
				}
				if(valid){
					Asteroid=new TerrainMap(A.Position);
					if(A.HitPosition!=null)
						Asteroid.Add((Vector3D)A.HitPosition);
				}
				for(int i=0;i<S.subsections.Length;i++)
					S.subsections[i]=true;
			}
		}
	}
	if(Asteroid!=null){
		EndTask();
		Tasks.Push(DroneTask.Scanning);
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

int last_scan_index=0;
void Scanning(){
	//TEST CODE
	EndTask();
	Tasks.Push(DroneTask.Returning);
	//TEST CODE
	
	Match_Direction=true;
	Target_Direction=Asteroid.Center-Controller.GetPosition();
	Target_Direction.Normalize();
	int count=0;
	bool found_spot=false;
	int index=last_scan_index;
	while(index<Asteroid.Points.Count&&count<100000){
		int neighbors=Asteroid.CountNeighbors(Asteroid.Points[index]);
		count+=Asteroid.Points.Count;
		if(neighbors<4){
			found_spot=true;
			last_scan_index=0;
			break;
		}
		index++;
	}
	if(found_spot){
		Target_Position=Asteroid.Points[index].Point-Asteroid.Center;
		double distance=Target_Position.Length();
		Target_Position.Normalize();
		Target_Position=(50+distance)*Target_Position+Asteroid.Center;
		if((Controller.GetPosition()-Target_Position).Length()<2.5&&GetAngle(Forward_Vector,Target_Direction)<1){
			List<TerrainPoint> Neighbors=Asteroid.GetNeighbors(Asteroid.Points[index]);
			List<Vector3D> Targets=new List<Vector3D>();
			for(int i=0;i<4;i++){
				Vector3D Tweak=new Vector3D(0,0,0);
				switch(i){
					case 0:
						Tweak+=Up_Vector;
						break;
					case 1:
						Tweak+=Down_Vector;
						break;
					case 2:
						Tweak+=new Vector3D(-1,0,0);
						break;
					case 3:
						Tweak+=new Vector3D(1,0,0);
						break;
				}
				Tweak*=5;
				Targets.Add(Asteroid.Points[index].Point+Tweak);
			}
			foreach(TerrainPoint P in Neighbors){
				for(int i=0;i<Targets.Count;i++){
					if(Math.Min((P.Point-Targets[i]).Length(),Asteroid.Distance(P.Point,Targets[i]))<=2.5)
						Targets.RemoveAt(i--);
				}
			}
			int target_index=0;
			foreach(IMyCameraBlock Camera in All_Cameras){
				MyDetectedEntityInfo A;
				Vector3D Target;
				if(Camera==Forward_Camera)
					Target=Asteroid.Points[index].Point;
				else
					Target=Targets[target_index++];
				Target=(Target-Camera.GetPosition());
				distance=Target.Length();
				Target.Normalize();
				Target=Target*(distance+25)+Camera.GetPosition();
				A=Camera.Raycast(Target);
				if(RaycastCheck(A)&&A.HitPosition!=null)
					Asteroid.Add((Vector3D)A.HitPosition);
			}
		}
	}
	else if(Asteroid.Points.Count==0){
		MyDetectedEntityInfo A=Forward_Camera.Raycast(Asteroid.Center);
		if(RaycastCheck(A)&&A.HitPosition!=null)
			Asteroid.Add((Vector3D)A.HitPosition);
	}
	else if(index>=Asteroid.Points.Count){
		EndTask();
		Tasks.Push(DroneTask.Mining);
	}
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

void Ejecting(){
	Match_Position=false;
	if(Asteroid!=null){
		Target_Direction=Asteroid.Center-Controller.GetPosition();
		Target_Direction.Normalize();
		if((Controller.GetPosition()-Asteroid.Center).Length()<Asteroid.Size+150){
			Match_Position=true;
			Speed_Limit=20;
			Target_Position=Controller.GetPosition()-Asteroid.Center;
			Target_Position.Normalize();
			Target_Position=(Asteroid.Size+200)*Target_Position+Asteroid.Center;
		}
	}
	Match_Direction=Match_Position;
	if(!Match_Position){
		foreach(IMyConveyorSorter S in Sorters){
			S.Enabled=false;
			S.SetFilter(MyConveyorSorterMode.Whitelist,new List<MyInventoryItemFilter>(){new MyInventoryItemFilter("MyObjectBuilder_Ore/Stone",false)});
			S.DrainAll=true;
		}
		bool has_stone=false;
		foreach(IMyShipConnector Connector in Connectors){
			Connector.ThrowOut=true;
			if(!has_stone)
				has_stone=Connector.GetInventory().GetItemAmount(new MyItemType("MyObjectBuilder_Ore","Stone")).ToIntSafe()>0;
		}
		if(!has_stone){
			foreach(IMyCargoContainer C in Cargo){
				if(C.GetInventory().GetItemAmount(new MyItemType("MyObjectBuilder_Ore","Stone")).ToIntSafe()>0){
					has_stone=true;
					break;
				}
			}
		}
		if(!has_stone){
			foreach(IMyShipDrill D in Drills){
				if(D.GetInventory().GetItemAmount(new MyItemType("MyObjectBuilder_Ore","Stone")).ToIntSafe()>0){
					has_stone=true;
					break;
				}
			}
		}
		if(!has_stone)
			EndTask();
	}
	if(Match_Position)
		Runtime.UpdateFrequency=UpdateFrequency.Update1;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

void SetRange(float range){
	Sensor.LeftExtend=range;
	Sensor.RightExtend=range;
	Sensor.BottomExtend=range;
	Sensor.TopExtend=range;
	Sensor.FrontExtend=range;
	Sensor.BackExtend=range;
	Sensor.DetectAsteroids=true;
}
void Mining(){
	SetRange(50);
	if(Asteroid==null){
		EndTask();
		return;
	}
	if(!Sensor.IsActive){
		if(Asteroid.RemoveAllInArea(Sensor.GetPosition(),50)>5){
			EndTask();
			Tasks.Push(DroneTask.Scanning);
		}
	}
	TerrainPoint P=Asteroid.GetOuterMost(Controller.GetPosition());
	if(P==null){
		EndTask();
		return;
	}
	float sum_all=0;
	float sum_stone=0;
	float max=0;
	foreach(IMyCargoContainer C in Cargo){
		sum_all+=C.GetInventory().CurrentVolume.ToIntSafe();
		max+=C.GetInventory().MaxVolume.ToIntSafe();
		sum_stone+=0.00037f*C.GetInventory().GetItemAmount(new MyItemType("MyObjectBuilder_Ore","Stone")).ToIntSafe();
	}
	foreach(IMyShipConnector C in Connectors){
		sum_all+=C.GetInventory().CurrentVolume.ToIntSafe();
		max+=C.GetInventory().MaxVolume.ToIntSafe();
		sum_stone+=0.00037f*C.GetInventory().GetItemAmount(new MyItemType("MyObjectBuilder_Ore","Stone")).ToIntSafe();
	}
	float sum_ore=sum_all-sum_stone;
	float stone_percent=sum_stone/(max-sum_ore);
	float ore_percent=sum_ore/max;
	
	Match_Direction=true;
	Target_Direction=Asteroid.Center-Controller.GetPosition();
	Target_Direction.Normalize();
	Match_Position=false;
	bool drill=false;
	RestingVelocity=new Vector3D(0,0,0);
	Speed_Limit=15;
	if(stone_percent>0.75f||(ore_percent>0.9f&&stone_percent>0)){
		EndTask(false);
		Tasks.Push(DroneTask.Ejecting);
	}
	else if(ore_percent>0.9f){
		EndTask(false);
		Tasks.Push(DroneTask.Returning);
	}
	else{
		Vector3D direction=P.Point-Asteroid.Center;
		double distance=direction.Length();
		direction.Normalize();
		if(GetAngle(direction,-1*Target_Direction)<1&&Asteroid.Distance(Controller.GetPosition(),P.Point)<2.5){
			if((Controller.GetPosition()-Asteroid.Center).Length()<distance-5){
				Asteroid.Remove(P);
				MyDetectedEntityInfo A=Forward_Camera.Raycast(distance,-1*direction);
				if(RaycastCheck(A)&&A.HitPosition!=null)
					Asteroid.Add((Vector3D)A.HitPosition);
			}
			else{
				drill=true;
				RestingVelocity=Target_Direction*0.25;
			}
		}
		else{
			SetRange(5);
			Sensor.FrontExtend=1.25f;
			Match_Position=true;
			if(Sensor.IsActive){
				Speed_Limit=5;
				Target_Position=Controller.GetPosition()-Asteroid.Center;
				distance=Target_Position.Length();
				Target_Position.Normalize();
				Target_Position=(distance+20)*Target_Position+Asteroid.Center;
			}
			else
				Target_Position=direction*(distance+25)+Asteroid.Center;
		}
	}
	foreach(IMyShipDrill D in Drills)
		D.Enabled=drill;
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
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
		if(Math.Abs(difference)>.1)
			input_roll+=(float)Math.Min(Math.Max(difference/5,-4),1)*gyro_multx;
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
	if(Speed_Limit!=1&&Speed_Limit!=2.5)
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
	seconds_since_last_update=Runtime.TimeSinceLastRun.TotalSeconds+(Runtime.LastRunTimeMs/1000);
	Echo(ToString(FromSeconds(seconds_since_last_update))+" since last cycle");
	Time_Since_Start=UpdateTimeSpan(Time_Since_Start,seconds_since_last_update);
	Echo(ToString(Time_Since_Start)+" since last reboot\n");
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
	if(Asteroid!=null)
		Asteroid.UpdateAges(seconds_since_last_update);
	LinearVelocity=Controller.GetShipVelocities().LinearVelocity;
	AngularVelocity=Controller.GetShipVelocities().AngularVelocity;
	if(Match_Position){
		Zone GZ;
		if(IntersectsGravityZone(out GZ)){
			Vector3D Direction=(Target_Position-Controller.GetPosition());
			Direction.Normalize();
			Pseudo_Target=Controller.GetPosition()+Direction*5000;
			if(InGravityZone(Pseudo_Target,out GZ)){
				Direction=Pseudo_Target-GZ.Center;
				Direction.Normalize();
				Pseudo_Target=GZ.Center+Direction*GZ.Radius;
			}
		}
		else
			Pseudo_Target=Target_Position;
	}
}

bool ProcessArgument(string argument){
	LastArgument=argument;
	if(argument.ToLower().IndexOf("dock:")==0){
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
		MyDock=new Dock(p,o,r);
		Vector3D u;
		if(args.Length==4&&Vector3D.TryParse(args[3],out u)){
			MyDock.Up=u;
			MyDock.DoUp=true;
		}
		return true;
	}
	else if(argument.ToLower().Equals("autoundock")){
		AutoUndock=!AutoUndock;
		return true;
	}
	else if(argument.ToLower().IndexOf("zone:")==0){
		Vector3D c;
		double r;
		string[] args=argument.Substring(5).Split('•');
		if(args.Length!=2)
			return false;
		if(!Vector3D.TryParse(args[0],out c))
			return false;
		if(!double.TryParse(args[1],out r))
			return false;
		Zone output=new Zone(c,r);
		output.Outpost=true;
		Zones.Add(output);
		return true;
	}
	else if(argument.ToLower().Equals("return")){
		EndTask();
		Tasks.Clear();
		Tasks.Push(DroneTask.Returning);
		return true;
	}
	else if(argument.ToLower().Equals("wipe")){
		Zones.Clear();
		Sectors.Clear();
		return true;
	}
	else if(argument.ToLower().IndexOf("goto:")==0){
		try{
			string str=argument.Substring(5).Trim();
			bool found=Vector3D.TryParse(str,out Target_Position);
			if(!found){
				MyWaypointInfo temp;
				found=MyWaypointInfo.TryParse(str,out temp);
				if(!found)
					found=MyWaypointInfo.TryParse(str.Substring(0,str.Length-10),out temp);
				if(found)
					Target_Position=temp.Coords;
			}
			if(found){
				Match_Position=true;
				return true;
			}
			else
				ArgumentError_Message=str;
		} 
		catch(Exception){
			ArgumentError_Message=argument.Substring(5).Trim();
			return false;
		}
	}
	else if(argument.ToLower().Equals("stop")){
		Match_Position=false;
		Match_Direction=false;
		return true;
	}
	else if(argument.ToLower().Equals("factory reset")){
		this.Storage="";
		Me.CustomData="";
		Runtime.UpdateFrequency=UpdateFrequency.None;
		Me.Enabled=false;
		Factory_Reset=true;
		return true;
	}
	return false;
}

bool ArgumentError=false;
string ArgumentError_Message="";
string LastArgument="";
bool Sent_Update=true;
int update_count=0;
public void Main(string argument, UpdateType updateSource)
{
	TerrainMap.Update_Size=true;
	UpdateProgramInfo();
	UpdateSystemInfo();
	try{
		if(Last_Sector>=Sectors.Count)
			Last_Sector=-1;
		if(Tasks.Count==0)
			Tasks.Push(DroneTask.None);
		if(MyDock!=null){
			if(Cycle_Time+(Controller.GetPosition()-MyDock.Return).Length()/95+120>=10800){
				AutoUndock=false;
				Write("Returning to Base; Drone Shift");
				if((int)Tasks.Peek()>(int)DroneTask.Returning){
					EndTask(false);
					Tasks.Push(DroneTask.Returning);
				}
			}
			else if((int)Tasks.Peek()>(int)DroneTask.Returning){
				double Time_To_Return=(Controller.GetPosition()-MyDock.Return).Length()/95+120;
				Time_To_Return=Math.Max(0,10800-Time_To_Return-Cycle_Time);
				if(Time_To_Return>60)
					Write("Time to Return: "+Math.Round(Time_To_Return/60,1)+" minutes");
				else
					Write("Time to Return: "+Math.Round(Time_To_Return,0)+" seconds");
			}
			if(Distance_To_Base<5000)
				Antenna.Radius=(float)Distance_To_Base+500;
			else if(Cycle_Time%600<=10){
				Write("Broadcasting Data");
				if(Cycle_Time%600<5){
					Sent_Update=true;
					Antenna.Radius=50000;
				}
				else if(Sent_Update){
					SendUpdate();
					Sent_Update=false;
				}
			}
			else
				Antenna.Radius=7500;
		}
		else
			Write("No Dock on Record");
		if(Controller.GetNaturalGravity().Length()>0){
			Vector3D Center;
			if(Controller.TryGetPlanetPosition(out Center)){
				double Radius=(Controller.GetPosition()-Center).Length()+1000;
				bool has=false;
				for(int i=0;i<Zones.Count;i++){
					if(Zones[i].Gravity&&(Zones[i].Center-Center).Length()<1){
						has=true;
						Zones[i].Radius=Math.Max(Zones[i].Radius,Radius);
						break;
					}
				}
				if(!has){
					Zone z=new Zone(Center,Radius);
					z.Gravity=true;
					Zones.Add(z);
				}
			}
		}
		else if((int)Tasks.Peek()>(int)DroneTask.Charging&&Charge<0.25f){
			if(Distance_To_Base<2500){
				if((int)Tasks.Peek()>(int)DroneTask.Returning){
					EndTask(false);
					Tasks.Push(DroneTask.Returning);
				}
			}
			else{
				EndTask(false);
				Tasks.Push(DroneTask.Charging);
			}
		}
		Write("Drone Shift Timer: "+Math.Round(Cycle_Time/60,2).ToString()+"/180 minutes");
		if(cycle%10==0)
			update_count=GetUpdates();
		Write("Received "+update_count.ToString()+" updates ("+(cycle%10).ToString()+"/10 cycles ago)");
		if((600-Cycle_Time%600)/60>1)
			Write("Next Long-Range Update in "+Math.Round((600-Cycle_Time%600)/60,1)+" minutes");
		else
			Write("Next Long-Range Update in "+Math.Round((600-Cycle_Time%600),0)+" seconds");
		Write("Batteries are "+Math.Round(Charge*100,1)+"% charged");
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
		for(int i=1;i<4;i++){
			FindSector(i,MyDock.Return,MyDock.Return);
		}
		Write("AutoUndock:"+AutoUndock.ToString());
		Write(Zones.Count+" Zones");
		Write(Sectors.Count+" Sectors");
		if(Asteroid==null){
			Write((Last_Sector+1).ToString()+" / "+Sectors.Count.ToString());
			if(Last_Sector>=0&&Last_Sector<Sectors.Count){
				Write("Last_Sector: "+Sectors[Last_Sector].NiceString(MyDock.Return));
			}
		}
		else
			Write("Asteroid: ("+Math.Round((Controller.GetPosition()-Asteroid.Center).Length()/1000,1).ToString()+"kM)");
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
					Tasks.Push(DroneTask.Exploring);
				}
				break;
			case DroneTask.Docked:
				Docked();
				break;
			case DroneTask.Docking:
				Docking();
				break;
			case DroneTask.Charging:
				Returning();
				break;
			case DroneTask.Returning:
				Returning();
				break;
			case DroneTask.Traveling:
				Traveling();
				break;
			case DroneTask.Exploring:
				Exploring();
				break;
			case DroneTask.Scanning:
				Scanning();
				break;
			case DroneTask.Ejecting:
				Ejecting();
				break;
			case DroneTask.Mining:
				Mining();
				break;
		}
		SetGyroscopes();
		SetThrusters();
	}
	catch (Exception e){
		Tasks.Clear();
		AutoUndock=false;
		if(Distance_To_Base>500)
			Tasks.Push(DroneTask.Returning);
		Me.CustomData+="\nFatal Error Occurred:\n"+e.ToString();
	}
	if(Me.CustomData.Length>0){
		Echo("Previous Errors:");
		string[] args=Me.CustomData.Split('\n');
		foreach(string arg in args)
			Echo("  "+arg+'\n');
		Echo("End Errors");
	}
}
