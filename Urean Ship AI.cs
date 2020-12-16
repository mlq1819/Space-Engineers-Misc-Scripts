/*
* Ship AI System 
* Built by mlq1616
* https://github.com/mlq1819
*/
const string Program_Name = ""; //Name me!
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);
string Lockdown_Door_Name="Air Seal";
string Lockdown_Light_Name="";
double Alert_Distance=15;

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
	public int Display_Count{
		get{
			if(Count>0&&Menu[0].Type==MenuType.Command){
				for(int i=1;i<Count;i++){
					if(Menu[i].Type!=MenuType.Display)
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
	public double AirlockTimer=10;
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

string GetRemovedString(string big_string, string small_string){
	string output=big_string;
	if(big_string.Contains(small_string)){
		output=big_string.Substring(0, big_string.IndexOf(small_string))+big_string.Substring(big_string.IndexOf(small_string)+small_string.Length);
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

TimeSpan Time_Since_Start=new TimeSpan(0);
long cycle=0;
char loading_char='|';
double seconds_since_last_update=0;
Random Rnd;

IMyShipController Controller;
IMyGyro Gyroscope;

List<IMyTextPanel> StatusLCDs;
List<IMyTextPanel> DebugLCDs;

List<IMyDoor> AutoDoors;
List<Airlock> Airlocks;

EntityList[5] EntityLists=new EntityList[5];
EntityList AsteroidList{
	set{
		EntityLists[0]=value;
	}
	get{
		return EntityLists[0];
	}
}
EntityList PlanetList{
	set{
		EntityLists[1]=value;
	}
	get{
		return EntityLists[1];
	}
}
EntityList SmallShipList{
	set{
		EntityLists[2]=value;
	}
	get{
		return EntityLists[2];
	}
}
EntityList LargeShipList{
	set{
		EntityLists[3]=value;
	}
	get{
		return EntityLists[3];
	}
}
EntityList CharacterList{
	set{
		EntityLists[4]=value;
	}
	get{
		return EntityLists[4];
	}
}

List<IMyThrust>[6] All_Thrusters=new List<IMyThrust>[6];
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
		return total;
	}
}
float Backward_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Backward_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
float Up_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Up_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
float Down_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Down_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
float Left_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Left_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}
float Right_Thrust{
	get{
		float total=0;
		foreach(IMyThrust Thruster in Right_Thrusters)
			total+=Thruster.MaxEffectiveThrust;
		return total;
	}
}

double Time_To_Crash=double.MaxValue;
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

bool Tracking=false;
bool Match_Direction=false;
Vector3D Target_Direction;
bool Match_Position=false;
Vector3D Target_Position;
Vector3D Relative_Target_Position{
	get{
		return GlobalToLocalPosition(Target_Position);
	}
}
double Target_Distance{
	get{
		return (Target_Position-Controller.GetPosition()).Length();
	}
}
long Target_ID=0;

float Mass_Accomodation=0.0f;

Vector3D RestingVelocity;
Vector3D Relative_RestingVelocity{
	get{
		return GlobalToLocal(RestingVelocity);
	}
}
Vector3D CurrentVelocity;
Vector3D Relative_CurrentVelocity{
	get{
		Vector3D output=Vector3D.Transform(CurrentVelocity+Controller.GetPosition(), MatrixD.Invert(Controller.WorldMatrix));
		output.Normalize();
		output *= CurrentVelocity.Length();
		return output;
	}
}
Vector3D Gravity;
Vector3D Relative_Gravity{
	get{
		return GlobalToLocal(Gravity);
	}
}
Vector3D Adjusted_Gravity{
	get{
		Vector3D temp=GlobalToLocal(Gravity);
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
		return GlobalToLocal(AngularVelocity);
	}
}

double Elevation;
double Sealevel;
Vector3D PlanetCenter;

bool ControllerFunction(IMyShipController ctrlr){
	IMyRemoteControl Remote=ctrlr as IMyRemoteControl;
	return ctrlr.CanControlShip&&ctrlr.ControlThrusters&&(ctrlr.IsMainCockpit||Remote!=null);
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
	foreach(Airlock airlock in Airlocks){
		string name=GetRemovedString(airlock.Door1.CustomName,"Door 1");
		List<IMyAirVent> Vents=GenericMethods<IMyAirVent>.GetAllConstruct(name+"Air Vent",airlock.Door1);
		foreach(IMyAirVent Vent in Vents){
			if(Vent.CustomName.Equals(name+"Air Vent")){
				airlock.Vent=Vent;
				break;
			}
		}
	}
}

string GetThrustTypeName(IMyThrust Thruster){
	string block_type=Thruster.BlockDefinition.SubtypeName;
	if(block_type.Contains("LargeBlock"))
		block_type=GetRemovedString(block_type,"LargeBlock");
	else if(block_type.Contains("SmallBlock"))
		block_type=GetRemovedString(block_type,"SmallBlock");
	if(block_type.Contains("Thrust"))
		block_type.GetRemovedString(block_type,"Thrust");
	string size="";
	if(block_type.Contains("Small")){
		size="Small";
		block_type.GetRemovedString(block_type,size);
	}
	else if(block_type.Contains("Large")){
		size="Large";
		block_type.GetRemovedString(block_type,size);
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
		string name=GetThrustTypeName();
		bool found=false;
		for(int i=0;i<Thruster_Types.Count;i++){
			if(name.Equals(Thruster_Types[i].Name)){
				found=true;
				Thruster_Types[i].Count++;
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
				Thruster.CustomName=(Direction+" "+name+" Thruster "+(Thruster_Types[i].Count--).ToString()).Trim();
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
	Runtime.UpdateFrequency=UpdateFrequency.None;
	Controller=null;
	Gyroscope=null;
	for(int i=0;i<EntityLists.Length;i++)
		EntityLists[i]=new EntityList();
	StatusLCDs=new List<IMyTextPanel>();
	DebugLCDs=new List<IMyTextPanel>();
	List<Airlock> Airlocks=new List<Airlock>();
	AutoDoors=new List<IMyDoor>();
	for(int i=0;i<All_Thrusters.Length;i++)
		All_Thrusters[i]=new List<IMyThrust>();
	RestingVelocity=new Vector3D(0,0,0);
}

bool Setup(){
	Reset();
	StatusLCDs=GenericMethods<IMyTextPanel>.GetAllConstruct("Ship Status");
	DebugLCDs=GenericMethods<IMyTextPanel>.GetAllConstruct("AI Visual Display");
	foreach(IMyTextPanel Display in DebugLCDs){
		if(Display.CustomName.ToLower().Contains("transparent")){
			Display.FontColor=DEFAULT_BACKGROUND_COLOR;
			Display.BackgroundColor=new Color(0,0,0,0);
		}
		else{
			Display.FontColor=DEFAULT_TEXT_COLOR;
			Display.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
		Display.Alignment=TextAlignment.CENTER;
		Display.ContentType=ContentType.TEXT_AND_IMAGE;
		Display.TextPadding=10.0f;
		Display.FontSize=1.0f;
	}
	SetupAirlocks();
	AutoDoors=GenericMethods<IMyDoor>.GetAllConstruct(AUTODOOR_NAME);
	Controller=GenericMethods<IMyShipController>.GetClosestFunc(ControllerFunction);
	if(Controller==null){
		Write("Failed to find Controller", false, false);
		return false;
	}
	Forward=Controller.Orientation.Forward;
	Up=Controller.Orientation.Up;
	Left=Controller.Orientation.Left;
	if((Controller as IMyTextSurfaceProvider)!=null){
		IMyTextSurfaceProvider Cockpit=Controller as IMyTextSurfaceProvider;
		for(int i=0;i<Cockpit.SurfaceCount;i++){
			Cockpit.GetSurface(i).FontColor=DEFAULT_TEXT_COLOR;
			Cockpit.GetSurface(i).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
			Cockpit.GetSurface(i).Alignment=TextAlignment.CENTER;
			Cockpit.GetSurface(i).ScriptForegroundColor=DEFAULT_TEXT_COLOR;
			Cockpit.GetSurface(i).ScriptBackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
	}
	MySize=Controller.CubeGrid.GridSize;
	Gyroscope=GenericMethods<IMyGyro>.GetConstruct("Control Gyroscope");
	if(Gyroscope==null){
		Gyroscope=GenericMethods<IMyGyro>.GetConstruct("");
		if(Gyroscope==null&&!Me.CubeGrid.IsStatic)
			return false;
		Gyroscope.CustomName="Control Gyroscope";
	}
	Gyroscope.GyroOverride=Controller.IsUnderControl;
	List<IMyThrust> MyThrusters=GenericMethods<IMyThrust>.GetAllConstruct("");
	for(int i=0;i<2;i++){
		bool retry=!Me.CubeGrid.IsStatic;
		foreach(IMyThrust Thruster in MyThrusters){
			if(HasBlockData(Thruster, "Owner")){
				long ID=0;
				if(i==0&&!Int64.TryParse(GetBlockData(Thruster, "Owner"),out ID)||(ID!=0&&ID!=Me.CubeGrid.EntityId))
					continue;
			}
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
	Me.GetSurface(1).TextPadding=40.0f;
	Echo("Beginning initialization");
	Rnd=new Random();
	Setup();
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		EntityInfo Entity=null;
		if(EntityInfo.TryParse(arg,out Entity)){
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
	IGC.RegisterBroadcastListener("Urean AI");
	IGC.RegisterBroadcastListener("Entity Report");
	IGC.RegisterBroadcastListener(Me.CubeGrid.CustomName);
	CreateMenu();
	DisplayMenu();
}

public void Save(){
    this.Storage="Lockdown:"+_Lockdown.ToString();
	for(int i=0;i<EntityLists.Length;i++){
		foreach(EntityInfo Entity in EntityLists[i])
			this.Storage+='•'+Entity.ToString();
	}
	if(Gyroscope!=null)
		Gyroscope.GyroOverride=false;
	for(int i=0;i<All_Thrusters.Length;i++){
		foreach(IMyThrust Thruster in All_Thrusters[i])
			ResetThruster(Thruster);
	}
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
		if(!Me.CubeGrid.IsStatic){
			if(Elevation-MySize<50){
				AlertStatus new_status=AlertStatus.Blue;
				status=(AlertStatus) Math.Max((int)status, (int)new_status);
				double psuedo_elevation=Math.Max(Elevation-MySize,0);
				Submessage+="\nShip at low Altitude ("+Math.Round(psuedo_elevation,1).ToString()+"-"+Math.Round(Elevation,1).ToString()+" meters)";
			}
			
			if(Time_To_Crash>0){
				if(Time_To_Crash<15 && Controller.GetShipSpeed()>5){
					AlertStatus new_status=AlertStatus.Orange;
					status=(AlertStatus) Math.Max((int)status, (int)new_status);
					Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
				}
				else if(Time_To_Crash<60 && Controller.GetShipSpeed()>15){
					AlertStatus new_status=AlertStatus.Yellow;
					status=(AlertStatus) Math.Max((int)status, (int)new_status);
					Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
				}
				else if(Time_To_Crash<180){
					AlertStatus new_status=AlertStatus.Blue;
					status=(AlertStatus) Math.Max((int)status, (int)new_status);
					Submessage += "\n"+Math.Round(Time_To_Crash,1).ToString()+" seconds to possible impact";
				}
			}
			if(_Autoland&&CurrentVelocity.Length()>1){
				AlertStatus new_status=AlertStatus.Blue;
				status=(AlertStatus) Math.Max((int)status, (int)new_status);
				Submessage += "\nAutoland Enabled";
			}
		}
		if(_Lockdown){
			AlertStatus new_status=AlertStatus.Yellow;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nCurrently in Lockdown";
		}
		
		double ActualEnemyShipDistance=Math.Min(SmallShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies), LargeShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies));
		double EnemyShipDistance=Math.Min(SmallShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies), LargeShipList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies)/2);
		if(EnemyShipDistance<800){
			AlertStatus new_status=AlertStatus.Red;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Ship at "+Math.Round(ActualEnemyShipDistance, 0)+" meters";
		}
		else if(EnemyShipDistance<2500){
			AlertStatus new_status=AlertStatus.Orange;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Ship at "+Math.Round(ActualEnemyShipDistance, 0)+" meters";
		}
		else if(EnemyShipDistance<5000){
			AlertStatus new_status=AlertStatus.Yellow;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Ship at "+Math.Round(ActualEnemyShipDistance, 0)+" meters";
		}
		
		double EnemyCharacterDistance=CharacterList.ClosestDistance(this, MyRelationsBetweenPlayerAndBlock.Enemies);
		if(EnemyCharacterDistance-MySize<0){
			AlertStatus new_status=AlertStatus.Red;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Creature has boarded ship";
		}
		else if(EnemyCharacterDistance-MySize<800){
			AlertStatus new_status=AlertStatus.Orange;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Creature at "+Math.Round(EnemyCharacterDistance, 0)+" meters";
		}
		else if(EnemyCharacterDistance-MySize<2000){
			AlertStatus new_status=AlertStatus.Yellow;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nEnemy Creature at "+Math.Round(EnemyCharacterDistance, 0)+" meters";
		}
		
		double ShipDistance=Math.Min(SmallShipList.ClosestDistance(this),LargeShipList.ClosestDistance(this))-MySize;
		if(ShipDistance<500 && ShipDistance > 0){
			AlertStatus new_status=AlertStatus.Blue;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nNearby ship at "+Math.Round(ShipDistance, 0)+" meters";
		}
		if((!Me.CubeGrid.IsStatic)&&AsteroidList.ClosestDistance(this)<500){
			AlertStatus new_status=AlertStatus.Blue;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			Submessage += "\nNearby asteroid at "+Math.Round(AsteroidList.ClosestDistance(this), 0)+" meters";
		}
		if(Controller.GetShipSpeed() > 30){
			AlertStatus new_status=AlertStatus.Blue;
			status=(AlertStatus) Math.Max((int)status, (int)new_status);
			double Speed=Controller.GetShipSpeed();
			Submessage += "\nHigh Ship Speed [";
			const int SECTIONS=20;
			for(int i=0; i<SECTIONS; i++){
				if(Speed >= ((100.0/SECTIONS)*i)){
					Submessage += '|';
				}
				else {
					Submessage += ' ';
				}
			}
			Submessage += ']';
		}
		if(status == AlertStatus.Green)
			Submessage="\nNo issues";
		return status;
	}
}
void SetStatus(string message, Color TextColor, Color BackgroundColor){
	float padding=40.0f;
	string[] lines=message.Split('\n');
	padding=Math.Max(10.0f, padding-(lines.Length*5.0f));
	foreach(IMyTextPanel LCD in StatusLCDs){
		LCD.Alignment=TextAlignment.CENTER;
		LCD.FontSize=1.2f;
		LCD.ContentType=ContentType.TEXT_AND_IMAGE;
		LCD.TextPadding=padding;
		LCD.WriteText(message, false);
		if(LCD.CustomName.ToLower().Contains("transparent")){
			LCD.FontColor=BackgroundColor;
			LCD.BackgroundColor=new Color(0,0,0,255);
		}
		else {
			LCD.FontColor=TextColor;
			LCD.BackgroundColor=BackgroundColor;
		}
	}
}

void UpdateList(List<MyDetectedEntityInfo> list,MyDetectedEntityInfo new_entity){
	if(new_entity.Type==MyDetectedEntityType.None||new_entity.EntityId==Me.CubeGrid.EntityId)
		return;
	for(int i=0;i<list.Count;i++){
		if(list[i].EntityId==new_entity.EntityId){
			if(list[i].TimeStamp<new_entity.TimeStamp||((list[i].HitPosition==null)&&(new_entity.HitPosition!=null)))
				list[i]=new_entity;
			return;
		}
	}
	list.Add(new_entity);
}
void UpdateList(List<EntityInfo>list,EntityInfo new_entity){
	if(new_entity.Type==MyDetectedEntityType.None||new_entity.ID == Me.CubeGrid.EntityId)
		return;
	for(int i=0;i<list.Count;i++){
		if(list[i].ID==new_entity.ID){
			list[i]=new_entity;
			return;
		}
	}
	list.Add(new_entity);
}

bool Stop(object obj=null){
	RestingVelocity=new Vector3D(0,0,0);
	Target_Position=new Vector3D(0,0,0);
	Match_Direction=false;
	Match_Position=false;
	Tracking=false;
	Target_ID=0;
	return true;
}
bool _Autoland=false;
bool Autoland(object obj=null){
	_Autoland=!_Autoland;
	return true;
}
bool _Lockdown=false;
bool Lockdown(object obj=null){
	_Lockdown=!_Lockdown;
	List<IMyDoor> Seals=GenericMethods<IMyDoor>.GetAllIncluding(Lockdown_Door_Name);
	foreach(IMyDoor Door in Seals){
		if(CanHaveJob(Door,"Lockdown")){
			if(_Lockdown){
				SetBlockData(Door,"Job","Lockdown");
				Door.Enabled=true;
				Door.CloseDoor();
			}
			else{
				SetBlockData(Door,"Job","None");
				Door.Enabled=true;
				Door.OpenDoor();
			}
		}
	}
	if(Lockdown_Light_Name.Length>0){
		List<IMyInteriorLight> Lights=GenericMethods<IMyInteriorLight>.GetAllIncluding(Lockdown_Light_Name);
		foreach(IMyInteriorLight Light in Lights){
			if(CanHaveJob(Light,"Lockdown")){
				if(_Lockdown){
					SetBlockData(Light,"Job","Lockdown");
					if(!HasBlockData(Light,"DefaultColor"))
						SetBlockData(Light,"DefaultColor",Light.Color.ToString());
					Light.Color=new Color(255,255,0,255);
				}
				else{
					SetBlockData(Light,"Job","None");
					if(HasBlockData(Light,"DefaultColor")){
						try{
							Light.Color=ColorParse(GetBlockData(Light,"DefaultColor"));
						}
						catch(Exception){
							Echo("Failed to parse color");
						}
					}
				}
			}
		}
	}
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
	for(int i=0;i<EntityLists.Length;i++)
		EntityLists[i].Clear();
	Me.CustomData="";
	this.Storage="";
	Reset();
	Me.Enabled=false;
	return true;
}
bool DeployFlares(IMyProgrammableBlock FlareBlock){
	return FlareBlock.TryRun("Deploy");
}

bool UpdateEntityListing(Menu_Submenu Menu){
	EntityList list=null;
	bool do_goto=false;
	switch(Menu.Name()){
		case "Asteroids":
			list=AsteroidList;
			do_goto=true;
			break;
		case "Planets":
			list=PlanetList;
			do_goto=false;
			break;
		case "Small Ships":
			list=SmallShipList;
			do_goto=true;
			break;
		case "Large Ships":
			list=LargeShipList;
			do_goto=true;
			break;
		case "Characters":
			list=CharacterList;
			do_goto=true;
			break;
	}
	if(list==null)
		return false;
	Menu=new Menu_Submenu(Menu.Name());
	Menu.Add(new Menu_Command<Menu_Submenu>("Refresh",UpdateEntityListing,"Updates "+Menu.Name(),Menu));
	list.Sort(Controller.GetPosition());
	for(int i=0;i<list.Count;i++)
		Menu.Add(new Menu_Display(list[i], this));
	if(Command_Menu.Replace(Menu)){
		DisplayMenu();
		return true;
	}
	return false;
}

Menu_Submenu[5] Object_Menus=new Menu_Submenu[5];
Menu_Submenu AsteroidMenu{
	set{
		Object_Menus[0]=value;
	}
	get{
		return Object_Menus[0];
	}
}
Menu_Submenu PlanetMenu{
	set{
		Object_Menus[1]=value;
	}
	get{
		return Object_Menus[1];
	}
}
Menu_Submenu SmallShipMenu{
	set{
		Object_Menus[2]=value;
	}
	get{
		return Object_Menus[2];
	}
}
Menu_Submenu LargeShipMenu{
	set{
		Object_Menus[3]=value;
	}
	get{
		return Object_Menus[3];
	}
}
Menu_Submenu CharacterMenu{
	set{
		Object_Menus[4]=value;
	}
	get{
		return Object_Menus[4];
	}
}
bool CreateMenu(object obj=null){
	Command_Menu=new Menu_Submenu("Command Menu");
	Command_Menu.Add(new Menu_Command<object>("Update Menu", CreateMenu, "Refreshes menu"));
	Menu_Submenu ShipCommands=new Menu_Submenu("Commands");
	if(!Me.CubeGrid.IsStatic){
		ShipCommands.Add(new Menu_Command<object>("Stop", Stop, "Disables autopilot"));
		ShipCommands.Add(new Menu_Command<object>("Toggle Autoland",Autoland,"Toggles On/Off the Autoland feature\nLands at 5 m/s\nDo not use on ships with poor mobility!"));
	}
	ShipCommands.Add(new Menu_Command<object>("Scan", PerformScan, "Immediately performs a scan operation"));
	IMyProgrammableBlock FlareBlock=(new GenericMethods<IMyProgrammableBlock>(this)).GetFull("Flare Printer Programmable block");
	if(FlareBlock!=null)
		ShipCommands.Add(new Menu_Command<IMyProgrammableBlock>("Deploy Flares",DeployFlares,"Deploys flares made using Flare Printers",FlareBlock));
	ShipCommands.Add(new Menu_Command<object>("Toggle Lockdown", Lockdown, "Closes/Opens Air Seals"));
	ShipCommands.Add(new Menu_Command<object>("Factory Reset", FactoryReset, "Resets AI memory and settings, and turns it off"));
	Command_Menu.Add(ShipCommands);
	AsteroidMenu=new Menu_Submenu("Asteroids");
	Command_Menu.Add(AsteroidMenu);
	UpdateEntityListing(AsteroidMenu);
	PlanetMenu=new Menu_Submenu("Planets");
	Command_Menu.Add(PlanetMenu);
	UpdateEntityListing(PlanetMenu);
	SmallShipMenu=new Menu_Submenu("Small Ships");
	Command_Menu.Add(SmallShipMenu);
	UpdateEntityListing(SmallShipMenu);
	LargeShipMenu=new Menu_Submenu("Large Ships");
	Command_Menu.Add(LargeShipMenu);
	UpdateEntityListing(LargeShipMenu);
	CharacterMenu=new Menu_Submenu("Characters");
	Command_Menu.Add(CharacterMenu);
	UpdateEntityListing(CharacterMenu);
	return true;
}
void DisplayMenu(){
	List<IMyTextPanel> Panels=GenericMethods<IMyTextPanel>.GetAllConstruct("Command Menu Display");
	foreach(IMyTextPanel Panel in Panels){
		Panel.WriteText(Command_Menu.ToString(),false);
		Panel.Alignment=TextAlignment.CENTER;
		Panel.FontSize=1.2f;
		Panel.ContentType=ContentType.TEXT_AND_IMAGE;
		Panel.TextPadding=10.0f;
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

bool last_performed_alarm=false;
void PerformAlarm(){
	bool nearby_enemy=(CharacterList.ClosestDistance(MyRelationsBetweenPlayerAndBlock.Enemies)<=(float) MySize);
	if(!nearby_enemy&&!last_performed_alarm)
		return;
	last_performed_alarm=nearby_enemy;
	List<IMyInteriorLight> AllLights=GenericMethods<IMyInteriorLight>.GetAllConstruct("");
	foreach(IMyInteriorLight Light in AllLights){
		if(!CanHaveJob(Light,"PlayerAlert"))
			continue;
		double distance=double.MaxValue;
		foreach(EntityInfo Entity in CharacterList){
			if((Entity.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies)&&Entity.Age.TotalSeconds<=60)
				distance=Math.Min(distance,Entity.GetDistance(Light.GetPosition()));
		}
		if(distance<=Alert_Distance){
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
			Light.Color=new Color(255,0,0,255);
			Light.BlinkLength=100.0f-(((float)(distance/Alert_Distance))*50.0f);
			Light.BlinkIntervalSeconds=1.0f;
		}
		else {
			if(HasBlockData(Light,"Job")&&GetBlockData(Light,"Job").Equals("PlayerAlert")){
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
				SetBlockData(Light,"Job","None");
			}
		}
	}
	List<IMySoundBlock> AllSounds=GenericMethods<IMySoundBlock>.GetAllConstruct("");
	foreach(IMySoundBlock Sound in AllSounds){
		if(!CanHaveJob(Sound,"PlayerAlert"))
			continue;
		double distance=double.MaxValue;
		foreach(EntityInfo Entity in CharacterList){
			if((Entity.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies)&&Entity.Age.TotalSeconds<=60)
				distance=Math.Min(distance,Entity.GetDistance(Sound.GetPosition()));
		}
		if(distance<=Alert_Distance){
			if(!HasBlockData(Sound,"DefaultSound"))
				SetBlockData(Sound,"DefaultSound",Sound.SelectedSound);
			if(!HasBlockData(Sound,"DefaultVolume"))
				SetBlockData(Sound,"DefaultVolume",Sound.Volume.ToString());
			SetBlockData(Sound,"Job","PlayerAlert");
			if(!HasBlockData(Sound,"Playing")||!GetBlockData(Sound,"Playing").Equals("True")){
				Sound.SelectedSound="SoundBlockEnemyDetected";
				Sound.Volume=100.0f;
				Sound.Play();
				SetBlockData(Sound,"Playing","True");
			}
		}
		else{
			if(HasBlockData(Sound,"Job")&&GetBlockData(Sound,"Job").Equals("PlayerAlert")){
				if(HasBlockData(Sound,"DefaultSound"))
					Sound.SelectedSound=GetBlockData(Sound,"DefaultSound");
				if(HasBlockData(Sound,"DefaultVolume")){
					try{
						Sound.Volume=float.Parse(GetBlockData(Sound,"DefaultVolume"));
					}
					catch(Exception){
						;
					}
				}
				Sound.Stop();
				SetBlockData(Sound,"Playing","False");
				SetBlockData(Sound,"Job","None");
			}
		}
	}
	List<IMyDoor> AllDoors=GenericMethods<IMyDoor>.GetAllConstruct("");
	foreach(IMyDoor Door in AllDoors){
		if(!CanHaveJob(Door,"PlayerAlert"))
			continue;
		double distance=double.MaxValue;
		double friendly=double.MaxValue;
		foreach(EntityInfo Entity in CharacterList){
			if(Entity.Age.TotalSeconds<=60){
				if(Entity.Relationship==MyRelationsBetweenPlayerAndBlock.Enemies)
					distance=Math.Min(distance,Entity.GetDistance(Door.GetPosition()));
				else if(Entity.Relationship!=MyRelationsBetweenPlayerAndBlock.Neutral)
					friendly=Math.Min(friendly,Entity.GetDistance(Door.GetPosition()));
			}
		}
		if(distance<=Alert_Distance&&distance<friendly){
			if(!HasBlockData(Door,"DefaultState")){
				if(Door.Status.ToString().Contains("Open"))
					SetBlockData(Door,"DefaultState","Open");
				else
					SetBlockData(Door,"DefaultState","Closed");
			}
			if(!HasBlockData(Door,"DefaultPower")){
				if(Door.Enabled)
					SetBlockData(Door,"DefaultPower","On");
				else
					SetBlockData(Door,"DefaultPower","Off");
			}
			SetBlockData(Door,"Job","PlayerAlert");
			Door.Enabled=(Door.Status!=DoorStatus.Closed);
			Door.CloseDoor();
		}
		else{
			if(HasBlockData(Door,"Job")&&GetBlockData(Door,"Job").Equals("PlayerAlert")){
				if(HasBlockData(Door,"DefaultPower"))
					Door.Enabled=GetBlockData(Door,"DefaultPower").Equals("On");
				if(HasBlockData(Door,"DefaultState")){
					if(GetBlockData(Door,"DefaultState").Equals("Open"))
						Door.OpenDoor();
					else
						Door.CloseDoor();
				}
				SetBlockData(Door,"Job","None");
			}
		}
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
	Me.GetSurface(1).WriteText("\n"+ToString(Time_Since_Start)+" since last reboot\n");
}

void UpdateTimers(){
	foreach(Airlock airlock in Airlocks){
		if(airlock.AirlockTimer<10&&airlock.AirlockTimer<Math.Max(3,airlock.Door1.GetPosition()-airlock.Door2.GetPosition()))
			airlock.AirlockTimer+=seconds_since_last_update;
	}
	
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
