/*
* Ship AI System 
* Built by mlq1616
* https://github.com/mlq1819
*/
//Name me!
private const string Program_Name = "Neptine Ship AI"; 
//The angle of what the ship will accept as "correct"
private const double ACCEPTABLE_ANGLE=20; //Suggested between 5° and 20°
//The maximum speed limit of the ship
private const double SPEED_LIMIT=100;
//The distance accepted for raycasts
private const double RAYCAST_DISTANCE=10000; //The lower the better, but whatever works for you
//Set this to the distance you want lights, sound blocks, and doors to update when enemies are nearby
private const double ALERT_DISTANCE=15;
//Time between scans
private const double SCAN_TIME=3;
private Color DEFAULT_TEXT_COLOR = new Color(197,137,255,255);
private Color DEFAULT_BACKGROUND_COLOR = new Color(44,0,88,255);

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
	
	public List<T> GetAllIncluding(string name, Vector3D Reference, double max_distance = double.MaxValue){
		List<T> AllBlocks = new List<T>();
		List<T> MyBlocks = new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			double distance = (Reference - Block.GetPosition()).Length();
			if(Block.CustomName.Contains(name) && distance <= max_distance){
				MyBlocks.Add(Block);
			}
		}
		return MyBlocks;
	}
	
	public List<T> GetAllIncluding(string name, IMyTerminalBlock Reference, double max_distance = double.MaxValue){
		return GetAllIncluding(name, Reference.GetPosition(), max_distance);
	}
	
	public List<T> GetAllIncluding(string name, double max_distance = double.MaxValue){
		return GetAllIncluding(name, Prog, max_distance);
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
		return "X:"+Math.Round(vector.X,1).ToString()+" Y:"+Math.Round(vector.Y,1).ToString()+" Z:"+Math.Round(vector.Z,1).ToString();
	}
	
	public void Update(double seconds){
		TimeSpan time=new TimeSpan((int)(seconds/60/60/24), ((int)(seconds/60/60))%24, ((int)(seconds/60))%60, ((int)(seconds))%60, ((int)(seconds*1000))%1000);
		Age.Add(time);
		Position+=seconds * Velocity;
		if(HitPosition!=null){
			HitPosition=(Vector3D?) (((Vector3D)HitPosition)+seconds * Velocity);
		}
	}
	
	public double GetDistance(Vector3D Reference){
		return (Position-Reference).Length();
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
				min_distance=Math.Min(min_distance, (P.Me.CubeGrid.GetPosition()-entity.Position).Length()-entity.Size);
			}
		}
		return min_distance;
	}
	
	public double ClosestDistance(MyGridProgram P, double min_size=0){
		double min_distance=double.MaxValue;
		foreach(EntityInfo entity in E_List){
			if(entity.Size >= min_size){
				min_distance=Math.Min(min_distance, (P.Me.CubeGrid.GetPosition()-entity.Position).Length()-entity.Size);
			}
		}
		return min_distance;
	}
	
	public void Sort(Vector3D Reference){
		List<EntityInfo> Sorted = new List<EntityInfo>();
		List<EntityInfo> Unsorted = new List<EntityInfo>();
		foreach(EntityInfo Entity in E_List){
			double distance=Entity.GetDistance(Reference);
			double last_distance=0;
			if(Sorted.Count>0)
				last_distance=Sorted[Sorted.Count-1].GetDistance(Reference);
			if(distance>=last_distance)
				Sorted.Add(Entity);
			else
				Unsorted.Add(Entity);
		}
		while(Unsorted.Count>0){
			double distance=Unsorted[0].GetDistance(Reference);
			if(distance>=Sorted[Sorted.Count-1].GetDistance(Reference)){
				Sorted.Add(Unsorted[0]);
				Unsorted.RemoveAt(0);
				continue;
			}
			for(int i=0;i<Sorted.Count;i++){
				if(distance<=Sorted[i].GetDistance(Reference)){
					Sorted.Insert(i,Unsorted[0]);
					Unsorted.RemoveAt(0);
					break;
				}
			}
		}
		E_List=Sorted;
	}
}

public enum MenuType{
	Submenu=0,
	Command=1,
	Display=2
}

public interface MenuOption{
	string Name();
	MenuType Type();
	bool AutoRefresh();
	int Depth();
	bool Back();
	bool Select();
}

public class Menu_Submenu : MenuOption{
	private string _Name;
	public string Name(){
		return _Name;
	}
	public MenuType Type(){
		return MenuType.Submenu;
	}
	public bool AutoRefresh(){
		if(IsSelected){
			return Menu[Selection].AutoRefresh();
		}
		return Last_Count==Count || Count>10;
	}
	public int Depth(){
		if(Selected){
			return 1+Menu[Selection].Depth();
		}
		return 1;
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
			if(Menu[Selection].Type()==MenuType.Submenu){
				return ((Menu_Submenu)(Menu[Selection])).Next();
			}
			return false;
		}
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
		Selection=(Selection-1+Count)%Count;
		return true;
	}
	
	public override string ToString(){
		Selection=Selection%Count;
		if(Selected){
			return Menu[Selection].ToString();
		}
		string output=" -- "+Name()+" -- ";
		if(Count <= 10){
			for(int i=0; i<Count; i++){
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
				if(Selection==i){
					output+=' '+Menu[i].Name().ToUpper()+' ';
				}
				else {
					output+=Menu[i].Name().ToLower();
				}
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
		else {
			int count = 0;
			for(int i=(Selection+Count-1)%Count; count<=10; i=(i+1)%Count){
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
				if(Selection==i){
					output+=' '+Menu[i].Name().ToUpper()+' ';
				}
				else {
					output+=Menu[i].Name().ToLower();
				}
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

public class Menu_Command<T> : MenuOption where T : class{
	private string _Name;
	public string Name(){
		return _Name;
	}
	public MenuType Type(){
		return MenuType.Command;
	}
	private bool _AutoRefresh;
	public bool AutoRefresh(){
		return _AutoRefresh;
	}
	public int Depth(){
		return 1;
	}
	private string Desc;
	private T Arg;
	private Func<T, bool> Command;
	
	public Menu_Command(string name, Func<T, bool> command, string desc="No description provided", T arg=null, bool autorefresh=false){
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
		int length=32;
		foreach(string word in words){
			if(length > 0 && length+word.Length > 32){
				length=0;
				output+='\n';
			}
			else {
				output+=' ';
			}
			output+=word;
			if(word.Contains('\n'))
				length=word.Length-word.IndexOf('\n')-1;
		}
		return output;
	}
}

public class Menu_Display : MenuOption{
	public string Name(){
		return Entity.Name.Substring(0, Math.Min(24, Entity.Name.Length));
	}
	public MenuType Type(){
		return MenuType.Display;
	}
	public bool AutoRefresh(){
		return true;
	}
	public int Depth(){
		if(Selected)
			return 2;
		return 1;
	}
	private bool Selected;
	private EntityInfo Entity;
	private bool Can_GoTo;
	private Func<EntityInfo, bool> Command;
	private Menu_Command<EntityInfo> Subcommand {
		get{
			double distance=(P.Me.CubeGrid.GetPosition()-Entity.Position).Length()-Entity.Size;
			return new Menu_Command<EntityInfo>("GoTo "+Entity.Name, Command, "Set autopilot to match target Entity's expected position and velocity", Entity);
		}
	}
	private MyGridProgram P;
	
	public Menu_Display(EntityInfo entity, MyGridProgram p, Func<EntityInfo, bool> GoTo){
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
private IMyGyro Gyroscope;

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

private double Time_To_Crash=double.MaxValue;
private Menu_Submenu Command_Menu;

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
private Vector3D Relative_Target_Position{
	get{
		return GlobalToLocal(Target_Position);
	}
}
private long Target_ID = 0;

private float Mass_Accomodation = 0.0f;

private Vector3D RestingVelocity;
private Vector3D Relative_RestingVelocity{
	get{
		return GlobalToLocal(RestingVelocity);
	}
}
private Vector3D CurrentVelocity;
private Vector3D Relative_CurrentVelocity{
	get{
		Vector3D output = Vector3D.Transform(CurrentVelocity, MatrixD.Invert(Controller.WorldMatrix));
		output.Normalize();
		output *= CurrentVelocity.Length();
		return output;
	}
}
private Vector3D Gravity;
private Vector3D Relative_Gravity{
	get{
		return GlobalToLocal(Gravity);
	}
}
private Vector3D Adjusted_Gravity{
	get{
		Vector3D temp = Vector3D.Transform(Gravity+Controller.GetPosition(), MatrixD.Invert(Controller.WorldMatrix));
		temp.Normalize();
		return temp*Mass_Accomodation;
	}
}
private Vector3D Gravity_Direction{
	get{
		Vector3D direction = Gravity;
		direction.Normalize();
		return direction;
	}
}

private double Speed_Deviation{
	get{
		return (CurrentVelocity-RestingVelocity).Length();
	}
}

private Vector3D AngularVelocity;
private Vector3D Relative_AngularVelocity{
	get{
		return GlobalToLocal(AngularVelocity);
	}
}

private double Elevation;
private double Sealevel;
private Vector3D PlanetCenter;

private struct Gyro_Tuple{
	public float Pitch;
	public float Yaw;
	public float Roll;
	
	public Gyro_Tuple(float p, float y, float r){
		Pitch = p;
		Yaw = y;
		Roll = r;
	}
	
	public static Gyro_Tuple Parse(string input){
		string[] args = input.Split(' ');
		if(args.Count() != 3)
			throw new ArgumentException("Invalid input for Gyro_Tuple");
		if(args[0].IndexOf("P:")!=0)
			throw new ArgumentException("Invalid input for Gyro_Tuple");
		if(args[1].IndexOf("Y:")!=0)
			throw new ArgumentException("Invalid input for Gyro_Tuple");
		if(args[2].IndexOf("R:")!=0)
			throw new ArgumentException("Invalid input for Gyro_Tuple");
		try{
			float pitch = float.Parse(args[0].Substring(args[0].IndexOf(':')+1));
			float yaw = float.Parse(args[1].Substring(args[1].IndexOf(':')+1));
			float roll = float.Parse(args[2].Substring(args[2].IndexOf(':')+1));
			return new Gyro_Tuple(pitch, yaw, roll);
		}
		catch(Exception){
			throw new ArgumentException("Invalid input for Gyro_Tuple");
		}
	}
	
	public override string ToString(){
		return "P:" + Pitch.ToString() + " Y:" + Yaw.ToString() + " R:" + Roll.ToString();
	}
	
	public string NiceString(){
		return "Pitch: " + ((int)Pitch).ToString() + "\nYaw: " + ((int)Yaw).ToString() + "\nRoll: " + ((int)Roll).ToString();
	}
}

private Gyro_Tuple Transform(Gyro_Tuple input){
	float pitch = 0, yaw = 0, roll = 0;
	switch(Forward){
		case Base6Directions.Direction.Forward:
			switch(Up){
				case Base6Directions.Direction.Up:
					pitch = input.Pitch;
					yaw = input.Yaw;
					roll = input.Roll;
					break;
				case Base6Directions.Direction.Down:
					pitch = -1 * input.Pitch;
					yaw = -1 * input.Yaw;
					roll = input.Roll;
					break;
				case Base6Directions.Direction.Left:
					yaw = input.Pitch;
					pitch = -1 * input.Yaw;
					roll = input.Roll;
					break;
				case Base6Directions.Direction.Right:
					yaw = -1 * input.Pitch;
					pitch = input.Yaw;
					roll = input.Roll;
					break;
			}
			break;
		case Base6Directions.Direction.Backward:
			switch(Up){
				case Base6Directions.Direction.Up:
					pitch = -1 * input.Pitch;
					yaw = input.Yaw;
					roll = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Down:
					pitch = input.Pitch;
					yaw = input.Yaw;
					roll = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Left:
					yaw = input.Pitch;
					pitch = input.Yaw;
					roll = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Right:
					yaw = -1 * input.Pitch;
					pitch = -1 * input.Yaw;
					roll = -1 * input.Roll;
					break;
			}
			break;
		case Base6Directions.Direction.Up:
			switch(Up){
				case Base6Directions.Direction.Forward:
					pitch = -1 * input.Pitch;
					roll = -1 * input.Yaw;
					yaw = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Backward:
					pitch = input.Pitch;
					roll = -1 * input.Yaw;
					yaw = input.Roll;
					break;
				case Base6Directions.Direction.Left:
					yaw = input.Pitch;
					roll = -1 * input.Yaw;
					pitch = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Right:
					yaw = -1 * input.Pitch;
					roll = -1 * input.Yaw;
					pitch = input.Roll;
					break;
			}
			break;
		case Base6Directions.Direction.Down:
			switch(Up){
				case Base6Directions.Direction.Forward:
					pitch = input.Pitch;
					roll = input.Yaw;
					yaw = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Backward:
					pitch = -1 * input.Pitch;
					roll = input.Yaw;
					yaw = input.Roll;
					break;
				case Base6Directions.Direction.Left:
					yaw = input.Pitch;
					roll = input.Yaw;
					pitch = input.Roll;
					break;
				case Base6Directions.Direction.Right:
					yaw = -1 * input.Pitch;
					roll = input.Yaw;
					pitch = -1 * input.Roll;
					break;
			}
			break;
		case Base6Directions.Direction.Left:
			switch(Up){
				case Base6Directions.Direction.Forward:
					roll = -1 * input.Pitch;
					pitch = input.Yaw;
					yaw = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Backward:
					roll = -1 * input.Pitch;
					pitch = -1 * input.Yaw;
					yaw = input.Roll;
					break;
				case Base6Directions.Direction.Up:
					roll = -1 * input.Pitch;
					yaw = input.Yaw;
					pitch = input.Roll;
					break;
				case Base6Directions.Direction.Down:
					roll = -1 * input.Pitch;
					yaw = -1 * input.Yaw;
					pitch = -1 * input.Roll;
					break;
			}
			break;
		case Base6Directions.Direction.Right:
			switch(Up){
				case Base6Directions.Direction.Forward:
					roll = input.Pitch;
					pitch = -1 * input.Yaw;
					yaw = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Backward:
					roll = input.Pitch;
					pitch = input.Yaw;
					yaw = input.Roll;
					break;
				case Base6Directions.Direction.Up:
					roll = input.Pitch;
					yaw = input.Yaw;
					pitch = -1 * input.Roll;
					break;
				case Base6Directions.Direction.Down:
					roll = input.Pitch;
					yaw = -1 * input.Yaw;
					pitch = input.Roll;
					break;
			}
			break;
	}
	return new Gyro_Tuple(pitch, yaw, roll);
}

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

private bool CanHaveJob(IMyTerminalBlock Block, string JobName){
	return (!HasBlockData(Block, "Job")) || GetBlockData(Block, "Job").Equals("None") || GetBlockData(Block, "Job").Equals(JobName);
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
	if(Controller.GetShipVelocities().AngularVelocity.Length() > .1f)
		return UpdateFrequency.Update1;
	if((Controller.GetShipVelocities().LinearVelocity - RestingVelocity).Length() > .5)
		return UpdateFrequency.Update1;
	return UpdateFrequency.Update10;
}

private string GetRemovedString(string big_string, string small_string){
	string output = big_string;
	if(big_string.Contains(small_string)){
		output = big_string.Substring(0, big_string.IndexOf(small_string)) + big_string.Substring(big_string.IndexOf(small_string) + small_string.Length);
	}
	return output;
}

private List<List<IMyDoor>> RemoveDoor(List<List<IMyDoor>> list, IMyDoor Door){
	List<List<IMyDoor>> output = new List<List<IMyDoor>>();
	Echo("\tRemoving Door \"" + Door.CustomName + "\" from list[" + list.Count + "]");
	if(list.Count == 0){
		return output;
	}
	string ExampleDoorName = "";
	foreach(List<IMyDoor> sublist in list){
		if(sublist.Count > 0){
			ExampleDoorName = sublist[0].CustomName;
			break;
		}
	}
	
	bool is_leading_group = (ExampleDoorName.Contains("Door 1") && Door.CustomName.Contains("Door 1")) || (ExampleDoorName.Contains("Door 2") && Door.CustomName.Contains("Door 2"));
	for(int i=0; i<list.Count; i++){
		if(list[i].Count > 1 && (!is_leading_group || !list[i][0].Equals(Door))){
			if(is_leading_group){
				output.Add(list[i]);
			}
			else{
				List<IMyDoor> pair = new List<IMyDoor>();
				pair.Add(list[i][0]);
				for(int j=1; j<list[i].Count; j++){
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
		if(!Me.CubeGrid.IsStatic)
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
	SetThrusterList(Forward_Thrusters, "Forward");
	SetThrusterList(Backward_Thrusters, "Backward");
	SetThrusterList(Up_Thrusters, "Up");
	SetThrusterList(Down_Thrusters, "Down");
	SetThrusterList(Left_Thrusters, "Left");
	SetThrusterList(Right_Thrusters, "Right");
	
	Operational=Me.IsWorking;
	Runtime.UpdateFrequency = GetUpdateFrequency();
	return true;
}

private bool Operational=false;

public Program()
{
    Me.CustomName=(Program_Name+" Programmable block").Trim();
	for(int i=0;i<Me.SurfaceCount;i++){
		Me.GetSurface(i).FontColor=DEFAULT_TEXT_COLOR;
		Me.GetSurface(i).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		Me.GetSurface(i).Alignment=TextAlignment.CENTER;
		Me.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
	}
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
					AsteroidList.UpdateEntry(Entity);
					break;
				case MyDetectedEntityType.Planet:
					PlanetList.UpdateEntry(Entity);
					break;
				case MyDetectedEntityType.SmallGrid:
					SmallShipList.UpdateEntry(Entity);
					break;
				case MyDetectedEntityType.LargeGrid:
					LargeShipList.UpdateEntry(Entity);
					break;
				case MyDetectedEntityType.CharacterHuman:
					CharacterList.UpdateEntry(Entity);
					break;
				case MyDetectedEntityType.CharacterOther:
					CharacterList.UpdateEntry(Entity);
					break;
			}
		}
		else if(arg.IndexOf("Lockdown:")==0){
			bool.TryParse(arg.Substring("Lockdown:".Length), out _Lockdown);
		}
	}
	IGC.RegisterBroadcastListener("Neptine AI");
	IGC.RegisterBroadcastListener("Entity Report");
	IGC.RegisterBroadcastListener(Me.CubeGrid.CustomName);
	CreateMenu();
}

private void SetThrusterList(List<IMyThrust> Thrusters, string Direction){
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
				tag="Large Hydrogen";
				number=large_hydrogen--;
			}
			else {
				tag="Small Hydrogen";
				number=small_hydrogen--;
			}
		}
		else if(Thruster.CustomName.ToLower().Contains("atmospheric")){
			if(Thruster.CustomName.ToLower().Contains("large")){
				tag="Large Atmospheric";
				number=large_atmospheric--;
			}
			else {
				tag="Small Atmospheric";
				number=small_atmospheric--;
			}
		}
		else if(Thruster.CustomName.ToLower().Contains("ion")){
			if(Thruster.CustomName.ToLower().Contains("large")){
				tag="Large Ion";
				number=large_ion--;
			}
			else {
				tag="Small Ion";
				number=small_ion--;
			}
		}
		else{
			if(Thruster.CustomName.ToLower().Contains("large")){
				tag="Large Misc";
				number=large_misc--;
			}
			else {
				tag="Small Misc";
				number=small_misc--;
			}
		}
		Thruster.CustomName = Direction + tag + " Thruster " + number.ToString();
	}
}

private void ResetThruster(IMyThrust Thruster){
	if(HasBlockData(Thruster, "DefaultOverride")){
		float ThrustOverride = 0.0f;
		if(float.TryParse(GetBlockData(Thruster, "DefaultOverride"), out ThrustOverride))
			Thruster.ThrustOverridePercentage=ThrustOverride;
		else
			Thruster.ThrustOverridePercentage = 0.0f;
	}
	if(HasBlockData(Thruster, "DefaultName")){
		Thruster.CustomName = GetBlockData(Thruster, "DefaultName");
	}
	SetBlockData(Thruster, "Owner", "0");
}

public void Save()
{
	this.Storage="Lockdown:"+_Lockdown.ToString();
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
}

public Vector3D GlobalToLocal(Vector3D Global){
	Vector3D Local = Vector3D.Transform(Global, MatrixD.Invert(Controller.WorldMatrix));
	Local.Normalize();
	return Local * Global.Length();
}

public Vector3D LocalToGlobal(Vector3D Local){
	Vector3D Global = Vector3D.Transform(Local, Controller.WorldMatrix);
	Global.Normalize();
	return Global * Local.Length();
}

private enum AlertStatus{
	Green = 0,
	Blue = 1,
	Yellow = 2,
	Orange = 3,
	Red = 4
}
private string Submessage = "";
private AlertStatus ShipStatus{
	get{
		AlertStatus status = AlertStatus.Green;
		Submessage = "";
		
		if(Time_To_Crash < 15 && Controller.GetShipSpeed()>5){
			AlertStatus new_status = AlertStatus.Orange;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
		}
		else if(Time_To_Crash < 60 && Controller.GetShipSpeed()>15){
			AlertStatus new_status = AlertStatus.Yellow;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
		}
		else if(Time_To_Crash < 180){
			AlertStatus new_status = AlertStatus.Blue;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
		}
		
		double ActualEnemyShipDistance = Math.Min(SmallShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies), LargeShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies));
		double EnemyShipDistance = Math.Min(SmallShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies), LargeShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies)/2);
		if(EnemyShipDistance<800){
			AlertStatus new_status = AlertStatus.Red;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Ship at " + Math.Round(ActualEnemyShipDistance, 0) + " meters";
		}
		else if(EnemyShipDistance<2500){
			AlertStatus new_status = AlertStatus.Orange;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Ship at " + Math.Round(ActualEnemyShipDistance, 0) + " meters";
		}
		else if(EnemyShipDistance<5000){
			AlertStatus new_status = AlertStatus.Yellow;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Ship at " + Math.Round(ActualEnemyShipDistance, 0) + " meters";
		}
		
		double EnemyCharacterDistance = CharacterList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies);
		if(EnemyCharacterDistance-Me.CubeGrid.GridSize<0){
			AlertStatus new_status = AlertStatus.Red;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Creature has boarded ship";
		}
		else if(EnemyCharacterDistance-Me.CubeGrid.GridSize<800){
			AlertStatus new_status = AlertStatus.Orange;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Creature at " + Math.Round(EnemyCharacterDistance, 0) + " meters";
		}
		else if(EnemyCharacterDistance-Me.CubeGrid.GridSize<2000){
			AlertStatus new_status = AlertStatus.Yellow;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Creature at " + Math.Round(EnemyCharacterDistance, 0) + " meters";
		}
		
		double ShipDistance = Math.Min(SmallShipList.ClosestDistance(this),LargeShipList.ClosestDistance(this))-Me.CubeGrid.GridSize;
		if(ShipDistance < 500 && ShipDistance > 0){
			AlertStatus new_status = AlertStatus.Blue;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nNearby ship at " + Math.Round(ShipDistance, 0) + " meters";
		}
		if(AsteroidList.ClosestDistance(this) < 500){
			AlertStatus new_status = AlertStatus.Blue;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nNearby asteroid at " + Math.Round(AsteroidList.ClosestDistance(this), 0) + " meters";
		}
		if(Controller.GetShipSpeed() > 30){
			AlertStatus new_status = AlertStatus.Blue;
			status = (AlertStatus) Math.Max((int)status, (int)new_status);
			double Speed = Controller.GetShipSpeed();
			Submessage += "\nHigh Ship Speed [";
			const int SECTIONS = 20;
			for(int i=0; i<SECTIONS; i++){
				if(Speed >= ((100.0/SECTIONS) * i)){
					Submessage += '|';
				}
				else {
					Submessage += ' ';
				}
			}
			Submessage += ']';
		}
		if(status == AlertStatus.Green)
			Submessage = "\nNo issues";
		return status;
	}
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
		if(list[i].ID == new_entity.ID){
			list[i] = new_entity;
			return;
		}
	}
	list.Add(new_entity);
}

private void SetStatus(string message, Color TextColor, Color BackgroundColor){
	float padding = 40.0f;
	string[] lines = message.Split('\n');
	padding = Math.Max(10.0f, padding-(lines.Length*5.0f));
	foreach(IMyTextPanel LCD in StatusLCDs){
		LCD.Alignment=TextAlignment.CENTER;
		LCD.FontSize=1.2f;
		LCD.ContentType=ContentType.TEXT_AND_IMAGE;
		LCD.TextPadding=padding;
		LCD.WriteText(message, false);
		if(LCD.CustomName.ToLower().Contains("transparent")){
			LCD.FontColor = BackgroundColor;
			LCD.BackgroundColor = new Color(0,0,0,255);
		}
		else {
			LCD.FontColor = TextColor;
			LCD.BackgroundColor = BackgroundColor;
		}
	}
}

private bool Stop(object obj=null){
	RestingVelocity=new Vector3D(0,0,0);
	Target_Position=new Vector3D(0,0,0);
	Match_Direction=false;
	Match_Position=false;
	Target_ID=0;
	return true;
}

private bool _Lockdown=false;

private bool Lockdown(object obj=null){
	_Lockdown=!_Lockdown;
	List<IMyAirtightHangarDoor> Seals = (new GenericMethods<IMyAirtightHangarDoor>(this)).GetAllIncluding("Air Seal");
	foreach(IMyAirtightHangarDoor Door in Seals){
		if(_Lockdown){
			if(CanHaveJob(Door, "Lockdown")){
				SetBlockData(Door, "Job", "Lockdown");
				Door.Enabled=(Door.Status!=DoorStatus.Closed);
				Door.CloseDoor();
			}
		}
		else{
			if(CanHaveJob(Door, "Lockdown")){
				SetBlockData(Door, "Job", "None");
				Door.Enabled=(Door.Status!=DoorStatus.Open);
				Door.OpenDoor();
			}
		}
	}
	return true;
}

private bool FactoryReset(object obj=null){
	SetStatus("Status LCD\nOffline", DEFAULT_TEXT_COLOR, DEFAULT_BACKGROUND_COLOR);
	Reset();
	Me.Enabled = false;
	return true;
}

private Vector3D GetOffsetPosition(Vector3D Position){
	Vector3D direction = Position-Me.CubeGrid.GetPosition();
	direction.Normalize();
	double distance = (Position-Me.CubeGrid.GetPosition()).Length();
	return (distance-Me.CubeGrid.GridSize/2-Math.Min(distance/2,400))*direction;
}

private bool GoTo(EntityInfo Entity){
	//Match velocity for hostiles, GoTo for others
	RestingVelocity=Entity.Velocity;
	Target_ID=Entity.ID;
	if(Entity.Relationship!=MyRelationsBetweenPlayerAndBlock.Enemies){
		Target_Direction = Entity.Position-Me.CubeGrid.GetPosition();
		Target_Direction.Normalize();
		Target_Position = GetOffsetPosition(Entity.Position);
		Match_Position=true;
		Match_Direction=true;
	}
	return true;
}

private bool UpdateEntityListing(Menu_Submenu Menu){
	EntityList list = null;
	bool do_goto = false;
	switch(Menu.Name()){
		case "Asteroids":
			list=AsteroidList;
			do_goto = true;
			break;
		case "Planets":
			list=PlanetList;
			do_goto = false;
			break;
		case "Small Ships":
			list=SmallShipList;
			do_goto = true;
			break;
		case "Large Ships":
			list=LargeShipList;
			do_goto = true;
			break;
		case "Characters":
			list=CharacterList;
			do_goto = true;
			break;
	}
	if(list==null)
		return false;
	Menu = new Menu_Submenu(Menu.Name());
	Menu.Add(new Menu_Command<Menu_Submenu>("Refresh", UpdateEntityListing, "Updates "+Menu.Name(), Menu));
	list.Sort(Me.CubeGrid.GetPosition());
	for(int i=0;i<list.Count;i++){
		if(do_goto)
			Menu.Add(new Menu_Display(list[i], this, GoTo));
		else
			Menu.Add(new Menu_Display(list[i]));
	}
	return true;
}

private Menu_Submenu AsteroidMenu;
private Menu_Submenu PlanetMenu;
private Menu_Submenu SmallShipMenu;
private Menu_Submenu LargeShipMenu;
private Menu_Submenu CharacterMenu;

private void CreateMenu(){
	Command_Menu = new Menu_Submenu("Command Menu");
	//Command_Menu.Add(new Menu_Command("Update Menu", CreateMenu));
	Menu_Submenu ShipCommands = new Menu_Submenu("Commands");
	ShipCommands.Add(new Menu_Command<object>("Stop", Stop, "Disables autopilot"));
	ShipCommands.Add(new Menu_Command<object>("Lockdown", Lockdown, "Closes/Opens Air Seals"));
	ShipCommands.Add(new Menu_Command<object>("Factory Reset", FactoryReset, "Resets AI memory and settings, and turns it off"));
	Command_Menu.Add(ShipCommands);
	AsteroidMenu=new Menu_Submenu("Asteroids");
	UpdateEntityListing(AsteroidMenu);
	PlanetMenu=new Menu_Submenu("Planets");
	UpdateEntityListing(PlanetMenu);
	SmallShipMenu=new Menu_Submenu("Small Ships");
	UpdateEntityListing(SmallShipMenu);
	LargeShipMenu=new Menu_Submenu("Large Ships");
	UpdateEntityListing(LargeShipMenu);
	CharacterMenu=new Menu_Submenu("Characters");
	UpdateEntityListing(CharacterMenu);
}

private void DisplayMenu(){
	List<IMyTextPanel> Panels = (new GenericMethods<IMyTextPanel>(this)).GetAllContaining("Command Menu Display");
	foreach(IMyTextPanel Panel in Panels){
		Panel.WriteText(Command_Menu.ToString(), false);
		Panel.Alignment=TextAlignment.CENTER;
		Panel.FontSize=1.2f;
		Panel.ContentType=ContentType.TEXT_AND_IMAGE;
		if(Panel.CustomName.ToLower().Contains("transparent")){
			Panel.FontColor=DEFAULT_BACKGROUND_COLOR;
			Panel.BackgroundColor=new Color(0,0,0,0);
		}
		else{
			Panel.FontColor=DEFAULT_TEXT_COLOR;
			Panel.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
	}
}

private Color ColorParse(string parse){
	parse = parse.Substring(parse.IndexOf('{')+1);
	parse = parse.Substring(0, parse.IndexOf('}') - 1);
	string[] args = parse.Split(' ');
	int r, g, b, a;
	r = Int32.Parse(args[0].Substring(args[0].IndexOf("R:")+2).Trim());
	g = Int32.Parse(args[1].Substring(args[1].IndexOf("G:")+2).Trim());
	b = Int32.Parse(args[2].Substring(args[2].IndexOf("B:")+2).Trim());
	a = Int32.Parse(args[3].Substring(args[3].IndexOf("A:")+2).Trim());
	return new Color(r,g,b,a);
}

private bool last_performed_alarm = false;
public void PerformAlarm(){
	bool nearby_enemy = (CharacterList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies) <= (float) Me.CubeGrid.GridSize);
	if(!nearby_enemy && !last_performed_alarm){
		return;
	}
	last_performed_alarm = nearby_enemy;
	List<IMyInteriorLight> AllLights = new List<IMyInteriorLight>();
	GridTerminalSystem.GetBlocksOfType<IMyInteriorLight>(AllLights);
	foreach(IMyInteriorLight Light in AllLights){
		if(!CanHaveJob(Light, "PlayerAlert"))
			continue;
		double distance = double.MaxValue;
		foreach(EntityInfo Entity in CharacterList){
			if((Entity.Relationship == MyRelationsBetweenPlayerAndBlock.Enemies) && Entity.Age.TotalSeconds <= 60)
				distance = Math.Min(distance, Entity.GetDistance(Light.GetPosition()));
		}
		if(distance <= ALERT_DISTANCE){
			if(!HasBlockData(Light, "DefaultColor")){
				SetBlockData(Light, "DefaultColor", Light.Color.ToString());
			}
			if(!HasBlockData(Light, "DefaultBlinkLength")){
				SetBlockData(Light, "DefaultBlinkLength", Light.BlinkLength.ToString());
			}
			if(!HasBlockData(Light, "DefaultBlinkInterval")){
				SetBlockData(Light, "DefaultBlinkInterval", Light.BlinkIntervalSeconds.ToString());
			}
			SetBlockData(Light, "Job", "PlayerAlert");
			Light.Color = new Color(255, 0, 0, 255);
			Light.BlinkLength = 100.0f - (((float) (distance / ALERT_DISTANCE)) * 50.0f);
			Light.BlinkIntervalSeconds = 1.0f;
		}
		else {
			if(HasBlockData(Light, "Job") && GetBlockData(Light, "Job").Equals("PlayerAlert")){
				if(HasBlockData(Light, "DefaultColor")){
					try{
						Light.Color = ColorParse(GetBlockData(Light, "DefaultColor"));
					}
					catch(Exception){
						Echo("Failed to parse color");
					}
				}
				if(HasBlockData(Light, "DefaultBlinkLength")){
					try{
						Light.BlinkLength = float.Parse(GetBlockData(Light, "DefaultBlinkLength"));
					}
					catch(Exception){
						;
					}
				}
				if(HasBlockData(Light, "DefaultBlinkInterval")){
					try{
						Light.BlinkIntervalSeconds = float.Parse(GetBlockData(Light, "DefaultBlinkInterval"));
					}
					catch(Exception){
						;
					}
				}
				SetBlockData(Light, "Job", "None");
			}
		}
	}
	List<IMySoundBlock> AllSounds = new List<IMySoundBlock>();
	GridTerminalSystem.GetBlocksOfType<IMySoundBlock>(AllSounds);
	foreach(IMySoundBlock Sound in AllSounds){
		if(!CanHaveJob(Sound, "PlayerAlert"))
			continue;
		double distance = double.MaxValue;
		foreach(EntityInfo Entity in CharacterList){
			if((Entity.Relationship == MyRelationsBetweenPlayerAndBlock.Enemies) && Entity.Age.TotalSeconds <= 60)
				distance = Math.Min(distance, Entity.GetDistance(Sound.GetPosition()));
		}
		if(distance <= ALERT_DISTANCE){
			if(!HasBlockData(Sound, "DefaultSound")){
				SetBlockData(Sound, "DefaultSound", Sound.SelectedSound);
			}
			if(!HasBlockData(Sound, "DefaultVolume")){
				SetBlockData(Sound, "DefaultVolume", Sound.Volume.ToString());
			}
			SetBlockData(Sound, "Job", "PlayerAlert");
			if(!HasBlockData(Sound, "Playing") || !GetBlockData(Sound, "Playing").Equals("True")){
				Sound.SelectedSound = "SoundBlockEnemyDetected";
				Sound.Volume = 100.0f;
				Sound.Play();
				SetBlockData(Sound, "Playing", "True");
			}
		}
		else {
			if(HasBlockData(Sound, "Job") && GetBlockData(Sound, "Job").Equals("PlayerAlert")){
				if(HasBlockData(Sound, "DefaultSound")){
					Sound.SelectedSound = GetBlockData(Sound, "DefaultSound");
				}
				if(HasBlockData(Sound, "DefaultVolume")){
					try{
						Sound.Volume = float.Parse(GetBlockData(Sound, "DefaultVolume"));
					}
					catch(Exception){
						;
					}
				}
				Sound.Stop();
				SetBlockData(Sound, "Playing", "False");
				SetBlockData(Sound, "Job", "None");
			}
		}
	}
	List<IMyDoor> AllDoors = new List<IMyDoor>();
	GridTerminalSystem.GetBlocksOfType<IMyDoor>(AllDoors);
	foreach(IMyDoor Door in AllDoors){
		if(!CanHaveJob(Door, "PlayerAlert"))
			continue;
		double distance = double.MaxValue;
		foreach(EntityInfo Entity in CharacterList){
			if((Entity.Relationship == MyRelationsBetweenPlayerAndBlock.Enemies) && Entity.Age.TotalSeconds <= 60)
				distance = Math.Min(distance, Entity.GetDistance(Door.GetPosition()));
		}
		if(distance <= ALERT_DISTANCE){
			if(!HasBlockData(Door, "DefaultState")){
				if(Door.Status.ToString().Contains("Open")){
					SetBlockData(Door, "DefaultState", "Open");
				}
				else {
					SetBlockData(Door, "DefaultState", "Closed");
				}
			}
			if(!HasBlockData(Door, "DefaultPower")){
				if(Door.Enabled){
					SetBlockData(Door, "DefaultPower", "On");
				}
				else {
					SetBlockData(Door, "DefaultPower", "Off");
				}
			}
			SetBlockData(Door, "Job", "PlayerAlert");
			Door.Enabled = (Door.Status != DoorStatus.Closed);
			Door.CloseDoor();
		}
		else {
			if(HasBlockData(Door, "Job") && GetBlockData(Door, "Job").Equals("PlayerAlert")){
				if(HasBlockData(Door, "DefaultPower")){
					Door.Enabled = GetBlockData(Door, "DefaultPower").Equals("On");
				}
				if(HasBlockData(Door, "DefaultState")){
					if(GetBlockData(Door, "DefaultState").Equals("Open"))
						Door.OpenDoor();
					else
						Door.CloseDoor();
				}
				SetBlockData(Door, "Job", "None");
			}
		}
	}
}

private void UpdateAirlock(Airlock airlock){
	if(airlock.Door1.Status != DoorStatus.Closed && airlock.Door2.Status != DoorStatus.Closed){
		airlock.Door1.Enabled = true;
		airlock.Door1.CloseDoor();
		airlock.Door2.Enabled = true;
		airlock.Door2.CloseDoor();
	}
	if(!(CanHaveJob(airlock.Door1, "Airlock")&&(CanHaveJob(airlock.Door2, "Airlock"))))
		return;
	bool detected = false;
	double min_distance_1 = double.MaxValue;
	double min_distance_2 = double.MaxValue;
	double min_distance_check = 3.75 * (1 + (Controller.GetShipSpeed() / 200));
	foreach(EntityInfo Entity in CharacterList){
		if(Entity.Relationship != MyRelationsBetweenPlayerAndBlock.Enemies && Entity.Relationship != MyRelationsBetweenPlayerAndBlock.Neutral){
			Vector3D position = Entity.Position + CurrentVelocity / 100;
			double distance = airlock.Distance(Entity.Position);
			bool is_closest_to_this_airlock = distance <= min_distance_check;
			if(is_closest_to_this_airlock){
				foreach(Airlock alock in Airlocks){
					if(is_closest_to_this_airlock && !alock.Equals(airlock)){
						is_closest_to_this_airlock = is_closest_to_this_airlock && distance < (alock.Distance(position));
					}
				}
			}
			if(is_closest_to_this_airlock){
				detected=true;
				min_distance_1 = Math.Min(min_distance_1, (airlock.Door1.GetPosition() - position).Length());
				min_distance_2 = Math.Min(min_distance_2, (airlock.Door2.GetPosition() - position).Length());
			}
		}
	}
	
	if(detected){
		SetBlockData(airlock.Door1, "Job", "Airlock");
		SetBlockData(airlock.Door2, "Job", "Airlock");
		if(min_distance_1 <= min_distance_2){
			airlock.Door2.Enabled = (airlock.Door2.Status != DoorStatus.Closed);
			if(airlock.Door2.Status != DoorStatus.Closing)
				airlock.Door2.CloseDoor();
			if(airlock.Door2.Enabled){
				airlock.Door1.Enabled = (airlock.Door1.Status != DoorStatus.Closed);
				if(airlock.Door1.Status != DoorStatus.Closing)
					airlock.Door1.CloseDoor();
				ScanString += '\t' + "Closing Door 2" + '\n';
			}
			else {
				airlock.Door1.Enabled = true;
				if(airlock.Door1.Status != DoorStatus.Opening)
					airlock.Door1.OpenDoor();
				ScanString += '\t' + "Opening Door 1" + '\n';
			}
		}
		else {
			airlock.Door1.Enabled = (airlock.Door1.Status != DoorStatus.Closed);
			if(airlock.Door1.Status != DoorStatus.Closing)
				airlock.Door1.CloseDoor();
			if(airlock.Door1.Enabled){
				airlock.Door2.Enabled = (airlock.Door2.Status != DoorStatus.Closed);
				if(airlock.Door2.Status != DoorStatus.Closing)
					airlock.Door2.CloseDoor();
				ScanString += '\t' + "Closing Door 1" + '\n';
			}
			else {
				airlock.Door2.Enabled = true;
				if(airlock.Door2.Status != DoorStatus.Opening)
					airlock.Door2.OpenDoor();
				ScanString += '\t' + "Opening Door 2" + '\n';
			}
		}
	}
	else{
		SetBlockData(airlock.Door1, "Job", "None");
		SetBlockData(airlock.Door2, "Job", "None");
		airlock.Door1.Enabled = (airlock.Door1.Status != DoorStatus.Closed);
		if(airlock.Door1.Status != DoorStatus.Closing)
			airlock.Door1.CloseDoor();
		airlock.Door2.Enabled = (airlock.Door2.Status != DoorStatus.Closed);
		if(airlock.Door2.Status != DoorStatus.Closing)
			airlock.Door2.CloseDoor();
		ScanString += '\t' + "Closing both Doors" + '\n';
	}
}

//Performs the scan function on all scanning devices
private double Scan_Frequency{
	get{
		double output = 10;
		double MySize = Me.CubeGrid.GridSize;
		double distance = SmallShipList.ClosestDistance(this);
		if(distance >= MySize){
			output = Math.Max(1, Math.Min(10,(distance+MySize+100)/100));
		}
		distance = SmallShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies);
		output = Math.Max(1, Math.Min(10,(distance-MySize+100)/100));
		distance = LargeShipList.ClosestDistance(this);
		if(distance >= MySize){
			output = Math.Max(1, Math.Min(10,(distance+MySize+100)/100));
		}
		distance = LargeShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies);
		output = Math.Max(1, Math.Min(10,(distance-MySize+100)/100));
		distance = CharacterList.ClosestDistance(this);
		output = Math.Max(1, Math.Min(10,(distance+MySize+100)/100));
		return output;
	}
}
private double Scan_Time = 10;
private string ScanString = "";
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
	List<IMyCameraBlock> AllCameras = new List<IMyCameraBlock>();
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
		double raycast_distance = RAYCAST_DISTANCE;
		if(Camera.RaycastDistanceLimit != -1){
			raycast_distance = Math.Min(raycast_distance, Camera.RaycastDistanceLimit);
		}
		MyDetectedEntityInfo Raycast_Entity = Camera.Raycast(raycast_distance, 0, 0);
		if(update_me && Raycast_Entity.EntityId != Me.CubeGrid.EntityId && Raycast_Entity.EntityId != Camera.CubeGrid.EntityId){
			SetBlockData(Camera, "DoRaycast", "true");
			update_me = false;
		}
		UpdateList(DetectedEntities, Raycast_Entity);
		if(!update_me){
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
					UpdateList(DetectedEntities, Raycast_Entity);
				}
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
		if(Entity.ID==Target_ID){
			RestingVelocity=Entity.Velocity;
			Target_Direction = Entity.Position-Me.CubeGrid.GetPosition();
			Target_Direction.Normalize();
			Target_Position = GetOffsetPosition(Entity.Position);
		}
		switch(Entity.Type){
			case MyDetectedEntityType.Asteroid:
				AsteroidList.UpdateEntry(Entity);
				break;
			case MyDetectedEntityType.Planet:
				PlanetList.UpdateEntry(Entity);
				break;
			case MyDetectedEntityType.SmallGrid:
				SmallShipList.UpdateEntry(Entity);
				break;
			case MyDetectedEntityType.LargeGrid:
				LargeShipList.UpdateEntry(Entity);
				break;
			case MyDetectedEntityType.CharacterHuman:
				CharacterList.UpdateEntry(Entity);
				break;
			case MyDetectedEntityType.CharacterOther:
				CharacterList.UpdateEntry(Entity);
				break;
		}
	}
	
	PerformAlarm();
	
	for(int i=0; i<Airlocks.Count; i++){
		ScanString += "Airlock " + (i+1).ToString() + " Status:" + '\n';
		UpdateAirlock(Airlocks[i]);
	}
	
	if(Command_Menu.AutoRefresh())
		DisplayMenu();
	
	switch(ShipStatus){
		case AlertStatus.Green:
			SetStatus("Condition " + ShipStatus.ToString() + Submessage, new Color(137, 255, 137, 255), new Color(0, 151, 0, 255));
			break;
		case AlertStatus.Blue:
			SetStatus("Condition " + ShipStatus.ToString() + Submessage, new Color(137, 239, 255, 255), new Color(0, 88, 151, 255));
			break;
		case AlertStatus.Yellow:
			SetStatus("Condition " + ShipStatus.ToString() + Submessage, new Color(255, 239, 137, 255), new Color(66, 66, 0, 255));
			break;
		case AlertStatus.Orange:
			SetStatus("Condition " + ShipStatus.ToString() + Submessage, new Color(255, 197, 0, 255), new Color(88, 44, 0, 255));
			break;
		case AlertStatus.Red:
			SetStatus("Condition " + ShipStatus.ToString() + Submessage, new Color(255, 137, 137, 255), new Color(151, 0, 0, 255));
			break;
	}
	
	ScanString += "Completed updating data" + '\n';
	Scan_Time = 0;
}

public void PerformDisarm(){
	List<IMyWarhead> Warheads = new List<IMyWarhead>();
	GridTerminalSystem.GetBlocksOfType<IMyWarhead>(Warheads);
	foreach(IMyWarhead Warhead in Warheads){
		if(HasBlockData(Warhead, "VerifiedWarhead") && GetBlockData(Warhead, "VerifiedWahead").Equals("Active"))
			continue;
		Warhead.DetonationTime = Math.Max(60, Warhead.DetonationTime);
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

private double GetAngle(Vector3D v1, Vector3D v2){
	return GenericMethods<IMyTerminalBlock>.GetAngle(v1,v2);
}

//Sets gyroscope outputs from player input, dampeners, gravity, and autopilot
double Pitch_Time =  1.0f;
double Yaw_Time = 1.0f;
double Roll_Time = 1.0f;
private void SetGyroscopes(){
	float current_pitch = (float) Relative_AngularVelocity.X;
	float current_yaw = (float) Relative_AngularVelocity.Y;
	float current_roll = (float) Relative_AngularVelocity.Z;
	
	float input_pitch = 0;
	float input_yaw = 0;
	float input_roll = 0;
	
	if(Pitch_Time<1)
		Pitch_Time+=seconds_since_last_update;
	if(Yaw_Time<1)
		Yaw_Time+=seconds_since_last_update;
	if(Roll_Time<1)
		Roll_Time+=seconds_since_last_update;
	
	bool adjusting_target = false;
	
	input_pitch = Math.Min(Math.Max(Controller.RotationIndicator.X / 200, -1), 1);
	if(Math.Abs(input_pitch) < 0.1f){
		input_pitch = current_pitch * -0.99f;
		if(Elevation<Controller.GetShipSpeed()*2 && Elevation<50 && GetAngle(Gravity, Controller_Down) < 90 && Pitch_Time>=1){
			double difference = Math.Abs(GetAngle(Gravity, Controller_Forward));
			if(difference < 90){
				input_pitch-=((float)Math.Min(Math.Abs((90-difference)/90), 1));
			}
		}
		if(Match_Direction){
			double difference = GetAngle(Controller_Down, Target_Direction) - GetAngle(Controller_Up, Target_Direction);
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE){
				adjusting_target = true;
				if(AngularVelocity.Length() < 1){
					if(difference>0){
						input_pitch -= 0.5f * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1));
					}
					else {
						input_pitch += 0.5f * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1));
					}
				}
			}
		}
	}
	else{
		Pitch_Time = 0;
		input_pitch *= 300;
	}
	input_yaw = Math.Min(Math.Max(Controller.RotationIndicator.Y / 200, -1), 1);
	if(Math.Abs(input_yaw) < 0.1f){
		input_yaw = current_yaw * -0.99f;
		if(Match_Direction){
			double difference = GetAngle(Controller_Right, Target_Direction) - GetAngle(Controller_Left, Target_Direction);
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE || GetAngle(Controller_Forward, Target_Direction) > ACCEPTABLE_ANGLE){
				adjusting_target = true;
				if(AngularVelocity.Length() < 1){
					if(difference>0 || difference==0 && GetAngle(Controller_Forward, Target_Direction) > ACCEPTABLE_ANGLE){
						input_yaw -= 0.5f * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1));
					}
					else {
						input_yaw += 0.5f * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1));
					}
				}
			}
		}
	}
	else{
		Yaw_Time = 0;
		input_yaw *= 300;
	}
	input_roll = Controller.RollIndicator;
	if(Math.Abs(input_roll) < 0.1f){
		input_roll = current_roll * -0.99f;
		if(Gravity.Length() > 0  && Roll_Time >= 1 && !adjusting_target){
			double difference = (GetAngle(Left_Vector, Gravity) - GetAngle(Right_Vector, Gravity));
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE){
				if(AngularVelocity.Length() < 1){
					if(difference>0){
						input_roll += 0.9f * 30 * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1));
					}
					else {
						input_roll -= 0.9f * 30 * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1));
					}
				}
			}
		}
	}
	else{
		Roll_Time = 0;
		input_roll *= 200;
	}
	
	Gyro_Tuple output = Transform(new Gyro_Tuple(input_pitch, input_yaw, input_roll));
	
	Gyroscope.Pitch = output.Pitch / 100;
	Gyroscope.Yaw = output.Yaw / 100;
	Gyroscope.Roll = output.Roll / 100;
	
	Write("Pitch: " + Math.Round(Gyroscope.Pitch*100, 1).ToString() + " RPM");
	Write("Yaw: " + Math.Round(Gyroscope.Yaw*100, 1).ToString() + " RPM");
	Write("Roll: " + Math.Round(Gyroscope.Roll*100, 1).ToString() + " RPM");
}

//Sets thruster outputs from player input, dampeners, gravity, and autopilot
private void SetThrusters(){
	float input_forward = 0.0f;
	float input_up = 0.0f;
	float input_right = 0.0f;
	
	float damp_multx = 0.99f;
	double effective_speed_limit = SPEED_LIMIT;
	
	if(Elevation<100){
		effective_speed_limit = Math.Min(effective_speed_limit, Elevation);
	}
	
	if(Controller.DampenersOverride){
		Write("Cruise Control: Off");
		
		Write(EntityInfo.NeatVector(CurrentVelocity));
		Write(EntityInfo.NeatVector(Relative_CurrentVelocity));
		Write("");
		input_right -= (float) (Relative_CurrentVelocity.X * 1 * damp_multx);
		input_up -= (float) (Relative_CurrentVelocity.Y * 1 * damp_multx);
		input_forward += (float) (Relative_CurrentVelocity.Z * 1 * damp_multx);
		Write(Math.Round(input_right/Gravity.Length(), 3).ToString());
		Write(Math.Round(input_up/Gravity.Length(), 3).ToString());
		Write(Math.Round(input_forward/Gravity.Length(), 3).ToString());
		
		/*input_right -= (float) ((Relative_CurrentVelocity.X-Relative_RestingVelocity.X) * Mass_Accomodation * damp_multx);
		input_up -= (float) ((Relative_CurrentVelocity.Y-Relative_RestingVelocity.Y) * Mass_Accomodation * damp_multx);
		input_forward += (float) ((Relative_CurrentVelocity.Z-Relative_RestingVelocity.Z) * Mass_Accomodation * damp_multx);*/
	}
	else {
		Write("Cruise Control: On");
		Vector3D velocity_direction = Controller.GetShipVelocities().LinearVelocity;
		velocity_direction.Normalize();
		double angle = Math.Min(GetAngle(Controller_Forward, velocity_direction), GetAngle(Controller_Backward, velocity_direction));
		if(angle <= ACCEPTABLE_ANGLE / 2){
			input_right -= (float) ((Relative_CurrentVelocity.X-Relative_RestingVelocity.X) * Mass_Accomodation * damp_multx);
			input_up -= (float) ((Relative_CurrentVelocity.Y-Relative_RestingVelocity.Y) * Mass_Accomodation * damp_multx);
			Write("Stabilizers: On (" + Math.Round(angle, 1) + "° dev)");
		}
		else {
			Write("Stabilizers: Off (" + Math.Round(angle, 1) + "° dev)");
		}
	}
	
	double Target_Distance = (Target_Position - Me.CubeGrid.GetPosition()).Length();
	if(Target_Distance<Math.Max(0.5f, Math.Min(10, Me.CubeGrid.GridSize/10))){
		Match_Direction = false;
		Match_Position = false;
	}
	effective_speed_limit = Math.Max(effective_speed_limit, 5);
	
	if(Gravity.Length() > 0 && Mass_Accomodation>0){
		input_right -= (float) Adjusted_Gravity.X;
		input_up -= (float) Adjusted_Gravity.Y;
		input_forward += (float) Adjusted_Gravity.Z;
	}
	
	bool matched_direction = !Match_Direction;
	if(Match_Direction){
		if(Gravity.Length() > 0){
			matched_direction = Math.Abs(GetAngle(Target_Direction, Controller_Left) - GetAngle(Target_Direction, Controller_Right)) <= ACCEPTABLE_ANGLE/2;
			double difference = GetAngle(Target_Direction, Gravity_Direction) - 90;
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE){
				matched_direction = matched_direction && (GetAngle(Target_Direction, Controller_Forward) < GetAngle(Target_Direction, Controller_Backward));
				if(difference > ACCEPTABLE_ANGLE){
					matched_direction = matched_direction && (GetAngle(Gravity_Direction, Controller_Forward) >= 90 + (ACCEPTABLE_ANGLE - 5));
				}
				else if(difference < -1 * ACCEPTABLE_ANGLE){
					matched_direction = matched_direction && (GetAngle(Gravity_Direction, Controller_Forward) <= 90 - (ACCEPTABLE_ANGLE - 5));
				}
			}
			else{
				matched_direction = matched_direction && (GetAngle(Controller_Forward, Target_Direction) <= ACCEPTABLE_ANGLE);
			}
		}
		else {
			matched_direction = (GetAngle(Controller_Forward, Target_Direction) <= ACCEPTABLE_ANGLE);
		}
	}
	
	if(Math.Abs(Controller.MoveIndicator.X)>0.5f){
		if(Controller.MoveIndicator.X > 0){
			if((CurrentVelocity + Controller_Right - RestingVelocity).Length() <= effective_speed_limit)
				input_right = 0.95f * Right_Thrust;
			else
				input_right = Math.Min(input_right, 0);
		} else {
			if((CurrentVelocity + Controller_Left - RestingVelocity).Length() <= effective_speed_limit)
				input_right = -0.95f * Left_Thrust;
			else
				input_right = Math.Max(input_right, 0);
		}
	}
	else if(Match_Position){
		double Relative_Speed = Relative_CurrentVelocity.X;
		double Relative_Target_Speed = RestingVelocity.X;
		double Relative_Distance = Relative_Target_Position.X;
		double deacceleration = 0;
		double difference = Relative_Speed - Relative_Target_Speed;
		if(difference > 0){
			deacceleration = Math.Abs(difference) / Left_Thrust;
		}
		else if(difference < 0){
			deacceleration = Math.Abs(difference) / Right_Thrust;
		}
		if((difference > 0) ^ (Relative_Distance < 0)){
			double time = difference / deacceleration;
			time = (Relative_Distance - (difference*time/2))/difference;
			if(time > 0 && (!Match_Direction || matched_direction) && Relative_Speed-Relative_Target_Speed<=0.05){
				if(difference > 0){
					if((CurrentVelocity + Controller_Left - RestingVelocity).Length() <= Math.Min(Elevation, Math.Min(effective_speed_limit, Target_Distance)))
						input_right = -0.95f * Left_Thrust;
				}
				else {
					if((CurrentVelocity + Controller_Right - RestingVelocity).Length() <= Math.Min(Elevation, Math.Min(effective_speed_limit, Target_Distance)))
						input_right = 0.95f * Right_Thrust;
				}
			}
		}
	}
	
	if(Math.Abs(Controller.MoveIndicator.Y)>0.5f){
		if(Controller.MoveIndicator.Y > 0){
			if((CurrentVelocity + Controller_Up - RestingVelocity).Length() <= effective_speed_limit)
				input_up = 0.95f * Up_Thrust;
			else
				input_up = Math.Min(input_up, 0);
		} else {
			if((CurrentVelocity + Controller_Down - RestingVelocity).Length() <= effective_speed_limit)
				input_up = -0.95f * Down_Thrust;
			else
				input_up = Math.Max(input_up, 0);
		}
	}
	else if(Match_Position){
		double Relative_Speed = Relative_CurrentVelocity.Y;
		double Relative_Target_Speed = RestingVelocity.Y;
		double Relative_Distance = Relative_Target_Position.Y;
		double deacceleration = 0;
		double difference = Relative_Speed - Relative_Target_Speed;
		if(difference > 0){
			deacceleration = Math.Abs(difference) / Down_Thrust;
		}
		else if(difference < 0){
			deacceleration = Math.Abs(difference) / Up_Thrust;
		}
		if((difference > 0) ^ (Relative_Distance < 0)){
			double time = difference / deacceleration;
			time = (Relative_Distance - (difference*time/2))/difference;
			if(time > 0 && (!Match_Direction || matched_direction) && Relative_Speed-Relative_Target_Speed<=0.05){
				if(difference > 0){
					if((CurrentVelocity + Controller_Down - RestingVelocity).Length() <= Math.Min(Elevation, Math.Min(effective_speed_limit, Target_Distance)))
						input_up = -0.95f * Down_Thrust;
				}
				else {
					if((CurrentVelocity + Controller_Up - RestingVelocity).Length() <= Math.Min(Elevation, Math.Min(effective_speed_limit, Target_Distance)))
						input_up = 0.95f * Up_Thrust;
				}
			}
		}
	}
	
	if(Math.Abs(Controller.MoveIndicator.Z)>0.5f){
		if(Controller.MoveIndicator.Z < 0){
			if((CurrentVelocity + Controller_Up - RestingVelocity).Length() <= effective_speed_limit)
				input_forward = 0.95f * Forward_Thrust;
			else
				input_forward = Math.Min(input_forward, 0);
		} else {
			if((CurrentVelocity + Controller_Down - RestingVelocity).Length() <= effective_speed_limit)
				input_forward = -0.95f * Backward_Thrust;
			else
				input_forward = Math.Max(input_forward, 0);
		}
	}
	else if(Match_Position){
		double Relative_Speed = Relative_CurrentVelocity.Z;
		double Relative_Target_Speed = RestingVelocity.Z;
		double Relative_Distance = Relative_Target_Position.Z;
		double deacceleration = 0;
		double difference = Relative_Speed - Relative_Target_Speed;
		if(difference > 0){
			deacceleration = Math.Abs(difference) / Backward_Thrust;
		}
		else if(difference < 0){
			deacceleration = Math.Abs(difference) / Forward_Thrust;
		}
		if((difference > 0) ^ (Relative_Distance < 0)){
			double time = difference / deacceleration;
			time = (Relative_Distance - (difference*time/2))/difference;
			if(time > 0 && (!Match_Direction || matched_direction) && Relative_Speed-Relative_Target_Speed<=0.05){
				if(difference > 0){
					if((CurrentVelocity + Controller_Down - RestingVelocity).Length() <= Math.Min(Elevation, Math.Min(effective_speed_limit, Target_Distance)))
						input_forward = -0.95f * Backward_Thrust;
				}
				else {
					if((CurrentVelocity + Controller_Up - RestingVelocity).Length() <= Math.Min(Elevation, Math.Min(effective_speed_limit, Target_Distance)))
						input_forward = 0.95f * Forward_Thrust;
				}
			}
		}
	}
	
	float output_forward = 0.0f;
	float output_backward = 0.0f;
	if(input_forward / Forward_Thrust > 0.05f){
		output_forward = Math.Min(Math.Abs(input_forward / Forward_Thrust), 1);
		Write("Forward: " + Math.Round(output_forward*100, 1).ToString() + '%');
	}
	else if(input_forward / Backward_Thrust < -0.05f){
		output_backward = Math.Min(Math.Abs(input_forward / Backward_Thrust), 1);
		Write("Backward: " + Math.Round(output_backward*100, 1).ToString() + '%');
	}
	float output_up = 0.0f;
	float output_down = 0.0f;
	if(input_up / Up_Thrust > 0.05f){
		output_up = Math.Min(Math.Abs(input_up / Up_Thrust), 1);
		Write("Up: " + Math.Round(output_up*100, 1).ToString() + '%');
	}
	else if(input_up / Down_Thrust < -0.05f){
		output_down = Math.Min(Math.Abs(input_up / Down_Thrust), 1);
		Write("Down: " + Math.Round(output_down*100, 1).ToString() + '%');
	}
	float output_right = 0.0f;
	float output_left = 0.0f;
	if(input_right / Right_Thrust > 0.05f){
		output_right = Math.Min(Math.Abs(input_right / Right_Thrust), 1);
		Write("Right: " + Math.Round(output_right*100, 1).ToString() + '%');
	}
	else if(input_right / Left_Thrust < -0.05f){
		output_left = Math.Min(Math.Abs(input_right / Left_Thrust), 1);
		Write("Left: " + Math.Round(output_left*100, 1).ToString() + '%');
	}
	
	foreach(IMyThrust Thruster in Forward_Thrusters){
		Thruster.ThrustOverridePercentage = output_forward;
	}
	foreach(IMyThrust Thruster in Backward_Thrusters){
		Thruster.ThrustOverridePercentage = output_backward;
	}
	foreach(IMyThrust Thruster in Up_Thrusters){
		Thruster.ThrustOverridePercentage = output_up;
	}
	foreach(IMyThrust Thruster in Down_Thrusters){
		Thruster.ThrustOverridePercentage = output_down;
	}
	foreach(IMyThrust Thruster in Right_Thrusters){
		Thruster.ThrustOverridePercentage = output_right;
	}
	foreach(IMyThrust Thruster in Left_Thrusters){
		Thruster.ThrustOverridePercentage = output_left;
	}
}

//Sets directional vectors, Elevation, etc
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
	
	Gravity = Controller.GetNaturalGravity();
	CurrentVelocity=Controller.GetShipVelocities().LinearVelocity;
	AngularVelocity=Controller.GetShipVelocities().AngularVelocity;
	
	if(Controller.TryGetPlanetElevation(MyPlanetElevation.Sealevel, out Sealevel)){
		if(Controller.TryGetPlanetPosition(out PlanetCenter)){
			if(Sealevel < 6000 && Controller.TryGetPlanetElevation(MyPlanetElevation.Surface, out Elevation)){
				if(Sealevel > 5000){
					double difference = Sealevel - 5000;
					Elevation =  ((Elevation * (1000-difference)) + (Sealevel * difference)) / 1000;
				}
				else if(Elevation < 50){
					double terrain_height = (Controller.GetPosition() - PlanetCenter).Length() - Elevation;
					List<IMyTerminalBlock> AllBlocks = new List<IMyTerminalBlock>();
					GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(AllBlocks);
					foreach(IMyTerminalBlock Block in AllBlocks){
						Elevation = Math.Min(Elevation, (Block.GetPosition() - PlanetCenter).Length() - terrain_height);
					}
				}
			}
			else {
				Elevation = Sealevel;
			}
			double height_dif = (Me.CubeGrid.GetPosition() - PlanetCenter).Length() - Elevation;
			Vector3D next_position = Me.CubeGrid.GetPosition() + 1 * CurrentVelocity;
			double Elevation_per_second = ((next_position-PlanetCenter).Length()-height_dif)-Elevation;
			Time_To_Crash = Elevation / Elevation_per_second;
			if(Time_To_Crash<15 && Controller.GetShipSpeed() > 5){
				Controller.DampenersOverride = true;
				Write("Crash predicted within 15 seconds; enabling Dampeners");
			}
			else {
				if(Time_To_Crash*Math.Max(Elevation,1000) < 1800000 && Controller.GetShipSpeed() > 1.0f){
					Write(Math.Round(Time_To_Crash, 1).ToString() + " seconds to crash");
				}
				else {
					Write("No crash likely at current velocity");
				}
			}
		}
		else {
			PlanetCenter = new Vector3D(0,0,0);
		}
	}
	else{
		Sealevel = double.MaxValue;
	}
	if(Match_Position){
		Target_Position+=seconds_since_last_update*RestingVelocity;
	}
	if(Match_Direction){
		Target_Direction = Target_Position-Me.CubeGrid.GetPosition();
		Target_Direction.Normalize();
	}
	
	Mass_Accomodation = (float) (Controller.CalculateShipMass().PhysicalMass * Gravity.Length());
	
}

public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	GetPositionData();
	Scan_Time+=seconds_since_last_update;
	if(Scan_Time >= Scan_Frequency){
		PerformScan();
	}
	Write(ScanString);
	
	if(argument.ToLower().Equals("back"))
		Command_Menu.Back();
	else if(argument.ToLower().Equals("prev"))
		Command_Menu.Prev();
	else if(argument.ToLower().Equals("next"))
		Command_Menu.Next();
	else if(argument.ToLower().Equals("select"))
		Command_Menu.Select();
	else if(argument.ToLower().Equals("factory reset"))
		FactoryReset();
    
	if(!Me.CubeGrid.IsStatic){
		SetThrusters();
		SetGyroscopes();
	}
	
	Runtime.UpdateFrequency = GetUpdateFrequency();
}
