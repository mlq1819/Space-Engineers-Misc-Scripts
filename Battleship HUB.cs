const string Program_Name = "Battleship HUB AI"; //Name me!
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
	public static void Write(string text,bool new_line=true,bool append=true){
		P.Echo(text);
		if(new_line)
			P.Me.GetSurface(0).WriteText(text+'\n', append);
		else
			P.Me.GetSurface(0).WriteText(text, append);
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
class RealShip{
	public int ID;
	public ShipStatus Status;
	public int Player_Num;
	public MyShip Type;
	public double Timer;
	public string Tag{
		get{
			return Type.ToString()+" "+Player_Num.ToString();
		}
	}
	public string Tag_Full{
		get{
			return Tag+"-"+ID.ToString();
		}
	}
	public IMyLaserAntenna Antenna;
	public Vector3D End1;
	public Vector3D End2;
	
	public RealShip(int id,MyShip type,int player_num,ShipStatus status=ShipStatus.Linking,double timer=0){
		ID=id;
		Type=type;
		Player_Num=player_num;
		Status=status;
		Timer=timer;
		Antenna=null;
		End1=new Vector3D(0,0,0);
		End2=new Vector3D(0,0,0);
	}
	
	public override string ToString(){
		return "("+ID.ToString()+","+((int)Type).ToString()+","+Player_Num.ToString()+","+((int)Status).ToString()+","+Timer.ToString()+","+End1.ToString()+","+End2.ToString()+")";
	}
	
	public static bool TryParse(string Parse,out RealShip output){
		output=null;
		if(Parse[0]!='('||Parse[Parse.Length-1]!=')')
			return false;
		string[] args=Parse.Substring(1,Parse.Length-2).Split(',');
		if(args.Length!=7)
			return false;
		int id,type_i,player_num,status_i;
		double timer;
		if(!Int32.TryParse(args[0],out id))
			return false;
		if(!Int32.TryParse(args[1],out type_i))
			return false;
		if(type_i<1||type_i>5)
			return false;
		if(!Int32.TryParse(args[2],out player_num))
			return false;
		if(player_num<1||player_num>2)
			return false;
		if(!Int32.TryParse(args[3],out status_i))
			return false;
		if(status_i<-1||status_i>7)
			return false;
		if(!double.TryParse(args[4],out timer))
			return false;
		Vector3D e1,e2;
		if(!Vector3D.TryParse(args[5],out e1))
			return false;
		if(!Vector3D.TryParse(args[6],out e2))
			return false;
		output=new RealShip(id,(MyShip)type_i,player_num,(ShipStatus)status_i,timer);
		output.End1=e1;
		output.End2=e2;
		return true;
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
				panel.ChangeInterval=0.5f;
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
	SettingUp=3,
	InProgress=4,
	Firing=5,
	Paused=6
}

enum MyShip{
	Unknown=-1,
	None=0,
	Carrier=1,
	Frigate=2,
	Cruiser=3,
	Prowler=4,
	Destroyer=5
}

int ShipSize(MyShip ship){
	return Prog.ShipSize(ship);
}

class GridSpace{
	public MyShip Ship; 
	public bool Hit;
	
	public GridSpace(MyShip ship){
		Ship=ship;
		Hit=false;
	}
	
	public GridSpace():this(MyShip.None){
		;
	}
	
	public override string ToString(){
		return "("+((int)Ship).ToString()+','+Hit.ToString()+")";
	}
	
	public static bool TryParse(string Parse,out GridSpace output){
		output=null;
		if(Parse.IndexOf("(")!=0||Parse.IndexOf(")")!=Parse.Length-1)
			return false;
		string[] args=Parse.Substring(1,Parse.Length-2).Split(',');
		if(args.Length!=2)
			return false;
		int i;
		bool b;
		if(!Int32.TryParse(args[0],out i))
			return false;
		if(i<-1||i>5)
			return false;
		if(!bool.TryParse(args[1],out b))
			return false;
		output=new GridSpace((MyShip)i);
		output.Hit=b;
		return true;
	}
}

class Board{
	public List<List<GridSpace>> Grid;
	public int CountShips(MyShip Type){
		int output=0;
		foreach(List<GridSpace> row in Grid){
			foreach(GridSpace cell in row){
				if(cell.Ship==Type)
					output++;
			}
		}
		return output;
	}
	
	public int RemainingSpaces{
		get{
			int count=0;
			foreach(List<GridSpace> Row in Grid){
				foreach(GridSpace Cell in Row){
					if(((int)Cell.Ship)>((int)MyShip.None)&&!Cell.Hit)
						count++;
				}
			}
			return count;
		}
	}
	public int RemainingHits(MyShip Type){
		int count=Prog.ShipSize(Type);
		foreach(List<GridSpace> Row in Grid){
			foreach(GridSpace Cell in Row){
				if(Cell.Ship==Type&&Cell.Hit)
					count--;
			}
		}
		return count;
	}
	
	Board(List<List<GridSpace>> grid){
		Grid=grid;
	}
	
	public Board(MyShip Default){
		Grid=new List<List<GridSpace>>();
		for(int i=0;i<8;i++){
			List<GridSpace> Row=new List<GridSpace>();
			for(int j=0;j<8;j++)
				Row.Add(new GridSpace(Default));
			Grid.Add(Row);
		}
	}
	
	public static bool InRange(int n){
		return n<8&&n>=0;
	}
	
	public bool AddShip(MyShip Type,int x1,int y1,int x2,int y2){
		if((!InRange(x1))||(!InRange(y1))||(!InRange(x2))||(!InRange(y2)))
			return false;
		if((x1!=x2)&&(y1!=y2))
			return false;
		if(Math.Abs(x1-x2)+Math.Abs(y1-y2)+1!=Prog.ShipSize(Type))
			return false;
		if(CountShips(Type)>0)
			return false;
		for(int i=Math.Min(y1,y2);i<=Math.Max(y1,y2);i++){
			for(int j=Math.Min(x1,x2);j<=Math.Max(x1,x2);j++){
				if(Grid[i][j].Ship!=MyShip.None)
					return false;
			}
		}
		int count=0;
		for(int i=Math.Min(y1,y2);i<=Math.Max(y1,y2);i++){
			for(int j=Math.Min(x1,x2);j<=Math.Max(x1,x2);j++){
				Grid[i][j].Ship=Type;
				count++;
			}
		}
		return count>0;
	}
	
	public override string ToString(){
		string output="[";
		foreach(List<GridSpace> Row in Grid){
			output+="[";
			foreach(GridSpace Cell in Row){
				output+=Cell.ToString()+";";
			}
			output=output.Substring(0,output.Length-1)+"] ";
		}
		output=output.Substring(0,output.Length-1)+"]";
		return output;
	}
	
	public static bool TryParse(string Parse,out Board output){
		output=null;
		if(Parse.IndexOf("[")!=0)
			return false;
		if(Parse[Parse.Length-1]!=']')
			return false;
		string[] args=Parse.Substring(1,Parse.Length-2).Split(' ');
		if(args.Length!=8)
			return false;
		List<List<GridSpace>> grid=new List<List<GridSpace>>();
		for(int i=0;i<args.Length;i++){
			args[i]=args[i].Trim();
			if(args[i].IndexOf('[')!=0||args[i][args[i].Length-1]!=']')
				return false;
			args[i]=args[i].Substring(1,args[i].Length-2);
			string[] strs=args[i].Split(';');
			if(strs.Length!=8)
				return false;
			List<GridSpace> row=new List<GridSpace>();
			for(int j=0;j<strs.Length;j++){
				GridSpace cell=null;
				if((!GridSpace.TryParse(strs[j],out cell))||cell==null)
					return false;
				row.Add(cell);
			}
			grid.Add(row);
		}
		output=new Board(grid);
		return true;
	}
	
	public int GetPossibilitiesForShip(MyShip Type,int X,int Y){
		int count=0;
		int number_hit=CountShips(Type);
		for(int i=0;i<Prog.ShipSize(Type);i++){
			bool possible=true;
			int number_found=0;
			for(int dy=0;dy<Prog.ShipSize(Type);dy++){
				int y=Y-i+dy;
				InstructionCount++;
				if(y<0||y>7){
					possible=false;
					break;
				}
				if(Grid[y][X].Ship!=Type&&Grid[y][X].Ship!=MyShip.Unknown){
					possible=false;
					break;
				}
				if(Grid[y][X].Ship==Type)
					number_found++;
			}
			if(number_hit>0&&number_found<number_hit)
				possible=false;
			if(possible){
				if(number_hit>0){
					count+=5;
					if(number_hit>1)
						count+=2;
					if(RemainingHits(Type)==1)
						count+=3;
				}
				else
					count++;
			}
		}
		for(int i=0;i<Prog.ShipSize(Type);i++){
			bool possible=true;
			int number_found=0;
			for(int dx=0;dx<Prog.ShipSize(Type);dx++){
				int x=X-i+dx;
				InstructionCount++;
				if(x<0||x>7){
					possible=false;
					break;
				}
				if(Grid[Y][x].Ship!=Type&&Grid[Y][x].Ship!=MyShip.Unknown){
					possible=false;
					break;
				}
				if(Grid[Y][x].Ship==Type)
					number_found++;
			}
			if(number_hit>0&&number_found<number_hit)
				possible=false;
			if(possible){
				if(number_hit>0){
					count+=2;
					if(number_hit>1)
						count+=3;
					if(RemainingHits(Type)==1)
						count+=5;
				}
				else
					count++;
			}
		}
		return count;
	}
	
	long InstructionCount=0;
	public int Section=0;
	List<Vector3> Choices=new List<Vector3>();
	public List<Vector2> GetBestChoices(int stupidity){
		List<Vector2> output=new List<Vector2>();
		if(Section>7){
			Section=0;
			InstructionCount=0;
			int max_pos=0;
			foreach(Vector3 Choice in Choices){
				max_pos=Math.Max(max_pos,(int)Choice.Z);
			}
			foreach(Vector3 Choice in Choices){
				int rnd=(new Random()).Next(0,19);
				rnd=Math.Max(0,rnd-17);
				if(((int)Choice.Z)>=Math.Max(1,max_pos-(stupidity*2)-rnd))
					output.Add(new Vector2(Choice.X,Choice.Y));
			}
			if(output.Count==0){
				foreach(Vector3 Choice in Choices){
					if(((int)Choice.Z)>=5)
						output.Add(new Vector2(Choice.X,Choice.Y));
				}
			}
			if(output.Count==0){
				foreach(Vector3 Choice in Choices){
					if(((int)Choice.Z)>=2)
						output.Add(new Vector2(Choice.X,Choice.Y));
				}
			}
			if(output.Count==0){
				foreach(Vector3 Choice in Choices){
					if(((int)Choice.Z)>0)
						output.Add(new Vector2(Choice.X,Choice.Y));
				}
			}
			if(output.Count==0){
				for(int y=0;y<8;y++){
					for(int x=0;x<8;x++){
						if(Grid[y][x].Ship==MyShip.Unknown)
							output.Add(new Vector2(x,y));
					}
				}
			}
			Choices.Clear();
			return output;
		}
		foreach(Vector3 V in GetBestChoices(Section++,stupidity))
			Choices.Add(V);
		return output;
	}
	
	public List<Vector3> GetBestChoices(int section,int stupidity){
		List<Vector3> output=new List<Vector3>();
		if(CountShips(MyShip.Unknown)==0)
			return output;
		int y=section;
		if(y<8){
			List<int> Row=new List<int>();
			for(int x=0;x<8;x++){
				int Cell=0;
				if(Grid[y][x].Ship==MyShip.Unknown){
					try{
						Cell+=GetPossibilitiesForShip(MyShip.Carrier,x,y);
						Cell+=GetPossibilitiesForShip(MyShip.Frigate,x,y);
						Cell+=GetPossibilitiesForShip(MyShip.Cruiser,x,y);
						Cell+=GetPossibilitiesForShip(MyShip.Prowler,x,y);
						Cell+=GetPossibilitiesForShip(MyShip.Destroyer,x,y);
					}
					catch(Exception e){
						Prog.Write(e.ToString());
					}
				}
				Row.Add(Cell);
			}
			for(int x=0;x<8;x++){
				if(Row[x]>0)
					output.Add(new Vector3(x,y,Row[x]));
			}
		}
		return output;
	}
	
}

class Player{
	public bool IsHuman;
	public Board OwnBoard;
	public Board EnemyBoard;
	public bool Paused=false;
	public Vector2 Selection;
	public bool CanMove;
	public Vector2 End1;
	public Vector2 End2;
	public bool Forfeiting;
	
	public int ReadyCount{
		get{
			int count=0;
			foreach(KeyValuePair<MyShip,bool> p in ReadyShips){
				if(p.Value)
					count++;
			}
			return count;
		}
	}
	
	public Dictionary<MyShip,bool> ReadyShips;
	
	public Player(bool human){
		IsHuman=human;
		OwnBoard=new Board(MyShip.None);
		EnemyBoard=new Board(MyShip.Unknown);
		ReadyShips=new Dictionary<MyShip,bool>();
		for(int i=1;i<=5;i++)
			ReadyShips.Add(((MyShip)i),false);
		Selection=new Vector2(0,0);
		CanMove=false;
		End1=new Vector2(-1,-1);
		End2=new Vector2(-1,-1);
		Forfeiting=false;
	}
	
	Player(bool human,Board Own,Board Enemy){
		IsHuman=human;
		OwnBoard=Own;
		EnemyBoard=Enemy;
		ReadyShips=new Dictionary<MyShip,bool>();
		for(int i=1;i<=5;i++){
			MyShip Ship=((MyShip)i);
			ReadyShips.Add(Ship,OwnBoard.CountShips(Ship)==Prog.ShipSize(Ship));
		}
		Selection=new Vector2(0,0);
		CanMove=false;
		End1=new Vector2(-1,-1);
		End2=new Vector2(-1,-1);
		Forfeiting=false;
	}
	
	public override string ToString(){
		string output=IsHuman.ToString();
		output+="◘"+OwnBoard.ToString();
		output+="◘"+EnemyBoard.ToString();
		return output;
	}
	
	public static bool TryParse(string Parse,out Player output){
		output=null;
		string[] args=Parse.Split('◘');
		if(args.Length!=3)
			return false;
		bool h;
		if(!bool.TryParse(args[0],out h))
			return false;
		Board o=null,e=null;
		if((!Board.TryParse(args[1],out o))||o==null)
			return false;
		if((!Board.TryParse(args[2],out e))||e==null)
			return false;
		output=new Player(h,o,e);
		return true;
	}
}

class Sound{
	public IMySoundBlock Block;
	public Queue<string> Sounds;
	public double Timer;
	
	public Sound(IMySoundBlock b){
		Block=b;
		Timer=2;
		Sounds=new Queue<string>();
	}
	
	public int RemoveSound(string remove){
		Queue<string> sounds=new Queue<string>();
		int count=0;
		while(Sounds.Count>0)
			sounds.Enqueue(Sounds.Dequeue());
		while(sounds.Count>0){
			string sound=sounds.Dequeue();
			if(!sound.Equals(remove))
				Sounds.Enqueue(sound);
			else
				count++;
		}
		return count;
	}
	
	public void Update(double seconds){
		if(Timer<2){
			Timer+=seconds;
		}
		if(Timer>=2){
			if(Sounds.Count>0){
				Block.Stop();
				Block.SelectedSound=Sounds.Dequeue();
				Block.Play();
				Timer=0;
			}
		}
	}
}

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;

Random Rnd;

IMyRemoteControl Controller;
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

int Player_Count=1;
double Turn_Timer=90;
bool Allow_Pause=true;
bool Use_Real_Ships=true;
bool Destroy_Ships=true;
bool See_Opponent_Choice=true;
GameStatus Status=GameStatus.Ready;
int AI_Difficulty=1;

Player Player1;
Player Player2;

bool Player_1_Ready=false;
bool Player_2_Ready=false;

DisplayArray Player1Enemy;
DisplayArray Player1Own;
DisplayArray Player2Enemy;
DisplayArray Player2Own;
List<IMyTextPanel> Player1StatusPanels;
List<IMyTextPanel> Player2StatusPanels;
List<IMyTextPanel> HubStatusPanels;
Sound Room1Sound;
Sound Room2Sound;
Sound HubSound;
List<RealShip> Player1Ships=new List<RealShip>();
List<RealShip> Player2Ships=new List<RealShip>();
bool IsReady(RealShip Ship,ShipStatus is_at=ShipStatus.Waiting){
	if(Ship==null)
		return false;
	if(Ship.ID<0||Ship.Status!=ShipStatus.Detonating&&(Ship.Timer>=300||(Ship.Timer>60&&Ship.Status!=ShipStatus.Traveling))||Ship.Status!=is_at||Ship.Antenna==null)
		return false;
	return true;
}
bool ReadyShips(List<ShipStatus> can_be_at){
	if(Player1Ships.Count<5||Player2Ships.Count<5)
		return false;
	foreach(RealShip Ship in Player1Ships){
		bool is_ready=false;
		foreach(ShipStatus is_at in can_be_at){
			if(IsReady(Ship,is_at)){
				is_ready=true;
				break;
			}
		}
		if(!is_ready)
			return false;
	}
	foreach(RealShip Ship in Player2Ships){
		bool is_ready=false;
		foreach(ShipStatus is_at in can_be_at){
			if(IsReady(Ship,is_at)){
				is_ready=true;
				break;
			}
		}
		if(!is_ready)
			return false;
	}
	return true;
}
bool ReadyShips(ShipStatus is_at=ShipStatus.Waiting){
	if(Player1Ships.Count<5||Player2Ships.Count<5)
		return false;
	foreach(RealShip Ship in Player1Ships){
		if(!IsReady(Ship,is_at))
			return false;
	}
	foreach(RealShip Ship in Player2Ships){
		if(!IsReady(Ship,is_at))
			return false;
	}
	return true;
}

List<IMyDoor> Room1Doors;
List<IMyDoor> Room2Doors;

IMyProgrammableBlock Vigilance;
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
FireStatus Cannon_FireStatus{
	get{
		if(Vigilance==null)
			return FireStatus.Idle;
		string[] args=Vigilance.CustomData.Split('\n');
		foreach(string arg in args){
			int index=arg.IndexOf(':');
			if(index>0){
				string type=arg.Substring(0,index);
				string data=arg.Substring(index+1);
				if(type.Equals("FireStatus")){
					FireStatus status;
					if(FireStatus.TryParse(data,out status))
						return status;
				}
			}
		}
		return FireStatus.Idle;
	}
}
PrintStatus Cannon_PrintStatus{
	get{
		if(Vigilance==null)
			return PrintStatus.StartingPrint;
		string[] args=Vigilance.CustomData.Split('\n');
		foreach(string arg in args){
			int index=arg.IndexOf(':');
			if(index>0){
				string type=arg.Substring(0,index);
				string data=arg.Substring(index+1);
				if(type.Equals("PrintStatus")){
					PrintStatus status;
					if(PrintStatus.TryParse(data,out status))
						return status;
				}
			}
		}
		return PrintStatus.StartingPrint;
	}
}
double Cannon_Seconds{
	get{
		if(Vigilance==null)
			return -1;
		string[] args=Vigilance.CustomData.Split('\n');
		foreach(string arg in args){
			int index=arg.IndexOf(':');
			if(index>0){
				string type=arg.Substring(0,index);
				string data=arg.Substring(index+1);
				if(type.Equals("Countdown")){
					double seconds;
					if(double.TryParse(data,out seconds))
						return seconds;
				}
			}
		}
		return -1;
	}
}
bool Is_Cannon_Ready{
	get{
		if(Vigilance==null)
			return false;
		if(Initiated_Firing)
			return false;
		string[] args=Vigilance.CustomData.Split('\n');
		foreach(string arg in args){
			int index=arg.IndexOf(':');
			if(index>0){
				string type=arg.Substring(0,index);
				string data=arg.Substring(index+1);
				switch(type){
					case "FireStatus":
						if(!data.Equals("Idle"))
							return false;
						break;
					case "PrintStatus":
						if(!data.Equals("Ready"))
							return false;
						break;
					case "Countdown":
						double seconds;
						if(double.TryParse(data,out seconds)&&seconds>0)
							return false;
						break;
				}
			}
		}
		return true;
	}
}


int Player_Turn=-1;
double Player_Timer=0;

Vector3D TransformCoordinates(Vector2 input,int Board_Num){
	if(Board_Num<1||Board_Num>2)
		return new Vector3D(0,0,0);
	if(input.X<0||input.X>7||input.Y<0||input.Y>7)
		return new Vector3D(0,0,0);
	Vector3D Center=Controller.GetPosition();
	double target_distance=1500;
	if(Board_Num==1)
		Center+=target_distance*Right_Vector;
	else
		Center+=target_distance*Left_Vector;
	Vector3D A1=Center+=625*Up_Vector;
	Vector3D To_Right;
	if(Board_Num==1){
		A1+=500*Forward_Vector;
		To_Right=Backward_Vector;
	}
	else{
		A1+=500*Backward_Vector;
		To_Right=Forward_Vector;
	}
	return A1+To_Right*(input.X)*125+Down_Vector*(input.Y)*125;
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
	for(int i=1;i<=5;i++){
		Player1Ships.Add(new RealShip(-1,(MyShip)i,1));
		Player2Ships.Add(new RealShip(-1,(MyShip)i,2));
		IGC.RegisterBroadcastListener(((MyShip)i).ToString()+" 1");
		IGC.RegisterBroadcastListener(((MyShip)i).ToString()+" 2");
	}
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		string command=GetCommand(arg);
		string data=GetData(arg);
		switch(command){
			case "Player_Count":
				Int32.TryParse(data,out Player_Count);
				break;
			case "Turn_Timer":
				if(double.TryParse(data,out Turn_Timer))
					Player_Timer=Turn_Timer;
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
			case "AI_Difficulty":
				Int32.TryParse(data,out AI_Difficulty);
				break;
			case "Player1":
				if(data==null)
					Player1=null;
				else 
					Player.TryParse(data,out Player1);
				break;
			case "Player2":
				if(data==null)
					Player2=null;
				else 
					Player.TryParse(data,out Player2);
				break;
			case "Player_Turn":
				Int32.TryParse(data,out Player_Turn);
				break;
			case "Player1Ships":
				string[] p1ss=data.Split(';');
				if(p1ss.Length==5){
					Player1Ships.Clear();
					foreach(string p1s in p1ss){
						RealShip ship;
						RealShip.TryParse(p1s,out ship);
						Player1Ships.Add(ship);
					}
				}
				break;
			case "Player2Ships":
				string[] p2ss=data.Split(';');
				if(p2ss.Length==5){
					Player2Ships.Clear();
					foreach(string p2s in p2ss){
						RealShip ship;
						RealShip.TryParse(p2s,out ship);
						Player2Ships.Add(ship);
					}
				}
				break;
			case "Initiated_Firing":
				bool.TryParse(data,out Initiated_Firing);
				break;
			case "SetUp_Timer":
				double.TryParse(data,out SetUp_Timer);
				break;
			case "Release_Timer":
				double.TryParse(data,out Release_Timer);
				break;
			case "Release_Number":
				Int32.TryParse(data,out Release_Number);
				break;
			case "Decoy_Target":
				Vector3D.TryParse(data,out Decoy_Target);
				break;
			case "Started_Game":
				bool.TryParse(data,out Started_Game);
				break;
		}
	}
	Room1Doors=GenericMethods<IMyDoor>.GetAllContaining("Room 1 Door");
	Room2Doors=GenericMethods<IMyDoor>.GetAllContaining("Room 2 Door");
	IMySoundBlock s=GenericMethods<IMySoundBlock>.GetFull("Room 1 Sound Block");
	if(s!=null)
		Room1Sound=new Sound(s);
	else
		return;
	s=GenericMethods<IMySoundBlock>.GetFull("Room 2 Sound Block");
	if(s!=null)
		Room2Sound=new Sound(s);
	else
		return;
	s=GenericMethods<IMySoundBlock>.GetFull("Hub Sound Block");
	if(s!=null)
		HubSound=new Sound(s);
	else
		return;
	double time=DateTime.Now.TimeOfDay.TotalHours%24;
	if(time>4.5&&time<12){
		Room1Sound.Sounds.Enqueue("Good MorningId");
		Room2Sound.Sounds.Enqueue("Good MorningId");
		HubSound.Sounds.Enqueue("Good MorningId");
	}
	else if(time>12&&time<18){
		Room1Sound.Sounds.Enqueue("Good AfternoonId");
		Room2Sound.Sounds.Enqueue("Good AfternoonId");
		HubSound.Sounds.Enqueue("Good AfternoonId");
	}
	else{
		Room1Sound.Sounds.Enqueue("Hello EngineerId");
		Room2Sound.Sounds.Enqueue("Hello EngineerId");
		HubSound.Sounds.Enqueue("Hello EngineerId");
	}
	List<IMyLaserAntenna> Player1LaserAntenna=GenericMethods<IMyLaserAntenna>.GetAllContaining("Player 1 Laser Antenna");
	List<IMyLaserAntenna> Player2LaserAntenna=GenericMethods<IMyLaserAntenna>.GetAllContaining("Player 2 Laser Antenna");
	if(Player1LaserAntenna.Count<5||Player2LaserAntenna.Count<5)
		return;
	for(int i=0;i<5;i++){
		if(Player1Ships[i]!=null)
			Player1Ships[i].Antenna=Player1LaserAntenna[i];
		if(Player2Ships[i]!=null)
			Player2Ships[i].Antenna=Player2LaserAntenna[i];
	}
	Controller=GenericMethods<IMyRemoteControl>.GetFull("Hub Remote Control");
	if(Controller==null)
		return;
	Forward=Controller.Orientation.Forward;
	Up=Controller.Orientation.Up;
	Left=Controller.Orientation.Left;
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
	Vigilance=GenericMethods<IMyProgrammableBlock>.GetFull("Battleship Vigilance AI Programmable block");
	
	Write("Completed Initialization");
	Runtime.UpdateFrequency=UpdateFrequency.Update10;//60tps
}

bool Factory_Reset=false;
public void Save(){
    if(!Factory_Reset){
		this.Storage="Status:"+((int)Status).ToString();
		this.Storage+="•Player_Count:"+Player_Count.ToString();
		this.Storage+="•AI_Difficulty:"+AI_Difficulty.ToString();
		this.Storage+="•Turn_Timer:"+Turn_Timer.ToString();
		this.Storage+="•Allow_Pause"+Allow_Pause.ToString();
		this.Storage+="•Use_Real_Ships:"+Use_Real_Ships.ToString();
		this.Storage+="•Destroy_Ships:"+Destroy_Ships.ToString();
		this.Storage+="•See_Opponent_Choice:"+See_Opponent_Choice.ToString();
		if(Player1==null)
			this.Storage+="•Player1:null";
		else
			this.Storage+="•Player1:"+Player1.ToString();
		if(Player2==null)
			this.Storage+="•Player2:null";
		else
			this.Storage+="•Player2:"+Player2.ToString();
		this.Storage+="•Player_Turn:"+Player_Turn.ToString();
		this.Storage+="•Player1Ships:";
		for(int i=0;i<Player1Ships.Count;i++){
			if(i>0)
				this.Storage+=";";
			if(Player1Ships[i]==null)
				this.Storage+="null";
			else
				this.Storage+=Player1Ships[i].ToString();
		}
		this.Storage+="•Player2Ships:";
		for(int i=0;i<Player2Ships.Count;i++){
			if(i>0)
				this.Storage+=";";
			if(Player2Ships[i]==null)
				this.Storage+="null";
			else
				this.Storage+=Player2Ships[i].ToString();
		}
		this.Storage+="•Initiated_Firing:"+Initiated_Firing.ToString();
		this.Storage+="•SetUp_Timer:"+SetUp_Timer.ToString();
		this.Storage+="•Release_Timer:"+Release_Timer.ToString();
		this.Storage+="•Release_Number:"+Release_Number.ToString();
		this.Storage+="•Decoy_Target:"+Decoy_Target.ToString();
		this.Storage+="•Started_Game:"+Started_Game.ToString();
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

void DisplayOwn(DisplayArray Da,Player P,Vector2 EnemyPos){
	for(int y=0;y<Da.Panels.Count;y++){
		for(int x=0;x<Da.Panels[y].Count;x++){
			Color color=new Color(10,10,10,255);
			switch(P.OwnBoard.Grid[y][x].Ship){
				case MyShip.Carrier:
					color=new Color(255,255,0,255);
					break;
				case MyShip.Frigate:
					color=new Color(0,255,0,255);
					break;
				case MyShip.Cruiser:
					color=new Color(0,255,255,255);
					break;
				case MyShip.Prowler:
					color=new Color(255,0,255,255);
					break;
				case MyShip.Destroyer:
					color=new Color(255,0,0,255);
					break;
			}
			if(Da.Panels[y][x].CurrentlyShownImage!=null)
				Da.Panels[y][x].ClearImagesFromSelection();
			if(x==((int)EnemyPos.X)&&y==((int)EnemyPos.Y))
				Da.Panels[y][x].AddImageToSelection("LCD_Economy_Faction_1");
			if(Status==GameStatus.SettingUp&&P.CanMove&&x==((int)P.Selection.X)&&y==((int)P.Selection.Y))
				Da.Panels[y][x].AddImageToSelection("LCD_Economy_Trinity");
			else if(x==((int)P.End1.X)&&y==((int)P.End1.Y))
				Da.Panels[y][x].AddImageToSelection("LCD_Economy_Trinity");
			else if(x==((int)P.End2.X)&&y==((int)P.End2.Y))
				Da.Panels[y][x].AddImageToSelection("LCD_Economy_Trinity");
			if(P.OwnBoard.Grid[y][x].Hit){
				if(P.OwnBoard.Grid[y][x].Ship==MyShip.None){
					color=new Color(0,0,0,255);
					Da.Panels[y][x].AddImageToSelection("Cross");
				}
				else{
					if(P.OwnBoard.RemainingHits(P.OwnBoard.Grid[y][x].Ship)>0)
						Da.Panels[y][x].AddImageToSelection("Danger");
					else{
						Da.Panels[y][x].AddImageToSelection("Cross");
						color=Color.Multiply(color,0.2f);
					}
				}
			}
			Da.Panels[y][x].BackgroundColor=color;
		}
	}
}
void DisplayOwn(DisplayArray Da,Player P){
	DisplayOwn(Da,P,new Vector2(-1,-1));
}
void DisplayEnemy(DisplayArray Da,Player P){
	for(int y=0;y<Da.Panels.Count;y++){
		for(int x=0;x<Da.Panels[y].Count;x++){
			Color color=new Color(50,50,50,255);
			switch(P.EnemyBoard.Grid[y][x].Ship){
				case MyShip.Carrier:
					color=new Color(255,255,0,255);
					break;
				case MyShip.Frigate:
					color=new Color(0,255,0,255);
					break;
				case MyShip.Cruiser:
					color=new Color(0,255,255,255);
					break;
				case MyShip.Prowler:
					color=new Color(255,0,255,255);
					break;
				case MyShip.Destroyer:
					color=new Color(255,0,0,255);
					break;
			}
			if(Da.Panels[y][x].CurrentlyShownImage!=null)
				Da.Panels[y][x].ClearImagesFromSelection();
			if(Status!=GameStatus.SettingUp&&P.CanMove&&x==((int)P.Selection.X)&&y==((int)P.Selection.Y))
				Da.Panels[y][x].AddImageToSelection("LCD_Economy_Trinity");
			if(P.EnemyBoard.Grid[y][x].Hit){
				if(P.EnemyBoard.Grid[y][x].Ship==MyShip.None){
					color=new Color(0,0,0,255);
					Da.Panels[y][x].AddImageToSelection("Cross");
				}
				else{
					if(P.EnemyBoard.RemainingHits(P.EnemyBoard.Grid[y][x].Ship)>0)
						Da.Panels[y][x].AddImageToSelection("Danger");
					else{
						Da.Panels[y][x].AddImageToSelection("Cross");
						color=Color.Multiply(color,0.2f);
					}
				}
			}
			Da.Panels[y][x].BackgroundColor=color;
		}
	}
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
	if(DisplayIdleTimer>0.1){
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
			bool in_worm=false;
			do{
				Target=new Vector2(Rnd.Next(0,8),Rnd.Next(0,8));
				in_worm=false;
				foreach(Vector2 Part in Parts){
					if(Target==Part){
						in_worm=true;
						break;
					}
				}
			}
			while(in_worm);
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
				int pick=Rnd.Next(0,Options.Count+1);
				int chance=Rnd.Next(0,2);
				while(pick<Options.Count&&chance>0&&Vector2.Distance(Parts[0],Target)>3){
					pick=Rnd.Next(0,Options.Count+1);
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
			if(Da.Panels[i][j].CurrentlyShownImage!=null)
				Da.Panels[i][j].ClearImagesFromSelection();
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

bool call_return=false;
void CallReturn(){
	call_return=false;
	if(Release_Timer>=30&&Release_Number<5){
		Release_Number++;
		Release_Timer=0;
	}
	for(int i=1;i<=2;i++){
		List<RealShip> ShipList;
		if(i==1)
			ShipList=Player1Ships;
		else
			ShipList=Player2Ships;
		for(int j=1;j<=5;j++){
			RealShip ship=ShipList[j-1];
			if(ship.Status!=ShipStatus.Waiting&&ship.Status!=ShipStatus.Returning&&ship.Timer<300){
				call_return=true;
				if(Release_Number>=(j-1)){
					Vector3D Target=Controller.GetPosition()+Up_Vector*50+Forward_Vector*20;
					double Length=50*ShipSize(ShipList[j-1].Type)+10;
					if((j-1)%2==0)
						Target+=75*(2-((j-1)/2))*Forward_Vector;
					else
						Target+=75*(2-((j-2)/2))*Backward_Vector;
					Target+=75*Up_Vector*((5-j)/2);
					if(i==1)
						Target+=Length*Right_Vector;
					else
						Target+=Length*Left_Vector;
					IGC.SendBroadcastMessage(ship.Tag_Full,"Return•"+Target.ToString()+"•"+Up_Vector.ToString(),TransmissionDistance.TransmissionDistanceMax);
				}
			}
		}
	}
	if(Release_Timer<30)
		Release_Timer+=seconds_since_last_update;
}

void CallHit(Player Attacker,Player Defender,Sound At_Sound,Sound Def_Sound){
	int x=(int)Attacker.Selection.X;
	int y=(int)Attacker.Selection.Y;
	if(!Attacker.EnemyBoard.Grid[y][x].Hit){
		Defender.OwnBoard.Grid[y][x].Hit=true;
		Attacker.EnemyBoard.Grid[y][x]=Defender.OwnBoard.Grid[y][x];
		Player_Turn=(Player_Turn%2)+1;
		Player_Timer=Turn_Timer;
		AI_Selection=new Vector2(-1,-1);
		Attacker.CanMove=false;
		if(Defender.OwnBoard.Grid[y][x].Ship!=MyShip.None){
			MyShip Ship=Defender.OwnBoard.Grid[y][x].Ship;
			int size=Prog.ShipSize(Ship);
			if(Player1.EnemyBoard.RemainingHits(Ship)==0){
				At_Sound.Sounds.Enqueue("ShutdownId");
				Def_Sound.Sounds.Enqueue("ShutdownId");
				HubSound.Sounds.Enqueue("ShutdownId");
			}
			else{
				if(size>3)
					At_Sound.Sounds.Enqueue("Large Ship DetectedId");
				else if(size>0)
					At_Sound.Sounds.Enqueue("Small ship detectedId");
				Def_Sound.Sounds.Enqueue("Damage detectedId");
				HubSound.Sounds.Enqueue("Damage detectedId");
			}
		}
	}
}

double DisplayIdleTimer=0;
int Selection=0;
void Argument_Processor(string argument){
	if(argument.ToLower().Equals("reset")){
		this.Storage="";
		Factory_Reset=true;
		Runtime.UpdateFrequency=UpdateFrequency.None;
	}
	else if(argument.ToLower().Equals("unlink")){
		for(int i=1;i<=2;i++){
			List<RealShip> ShipList;
			if(i==1)
				ShipList=Player1Ships;
			else
				ShipList=Player2Ships;
			for(int j=0;j<ShipList.Count;j++){
				IGC.SendBroadcastMessage(ShipList[i].Tag_Full,"Unlink",TransmissionDistance.TransmissionDistanceMax);
			}
		}
		Runtime.UpdateFrequency=UpdateFrequency.None;
		Me.Enabled=false;
	}
	else if(argument.ToLower().Equals("detonate")){
		for(int i=1;i<=2;i++){
			List<RealShip> ShipList;
			if(i==1)
				ShipList=Player1Ships;
			else
				ShipList=Player2Ships;
			for(int j=0;j<ShipList.Count;j++){
				IGC.SendBroadcastMessage(ShipList[i].Tag_Full,"Detonate",TransmissionDistance.TransmissionDistanceMax);
			}
		}
	}
	if(((int)Status)<((int)GameStatus.Awaiting)){
		if(argument.ToLower().Equals("prev")){
			do{
				Selection=(Selection-1+8)%8;
			}
			while((Selection==6&&!Use_Real_Ships)||(Selection==0&&Status!=GameStatus.Ready)||(Selection==4&&Turn_Timer<30)||(Selection==2&&Player_Count==2));
		}
		else if(argument.ToLower().Equals("next")){
			do{
				Selection=(Selection+1)%8;
			}
			while((Selection==6&&!Use_Real_Ships)||(Selection==0&&Status!=GameStatus.Ready)||(Selection==4&&Turn_Timer<30)||(Selection==2&&Player_Count==2));
		}
		else if(argument.ToLower().Equals("down")){
			switch(Selection){
				case 1:
					Player_Count=Math.Max(0,Player_Count-1);
					break;
				case 2:
					AI_Difficulty=Math.Min(2,AI_Difficulty+1);
					break;
				case 3:
					Turn_Timer-=10;
					if(Turn_Timer<30)
						Turn_Timer=0;
					break;
				case 4:
					Allow_Pause=false;
					break;
				case 5:
					Use_Real_Ships=false;
					Destroy_Ships=false;
					break;
				case 6:
					Destroy_Ships=false;
					break;
				case 7:
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
						Player1=new Player(Player_Count>=1);
						Player2=new Player(Player_Count>=2);
						HubSound.Sounds.Enqueue("Access GrantedId");
						Last_Winner=-1;
						if(Player_Count>0)
							HubSound.Sounds.Enqueue("OpeningId");
					}
					break;
				case 1:
					Player_Count=Math.Min(2,Player_Count+1);
					break;
				case 2:
					AI_Difficulty=Math.Max(0,AI_Difficulty-1);
					break;
				case 3:
					if(Turn_Timer<30)
						Turn_Timer=30;
					else
						Turn_Timer=Math.Min(300,Turn_Timer+10);
					break;
				case 4:
					Allow_Pause=true;
					break;
				case 5:
					Use_Real_Ships=true;
					break;
				case 6:
					Destroy_Ships=true;
					
					break;
				case 7:
					See_Opponent_Choice=true;
					break;
			}
		}
	}
	else if(argument.ToLower().Equals("down")&&Status==GameStatus.Awaiting){
		Status=GameStatus.Ready;
		Player1=null;
		Player2=null;
	}
	if(argument.IndexOf("Player ")==0){
		int player_num=-1;
		string command=GetCommand(argument);
		string data=GetData(argument);
		if(command.Equals("Player 1"))
			player_num=1;
		else if(command.Equals("Player 2"))
			player_num=2;
		if(player_num>0&&player_num<=2){
			switch(data){
				case "Left":
					if(player_num==1){
						if(Player1.CanMove){
							if(Status==GameStatus.SettingUp&&Player1.End1.X>=0&&Player1.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player1.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player1.End1-new Vector2(size-1,0);
								if(sel.X>=0)
									Player1.Selection=sel;
							}
							else{
								Player1.Selection.X-=1;
								if(Player1.Selection.X<0)
									Player1.Selection.X=7;
							}
						}
					}
					else if(player_num==2){
						if(Player2.CanMove){
							if(Status==GameStatus.SettingUp&&Player2.End1.X>=0&&Player2.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player2.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player2.End1-new Vector2(size-1,0);
								if(sel.X>=0)
									Player2.Selection=sel;
							}
							else{
								Player2.Selection.X-=1;
								if(Player2.Selection.X<0)
									Player2.Selection.X=7;
							}
						}
					}
					break;
				case "Up":
					if(player_num==1){
						if(Player1.CanMove){
							if(Status==GameStatus.SettingUp&&Player1.End1.X>=0&&Player1.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player1.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player1.End1-new Vector2(0,size-1);
								if(sel.Y>=0)
									Player1.Selection=sel;
							}
							else{
								Player1.Selection.Y-=1;
								if(Player1.Selection.Y<0)
									Player1.Selection.Y=7;
							}
						}
					}
					else if(player_num==2){
						if(Player2.CanMove){
							if(Status==GameStatus.SettingUp&&Player2.End1.X>=0&&Player2.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player2.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player2.End1-new Vector2(0,size-1);
								if(sel.Y>=0)
									Player2.Selection=sel;
							}
							else{
								Player2.Selection.Y-=1;
								if(Player2.Selection.Y<0)
									Player2.Selection.Y=7;
							}
						}
					}
					break;
				case "Down":
					if(player_num==1){
						if(Player1.CanMove){
							if(Status==GameStatus.SettingUp&&Player1.End1.X>=0&&Player1.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player1.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player1.End1+new Vector2(0,size-1);
								if(sel.Y<=7)
									Player1.Selection=sel;
							}
							else{
								Player1.Selection.Y+=1;
								if(Player1.Selection.Y>7)
									Player1.Selection.Y=0;
							}
						}
					}
					else if(player_num==2){
						if(Player2.CanMove){
							if(Status==GameStatus.SettingUp&&Player2.End1.X>=0&&Player2.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player2.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player2.End1+new Vector2(0,size-1);
								if(sel.Y<=7)
									Player2.Selection=sel;
							}
							else{
								Player2.Selection.Y+=1;
								if(Player2.Selection.Y>7)
									Player2.Selection.Y=0;
							}
						}
					}
					break;
				case "Right":
					if(player_num==1){
						if(Player1.CanMove){
							if(Status==GameStatus.SettingUp&&Player1.End1.X>=0&&Player1.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player1.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player1.End1+new Vector2(size-1,0);
								if(sel.X<=7)
									Player1.Selection=sel;
							}
							else{
								Player1.Selection.X+=1;
								if(Player1.Selection.X>7)
									Player1.Selection.X=0;
							}
						}
					}
					else if(player_num==2){
						if(Player2.CanMove){
							if(Status==GameStatus.SettingUp&&Player2.End1.X>=0&&Player2.End2.X<0){
								int size=0;
								foreach(KeyValuePair<MyShip,bool> p in Player2.ReadyShips){
									if(!p.Value){
										size=Prog.ShipSize(p.Key);
										break;
									}
								}
								Vector2 sel=Player2.End1+new Vector2(size-1,0);
								if(sel.X<=7)
									Player2.Selection=sel;
							}
							else{
								Player2.Selection.X+=1;
								if(Player2.Selection.X>7)
									Player2.Selection.X=0;
							}
						}
					}
					break;
				case "Confirm":
					if(player_num==1&&Player1!=null&&Player1.Forfeiting){
						Last_Winner=2;
						Player1.Forfeiting=false;
						Status=GameStatus.Ready;
						Player1=null;
						Player2=null;
						Room1Sound.Sounds.Clear();
						Room2Sound.Sounds.Clear();
						Room1Sound.Sounds.Enqueue("Shutting DownId");
						Room2Sound.Sounds.Enqueue("SoundBlockObjectiveComplete");
						HubSound.Sounds.Enqueue("Objective CompleteId");
						Release_Number=0;
						Release_Timer=0;
						if(Use_Real_Ships)
							CallReturn();
					}
					else if(player_num==2&&Player2!=null&&Player2.Forfeiting){
						Last_Winner=1;
						Player2.Forfeiting=false;
						Status=GameStatus.Ready;
						Player1=null;
						Player2=null;
						Room1Sound.Sounds.Clear();
						Room2Sound.Sounds.Clear();
						Room2Sound.Sounds.Enqueue("Shutting DownId");
						Room1Sound.Sounds.Enqueue("SoundBlockObjectiveComplete");
						HubSound.Sounds.Enqueue("Objective CompleteId");
						Release_Number=0;
						Release_Timer=0;
						if(Use_Real_Ships)
							CallReturn();
					}
					else{
						if(Status==GameStatus.Awaiting){
							if(player_num==1)
								Player_1_Ready=true;
							else if(player_num==2)
								Player_2_Ready=true;
						}
						if(Status==GameStatus.SettingUp){
							if(player_num==1){
								if(Player1.End1.X<0){
									Player1.End1=Player1.Selection;
								}
								else if(Player1.End2.X<0){
									Player1.End2=Player1.Selection;
									Player1.Selection=new Vector2(0,0);
									MyShip Ship=MyShip.Unknown;
									foreach(KeyValuePair<MyShip,bool> p in Player1.ReadyShips){
										if(!p.Value){
											Ship=p.Key;
											break;
										}
									}
									if(((int)Ship)>((int)MyShip.None)){
										if(Player1.OwnBoard.AddShip(Ship,(int)Player1.End1.X,(int)Player1.End1.Y,(int)Player1.End2.X,(int)Player1.End2.Y)){
											Player1Ships[((int)Ship)-1].End1=TransformCoordinates(Player1.End1,1);
											Player1Ships[((int)Ship)-1].End2=TransformCoordinates(Player1.End2,1);
											Player1.ReadyShips[Ship]=true;
										}
									}
									Player1.End1=new Vector2(-1,-1);
									Player1.End2=new Vector2(-1,-1);
								}
								else{
									Player1.End1=new Vector2(-1,-1);
									Player1.End2=new Vector2(-1,-1);
								}
							}
							else if(player_num==2){
								if(Player2.End1.X<0){
									Player2.End1=Player2.Selection;
								}
								else if(Player2.End2.X<0){
									Player2.End2=Player2.Selection;
									Player2.Selection=new Vector2(0,0);
									MyShip Ship=MyShip.Unknown;
									foreach(KeyValuePair<MyShip,bool> p in Player2.ReadyShips){
										if(!p.Value){
											Ship=p.Key;
											break;
										}
									}
									if(Ship!=MyShip.Unknown){
										if(Player2.OwnBoard.AddShip(Ship,(int)Player2.End1.X,(int)Player2.End1.Y,(int)Player2.End2.X,(int)Player2.End2.Y)){
											Player2Ships[((int)Ship)-1].End1=TransformCoordinates(Player2.End1,2);
											Player2Ships[((int)Ship)-1].End2=TransformCoordinates(Player2.End2,2);
											Player2.ReadyShips[Ship]=true;
										}
									}
									Player2.End1=new Vector2(-1,-1);
									Player2.End2=new Vector2(-1,-1);
								}
								else{
									Player2.End1=new Vector2(-1,-1);
									Player2.End2=new Vector2(-1,-1);
								}
							}
						}
						if(Status==GameStatus.InProgress){
							if(player_num==Player_Turn){
								Room1Sound.RemoveSound("CompletedId");
								Room2Sound.RemoveSound("CompletedId");
								Room1Sound.Sounds.Enqueue("CompletedId");
								Room2Sound.Sounds.Enqueue("CompletedId");
								if(Destroy_Ships){
									Initiated_Firing=true;
									Fire_Timer=0;
								}
								else{
									if(player_num==1){
										CallHit(Player1,Player2,Room1Sound,Room2Sound);
									}
									else if(player_num==2){
										CallHit(Player2,Player1,Room2Sound,Room1Sound);
									}
								}
							}
						}
					}
					;
					break;
				case "Pause":
					if(player_num==1){
						if(Player1.Paused)
							Player1.Paused=false;
						else{
							if(Status==GameStatus.InProgress&&Allow_Pause)
								Player1.Paused=true;
						}
					}
					else if(player_num==2){
						if(Player2.Paused)
							Player2.Paused=false;
						else{
							if(Status==GameStatus.InProgress&&Allow_Pause)
								Player2.Paused=true;
						}
					}
					break;
				case "Cancel":
					if(player_num==1&&Player1!=null&&Player1.Forfeiting)
						Player1.Forfeiting=false;
					else if(player_num==2&&Player2!=null&&Player2.Forfeiting)
						Player2.Forfeiting=false;
					else if(Status==GameStatus.Awaiting){
						if(player_num==1)
							Player_1_Ready=false;
						else if(player_num==2)
							Player_2_Ready=false;
					}
					else if(Status==GameStatus.SettingUp){
						if(player_num==1){
							if(Player1.End2.X>=0)
								Player1.End2=new Vector2(-1,-1);
							else if(Player1.End1.X>=0){
								Player1.Selection=Player1.End1;
								Player1.End1=new Vector2(-1,-1);
							}
							else{
								Player1.OwnBoard=new Board(MyShip.None);
								for(int i=1;i<=5;i++)
									Player1.ReadyShips[(MyShip)i]=false;
								Player1.CanMove=false;
							}
						}
						else if(player_num==2){
							if(Player2.End2.X>=0)
								Player2.End2=new Vector2(-1,-1);
							else if(Player2.End1.X>=0){
								Player2.Selection=Player2.End1;
								Player2.End1=new Vector2(-1,-1);
							}
							else{
								Player2.OwnBoard=new Board(MyShip.None);
								for(int i=1;i<=5;i++)
									Player2.ReadyShips[(MyShip)i]=false;
								Player2.CanMove=false;
							}
						}
					}
					;
					break;
				case "Forfeit":
					if(Player1!=null&&player_num==1)
						Player1.Forfeiting=true;
					else if(Player2!=null&&player_num==2)
						Player2.Forfeiting=true;
					break;
			}
		}
	}
}

Vector3D GetTargetedCoordinates(){
	Player Attacker,Defender;
	if(Player_Turn==1){
		Attacker=Player1;
		Defender=Player2;
	}
	else if(Player_Turn==2){
		Attacker=Player2;
		Defender=Player1;
	}
	else
		return new Vector3D(0,0,0);
	return TransformCoordinates(Attacker.Selection,(Player_Turn%2)+1);
}
MyShip GetTargetedType(){
	Player Attacker,Defender;
	List<RealShip> ShipList;
	
	if(Player_Turn==1){
		Attacker=Player1;
		Defender=Player2;
		ShipList=Player2Ships;
	}
	else if(Player_Turn==2){
		Attacker=Player2;
		Defender=Player1;
		ShipList=Player1Ships;
	}
	else
		return MyShip.Unknown;
	int x=(int)Attacker.Selection.X;
	int y=(int)Attacker.Selection.Y;
	return Defender.OwnBoard.Grid[y][x].Ship;
}
RealShip GetTargetedShip(){
	Player Attacker,Defender;
	List<RealShip> ShipList;
	
	if(Player_Turn==1){
		Attacker=Player1;
		Defender=Player2;
		ShipList=Player2Ships;
	}
	else if(Player_Turn==2){
		Attacker=Player2;
		Defender=Player1;
		ShipList=Player1Ships;
	}
	else
		return null;
	int x=(int)Attacker.Selection.X;
	int y=(int)Attacker.Selection.Y;
	MyShip Type=Defender.OwnBoard.Grid[y][x].Ship;
	foreach(RealShip Ship in ShipList){
		if(Ship.Type==Type)
			return Ship;
	}
	return null;
}

int Last_Winner=-1;
Vector2 AI_Selection=new Vector2(-1,-1);
double AI_Timer=0;
double Release_Timer=0;
int Release_Number=4;
double Ready_Timer=0;
double SetUp_Timer=0;
bool Initiated_Firing=false;
Vector3D Decoy_Target=new Vector3D(0,0,0);
bool Was_Firing=false;
bool Started_Game=false;
double Fire_Timer=0;
public void Main(string argument, UpdateType updateSource)
{
	try{
		UpdateProgramInfo();
		if(DisplayIdleTimer<5)
			DisplayIdleTimer+=seconds_since_last_update;
		if(AI_Timer<5)
			AI_Timer+=seconds_since_last_update;
		Me.CustomData="";
		
		string HubText="";
		string Player1Text="";
		string Player2Text="";
		List<IMyBroadcastListener> listeners=new List<IMyBroadcastListener>();
		IGC.GetBroadcastListeners(listeners);
		for(int j=1;j<=2;j++){
			List<RealShip> ShipList;
			if(j==1)
				ShipList=Player1Ships;
			else
				ShipList=Player2Ships;
			Echo("Player "+j.ToString()+" Ship Statuses:");
			for(int i=0;i<ShipList.Count;i++){
				if(ShipList[i]==null){
					ShipList[i]=new RealShip(-1,(MyShip)(i+1),j);
					List<IMyLaserAntenna> Antennas=GenericMethods<IMyLaserAntenna>.GetAllContaining("Player "+j.ToString()+" Laser Antenna");
					if(Antennas.Count>=5)
						ShipList[i].Antenna=Antennas[i];
				}
				if(ShipList[i].Timer<300)
					ShipList[i].Timer+=seconds_since_last_update;
				foreach(IMyBroadcastListener Listener in listeners){
					if(Listener.Tag.Equals(ShipList[i].Tag)){
						bool accepting_new=(ShipList[i].ID<0||ShipList[i].Timer>=300||(ShipList[i].Status!=ShipStatus.Traveling&&ShipList[i].Timer>60));
						while(Listener.HasPendingMessage){
							MyIGCMessage message=Listener.AcceptMessage();
							string[] args=message.Data.ToString().Split('•');
							if(accepting_new&&args.Length==2&&args[0].Equals("ID")){
								int id;
								if(Int32.TryParse(args[1],out id)){
									if(id>=0){
										ShipList[i].ID=id;
										ShipList[i].Timer=0;
										if(ShipList[i].Antenna!=null)
											IGC.SendBroadcastMessage(ShipList[i].Tag_Full,id.ToString()+":"+(ShipList[i].Antenna.GetPosition()+1.25*Up_Vector).ToString(),TransmissionDistance.TransmissionDistanceMax);
									}
								}
							}
							if(args.Length==3&&args[0].Equals("Status")){
								ShipStatus status;
								int id;
								if(Int32.TryParse(args[1],out id)){
									if(id==ShipList[i].ID||accepting_new){
										if(id!=ShipList[i].ID)
											ShipList[i].ID=id;
										if(ShipStatus.TryParse(args[2],out status)){
											if(ShipList[i].Status!=ShipStatus.Detonating||accepting_new)
												ShipList[i].Status=status;
											ShipList[i].Timer=0;
										}
									}
								}
							}
							else if(args.Length==3&&args[0].Equals("Target")){
								Write("Received Targeting Information for "+ShipList[i].Tag);
								int id;
								Vector3D target;
								if(Int32.TryParse(args[1],out id)&&id==ShipList[i].ID){
									RealShip Target_Ship=GetTargetedShip();
									if(Target_Ship.Type==ShipList[i].Type&&Target_Ship.ID==ShipList[i].ID&&Target_Ship.Player_Num==ShipList[i].Player_Num){
										if(Vector3D.TryParse(args[2],out target)){
											Decoy_Target=target;
											Write("Setup Decoy_Target");
										}
									}
								}
							}
						}
					}
				}
				Echo("  "+ShipList[i].Type.ToString()+":"+ShipList[i].ID.ToString());
				if(ShipList[i].ID>=0){
					Echo("    "+ShipList[i].Status.ToString());
					if(ShipList[i].Timer>10)
						Echo("    Last Received "+Math.Round(ShipList[i].Timer,1).ToString()+" seconds ago");
					if(ShipList[i].Antenna==null)
						Echo("    Antenna:Invalid");
					else
						ShipList[i].Antenna.CustomData=ShipList[i].Type.ToString()+" "+ShipList[i].Player_Num.ToString()+":"+ShipList[i].ID.ToString();
				}
				if(ShipList[i].ID!=0&&ShipList[i].Status==ShipStatus.Linking&&ShipList[i].Antenna!=null){
					IGC.SendBroadcastMessage(ShipList[i].Tag_Full,ShipList[i].ID.ToString()+":"+(ShipList[i].Antenna.GetPosition()+1.25*Up_Vector).ToString(),TransmissionDistance.TransmissionDistanceMax);
				}
			}
			
		}
		if(call_return)
			CallReturn();
		
		Echo("Status: "+Status.ToString());
		if(Decoy_Target.Length()>0)
			Echo("Decoy Target:"+Decoy_Target.ToString());
		if(Player1!=null){
			Echo("Player 1:");
			Echo("  CanMove:"+Player1.CanMove.ToString());
			Echo("  Selection:"+Player1.Selection.ToString());
			Echo("  End1:"+Player1.End1.ToString());
			Echo("  End2:"+Player1.End2.ToString());
			Echo("  ReadyCount:"+Player1.ReadyCount.ToString());
		}
		if(Player2!=null){
			Echo("Player 2:");
			Echo("  CanMove:"+Player2.CanMove.ToString());
			Echo("  Selection:"+Player2.Selection.ToString());
			Echo("  End1:"+Player1.End1.ToString());
			Echo("  End2:"+Player1.End2.ToString());
			Echo("  ReadyCount:"+Player2.ReadyCount.ToString());
		}
		Echo("AI_Timer:"+Math.Round(AI_Timer,2).ToString()+"s");
		Echo("AI_Selection:"+AI_Selection.ToString());
		Echo("Sound1:"+Room1Sound.Block.SelectedSound);
		Echo("Sound2:"+Room2Sound.Block.SelectedSound);
		Echo("SoundH:"+HubSound.Block.SelectedSound);
		
		if(((int)Status)<((int)GameStatus.Awaiting)){
			Player1Text="Start game in Lobby";
			Player2Text="Start game in Lobby";
			HubText="Start game in Lobby\n";
			if(Last_Winner==1){
				Player1Text="You WON!!!";
				Player2Text="Defeat...";
				HubText+="Game won by Player 1!\n";
			}
			else if(Last_Winner==2){
				Player1Text="Defeat...";
				Player2Text="You WON!!!";
				HubText+="Game won by Player 2!\n";
			}
			
			
			foreach(IMyDoor Door in Room1Doors){
				double Timer=5;
				Door.Enabled=true;
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
				Door.Enabled=true;
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
			if(Use_Real_Ships&&(!ReadyShips()||(Destroy_Ships&&!Is_Cannon_Ready)))
				Status=GameStatus.Waiting;
			else
				Status=GameStatus.Ready;
			if(Status==GameStatus.Ready)
				Write(s+"Start Game");
			else{
				int count=0;
				foreach(RealShip Ship in Player1Ships){
					if(IsReady(Ship))
						count++;
				}
				foreach(RealShip Ship in Player2Ships){
					if(IsReady(Ship))
						count++;
				}
				Write("Waiting for Ships ("+count.ToString()+"/10)");
			}
			if(Selection==1)
				s="> ";
			else
				s="";
			Write(s+"Player Count: "+Player_Count.ToString());
			if(Selection==2)
				s="> ";
			else
				s="";
			if(Player_Count<2){
				string dif="";
				switch(AI_Difficulty){
					case 0:
						dif="Hard";
						break;
					case 1:
						dif="Normal";
						break;
					case 2:
						dif="Easy";
						break;
				}
				Write(s+"AI Difficulty: "+dif);
			}
			
			if(Selection==3)
				s="> ";
			else
				s="";
			if(Turn_Timer<30)
				Write(s+"Turn Timer: Off");
			else
				Write(s+"Turn Timer: "+Math.Round(Turn_Timer,0).ToString()+"s");
			if(Turn_Timer>=30){
				if(Selection==4)
					s="> ";
				else
					s="";
				Write(s+"Allow Pause: "+Allow_Pause.ToString());
			}
			if(Selection==5)
				s="> ";
			else
				s="";
			Write(s+"Use Real Ships: "+Use_Real_Ships.ToString());
			if(Selection==6)
				s="> ";
			else
				s="";
			if(Use_Real_Ships)
				Write(s+"Destroy Ships: "+Destroy_Ships.ToString());
			if(Selection==7)
				s="> ";
			else
				s="";
			Write(s+"See Opponent's Choice: "+See_Opponent_Choice.ToString());
		}
		if((int)Status<=((int)GameStatus.Awaiting)&&Last_Winner<0){
			DisplayCheck(Player1Enemy);
			DisplayCheck(Player1Own);
			DisplayCheck(Player2Enemy);
			DisplayCheck(Player2Own);
		}
		if(Status==GameStatus.Awaiting){
			Write("Go to Available Room for game to start");
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
					Door.Enabled=Door.Status!=DoorStatus.Closed||Player_Count==0;
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
					Door.Enabled=Door.Status!=DoorStatus.Closed||Player_Count==0;
					Door.CloseDoor();
				}
				Player2Text="Waiting for Player 1...\n";
				HubText+="Ready!";
			}
			if(Player_1_Ready&&Player_2_Ready){
				Status=GameStatus.SettingUp;
			}
		}
		if(Status==GameStatus.SettingUp){
			HubText="Players placing ships\n";
			HubText+="Player 1: "+Player1.ReadyCount.ToString()+"/5\n";
			HubText+="Player 2: "+Player2.ReadyCount.ToString()+"/5\n";
			Player1Text="";
			Player2Text="";
			foreach(KeyValuePair<MyShip,bool> p in Player1.ReadyShips){
				if(p.Value){
					Player1Text+=p.Key.ToString()+" ready\n";
					break;
				}
			}
			foreach(KeyValuePair<MyShip,bool> p in Player2.ReadyShips){
				if(p.Value){
					Player2Text+=p.Key.ToString()+" ready\n";
					break;
				}
			}
			if(Player1.ReadyCount<5){
				Player1.CanMove=true;
				MyShip Ship=MyShip.Unknown;
				foreach(KeyValuePair<MyShip,bool> p in Player1.ReadyShips){
					if(!p.Value){
						Ship=p.Key;
						break;
					}
				}
				if(Player1.End1.X<0)
					Player1Text+="Place "+Ship.ToString()+" End 1";
				else if(Player1.End2.X<0)
					Player1Text+="Place "+Ship.ToString()+" End 2";
				if(!Player1.IsHuman){
					string p="Player 1:";
					if(Player1.End1.X<0){
						int x=Rnd.Next(0,7);
						int y=Rnd.Next(0,7);
						for(int i=0;i<x;i++)
							Argument_Processor(p+"Right");
						for(int i=0;i<y;i++)
							Argument_Processor(p+"Down");
						Argument_Processor(p+"Confirm");
					}
					if(Player1.End1.X>=0&&Player1.End2.X<0){
						switch(Rnd.Next(0,3)){
							case 0:
								Argument_Processor(p+"Up");
								break;
							case 1:
								Argument_Processor(p+"Left");
								break;
							case 2:
								Argument_Processor(p+"Down");
								break;
							case 3:
								Argument_Processor(p+"Right");
								break;
						}
						Argument_Processor(p+"Confirm");
					}
				}
			}
			else{
				Player1.CanMove=false;
				Player1Text="Waiting for Player 2 ("+Player2.ReadyCount.ToString()+"/5)";
				DisplayCheck(Player1Enemy);
			}
			DisplayOwn(Player1Own,Player1);
			DisplayEnemy(Player1Enemy,Player1);
			if(Player2.ReadyCount<5){
				Player2.CanMove=true;
				MyShip Ship=MyShip.Unknown;
				foreach(KeyValuePair<MyShip,bool> p in Player2.ReadyShips){
					if(!p.Value){
						Ship=p.Key;
						break;
					}
				}
				if(Player2.End1.X<0)
					Player2Text+="Place "+Ship.ToString()+" End 1";
				else if(Player2.End2.X<0)
					Player2Text+="Place "+Ship.ToString()+" End 2";
				if(!Player2.IsHuman){
					string p="Player 2:";
					if(Player2.End1.X<0){
						int x=Rnd.Next(0,7);
						int y=Rnd.Next(0,7);
						for(int i=0;i<x;i++)
							Argument_Processor(p+"Right");
						for(int i=0;i<y;i++)
							Argument_Processor(p+"Down");
						Argument_Processor(p+"Confirm");
					}
					if(Player2.End1.X>=0&&Player2.End2.X<0){
						switch(Rnd.Next(0,3)){
							case 0:
								Argument_Processor(p+"Up");
								break;
							case 1:
								Argument_Processor(p+"Left");
								break;
							case 2:
								Argument_Processor(p+"Down");
								break;
							case 3:
								Argument_Processor(p+"Right");
								break;
						}
						Argument_Processor(p+"Confirm");
					}
				}
			}
			else{
				Player2.CanMove=false;
				Player2Text="Waiting for Player 1 ("+Player1.ReadyCount.ToString()+"/5)";
				DisplayCheck(Player2Enemy);
			}
			DisplayOwn(Player2Own,Player2);
			DisplayEnemy(Player2Enemy,Player2);
			if(Player1.ReadyCount+Player2.ReadyCount==10){
				Player1.Selection=new Vector2(0,0);
				Player2.Selection=new Vector2(0,0);
				Status=GameStatus.InProgress;
				Player_Turn=1;
				Player_Timer=Turn_Timer;
				AI_Selection=new Vector2(-1,-1);
				Room1Sound.Sounds.Enqueue("Weapons ArmedId");
				Room2Sound.Sounds.Enqueue("Weapons ArmedId");
				Release_Number=4;
				Release_Timer=0;
				Ready_Timer=0;
				SetUp_Timer=0;
				Started_Game=false;
			}
		}
		if((!call_return)&&Status<=GameStatus.SettingUp){
			for(int i=1;i<=2;i++){
				List<RealShip> ShipList;
				if(i==1)
					ShipList=Player1Ships;
				else
					ShipList=Player2Ships;
				foreach(RealShip Ship in ShipList){
					if(Ship.Status!=ShipStatus.Returning&&((int)Ship.Status)>((int)ShipStatus.Waiting)){
						call_return=true;
						break;
					}
					if(call_return)
						break;
				}
				
			}
		}
		if(Ready_Timer<5)
			Ready_Timer+=seconds_since_last_update;
		List<ShipStatus> ValidStatuses=new List<ShipStatus>();
		ValidStatuses.Add(ShipStatus.InPosition);
		ValidStatuses.Add(ShipStatus.Detonating);
		bool ships_are_ready=Started_Game||(!Use_Real_Ships)||(ReadyShips(ValidStatuses)&&Ready_Timer>=5);
		bool cannon_is_ready=(!Destroy_Ships)||Is_Cannon_Ready;
		if(Status==GameStatus.InProgress){
			if(ships_are_ready&&cannon_is_ready){
				if(Was_Firing){
					if(Player_Turn==1)
						CallHit(Player1,Player2,Room1Sound,Room2Sound);
					else 
						CallHit(Player2,Player1,Room2Sound,Room1Sound);
				}
				if(Player1.Paused||Player2.Paused){
					Status=GameStatus.Paused;
				}
				else{
					Started_Game=true;
					Decoy_Target=new Vector3D(0,0,0);
					if(Player_Turn<0||Player_Turn>2)
						Player_Turn=1;
					if(Turn_Timer>=30){
						Player_Timer-=seconds_since_last_update;
						if(Player_Timer<0){
							Player_Turn=(Player_Turn%2)+1;
							Player_Timer=Turn_Timer;
							AI_Selection=new Vector2(-1,-1);
						}
					}
					HubText="Player "+Player_Turn.ToString()+"'s Turn\n";
					Player1.CanMove=Player_Turn==1;
					Player2.CanMove=Player_Turn==2;
					if(Player_Turn==1){
						Player1Text="Your Turn!\n";
						Player2Text=HubText;
						if(Turn_Timer>=30&&Math.Abs(Player_Turn-5.5)<1&&Room1Sound.Sounds.Count==0)
							Room1Sound.Sounds.Enqueue("5 Second CountdownId");
						if(!Player1.IsHuman){
							if(AI_Selection.X<0){
								List<Vector2> pos=Player1.EnemyBoard.GetBestChoices(AI_Difficulty);
								if(pos.Count>0)
									AI_Selection=pos[Rnd.Next(0,pos.Count-1)];
							}
							if(AI_Selection.X>=0&&AI_Timer>0.25){
								AI_Timer=0;
								string s="Player 1:";
								if(AI_Selection==Player1.Selection)
									Argument_Processor(s+"Confirm");
								else if(AI_Selection.X<Player1.Selection.X)
									Argument_Processor(s+"Left");
								else if(AI_Selection.X>Player1.Selection.X)
									Argument_Processor(s+"Right");
								else if(AI_Selection.Y<Player1.Selection.Y)
									Argument_Processor(s+"Up");
								else if(AI_Selection.Y>Player1.Selection.Y)
									Argument_Processor(s+"Down");
							}
						}
					}
					else{
						Player1Text=HubText;
						Player2Text="Your Turn!\n";
						if(Turn_Timer>=30&&Math.Abs(Player_Turn-5.5)<1&&Room2Sound.Sounds.Count==0)
							Room2Sound.Sounds.Enqueue("5 Second CountdownId");
						if(!Player2.IsHuman){
							if(AI_Selection.X<0){
								List<Vector2> pos=Player2.EnemyBoard.GetBestChoices(AI_Difficulty);
								if(pos.Count>0)
									AI_Selection=pos[Rnd.Next(0,pos.Count-1)];
							}
							if(AI_Selection.X>=0&&AI_Timer>0.25){
								AI_Timer=0;
								string s="Player 2:";
								if(AI_Selection==Player2.Selection)
									Argument_Processor(s+"Confirm");
								else if(AI_Selection.X<Player2.Selection.X)
									Argument_Processor(s+"Left");
								else if(AI_Selection.X>Player2.Selection.X)
									Argument_Processor(s+"Right");
								else if(AI_Selection.Y<Player2.Selection.Y)
									Argument_Processor(s+"Up");
								else if(AI_Selection.Y>Player2.Selection.Y)
									Argument_Processor(s+"Down");
							}
						}
					}
					if(Turn_Timer>=30){
						string timer=Math.Round(Player_Timer,0).ToString()+"s Remaining\n";
						HubText+=timer;
						Player1Text+=timer;
						Player2Text+=timer;
					}
					if(See_Opponent_Choice&&Player_Turn==2)
						DisplayOwn(Player1Own,Player1,Player2.Selection);
					else
						DisplayOwn(Player1Own,Player1);
					DisplayEnemy(Player1Enemy,Player1);
					if(See_Opponent_Choice&&Player_Turn==1)
						DisplayOwn(Player2Own,Player2,Player1.Selection);
					else
						DisplayOwn(Player2Own,Player2);
					DisplayEnemy(Player2Enemy,Player2);
					if(Player1.OwnBoard.RemainingSpaces==0){
						Last_Winner=2;
						Status=GameStatus.Ready;
						Player1=null;
						Player2=null;
						Room1Sound.Sounds.Clear();
						Room2Sound.Sounds.Clear();
						Room1Sound.Sounds.Enqueue("Shutting DownId");
						Room2Sound.Sounds.Enqueue("SoundBlockObjectiveComplete");
						HubSound.Sounds.Enqueue("Objective CompleteId");
						Release_Number=0;
						Release_Timer=0;
						if(Use_Real_Ships)
							CallReturn();
					}
					else if(Player2.OwnBoard.RemainingSpaces==0){
						Last_Winner=1;
						Status=GameStatus.Ready;
						Player1=null;
						Player2=null;
						Room1Sound.Sounds.Clear();
						Room2Sound.Sounds.Clear();
						Room2Sound.Sounds.Enqueue("Shutting DownId");
						Room1Sound.Sounds.Enqueue("SoundBlockObjectiveComplete");
						HubSound.Sounds.Enqueue("Objective CompleteId");
						Release_Number=0;
						Release_Timer=0;
						if(Use_Real_Ships)
							CallReturn();
					}
				}
			}
			else if(!ships_are_ready){
				HubText="Waiting for Ships:";
				Player1Text="Waiting for Ships:";
				Player2Text="Waiting for Ships:";
				if(SetUp_Timer<190)
					HubText+="\n~"+Math.Round(190-SetUp_Timer,0).ToString()+" seconds remaining";
				else
					HubText+="\nAlmost ready...";
				if(SetUp_Timer<300)
					SetUp_Timer+=seconds_since_last_update;
				int ready=0;
				int traveling=0;
				int receiving=0;
				int waiting=0;
				int disconnected=0;
				int other=0;
				if(Release_Timer>=30&&Release_Number>0){
					Release_Number--;
					Release_Timer=0;
				}
				for(int i=1;i<=2;i++){
					List<RealShip> ShipList;
					if(i==1)
						ShipList=Player1Ships;
					else
						ShipList=Player2Ships;
					for(int j=1;j<=5;j++){
						if(ShipList[j-1].Status==ShipStatus.InPosition&&ShipList[j-1].Timer<60)
							ready++;
						else if(ShipList[j-1].Status==ShipStatus.Traveling)
							traveling++;
						else if(ShipList[j-1].Status==ShipStatus.Receiving)
							receiving++;
						else if(ShipList[j-1].Status==ShipStatus.Waiting)
							waiting++;
						else if(ShipList[j-1].Timer>60)
							disconnected++;
						else
							other++;
						if(Release_Number<=(j-1)&&(int)ShipList[j-1].Status<(int)ShipStatus.Traveling){
							RealShip ship=ShipList[j-1];
							Vector3D ship_forward=ship.End1-ship.End2;
							ship_forward.Normalize();
							Vector3D ship_up;
							if(i==1)
								ship_up=Left_Vector;
							else
								ship_up=Right_Vector;
							string message="Ends•"+ship.End1.ToString()+"•"+ship.End2.ToString()+"•"+ship_forward.ToString()+"•"+ship_up.ToString();
							Echo("Sending message to \n  "+ship.Tag_Full);
							IGC.SendBroadcastMessage(ship.Tag_Full,message,TransmissionDistance.TransmissionDistanceMax);
						}
					}
				}
				if(ready<10)
					Ready_Timer=0;
				if(ready>0)
					HubText+="\n  "+ready.ToString()+"/10 Ready";
				if(disconnected>0)
					HubText+="\n  "+disconnected.ToString()+" Disconnected";
				if(traveling>0)
					HubText+="\n  "+traveling.ToString()+" Traveling";
				if(receiving>0)
					HubText+="\n  "+receiving.ToString()+" Receiving";
				if(waiting>0)
					HubText+="\n  "+waiting.ToString()+" Waiting";
				if(other>0)
					HubText+="\n  "+other.ToString()+" Other";
				if(Release_Number>0){
					HubText+="\nSent "+(5-Release_Number).ToString()+"/5 Ships\n"+Math.Round(30-Release_Timer,1)+" seconds to next batch";
				}
				for(int i=5;i>Math.Max(0,Release_Number);i--)
					HubText+="\nReleased "+((MyShip)i).ToString()+"s";
				if(Release_Timer<30)
					Release_Timer+=seconds_since_last_update;
				Player1Text=HubText;
				Player2Text=HubText;
			}
			else if(!cannon_is_ready){
				HubText="Player "+Player_Turn.ToString()+" ";
				if(Cannon_FireStatus==FireStatus.Aiming)
					HubText+="Aiming... ("+loading_char+")";
				else
					HubText+="Firing... ("+loading_char+")\n"+Math.Round(Cannon_Seconds,1).ToString()+"s to detonation";
				if(Math.Abs(Cannon_Seconds-5.05)<=0.1){
					if(Room1Sound.Timer>=2&&Room1Sound.Sounds.Count==0){
						Room1Sound.Block.SelectedSound="5 Second CountdownId";
						Room1Sound.Block.Play();
					}
					if(Room2Sound.Timer>=2&&Room2Sound.Sounds.Count==0){
						Room2Sound.Block.SelectedSound="5 Second CountdownId";
						Room2Sound.Block.Play();
					}
					if(HubSound.Timer>=2&&HubSound.Sounds.Count==0){
						HubSound.Block.SelectedSound="5 Second CountdownId";
						HubSound.Block.Play();
					}
				}
				
				Player1Text=HubText;
				Player2Text=HubText;
				RealShip Targeted_Ship=GetTargetedShip();
				MyShip Targeted_Type=GetTargetedType();
				if(Targeted_Ship!=null)
					Write("Targeting "+Targeted_Ship.Tag);
				if(Initiated_Firing){
					Write("Firing...");
					Vector3D Target_Coords=GetTargetedCoordinates();
					if(Target_Coords.Length()>0){
						if(Decoy_Target.Length()==0){
							if(Fire_Timer>=10){
								Decoy_Target=Target_Coords;
							}
							else if(Targeted_Ship!=null){
								Write("Requesting Targeting Information...");
								if(Fire_Timer<10)
									Fire_Timer+=seconds_since_last_update;
								IGC.SendBroadcastMessage(Targeted_Ship.Tag_Full,"Request•"+Target_Coords.ToString(),TransmissionDistance.TransmissionDistanceMax);
							}
							else if(Targeted_Type==MyShip.None)
								Decoy_Target=Target_Coords;
							else
								Write("Cannot find Target Ship");
							
						}
						if(Decoy_Target.Length()>0&&Cannon_FireStatus==FireStatus.Idle&&Cannon_PrintStatus==PrintStatus.Ready&&!Vigilance.IsRunning){
							Write("Firing, now!");
							Room1Sound.Sounds.Enqueue("Weapons ArmedId");
							Room2Sound.Sounds.Enqueue("Weapons ArmedId");
							HubSound.Sounds.Enqueue("LoadingId");
							if(Targeted_Type!=MyShip.None){
								Player Defender;
								if(Player_Turn==1)
									Defender=Player2;
								else 
									Defender=Player1;
								if(Defender.OwnBoard.RemainingHits(Targeted_Type)<=1&&Targeted_Ship!=null)
									Targeted_Ship.Status=ShipStatus.Detonating;
							}
							Initiated_Firing=!Vigilance.TryRun("Fire:"+Decoy_Target.ToString());
						}
					}
					else
						Write("Cannot find Target Coords");
				}
				if(!Initiated_Firing){
					Write("Fired!");
					Fire_Timer=0;
					if(Targeted_Ship!=null&&Decoy_Target.Length()>0)
						IGC.SendBroadcastMessage(Targeted_Ship.Tag_Full,"Fire•"+Decoy_Target.ToString()+"•"+Cannon_Seconds.ToString(),TransmissionDistance.TransmissionDistanceMax);
				}
			}
			if(ships_are_ready&&Player1!=null&&Player2!=null){
				for(int i=1;i<=2;i++){
					Player Play;
					if(i==1)
						Play=Player1;
					else
						Play=Player2;
					string output="\nPlayer "+i.ToString();
					
					int sum=0;
					int count=0;
					for(int j=1;j<=5;j++){
						sum+=Play.OwnBoard.RemainingHits((MyShip)j);
						count+=ShipSize((MyShip)j);
					}
					output+=": "+Math.Round((100.0f*sum)/count,1).ToString()+"%\n[";
					for(int j=1;j<=5;j++){
						int Rem=Play.OwnBoard.RemainingHits((MyShip)j);
						int Total=ShipSize((MyShip)j);
						for(int k=0;k<Total;k++){
							if(k<Rem)
								output+="|";
							else
								output+=" ";
						}
					}
					output+="]";
					for(int j=1;j<=5;j++){
						int Rem=Play.OwnBoard.RemainingHits((MyShip)j);
						int Total=ShipSize((MyShip)j);
						if(Rem<Total&&Rem>0){
							output+="\n"+((MyShip)j).ToString()+"  [";
							for(int k=0;k<Total;k++){
								if(k<Rem)
									output+="|";
								else
									output+=" ";
							}
							output+="]";
						}
					}
					HubText+=output;
					if(i==1)
						Player1Text+=output;
					else
						Player2Text+=output;
				}
			}
		}
		Was_Firing=!cannon_is_ready;
		if(Status==GameStatus.Paused){
			HubText="Game is Paused\n";
			Player1Text="Game is Paused\n";
			Player2Text="Game is Paused\n";
			if(Player1.Paused){
				Player1Text+="Unpause when Ready\n";
				Player2Text="Paused by Player 1\n";
				HubText="Paused by Player 1\n";
			}
			if(Player2.Paused){
				Player1Text="Paused by Player 2\n";
				Player2Text+="Unpause when Ready\n";
				HubText="Paused by Player 2\n";
			}
			if((!Player1.Paused)&&(!Player2.Paused))
				Status=GameStatus.InProgress;
		}
		
		if(argument.Length>0){
			Argument_Processor(argument);
			while((Selection==6&&!Use_Real_Ships)||(Selection==0&&Status!=GameStatus.Ready)||(Selection==4&&Turn_Timer<30)||(Selection==2&&Player_Count==2)){
				Selection=(Selection+1)%8;
			}
		}
		
		if(Player1!=null&&Player1.Forfeiting)
			Player1Text="Press Confirm to Forfeit\n"+Player1Text;
		if(Player2!=null&&Player2.Forfeiting)
			Player2Text="Press Confirm to Forfeit\n"+Player2Text;
		
		Room1Sound.Update(seconds_since_last_update);
		Room2Sound.Update(seconds_since_last_update);
		HubSound.Update(seconds_since_last_update);
		
		if(((int)Status)>((int)GameStatus.Awaiting))
			Write(HubText);
		
		foreach(IMyTextPanel Panel in Player1StatusPanels)
			Panel.WriteText(Player1Text,false);
		foreach(IMyTextPanel Panel in Player2StatusPanels)
			Panel.WriteText(Player2Text,false);
		foreach(IMyTextPanel Panel in HubStatusPanels){
			Panel.WriteText(HubText,false);
			if(Player_Turn==1&&ships_are_ready)
				Panel.FontColor=new Color(255,137,137,255);
			else if(Player_Turn==2&&ships_are_ready)
				Panel.FontColor=new Color(137,137,255,255);
			else
				Panel.FontColor=new Color(255,255,255,255);
		}
	}
	catch (Exception e){
		try{
			Room1Sound.Block.SelectedSound="Malfunction DetectedId";
			Room1Sound.Block.Play();
			Room2Sound.Block.SelectedSound="Malfunction DetectedId";
			Room2Sound.Block.Play();
			HubSound.Block.SelectedSound="Malfunction DetectedId";
			HubSound.Block.Play();
			Me.CustomData=Status.ToString();
			Me.CustomData+="\nPlayer1Ships.Count="+Player1Ships.Count.ToString();
			Me.CustomData+="\nPlayer2Ships.Count="+Player2Ships.Count.ToString();
			foreach(IMyTextPanel Panel in Player1StatusPanels)
				Panel.WriteText(e.ToString(),false);
			foreach(IMyTextPanel Panel in Player2StatusPanels)
				Panel.WriteText(e.ToString(),false);
			foreach(IMyTextPanel Panel in HubStatusPanels)
				Panel.WriteText(e.ToString(),false);
		}
		catch(Exception){
			;
		}
		throw e;
	}
}
