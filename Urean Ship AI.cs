/*
* Ship AI System 
* Built by mlq1616
* https://github.com/mlq1819
*/
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
			E_List.Insert(index,Unsorted.Deqeue());
		}
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
		if(Count<=10){
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
						output+=" ("+(Menu[i] as Menu_Submenu).Count.ToString()+")]";
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
			for(int i=Selection; count<=10 && i<Count;i++){
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

class Menu_Display:MenuOption{
	public string Name(){
		if(Entity==null){
			return "null";
		}
		string name=Entity.Name.Substring(0,Math.Min(24,Entity.Name.Length));
		string[] args=name.Split(' ');
		int number=0;
		if(args.Length==3&&args[1].ToLower().Equals("grid")&&Int32.TryParse(args[2],out number))
			name="Unnamed "+args[0]+' '+args[1];
		double distance=Entity.GetDistance(Prog.P.Me.GetPosition())-Entity.Size;
		string distance_string=Math.Round(distance,0).ToString()+"M";
		if(distance>=1000)
			distance_string=Math.Round(distance/1000,1).ToString()+"kM";
		string output=' '+name+' '+distance_string;
		switch(Entity.Relationship){
			case MyRelationsBetweenPlayerAndBlock.Owner:
				return ''+output;
			case MyRelationsBetweenPlayerAndBlock.FactionShare:
				return ''+output;
			case MyRelationsBetweenPlayerAndBlock.Friends:
				return ''+output;
			case MyRelationsBetweenPlayerAndBlock.NoOwnership:
				return ''+output;
			case MyRelationsBetweenPlayerAndBlock.Enemies:
				return ''+output;
		}
		return ''+output;
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
	bool Selected;
	EntityInfo Entity;
	bool Can_GoTo;
	Func<EntityInfo, bool> Command;
	Menu_Command<EntityInfo> Subcommand {
		get{
			double distance=(P.Me.GetPosition()-Entity.Position).Length()-Entity.Size;
			return new Menu_Command<EntityInfo>("GoTo "+Entity.Name, Command, "Set autopilot to match target Entity's expected position and velocity", Entity);
		}
	}
		
	public Menu_Display(EntityInfo entity, Func<EntityInfo, bool> GoTo){
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
		if(!Can_GoTo)
			return false;
		if(Selected){
			if(Command==null)
				return false;
			if(Subcommand.Select()){
				Selected=false;
				return true;
			}
			return false;
		}
		Selected=true;
		return true;
	}
	
	public bool Back(){
		if(!Selected)
			return false;
		Selected=false;
		return true;
	}
	
	public override string ToString(){
		if(Selected)
			return Subcommand.ToString();
		else {
			double distance=Entity.GetDistance(Prog.P.Me.GetPosition());
			string distance_string=Math.Round(distance,0)+"M";
			if(distance>=1000)
				distance_string=Math.Round(distance/1000,1)+"kM";
			return Entity.NiceString()+"Distance: "+distance_string;
		}
	}
}

struct Airlock{
	public IMyDoor Door1;
	public IMyDoor Door2;
	public IMyAirVent Vent;
	public double VentTimer=1;
	public Airlock(IMyDoor d1,IMyDoor d2,IMyAirVent v=null){
		Door1=d1;
		Door2=d2;
		Vent=v;
	}
	public bool Equals(Airlock o){
		return Door1.Equals(o.Door1)&&Door2.Equals(o.Door2)&&Vent.Equals(o.Vent);
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
}

public void Save(){
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
