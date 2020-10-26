/*
* Ship AI System 
* Built by mlq1616
* https://github.com/mlq1819
*/
//Name me!
private const string Program_Name = "Neptine Ship AI"; 
//The angle of what the ship will accept as "correct"
private const double ACCEPTABLE_ANGLE=20; //Suggested between 5° and 20°
//The distance accepted for raycasts
private const double RAYCAST_DISTANCE=2000; //Suggested below 10000
//Time between scans
private const double SCAN_TIME=3;

public class GenericMethods<T> where T : class, IMyTerminalBlock{
	private IMyGridTerminalSystem TerminalSystem;
	private IMyTerminalBlock Prog;
	private MyGridProgram Program;
	
	public GenericMethods(MyGridProgram Program){
		this.Program=Program;
		TerminalSystem=Program.GridTerminalSystem;
		Prog=Program.Me;
	}
	
	public T GetFull(string name, double max_distance, Vector3D Reference){
		List<T> AllBlocks=new List<T>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		double min_distance=max_distance;
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Equals(name)){
				double distance=(Reference-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance, distance);
				MyBlocks.Add(Block);
			}
		}
		foreach(T Block in MyBlocks){
			double distance=(Reference-Block.GetPosition()).Length();
			if(distance<=min_distance+0.1){
				return Block;
			}
		}
		return null;
	}
	
	public T GetFull(string name, double max_distance, IMyTerminalBlock Reference){
		return GetFull(name, max_distance, Reference.GetPosition());
	}
	
	public T GetFull(string name, double max_distance){
		return GetFull(name, max_distance, Prog);
	}
	
	public T GetFull(string name){
		return GetFull(name, double.MaxValue);
	}
	
	public T GetContaining(string name, double max_distance, Vector3D Reference){
		List<T> AllBlocks=new List<T>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		double min_distance=max_distance;
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Contains(name)){
				double distance=(Reference-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance, distance);
				MyBlocks.Add(Block);
			}
		}
		foreach(T Block in MyBlocks){
			double distance=(Reference-Block.GetPosition()).Length();
			if(distance<=min_distance+0.1){
				return Block;
			}
		}
		return null;
	}
	
	public T GetContaining(string name, double max_distance, IMyTerminalBlock Reference){
		return GetContaining(name, max_distance, Reference.GetPosition());
	}
	
	public T GetContaining(string name, double max_distance){
		return GetContaining(name, max_distance, Prog);
	}
	
	public T GetContaining(string name){
		return GetContaining(name, double.MaxValue);
	}
	
	public List<T> GetAllContaining(string name, double max_distance, Vector3D Reference){
		List<T> AllBlocks=new List<T>();
		List<List<T>> MyLists=new List<List<T>>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Contains(name)){
				bool has_with_name=false;
				for(int i=0; i<MyLists.Count && !has_with_name; i++){
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
			double min_distance=max_distance;
			foreach(T Block in list){
				double distance=(Reference-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance, distance);
			}
			foreach(T Block in list){
				double distance=(Reference-Block.GetPosition()).Length();
				if(distance<=min_distance+0.1){
					MyBlocks.Add(Block);
					break;
				}
			}
		}
		return MyBlocks;
	}
	
	public List<T> GetAllContaining(string name, double max_distance, IMyTerminalBlock Reference){
		return GetAllContaining(name, max_distance, Reference.GetPosition());
	}
	
	public List<T> GetAllContaining(string name, double max_distance){
		return GetAllContaining(name, max_distance, Prog);
	}
	
	public List<T> GetAllContaining(string name){
		return GetAllContaining(name, double.MaxValue);
	}
	
	public List<T> GetAllFunc(Func<T, bool> f){
		List<T> AllBlocks=new List<T>();
		List<T> MyBlocks=new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			if(f(Block)){
				MyBlocks.Add(Block);
			}
		}
		return MyBlocks;
	}
	
	public T GetClosestFunc(Func<T, bool> f, double max_distance, Vector3D Reference){
		List<T> MyBlocks=GetAllFunc(f);
		double min_distance=max_distance;
		foreach(T Block in MyBlocks){
			double distance=(Reference-Block.GetPosition()).Length();
			min_distance=Math.Min(min_distance, distance);
		}
		foreach(T Block in MyBlocks){
			double distance=(Reference-Block.GetPosition()).Length();
			if(distance<=min_distance+0.1){
				return Block;
			}
		}
		return null;
	}
	
	public T GetClosestFunc(Func<T, bool> f, double max_distance, IMyTerminalBlock Reference){
		return GetClosestFunc(f, max_distance, Reference.GetPosition());
	}
	
	public T GetClosestFunc(Func<T, bool> f, double max_distance){
		return GetClosestFunc(f, max_distance, Program.Me);
	}
	
	public T GetClosestFunc(Func<T, bool> f){
		return GetClosestFunc(f, double.MaxValue);
	}
	
	public static List<T> SortByDistance(List<T> unsorted, Vector3D Reference){
		List<T> output=new List<T>();
		while(unsorted.Count > 0){
			double min_distance=double.MaxValue;
			foreach(T Block in unsorted){
				double distance=(Reference-Block.GetPosition()).Length();
				min_distance=Math.Min(min_distance, distance);
			}
			for(int i=0; i<unsorted.Count; i++){
				double distance=(Reference-unsorted[i].GetPosition()).Length();
				if(distance<=min_distance+0.1){
					output.Add(unsorted[i]);
					unsorted.RemoveAt(i);
					break;
				}
			}
		}
		return output;
	}
	
	public static List<T> SortByDistance(List<T> unsorted, IMyTerminalBlock Reference){
		return SortByDistance(unsorted, Reference.GetPosition());
	}
	
	public List<T> SortByDistance(List<T> unsorted){
		return SortByDistance(unsorted, Prog);
	}
	
	public static double GetAngle(Vector3D v1, Vector3D v2){
		v1.Normalize();
		v2.Normalize();
		return Math.Round(Math.Acos(v1.X*v2.X+v1.Y*v2.Y+v1.Z*v2.Z) * 57.295755, 5);
	}
}

public class EntityInfo{
	public long ID;
	public string Name;
	public MyDetectedEntityType Type;
	private Vector3D? _hitposition;
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
	private Vector3D _velocity;
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
		return "X:"+((long)vector.X).ToString()+" Y:"+((long)vector.Y).ToString()+" Z:"+((long)vector.Z).ToString();
	}
	
	public void Update(double seconds){
		TimeSpan time=new TimeSpan((int)(seconds/60/60/24), ((int)(seconds/60/60))%24, ((int)(seconds/60))%60, ((int)(seconds))%60, ((int)(seconds*1000))%1000);
		Age.Add(time);
		Position+=seconds * Velocity;
		if(HitPosition!=null){
			HitPosition=(Vector3D?) (((Vector3D)HitPosition)+seconds * Velocity);
		}
	}
	
}

public class EntityList : IEnumerable<EntityInfo>{
	private List<EntityInfo> E_List;
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
	
	public EntityList(){
		E_List=new List<EntityInfo>();
	}
	
	public void UpdatePositions(double seconds){
		foreach(EntityInfo entity in E_List){
			entity.Update(seconds);
		}
	}
	
	public bool UpdateEntry(EntityInfo Entity){
		for(int i=0; i<E_List.Count; i++){
			if(E_List[i].ID==Entity.ID){
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
	
	public EntityInfo Get(long ID){
		foreach(EntityInfo entity in E_List){
			if(entity.ID==ID)
				return entity;
		}
		return null;
	}
	
	public double ClosestDistance(MyGridProgram P, MyRelationsBetweenPlayerAndBlock Relationship, double min_size=0){
		double min_distance=double.MaxValue;
		foreach(EntityInfo entity in E_List){
			if(entity.Size >= min_size && entity.Relationship==Relationship){
				min_distance=Math.Min(min_distance, (P.CubeGrid.GetPosition()-entity.Position).Length()-entity.Size);
			}
		}
		return min_distance;
	}
	
	public double ClosestDistance(MyGridProgram P, double min_size=0){
		double min_distance=double.MaxValue;
		foreach(EntityInfo entity in E_List){
			if(entity.Size >= min_size){
				min_distance=Math.Min(min_distance, (P.CubeGrid.GetPosition()-entity.Position).Length()-entity.Size);
			}
		}
		return min_distance;
	}
}

public enum MenuType{
	Submenu=0,
	Command=1,
	Display=2
}

public interface MenuOption{
	string Name{get;}
	MenuType Type{get;}
	bool AutoRefresh{get;}
	int Depth{get;}
	bool Back();
	bool Select();
}

public class Menu_Submenu : MenuOption{
	private string _Name;
	public string Name{
		get{
			return _Name;
		}
	}
	public MenuType Type{
		get{
			return MenuType.Submenu;;
		}
	}
	public bool AutoRefresh{
		get{
			if(IsSelected){
				return Menu[Selection].AutoRefresh;
			}
			return LastCount==Count;
		}
	}
	public int Depth{
		get{
			if(Selected){
				return 1+Menu[Selection].Depth;
			}
			return 1;
		}
	}
	private bool Selected;
	public bool IsSelected{
		get{
			return Selected;
		}
	}
	private int Selection;
	
	private int Last_Count;
	public int Count{
		get{
			return Menu.Count;
		}
	}
	
	private List<MenuOption> Menu;
	
	public Menu_Submenu(string name){
		_Name=name.Trim().Substring(0, Math.Min(name.Trim().Length, 24));
		Menu=new List<MenuOption>();
		Selection=0;
		Last_Count=0;
	}
	
	public bool Add(MenuOption new_item){
		foreach(MenuOption Item in Menu){
			if(Menu.Name==new_item.Name)
				return false;
		}
		Menu.Add(new_item);
		return true;
	}
	
	public bool Back(){
		if(Selected){
			if(Menu[Selection].Back())
				return true;
			Selected=false;
		}
		return false;
	}
	
	public bool Select(){
		if(Selected)
			return Menu[Selection].Select();
		Selected=true;
		return true;
	}
	
	public bool Next(){
		if(Selected){
			if(Menu[Selection].Type==MenuType.Submenu){
				return ((Menu_Submenu)(Menu[Selection])).Next();
			}
			return false;
		}
		Selection=(Selection+1)%Count;
		return true;
	}
	
	public bool Prev(){
		if(Selected){
			if(Menu[Selection].Type==MenuType.Submenu){
				return ((Menu_Submenu)(Menu[Selection])).Prev();
			}
			return false;
		}
		Selection=(Selection-1+Count)%Count;
		return true;
	}
	
	public override string ToString(){
		Selection=Selection%Count;
		if(Selected){
			return Menu[Selection].ToString();
		}
		string output=" -- "+Name+" -- ";
		for(int i=0; i<Menu.Count; i++){
			output+="\n ";
			switch(Selection.Type){
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
			if(Selection==i){
				output+=' '+Menu[i].Name.ToUpper()+' ';
			}
			else {
				output+=Menu[i].Name.ToLower();
			}
			switch(Selection.Type){
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
		return output;
	}
}

public class Menu_Command<T> : MenuOption where T : class{
	private string _Name;
	public string Name{
		get{
			return _Name;
		}
	}
	public MenuType Type{
		get{
			return MenuType.Command;
		}
	}
	private bool _AutoRefresh;
	public bool AutoRefresh{
		get{
			return _AutoRefresh;
		}
	}
	public int Depth{
		get{
			if(Selected)
				return 2;
			return 1;
		}
	}
	private string Desc;
	private T Arg;
	private Func<bool, T> Command;
	
	public Menu_Command(string name, Func<bool, T> command, string desc="No description provided", T arg=null, bool autorefresh=false){
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
		string output=Name+'\n';
		string words[]=desc.Split(' ');
		int length=32;
		foreach(string word in words){
			if(length > 0 && length+word.Length > 32){
				length=0;
				output+='\n';
			}
			else {
				output+=' ';
			}
			outut+=word;
			if(word.Contains('\n'))
				length=word.Length-word.IndexOf('\n')-1;
		}
		return output;
	}
}

public class Menu_Display : MenuOption{
	public string Name{
		get{
			return Entity.Name.Substring(0, Math.Min(24, Entity.Name.Length));
		}
	}
	public MenuType Type{
		get{
			return MenuType.Display;
		}
	}
	public bool AutoRefresh{
		get{
			return true;
		}
	}
	public int Depth{
		get{
			if(Selected)
				return 2;
			return 1;
		}
	}
	private bool Selected;
	private EntityInfo Entity;
	private bool Selected;
	private bool Can_GoTo;
	private Func<bool, EntityInfo> Command;
	private Menu_Command Subcommand {
		get{
			double distance=(Me.CubeGrid.GetPosition()-Entity.Position).Length-Entity.Size;
			return new Menu_Command<EntityInfo>("GoTo "+Entity.Name, Command, "Set autopilot to match target Entity's expected position and velocity", Entity);
		}
	}
	private MyGridProgram P;
	
	public Menu_Display(EntityInfo entity, MyGridProgram p, Func<bool, EntityInfo> GoTo){
		P=p;
		Entity=entity;
		Command=GoTo;
		Selected=false;
		Can_GoTo=true;
	}
	
	public Menu_Display(EntityInfo entity){
		Entity=entity;
		Selected=false;
		Can_GoTo=false;
	}
	
	public bool Select(){
		if(Can_GoTo)
			return false;
		if(Selected){
			return Subcommand.Select();
		}
		Selected=true;
		return true;
	}
	
	public bool Back(){
		if(!Selected){
			return false;
		}
		Selected=false;
		return true;
	}
	
	public override string ToString(){
		if(Selected){
			return Subcommand.ToString();
		}
		else {
			return Entity.NiceString();
		}
	}
}

private struct Airlock{
	public IMyDoor Door1;
	public IMyDoor Door2;
	public Airlock(IMyDoor d1, IMyDoor d2){
		Door1=d1;
		Door2=d2;
	}
	public bool Equals(Airlock o){
		return Door1.Equals(o.Door1) && Door2.Equals(o.Door2);
	}
	public double Distance(Vector3D Reference){
		double distance_1=(Reference-Door1.GetPosition()).Length();
		double distance_2=(Reference-Door2.GetPosition()).Length();
		return Math.Min(distance_1, distance_2);
	}
}

private long cycle_long=1;
private long cycle=0;
private char loading_char='|';
double seconds_since_last_update=0;
private Random Rnd;

private IMyShipController Controller;
private IMyGyro Gyro;

private List<IMyTextPanel> StatusLCDs;

private List<Airlock> Airlocks;

private EntityList AsteroidList;
private EntityList PlanetList;
private EntityList SmallShipList;
private EntityList LargeShipList;
private EntityList CharacterList;

private List<IMyThrust> Forward_Thrusters;
private List<IMyThrust> Backward_Thrusters;
private List<IMyThrust> Up_Thrusters;
private List<IMyThrust> Down_Thrusters;
private List<IMyThrust> Left_Thrusters;
private List<IMyThrust> Right_Thrusters;
private float Forward_Thrust{
	get{
		float total = 0;
		foreach(IMyThrust Thruster in Forward_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
private float Backward_Thrust{
	get{
		float total = 0;
		foreach(IMyThrust Thruster in Backward_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
private float Up_Thrust{
	get{
		float total = 0;
		foreach(IMyThrust Thruster in Up_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
private float Down_Thrust{
	get{
		float total = 0;
		foreach(IMyThrust Thruster in Down_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
private float Left_Thrust{
	get{
		float total = 0;
		foreach(IMyThrust Thruster in Left_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
private float Right_Thrust{
	get{
		float total = 0;
		foreach(IMyThrust Thruster in Right_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}

private Base6Directions.Direction Forward;
private Base6Directions.Direction Backward;
private Base6Directions.Direction Up;
private Base6Directions.Direction Down;
private Base6Directions.Direction Left;
private Base6Directions.Direction Right;

private Vector3D Forward_Vector;
private Vector3D Backward_Vector;
private Vector3D Up_Vector;
private Vector3D Down_Vector;
private Vector3D Left_Vector;
private Vector3D Right_Vector;

private Vector3D Controller_Forward;
private Vector3D Controller_Backward;
private Vector3D Controller_Up;
private Vector3D Controller_Down;
private Vector3D Controller_Left;
private Vector3D Controller_Right;

private bool Match_Direction = false;
private Vector3D Target_Direction;
private bool Match_Position = false;
private Vector3D Target_Position;


private Vector3D RestingVelocity;
private Vector3D Relative_RestingVelocity{
	get{
		return GlobalToLocal(RestingVelocity);
	}
}
private Vector3D CurrentVelocity;
private Vector3D Relative_CurrentVelocity{
	get{
		return GlobalToLocal(CurrentVelocity);
	}
}
private Vector3D Gravity;
private Vector3D Relative_Gravity{
	get{
		return GlobalToLocal(Gravity);
	}
}

private double Elevation;
private double Sealevel;
private Vector3D PlanetCenter;


private bool HasBlockData(IMyTerminalBlock Block, string Name){
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

private string GetBlockData(IMyTerminalBlock Block, string Name){
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

private bool SetBlockData(IMyTerminalBlock Block, string Name, string Data){
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

public void Write(string text, bool new_line=true, bool append=true){
	Echo(text);
	if(new_line)
		Me.GetSurface(0).WriteText(text+'\n', append);
	else
		Me.GetSurface(0).WriteText(text, append);
}

public void Reset(){
	Write("Resetting");
	Operational=false;
	Controller=null;
	Gyroscope=null;
	AsteroidList=new EntityList();
	PlanetList=new EntityList();
	SmallShipList=new EntityList();
	LargeShipList=new EntityList();
	CharacterList=new EntityList();
	StatusLCDs=new List<IMyTextPanel>();
	List<Airlock> Airlocks=new List<Airlock>();
	Forward_Thrusters=new List<IMyThrust>();
	Backward_Thrusters=new List<IMyThrust>();
	Up_Thrusters=new List<IMyThrust>();
	Down_Thrusters=new List<IMyThrust>();
	Left_Thrusters=new List<IMyThrust>();
	Right_Thrusters=new List<IMyThrust>();
	RestingVelocity = new Vector3D(0,0,0);
	Runtime.UpdateFrequency=UpdateFrequency.None;
}

private bool ControllerFunction(IMyShipController ctrlr){
	IMyRemoteControl Remote=ctrlr as IMyRemoteControl;
	if(Remote!=null)
		return ctrlr.ControlThrusters;
	else
		return (ctrlr.ControlThrusters && ctrlr.IsMainCockpit);
}

private void SetupAirlocks(){
	Airlocks=new List<Airlock>();
	List<IMyDoor> AllAirlockDoors=(new GenericMethods<IMyDoor>(this)).GetAllContaining("Airlock");
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
		string name=GetRemovedString(Door1.CustomName, "Door 1");
		foreach(IMyDoor Door2 in AllAirlockDoor2s){
			Copy.Add(Door2);
		}
		foreach(IMyDoor Door2 in GenericMethods<IMyDoor>.SortByDistance(Copy, Door1)){
			if(GetRemovedString(Door2.CustomName, "Door 2").Equals(name))
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
		string name=GetRemovedString(Door2.CustomName, "Door 2");
		foreach(IMyDoor Door1 in AllAirlockDoor1s){
			Copy.Add(Door1);
		}
		foreach(IMyDoor Door1 in GenericMethods<IMyDoor>.SortByDistance(Copy, Door2)){
			if(GetRemovedString(Door1.CustomName, "Door 1").Equals(name))
				pair.Add(Door1);
		}
		if(pair.Count > 1){
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
				if(pair2[0].Equals(pair1[1]) && pair1[0].Equals(pair2[1])){
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
	while(removed > 0 && PossibleAirlockDoor1Pairs.Count > 0 && PossibleAirlockDoor2Pairs.Count > 0);
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
}

private void SetControllerDirections(){
	Forward = Controller.Orientation.Forward;
	switch(Forward){
		case Base6Directions.Direction.Forward:
			Backward = Base6Directions.Direction.Backward;
			break;
		case Base6Directions.Direction.Backward:
			Backward = Base6Directions.Direction.Forward;
			break;
		case Base6Directions.Direction.Up:
			Backward = Base6Directions.Direction.Down;
			break;
		case Base6Directions.Direction.Down:
			Backward = Base6Directions.Direction.Up;
			break;
		case Base6Directions.Direction.Left:
			Backward = Base6Directions.Direction.Right;
			break;
		case Base6Directions.Direction.Right:
			Backward = Base6Directions.Direction.Left;
			break;
	}
	Up = Controller.Orientation.Up;
	switch(Up){
		case Base6Directions.Direction.Forward:
			Down = Base6Directions.Direction.Backward;
			break;
		case Base6Directions.Direction.Backward:
			Down = Base6Directions.Direction.Forward;
			break;
		case Base6Directions.Direction.Up:
			Down = Base6Directions.Direction.Down;
			break;
		case Base6Directions.Direction.Down:
			Down = Base6Directions.Direction.Up;
			break;
		case Base6Directions.Direction.Left:
			Down = Base6Directions.Direction.Right;
			break;
		case Base6Directions.Direction.Right:
			Down = Base6Directions.Direction.Left;
			break;
	}
	Left = Controller.Orientation.Left;
	switch(Left){
		case Base6Directions.Direction.Forward:
			Right = Base6Directions.Direction.Backward;
			break;
		case Base6Directions.Direction.Backward:
			Right = Base6Directions.Direction.Forward;
			break;
		case Base6Directions.Direction.Up:
			Right = Base6Directions.Direction.Down;
			break;
		case Base6Directions.Direction.Down:
			Right = Base6Directions.Direction.Up;
			break;
		case Base6Directions.Direction.Left:
			Right = Base6Directions.Direction.Right;
			break;
		case Base6Directions.Direction.Right:
			Right = Base6Directions.Direction.Left;
			break;
	}
}

private UpdateFrequency GetUpdateFrequency(){
	if(!Operational)
		return UpdateFrequency.None;
	if(Controller.IsUnderControl)
		return UpdateFrequency.Update1;
	if(Controller.GetShipVelocities().AngularVelocity > .1f)
		return UpdateFrequency.Update1;
	if((Controller.GetShipVelocities() - RestingVelocity).Length() > .5)
		return UpdateFrequency.Update1;
	return UpdateFrequency.Update10;
}

public bool Setup(){
	Reset();
	
	StatusLCDs=(new GenericMethods<IMyTextPanel>(this)).GetAllContaining("Ship Status");
	
	SetupAirlocks();
	
	Controller=(new GenericMethods<IMyShipController>(this)).GetClosestFunc(ControllerFunction);
	if(Controller==null){
		Write("Failed to find Controller", false, false);
		return false;
	}
	else {
		SetControllerDirections();
	}
	Gyroscope=(new GenericMethods<IMyGyro>(this)).GetContaining("Control Gyroscope");
	if(Gyroscope==null){
		Write("Failed to find Gyroscope", false, false);
		return false;
	}
	else {
		Gyroscope.GyroOverride=Controller.IsUnderControl;
	}
	
	List<IMyThrust> MyThrusters=(new GenericMethods<IMyThrust>(this)).GetAllContaining("");
	foreach(IMyThrust Thruster in MyThrusters){
		if(HasBlockData(Thruster, "Owner")){
			long ID = 0;
			if(!Int64.TryParse(GetBlockData(Thruster, "Owner"), out ID) || (ID != 0 && ID!=Me.EntityId))
				continue;
		}
		Base6Directions.Direction ThrustDirection=Thruster.Orientation.Forward;
		if(ThrustDirection==Backward){
			Forward_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection==Forward){
			Backward_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection==Down){
			Up_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection==Up){
			Down_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection==Right){
			Left_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection==Left){
			Right_Thrusters.Add(Thruster);
		}
	}
	SetThrusters(Forward_Thrusters, "Forward");
	SetThrusters(Backward_Thrusters, "Backward");
	SetThrusters(Up_Thrusters, "Up");
	SetThrusters(Down_Thrusters, "Down");
	SetThrusters(Left_Thrusters, "Left");
	SetThrusters(Right_Thrusters, "Right");
	
	Operational=Me.IsWorking;
	Runtime.UpdateFrequency = GetUpdateFrequency();
	return true;
}

private bool Operational=false;

public Program()
{
    Me.CustomName=(Program_Name+" Programmable block").Trim();
	Echo("Beginning initialization");
	Me.Enabled = true;
	Rnd = new Random();
	Setup();
	string[] args = this.Storage.Split('•');
	foreach(string arg in args){
		EntityInfo Entity = null;
		if(EntityInfo.TryParse(arg, out Entity)){
			switch(Entity.Type){
				case MyDetectedEntityType.Asteroid:
					AsteroidList.Add(Entity);
					break;
				case MyDetectedEntityType.Planet:
					PlanetList.Add(Entity);
					break;
				case MyDetectedEntityType.SmallGrid:
					SmallShipList.Add(Entity);
					break;
				case MyDetectedEntityType.LargeGrid:
					LargeShipList.Add(Entity);
					break;
				case MyDetectedEntityType.CharacterHuman:
					CharacterList.Add(Entity);
					break;
				case MyDetectedEntityType.CharacterOther:
					CharacterList.Add(Entity);
					break;
			}
		}
	}
	IGC.RegisterBroadcastListener("Neptine AI");
	IGC.RegisterBroadcastListener("Entity Report");
	IGC.RegisterBroadcastListener(Me.CubeGrid.CustomName);
}

private void SetThrusters(List<IMyThrust> Thrusters, string Direction){
	int small_misc=0;
	int large_misc=0;
	int small_hydrogen=0;
	int large_hydrogen=0;
	int small_atmospheric=0;
	int large_atmospheric=0;
	int small_ion=0;
	int large_ion=0;
	
	foreach(IMyThrust Thruster in Thrusters){
		if(!HasBlockData(Thruster, "DefaultOverride")){
			SetBlockData(Thruster, "DefaultOverride", Thruster.ThrustOverridePercentage.ToString());
		}
		SetBlockData(Thruster, "Owner", Me.EntityId.ToString());
		SetBlockData(Thruster, "DefaultName", Thruster.CustomName);
		if(Thruster.CustomName.ToLower().Contains("hydrogen")){
			if(Thruster.CustomName.ToLower().Contains("large"))
				large_hydrogen++;
			else
				small_hydrogen++;
		}
		else if(Thruster.CustomName.ToLower().Contains("atmospheric")){
			if(Thruster.CustomName.ToLower().Contains("large"))
				large_atmospheric++;
			else
				small_atmospheric++;
		}
		else if(Thruster.CustomName.ToLower().Contains("ion")){
			if(Thruster.CustomName.ToLower().Contains("large"))
				large_ion++;
			else
				small_ion++;
		}
		else{
			if(Thruster.CustomName.ToLower().Contains("large"))
				large_misc++;
			else
				small_misc++;
		}
	}
	foreach(IMyThrust Thruster in Thrusters){
		string tag;
		int number;
		if(Thruster.CustomName.ToLower().Contains("hydrogen")){
			if(Thruster.CustomName.ToLower().Contains("large")){
				tag="Large Hydrogen"
				number=large_hydrogen--;
			}
			else {
				tag="Small Hydrogen"
				number=small_hydrogen--;
			}
		}
		else if(Thruster.CustomName.ToLower().Contains("atmospheric")){
			if(Thruster.CustomName.ToLower().Contains("large")){
				tag="Large Atmospheric"
				number=large_atmospheric--;
			}
			else {
				tag="Small Atmospheric"
				number=small_atmospheric--;
			}
		}
		else if(Thruster.CustomName.ToLower().Contains("ion")){
			if(Thruster.CustomName.ToLower().Contains("large")){
				tag="Large Ion"
				number=large_ion--;
			}
			else {
				tag="Small Ion"
				number=small_ion--;
			}
		}
		else{
			if(Thruster.CustomName.ToLower().Contains("large")){
				tag="Large Misc"
				number=large_misc--;
			}
			else {
				tag="Small Misc"
				number=small_misc--;
			}
		}
		Thruster.CustomName = Direction + tag + " Thruster " + number.ToString();
	}
}

private void ResetThruster(IMyThrust Thruster){
	if(HasBlockData(Thruster, "DefaultOverride")){
		if(!float.TryParse(GetBlockData(Thruster, "DefaultOverride"), out Thruster.ThrustOverridePercentage))
			Thruster.ThrustOverridePercentage = 0.0f;
	}
	if(HasBlockData(Thruster, "DefaultName")){
		Thruster.CustomName = GetBlockData(Thruster, "DefaultName");
	}
	SetBlockData(Thruster, "Owner", "0");
}

public void Save()
{
    this.Storage = "";
	foreach(EntityInfo Entity in AsteroidList){
		this.Storage += '•' + Entity.ToString();
	}
	foreach(EntityInfo Entity in PlanetList){
		this.Storage += '•' + Entity.ToString();
	}
	foreach(EntityInfo Entity in SmallShipList){
		this.Storage += '•' + Entity.ToString();
	}
	foreach(EntityInfo Entity in LargeShipList){
		this.Storage += '•' + Entity.ToString();
	}
	foreach(EntityInfo Entity in CharacterList){
		this.Storage += '•' + Entity.ToString();
	}
	Me.CustomData = this.Storage;
	
	Gyroscope.GyroOverride = false;
	foreach(IMyThrust Thruster in Forward_Thrusters){
		ResetThruster(Thruster);
	}
	foreach(IMyThrust Thruster in Backward_Thrusters){
		ResetThruster(Thruster);
	}
	foreach(IMyThrust Thruster in Up_Thrusters){
		ResetThruster(Thruster);
	}
	foreach(IMyThrust Thruster in Down_Thrusters){
		ResetThruster(Thruster);
	}
	foreach(IMyThrust Thruster in Left_Thrusters){
		ResetThruster(Thruster);
	}
	foreach(IMyThrust Thruster in Right_Thrusters){
		ResetThruster(Thruster);
	}
	Write("Powering Off...", false, false);
	Runtime.UpdateFrequency = UpdateFrequency.None;
}

public Vector3D GlobalToLocal(Vector3D Global){
	double Length = Global.Length();
	Vector3D Local = Vector3D.Transform(Global, MatrixD.Invert(Controller.WorldMatrix));
	Local.Normalize();
	return Local * Length;
}

public Vector3D LocalToGlobal(Vector3D Local){
	double Length = Local.Length();
	Vector3D Global = Vector3D.Transform(Local, Controller.WorldMatrix);
	Global.Normalize();
	return Global * Length;
}

//Sets directional vectors, elevation, etc
private void GetPositionData(){
	Vector3D base_vector = new Vector3D(0,0,10);
	Forward_Vector = Vector3D.Transform(base_vector, Controller.WorldMatrix) - Controller.GetPosition();
	Forward_Vector.Normalize();
	Backward_Vector = -1 * Forward_Vector;
	
	base_vector = new Vector3D(0,10,0);
	Up_Vector = Vector3D.Transform(base_vector, Controller.WorldMatrix) - Controller.GetPosition();
	Up_Vector.Normalize();
	Down_Vector = -1 * Up_Vector;
	
	base_vector = new Vector3D(10,0,0);
	Left_Vector = Vector3D.Transform(base_vector, Controller.WorldMatrix) - Controller.GetPosition();
	Left_Vector.Normalize();
	Right_Vector = -1 * Left_Vector;
	
	switch(Forward){
		case Base6Directions.Direction.Forward:
			Controller_Forward = Forward_Vector;
			Controller_Backward = Backward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Forward = Backward_Vector;
			Controller_Backward = Forward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Forward = Up_Vector;
			Controller_Backward = Down_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Forward = Down_Vector;
			Controller_Backward = Up_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Forward = Left_Vector;
			Controller_Backward = Right_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Forward = Right_Vector;
			Controller_Backward = Left_Vector;
			break;
	}
	switch(Up){
		case Base6Directions.Direction.Forward:
			Controller_Up = Forward_Vector;
			Controller_Down = Backward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Up = Backward_Vector;
			Controller_Down = Forward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Up = Up_Vector;
			Controller_Down = Down_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Up = Down_Vector;
			Controller_Down = Up_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Up = Left_Vector;
			Controller_Down = Right_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Up = Right_Vector;
			Controller_Down = Left_Vector;
			break;
	}
	switch(Left){
		case Base6Directions.Direction.Forward:
			Controller_Left = Forward_Vector;
			Controller_Right = Backward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Left = Backward_Vector;
			Controller_Right = Forward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Left = Up_Vector;
			Controller_Right = Down_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Left = Down_Vector;
			Controller_Right = Up_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Left = Left_Vector;
			Controller_Right = Right_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Left = Right_Vector;
			Controller_Right = Left_Vector;
			break;
	}
	Forward_Thrust=0.0f;
	foreach(IMyThrust Thruster in Forward_Thrusters){
		if(Thruster.IsWorking)
			Forward_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Backward_Thrust=0.0f;
	foreach(IMyThrust Thruster in Backward_Thrusters){
		if(Thruster.IsWorking)
			Backward_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Up_Thrust=0.0f;
	foreach(IMyThrust Thruster in Up_Thrusters){
		if(Thruster.IsWorking)
			Up_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Down_Thrust=0.0f;
	foreach(IMyThrust Thruster in Down_Thrusters){
		if(Thruster.IsWorking)
			Down_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Left_Thrust=0.0f;
	foreach(IMyThrust Thruster in Left_Thrusters){
		if(Thruster.IsWorking)
			Left_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Right_Thrust=0.0f;
	foreach(IMyThrust Thruster in Right_Thrusters){
		if(Thruster.IsWorking)
			Right_Thrust+=Thruster.MaxEffectiveThrust;
	}
	
	if(Controller.TryGetPlanetElevation(MyPlanetElevation.SeaLevel, out Sealevel)){
		if(Controller.TryGetPlanetPosition(out PlanetCenter)){
			if(Sealevel < 6000 && Controller.TryGetPlanetPosition(MyPlanetElevation.Surface, out Elevation)){
				if(Sealevel > 5000){
					double difference = Sealevel - 5000;
					Elevation =  ((Elevation * (1000-difference)) + (Sealevel * difference)) / 1000;
				}
				else if(Elevation < 50){
					double terrain_height = (Controller.GetPosition() - PlanetCenter).Length() - Elevation;
					List<IMyTerminalBlock> AllBlocks = new List<IMyTerminalBlock>();
					GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(AllBlocks);
					foreach(IMyTerminalBlock Block in AllBlocks){
						Elevation = Math.Min(Elevation, (Block.GetPosition() - PlanetCenter)/Length() - terrain_height);
					}
				}
			}
			else {
				Elevation = Sealevel;
			}
		}
		else {
			PlanetCenter = new Vector3D(0,0,0);
		}
	}
	else{
		Sealevel = double.MaxValue;
	}
	
	Gravity = Controller.GetNaturalGravity();
	CurrentVelocity=Controller.GetShipVelocities().LinearVelocity;
}

private void UpdateList(List<MyDetectedEntityInfo> list, MyDetectedEntityInfo new_entity){
	if(new_entity.Type == MyDetectedEntityType.None || new_entity.EntityId == Me.CubeGrid.EntityId)
		return;
	for(int i=0; i<list.Count; i++){
		if(list[i].EntityId == new_entity.EntityId){
			if(list[i].TimeStamp < new_entity.TimeStamp || ((list[i].HitPosition==null)&&(new_entity.HitPosition!=null)))
				list[i] = new_entity;
			return;
		}
	}
	list.Add(new_entity);
}

private void UpdateList(List<EntityInfo> list, EntityInfo new_entity){
	if(new_entity.Type == MyDetectedEntityType.None || new_entity.ID == Me.CubeGrid.EntityId)
		return;
	for(int i=0; i<list.Count; i++){
		if(list[i].ID == new_entity.EntityId){
			list[i] = new_entity;
			return;
		}
	}
	list.Add(new_entity);
}

private string ScanString = "";

//Performs the scan function on all scanning devices
public void PerformScan(){
	Write("Beginning Scan");
	ScanString = "";
	PerformDisarm();
	List<MyDetectedEntityInfo> DetectedEntities = new List<MyDetectedEntityInfo>();
	List<IMySensorBlock> AllSensors = new List<IMySensorBlock>();
	GridTerminalSystem.GetBlocksOfType<IMySensorBlock>(AllSensors);
	foreach(IMySensorBlock Sensor in AllSensors){
		UpdateList(DetectedEntities, Sensor.LastDetectedEntity);
		List<MyDetectedEntityInfo> entities = new List<MyDetectedEntityInfo>();
		Sensor.DetectedEntities(entities);
		foreach(MyDetectedEntityInfo Entity in entities){
			UpdateList(DetectedEntities, Entity);
		}
	}
	List<IMyLargeTurretBase> AllTurrets = new List<IMyLargeTurretBase>();
	GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(AllTurrets);
	foreach(IMyLargeTurretBase Turret in AllTurrets){
		if(Turret.HasTarget){
			UpdateList(DetectedEntities, Turret.GetTargetedEntity());
		}
	}
	List<IMyIMyCameraBlock> AllCameras = new List<IMyCameraBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyCameraBlock>(AllCameras);
	foreach(IMyCameraBlock Camera in AllCameras){
		if(!HasBlockData(Camera, "DoRaycast")){
			SetBlockData(Camera, "DoRaycast", "maybe");
			SetBlockData(Camera, "RaycastTestCount", "0");
		}
		if(GetBlockData(Camera, "DoRaycast").Equals("false")){
			Camera.EnableRaycast = false;
			continue;
		}
		Camera.EnableRaycast = true;
		int count = 0;
		bool update_me = false;
		if(GetBlockData(Camera, "DoRaycast").Equals("maybe")){
			Int32.TryParse(GetBlockData(Camera, "RaycastTestCount"), out count);
			if(count>=100){
				SetBlockData(Camera, "DoRaycast", "false");
				continue;
			}
			update_me = true;
		}
		double raycast_distance = 10000;
		if(Camera.RaycastDistanceLimit != -1){
			raycast_distance = Math.Min(raycast_distance, Camera.RaycastDistanceLimit);
		}
		MyDetectedEntityInfo Raycast_Entity = Camera.Raycast(raycast_distance, 0, 0);
		if(update_me && Raycast_Entity.Type != MyDetectedEntityType.None && Raycast_Entity.EntityId != Me.CubeGrid.EntityId && Raycast_Entity.EntityId != Camera.CubeGrid.EntityId){
			SetBlockData(Camera, "DoRaycast", "true");
			update_me = false;
		}
		UpdateList(Detected_Entities, Raycast_Entity);
		const int SECTIONS = 3;
		for(int i=0; i<9; i++){
			if(i==4)
				continue;
			float Pitch = 0;
			float Yaw = 0;
			if(i<3 || i>5){
				Pitch = Rnd.Next(0, ((int)(Camera.RaycastConeLimit/SECTIONS))*10)/10.0f;
				if(i>5)
					Pitch *= -1;
			}
			if((i%3)!=1){
				Yaw = Rnd.Next(0, ((int)(Camera.RaycastConeLimit/SECTIONS))*10)/10.0f;
				if((i%3)==0)
					Yaw *= -1;
			}
			for(int j=1; j<=SECTIONS; j++){
				Raycast_Entity = Camera.Raycast(raycast_distance, Pitch*j, Yaw*j);
				if(update_me && Raycast_Entity.Type != MyDetectedEntityType.None && Raycast_Entity.EntityId != Me.CubeGrid.EntityId && Raycast_Entity.EntityId != Camera.CubeGrid.EntityId){
					SetBlockData(Camera, "DoRaycast", "true");
					update_me = false;
				}
				UpdateList(Detected_Entities, Raycast_Entity);
			}
		}
	}
	
	ScanString += "Retrieved updated data on " + DetectedEntities.Count + " relevant entities" + '\n';
	List<IMyBroadcastListener> listeners = new List<IMyBroadcastListener>();
	IGC.GetBroadcastListeners(listeners);
	foreach(MyDetectedEntityInfo entity in DetectedEntities){
		EntityInfo Entity = new EntityInfo(entity);
		foreach(IMyBroadcastListener Listener in listeners){
			IGC.SendBroadcastMessage(Listener.Tag, Entity.ToString(), TransmissionDistance.TransmissionDistanceMax);
		}
	}
	
	List<EntityInfo> Entities = new List<EntityInfo>();
	foreach(MyDetectedEntityInfo entity in DetectedEntities){
		Entities.Add(new EntityInfo(entity));
	}
	
	foreach(IMyBroadcastListener Listener in listeners){
		while(Listener.HasPendingMessage){
			MyIGCMessage message = Listener.AcceptMessage();
			ScanString += "Received message on " + Listener.Tag + '\n';
			EntityInfo Entity;
			if(EntityInfo.TryParse(message.Data.ToString(), out Entity)){
				UpdateList(Entities, Entity);
			}
		}
	}
	
	foreach(EntityInfo Entity in Entities){
		switch(Entity.Type){
			case MyDetectedEntityType.Asteroid:
				AsteroidList.UpdateEntity(Entity);
				break;
			case MyDetectedEntityType.Planet:
				PlanetList.UpdateEntity(Entity);
				break;
			case MyDetectedEntityType.SmallGrid:
				SmallShipList.UpdateEntity(Entity);
				break;
			case MyDetectedEntityType.LargeGrid:
				LargeShipList.UpdateEntity(Entity);
				break;
			case MyDetectedEntityType.CharacterHuman:
				CharacterList.UpdateEntity(Entity);
				break;
			case MyDetectedEntityType.CharacterOther:
				CharacterList.UpdateEntity(Entity);
				break;
		}
	}
	
	
}

public void PerformDisarm(){
	List<IMyWarhead> Warheads = new List<IMyWarhead>();
	GridTerminalSystem.GetBlocksOfType<IMyWarhead>(Warheads);
	foreach(IMyWarhead Warhead in Warheads){
		Warhead.DetonationTime = Math.Max(10 * SCAN_TIME, Warhead.DetonationTime);
		Warhead.IsArmed = false;
		Warhead.StopCountdown();
	}
}

private void UpdateProgramInfo(){
	cycle_long+=((++cycle)/long.MaxValue)%long.MaxValue;
	cycle=cycle % long.MaxValue;
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
	Echo(Program_Name+" OS "+cycle_long.ToString()+'-'+cycle.ToString()+" ("+loading_char+")");
	Me.GetSurface(1).WriteText(Program_Name+" OS "+cycle_long.ToString()+'-'+cycle.ToString()+" ("+loading_char+")", false);
	Me.GetSurface(0).WriteText("", false);
	seconds_since_last_update=Runtime.TimeSinceLastRun.TotalSeconds+(Runtime.LastRunTimeMs / 1000);
	if(seconds_since_last_update<1){
		Echo(Math.Round(seconds_since_last_update*1000, 0).ToString()+" milliseconds\n");
	}
	else if(seconds_since_last_update<60){
		Echo(Math.Round(seconds_since_last_update, 3).ToString()+" seconds\n");
	}
	else if(seconds_since_last_update/60<60){
		Echo(Math.Round(seconds_since_last_update/60, 2).ToString()+" minutes\n");
	}
	else if(seconds_since_last_update/60/60<24){
		Echo(Math.Round(seconds_since_last_update/60/60, 2).ToString()+" hours\n");
	}
	else {
		Echo(Math.Round(seconds_since_last_update/60/60/24, 2).ToString()+" days\n");
	}
}

public void Main(string argument, UpdateType updateSource)
{
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
}
