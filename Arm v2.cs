const string Program_Name = "Arm v2"; //Name me!
private Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
private Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);

class GenericMethods<T> where T : class, IMyTerminalBlock{
	private IMyGridTerminalSystem TerminalSystem;
	private IMyTerminalBlock Prog;
	private MyGridProgram Program;
	
	public GenericMethods(MyGridProgram Program){
		this.Program = Program;
		TerminalSystem = Program.GridTerminalSystem;
		Prog = Program.Me;
	}
	
	public T GetFull(string name, double max_distance, Vector3D Reference){
		List<T> AllBlocks = new List<T>();
		List<T> MyBlocks = new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		double min_distance = max_distance;
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Equals(name)){
				double distance = (Reference - Block.GetPosition()).Length();
				min_distance = Math.Min(min_distance, distance);
				MyBlocks.Add(Block);
			}
		}
		foreach(T Block in MyBlocks){
			double distance = (Reference - Block.GetPosition()).Length();
			if(distance <= min_distance + 0.1){
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
	
	public T GetContaining(string name, Vector3D Reference, double max_distance){
		List<T> AllBlocks = new List<T>();
		List<T> MyBlocks = new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		double min_distance = max_distance;
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Contains(name)){
				double distance = (Reference - Block.GetPosition()).Length();
				min_distance = Math.Min(min_distance, distance);
				MyBlocks.Add(Block);
			}
		}
		foreach(T Block in MyBlocks){
			double distance = (Reference - Block.GetPosition()).Length();
			if(distance <= min_distance + 0.1){
				return Block;
			}
		}
		return null;
	}
	
	public T GetContaining(string name, IMyTerminalBlock Reference, double max_distance){
		return GetContaining(name, Reference.GetPosition(), max_distance);
	}
	
	public T GetContaining(string name, double max_distance){
		return GetContaining(name, Prog, max_distance);
	}
	
	public T GetContaining(string name){
		return GetContaining(name, double.MaxValue);
	}
	
	public List<T> GetAllContaining(string name, double max_distance, Vector3D Reference){
		List<T> AllBlocks = new List<T>();
		List<List<T>> MyLists = new List<List<T>>();
		List<T> MyBlocks = new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			if(Block.CustomName.Contains(name)){
				bool has_with_name = false;
				for(int i=0; i<MyLists.Count && !has_with_name; i++){
					if(Block.CustomName.Equals(MyLists[i][0].CustomName)){
						MyLists[i].Add(Block);
						has_with_name = true;
						break;
					}
				}
				if(!has_with_name){
					List<T> new_list = new List<T>();
					new_list.Add(Block);
					MyLists.Add(new_list);
				}
			}
		}
		foreach(List<T> list in MyLists){
			if(list.Count == 1){
				MyBlocks.Add(list[0]);
				continue;
			}
			double min_distance = max_distance;
			foreach(T Block in list){
				double distance = (Reference - Block.GetPosition()).Length();
				min_distance = Math.Min(min_distance, distance);
			}
			foreach(T Block in list){
				double distance = (Reference - Block.GetPosition()).Length();
				if(distance <= min_distance + 0.1){
					MyBlocks.Add(Block);
					break;
				}
			}
		}
		return MyBlocks;
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
		List<T> AllBlocks = new List<T>();
		List<T> MyBlocks = new List<T>();
		TerminalSystem.GetBlocksOfType<T>(AllBlocks);
		foreach(T Block in AllBlocks){
			if(f(Block)){
				MyBlocks.Add(Block);
			}
		}
		return MyBlocks;
	}
	
	public T GetClosestFunc(Func<T, bool> f, double max_distance, Vector3D Reference){
		List<T> MyBlocks = GetAllFunc(f);
		double min_distance = max_distance;
		foreach(T Block in MyBlocks){
			double distance = (Reference - Block.GetPosition()).Length();
			min_distance = Math.Min(min_distance, distance);
		}
		foreach(T Block in MyBlocks){
			double distance = (Reference - Block.GetPosition()).Length();
			if(distance <= min_distance + 0.1){
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
	
	public T GetGrid(string name, IMyCubeGrid Grid, double max_distance, IMyTerminalBlock Reference){
		List<T> input=GetAllGrid(name,Grid,max_distance,Reference);
		if(input.Count>0)
			return input[0];
		return null;
	}
	
	public T GetGrid(string name,IMyCubeGrid Grid,double max_distance=double.MaxValue){
		return GetGrid(name,Grid,max_distance,Prog);
	}
	
	public List<T> GetAllGrid(string name, IMyCubeGrid Grid, double max_distance,IMyTerminalBlock Reference){
		List<T> output=new List<T>();
		List<T> input=GetAllContaining(name,max_distance,Reference);
		foreach(T Block in input){
			if(Block.CubeGrid==Grid)
				output.Add(Block);
		}
		return output;
	}
	
	public List<T> GetAllGrid(string name, IMyCubeGrid Grid, double max_distance=double.MaxValue){
		return GetAllGrid(name,Grid,max_distance,Prog);
	}
	
	public static List<T> SortByDistance(List<T> unsorted, Vector3D Reference){
		List<T> output = new List<T>();
		while(unsorted.Count > 0){
			double min_distance = double.MaxValue;
			foreach(T Block in unsorted){
				double distance = (Reference - Block.GetPosition()).Length();
				min_distance = Math.Min(min_distance, distance);
			}
			for(int i=0; i<unsorted.Count; i++){
				double distance = (Reference - unsorted[i].GetPosition()).Length();
				if(distance <= min_distance + 0.1){
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
		return Math.Round(Math.Acos(v1.X*v2.X + v1.Y*v2.Y + v1.Z*v2.Z) * 57.295755, 5);
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
	
	public Vector3D GetPosition(){
		if(HitPosition!=null){
			return ((Vector3D)HitPosition);
		}
		return Position;
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
		Position+=seconds*Velocity;
		if(HitPosition!=null){
			HitPosition=(Vector3D?) (((Vector3D)HitPosition)+seconds*Velocity);
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
	
	public double ClosestDistance(MyGridProgram P, MyRelationsBetweenPlayerAndBlock Relationship, double min_size=0){
		double min_distance=double.MaxValue;
		foreach(EntityInfo entity in E_List){
			if(entity.Size >= min_size && entity.Relationship==Relationship){
				min_distance=Math.Min(min_distance, (P.Me.GetPosition()-entity.Position).Length()-entity.Size);
			}
		}
		return min_distance;
	}
	
	public double ClosestDistance(MyGridProgram P, double min_size=0){
		double min_distance=double.MaxValue;
		foreach(EntityInfo entity in E_List){
			if(entity.Size >= min_size){
				min_distance=Math.Min(min_distance, (P.Me.GetPosition()-entity.Position).Length()-entity.Size);
			}
		}
		return min_distance;
	}
	
	public void Clear(){
		E_List.Clear();
	}
	
	public void Sort(Vector3D Reference){
		List<EntityInfo> Sorted=new List<EntityInfo>();
		List<EntityInfo> Unsorted=new List<EntityInfo>();
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

struct Angle{
	private float _Degrees;
	public float Degrees{
		get{
			return _Degrees;
		}
		set{
			_Degrees=value%360;
			while(_Degrees<0)
				_Degrees+=360;
		}
	}
	
	public Angle(float degrees){
		_Degrees=degrees;
		Degrees=degrees;
	}
	
	public static Angle FromRadians(float Rads){
		return new Angle((float)(Rads/Math.PI*180));
	}
	
	public float Difference_From_Top(Angle other){
		if(other.Degrees>=Degrees)
			return other.Degrees-Degrees;
		return other.Degrees-Degrees+360;
	}
	
	public float Difference_From_Bottom(Angle other){
		if(other.Degrees<=Degrees)
			return Degrees-other.Degrees;
		return Degrees-other.Degrees+360;
	}
	
	public float Difference(Angle other){
		return Math.Min(Difference_From_Top(other),Difference_From_Bottom(other));
	}
	
	public static bool IsBetween(Angle Bottom, Angle Middle, Angle Top){
		return Bottom.Difference_From_Top(Middle)<=Bottom.Difference_From_Top(Top);
	}
	
	public static bool TryParse(string parse,out Angle output){
		output=new Angle(0);
		float d;
		if(!float.TryParse(parse.Substring(0,Math.Max(0,parse.Length-1)),out d))
			return false;
		output=new Angle(d);
		return true;
	}
	
	public static Angle operator +(Angle a1, Angle a2){
		return new Angle(a1.Degrees+a2.Degrees);
	}
	
	public static Angle operator +(Angle a1, float a2){
		return new Angle(a1.Degrees+a2);
	}
	
	public static Angle operator +(float a1, Angle a2){
		return a2 + a1;
	}
	
	public static Angle operator -(Angle a1, Angle a2){
		return new Angle(a1.Degrees-a2.Degrees);
	}
	
	public static Angle operator -(Angle a1, float a2){
		return new Angle(a1.Degrees-a2);
	}
	
	public static Angle operator -(float a1, Angle a2){
		return new Angle(a1-a2.Degrees);
	}
	
	public static Angle operator *(Angle a1, float m){
		return new Angle(a1.Degrees*m);
	}
	
	public static Angle operator *(float m, Angle a2){
		return a2*m;
	}
	
	public static Angle operator /(Angle a1, float m){
		return new Angle(a1.Degrees/m);
	}
	
	public static bool operator ==(Angle a1, Angle a2){
		float degrees=(a1-a2).Degrees;
		if(degrees>180)
			degrees-=360;
		return Math.Abs(degrees)<0.000001f;
	}
	
	public static bool operator !=(Angle a1, Angle a2){
		return Math.Abs(a1.Degrees-a2.Degrees)>=0.000001f;
	}
	
	public override bool Equals(object o){
		return (o.GetType()==this.GetType()) && this==((Angle)o);
	}
	
	public override int GetHashCode(){
		return Degrees.GetHashCode();
	}
	
	public static bool operator >(Angle a1, Angle a2){
		return a1.Difference_From_Top(a2)>a1.Difference_From_Bottom(a2);
	}
	
	public static bool operator >=(Angle a1, Angle a2){
		return (a1==a2)||(a1>a2);
	}
	
	public static bool operator <=(Angle a1, Angle a2){
		return (a1==a2)||(a1<a2);
	}
	
	public static bool operator <(Angle a1, Angle a2){
		return a1.Difference_From_Top(a2)<a1.Difference_From_Bottom(a2);
	}
	
	public override string ToString(){
		if(Degrees>=180)
			return (Degrees-360).ToString()+'°';
		else
			return Degrees.ToString()+'°';
	}
	
	public string ToString(int n){
		n=Math.Min(0,n);
		if(Degrees>=180)
			return Math.Round(Degrees-360,n).ToString()+'°';
		else
			return Math.Round(Degrees,n).ToString()+'°';
	}
	
	public static string LinedFloat(float deg, int sections=10){
		string output="[";
		for(int i=-180/sections;i<0;i++){
			if(deg<0){
				if(deg/sections<=i+1)
					output+='|';
				else
					output+=' ';
			}
			else
				output+=' ';
		}
		for(int i=0;i<180/sections;i++){
			if(deg<0)
				output+=' ';
			else{
				if(deg/sections>=(i+1))
					output+='|';
				else
					output+=' ';
			}
		}
		output+=']';
		return output;
	}
	
	public string LinedString(int sections=10){
		if(Degrees>=180)
			return LinedFloat(Degrees-360,sections);
		return LinedFloat(Degrees,sections);
	}
	
	public string LinedString(Angle Comparison){
		string output="Actual: "+LinedString();
		output+="\nComparison: ";
		float deg=Difference(Comparison);
		if(deg>=180)
			output+=LinedFloat(deg-360);
		else
			output+=LinedFloat(deg);
		return output;
	}
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

Vector3D GlobalToLocal(Vector3D Global, IMyCubeBlock Reference){
	Vector3D Local=Vector3D.Transform(Global+Reference.GetPosition(), MatrixD.Invert(Reference.WorldMatrix));
	Local.Normalize();
	return Local*Global.Length();
}

Vector3D GlobalToLocalPosition(Vector3D Global, IMyCubeBlock Reference){
	Vector3D Local=Vector3D.Transform(Global, MatrixD.Invert(Reference.WorldMatrix));
	Local.Normalize();
	return Local*(Global-Reference.GetPosition()).Length();
}

Vector3D LocalToGlobal(Vector3D Local, IMyCubeBlock Reference){
	Vector3D Global=Vector3D.Transform(Local, Reference.WorldMatrix)-Reference.GetPosition();
	Global.Normalize();
	return Global*Local.Length();
}

Vector3D LocalToGlobalPosition(Vector3D Local, IMyCubeBlock Reference){
	return Vector3D.Transform(Local,Reference.WorldMatrix);
}

double GetAngle(Vector3D v1, Vector3D v2){
	return GenericMethods<IMyTerminalBlock>.GetAngle(v1,v2);
}

void Write(string text, bool new_line=true, bool append=true){
	Echo(text);
	if(new_line)
		Me.GetSurface(0).WriteText(text+'\n', append);
	else
		Me.GetSurface(0).WriteText(text, append);
	if(ProgramPanel!=null){
		if(new_line)
			ProgramPanel.WriteText(text+'\n', append);
		else
			ProgramPanel.WriteText(text, append);
	}
}

bool IsHinge(IMyMotorStator Motor){
	return Motor.BlockDefinition.SubtypeName.Contains("Hinge");
}

bool IsRotor(IMyMotorStator Motor){
	return (!IsHinge(Motor))&&Motor.BlockDefinition.SubtypeName.Contains("Stator");
}

class Hand : List<Arm>{
	public Hand():base(){
		;
	}
}

class Arm{
	public static MyGridProgram P;
	public static Func<IMyTerminalBlock,string,bool> HasBlockData;
	public static Func<IMyTerminalBlock,string,string> GetBlockData;
	public static Func<IMyTerminalBlock,string,string,bool> SetBlockData;
	public static Func<Vector3D,IMyCubeBlock,Vector3D> GlobalToLocalPosition;
	public static Func<Vector3D,IMyCubeBlock,Vector3D> LocalToGlobal;
	public static Func<IMyMotorStator,bool> IsHinge;
	public static Func<IMyMotorStator,bool> IsRotor;
	public static Func<Vector3D,Vector3D,double> GetAngle;
	
	public List<IMyMotorStator> Motors;
	public Hand MyHand;
	public string Name;
	
	public List<IMyMotorStator> Hinges{
		get{
			List<IMyMotorStator> output=new List<IMyMotorStator>();
			foreach(IMyMotorStator Motor in Motors){
				if(IsHinge(Motor))
					output.Add(Motor);
			}
			return output;
		}
	}
	
	public List<IMyMotorStator> Rotors{
		get{
			List<IMyMotorStator> output=new List<IMyMotorStator>();
			foreach(IMyMotorStator Motor in Motors){
				if(IsRotor(Motor))
					output.Add(Motor);
			}
			return output;
		}
	}
	
	public IMyMotorStator FirstHinge{
		get{
			if(Hinges.Count>0)
				return Hinges[0];
			return null;
		}
	}
	
	public IMyMotorStator LastRotor{
		get{
			if(Rotors.Count>0)
				return Rotors[Rotors.Count-1];
			return null;
		}
	}
	
	public List<IMyMotorStator> AllMotors{
		get{
			List<IMyMotorStator> output=new List<IMyMotorStator>();
			foreach(IMyMotorStator Motor in Motors)
				output.Add(Motor);
			foreach(Arm Finger in MyHand){
				foreach(IMyMotorStator Motor in Finger.AllMotors)
					output.Add(Motor);
			}
			return output;
		}
	}
	
	public double MaxLength{
		get{
			double output=0;
			for(int i=1;i<Motors.Count;i++){
				if(i>1)
					output+=(Motors[i-1].Top.GetPosition()-Motors[i].GetPosition()).Length();
				output+=(Motors[i].GetPosition()-Motors[i].Top.GetPosition()).Length();
			}
			if(MyHand!=null&&MyHand.Count>0){
				double sum=0;
				foreach(Arm Finger in MyHand)
					sum+=(Finger.Motors[0].GetPosition()-Motors[Motors.Count-1].Top.GetPosition()).Length();
				sum/=MyHand.Count;
				output+=sum;
			}
			return output;
		}
	}
	
	public double MotorLength(int MotorNum){
		IMyMotorStator Motor=Motors[MotorNum];
		if(MotorNum<Motors.Count-1)
			return (Motor.Top.GetPosition()-Motors[MotorNum+1].GetPosition()).Length();
		else if(MotorNum==Motors.Count-1){
			double sum=0;
			foreach(Arm Finger in MyHand)
				sum+=(Finger.Motors[0].GetPosition()-Motors[MotorNum].Top.GetPosition()).Length();
			return sum/MyHand.Count;
		}
		return 0;
	}
	
	public double MotorLength(IMyMotorStator Motor){
		for(int i=0;i<Motors.Count;i++){
			if(Motor==Motors[i])
				return MotorLength(i);
		}
		return 0;
	}
	
	public double SumLength(int MotorNum){
		double sum=0;
		for(int i=MotorNum;i<Motors.Count;i++){
			sum+=MotorLength(i);
			sum+=(Motors[i].GetPosition()-Motors[i].Top.GetPosition()).Length();
		}
		return sum;
	}
	
	public Arm(IMyMotorStator BaseMotor, string name="Arm"){
		Motors=new List<IMyMotorStator>();
		Name=name;
		Motors.Add(BaseMotor);
		List<IMyMotorStator> allmotors=(new GenericMethods<IMyMotorStator>(P)).GetAllIncluding("",50);
		List<IMyMotorStator> gridmotors;
		do{
			gridmotors=(new GenericMethods<IMyMotorStator>(P)).GetAllGrid("",Motors[Motors.Count-1].TopGrid,50);
			if(gridmotors.Count==1)
				Motors.Add(gridmotors[0]);
		} while(gridmotors.Count==1);
		MyHand=new Hand();
		if(gridmotors.Count>0){
			for(int i=0;i<gridmotors.Count;i++)
				MyHand.Add(new Arm(gridmotors[i],Name+" Finger "+(i+1).ToString()));
		}
		for(int i=0;i<Motors.Count;i++){
			if(IsHinge(Motors[i]))
				Motors[i].CustomName=Name+" Stator "+(i+1).ToString()+" (Hinge)";
			else if(IsRotor(Motors[i]))
				Motors[i].CustomName=Name+" Stator "+(i+1).ToString()+" (Rotor)";
			else
				Motors[i].CustomName=Name+" Stator "+(i+1).ToString();
		}
	}
	
	public override string ToString(){
		return Name;
	}
}

public enum MenuType{
	Submenu=0,
	Command=1,
	Display=2,
	Text=3
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
	public int Selection;
	
	private int Last_Count;
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
		else {
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

public class Menu_Text : MenuOption{
	private string _Name;
	public string Name(){
		return _Name;
	}
	public MenuType Type(){
		return MenuType.Text;
	}
	public bool AutoRefresh(){
		return false;
	}
	public int Depth(){
		return 1;
	}
	public bool Back(){
		return false;
	}
	public bool Select(){
		return false;
	}
	
	public Menu_Text(string name){
		_Name=name;
	}
	
	public override string ToString(){
		return Name();
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
	public T Arg;
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
		int length=24;
		foreach(string word in words){
			if(length > 0 && length+word.Length > 24){
				length=0;
				output+='\n';
			}
			else {
				output+=' ';
			}
			output+=word;
			if(word.Contains('\n'))
				length=word.Length-word.IndexOf('\n')-1;
			else
				length+=word.Length;
		}
		return output+"\n\nSelect to Execute";
	}
}

public class Menu_Display : MenuOption{
	public string Name(){
		if(Entity==null){
			return "null";
		}
		string name=Entity.Name.Substring(0, Math.Min(24, Entity.Name.Length));
		string[] args=name.Split(' ');
		int number=0;
		if(args.Length==3 && args[1].ToLower().Equals("grid") && Int32.TryParse(args[2], out number)){
			name="Unnamed "+args[0]+' '+args[1];
		}
		double distance=Entity.GetDistance(P.Me.GetPosition())-Entity.Size;
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
	private bool Selected;
	public EntityInfo Entity;
	private bool Can_GoTo;
	private Func<EntityInfo, bool> Command;
	private Menu_Command<EntityInfo> Subcommand {
		get{
			double distance=(P.Me.GetPosition()-Entity.Position).Length()-Entity.Size;
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
	
	public Menu_Display(EntityInfo entity, MyGridProgram p){
		Entity=entity;
		P=p;
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
			double distance=Entity.GetDistance(P.Me.GetPosition());
			string distance_string=Math.Round(distance,0)+"M";
			if(distance>=1000)
				distance_string=Math.Round(distance/1000,1)+"kM";
			return Entity.NiceString()+"Distance: "+distance_string;
		}
	}
}

long cycle_long = 1;
long cycle = 0;
char loading_char = '|';
double seconds_since_last_update = 0;

Menu_Submenu CommandMenu=null;
Menu_Submenu EntityMenu=null;

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

Arm MyArm;
IMyShipController Controller;
EntityList MyEntities;
IMyTextPanel CommandPanel;
IMyTextPanel EntityPanel;
IMyTextPanel ProgramPanel;

bool FreeLCDFunction(IMyTextPanel Panel){
	return Panel.ContentType==ContentType.NONE&&(Panel.GetPublicTitle().Equals("Public title")||Panel.GetPublicTitle().Length==0);
}

public Program()
{
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
	Arm.P=this;
	Arm.HasBlockData=HasBlockData;
	Arm.GetBlockData=GetBlockData;
	Arm.GlobalToLocalPosition=GlobalToLocalPosition;
	Arm.SetBlockData=SetBlockData;
	Arm.IsHinge=IsHinge;
	Arm.IsRotor=IsRotor;
	Arm.GetAngle=GetAngle;
	Arm.LocalToGlobal=LocalToGlobal;
	List<IMyMotorStator> Motors=(new GenericMethods<IMyMotorStator>(this)).GetAllGrid("Arm",Me.CubeGrid,50);
	if(Motors.Count==0)
		Motors=(new GenericMethods<IMyMotorStator>(this)).GetAllGrid("",Me.CubeGrid,50);
	if(Motors.Count==0){
		Write("Failed to initialize Arm");
		return;
	}
	MyArm=new Arm(Motors[0]);
	Controller=(new GenericMethods<IMyShipController>(this)).GetContaining("",50);
	if(Controller==null){
		Write("Failed to initialize Controller");
		return;
	}
	MyEntities=new EntityList();
	string[] args=this.Storage.Split('\n');
	if(args.Length>0){
		int temp=0;
		if(Int32.TryParse(args[0],out temp))
			Current_Command=(ArmCommand)temp;
		if(args.Length>1){
			if(Int32.TryParse(args[1],out temp))
				Next_Command=(ArmCommand)temp;
			if(args.Length>2){
				Vector3D t2=new Vector3D(0,0,0);
				if(Vector3D.TryParse(args[2],out t2))
					Target_Position=t2;
				if(args.Length>3)
					if(Vector3D.TryParse(args[3],out t2))
						Target_2=t2;
			}
		}
	}
	
	List<IMyInteriorLight> Lights=new List<IMyInteriorLight>();
	GridTerminalSystem.GetBlocksOfType<IMyInteriorLight>(Lights);
	foreach(IMyInteriorLight Light in Lights){
		if(HasBlockData(Light,"ExceptionState")&&GetBlockData(Light,"ExceptionState").Equals("active")){
			SetBlockData(Light,"ExceptionState","inactive");
			Light.Enabled=true;
		}
	}
	ProgramPanel=(new GenericMethods<IMyTextPanel>(this)).GetContaining("Arm Program Display");
	if(ProgramPanel==null){
		ProgramPanel=(new GenericMethods<IMyTextPanel>(this)).GetClosestFunc(FreeLCDFunction,10,Controller);
		if(ProgramPanel!=null){
			string name="Arm Program Display";
			if(ProgramPanel.CustomName.ToLower().Contains("transparent"))
				name="Transparent "+name;
			ProgramPanel.CustomName=name;
		}
	}
	if(ProgramPanel!=null){
		ProgramPanel.Alignment=TextAlignment.CENTER;
		ProgramPanel.ContentType=ContentType.TEXT_AND_IMAGE;
		ProgramPanel.FontSize=1.2f;
		ProgramPanel.TextPadding=10.0f;
		ProgramPanel.WritePublicTitle("Program Display",false);
		if(ProgramPanel.CustomName.ToLower().Contains("transparent")){
			ProgramPanel.FontColor=DEFAULT_BACKGROUND_COLOR;
			ProgramPanel.BackgroundColor=new Color(10,10,10,10);
		}
		else{
			ProgramPanel.FontColor=DEFAULT_TEXT_COLOR;
			ProgramPanel.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
	}
	CommandPanel=(new GenericMethods<IMyTextPanel>(this)).GetContaining("Arm Command Display");
	if(CommandPanel==null){
		CommandPanel=(new GenericMethods<IMyTextPanel>(this)).GetClosestFunc(FreeLCDFunction,10,Controller);
		if(CommandPanel!=null){
			string name="Arm Command Display";
			if(CommandPanel.CustomName.ToLower().Contains("transparent"))
				name="Transparent "+name;
			CommandPanel.CustomName=name;
		}
	}
	if(CommandPanel!=null){
		CommandPanel.Alignment=TextAlignment.CENTER;
		CommandPanel.ContentType=ContentType.TEXT_AND_IMAGE;
		CommandPanel.FontSize=1.2f;
		CommandPanel.TextPadding=10.0f;
		CommandPanel.WritePublicTitle("Command Display",false);
		CommandPanel.FontColor=DEFAULT_TEXT_COLOR;
		CommandPanel.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
	}
	EntityPanel=(new GenericMethods<IMyTextPanel>(this)).GetContaining("Arm Entity Display");
	if(EntityPanel==null){
		EntityPanel=(new GenericMethods<IMyTextPanel>(this)).GetClosestFunc(FreeLCDFunction,10,Controller);
		if(EntityPanel!=null){
			string name="Arm Entity Display";
			if(EntityPanel.CustomName.ToLower().Contains("transparent"))
				name="Transparent "+name;
			EntityPanel.CustomName=name;
		}
	}
	if(EntityPanel!=null){
		EntityPanel.Alignment=TextAlignment.CENTER;
		EntityPanel.ContentType=ContentType.TEXT_AND_IMAGE;
		EntityPanel.FontSize=1.2f;
		EntityPanel.TextPadding=10.0f;
		EntityPanel.WritePublicTitle("Entity Display",false);
		EntityPanel.FontColor=DEFAULT_TEXT_COLOR;
		EntityPanel.BackgroundColor=DEFAULT_BACKGROUND_COLOR;
	}
	Create_CommandMenu();
	DisplayMenus();
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

public void Save()
{
    this.Storage=((int)Current_Command).ToString()+'\n'+((int)Next_Command).ToString()+'\n'+Target_Position.ToString()+'\n'+Target_2.ToString();
}

EntityInfo Selected{
	get{
		try{
			return (EntityMenu.Menu[EntityMenu.Selection] as Menu_Display).Entity;
		}
		catch(Exception){
			return new EntityInfo(-1,"Invalid",MyDetectedEntityType.None,null,new Vector3D(0,0,0),MyRelationsBetweenPlayerAndBlock.NoOwnership,new Vector3D(0,0,0));
		}
	}
}
int GetTargetCount(ArmCommand Command){
	switch(Command){
		case ArmCommand.Idle:
			return 0;
		case ArmCommand.Punch:
			return 1;
		case ArmCommand.Brace:
			return 1;
		case ArmCommand.Grab:
			return 2;
		case ArmCommand.Throw:
			return 2;
		case ArmCommand.Block:
			return 0;
		case ArmCommand.Wave:
			return 0;
		case ArmCommand.Spin:
			return 1;
	}
	return -1;
}
ArmCommand Highlighted{
	get{
		try{
			ArmCommand output;
			if(Enum.TryParse((CommandMenu.Menu[CommandMenu.Selection] as Menu_Command<string>).Arg,out output))
				return output;
			return ArmCommand.Idle;
		}
		catch(Exception){
			return ArmCommand.Idle;
		}
	}
}
int Target_Count{
	get{
		return GetTargetCount(Highlighted);
	}
}
bool Characters{
	get{
		return Target_Count>0;
	}
}
bool Small_Ships{
	get{
		return Target_Count>0;
	}
}
bool Large_Ships{
	get{
		return Target_Count>0&&Highlighted!=ArmCommand.Grab&&Highlighted!=ArmCommand.Throw;
	}
}
bool Objects{
	get{
		return Target_Count>0&&Highlighted!=ArmCommand.Brace&&Highlighted!=ArmCommand.Punch;
	}
}
bool Voxels{
	get{
		return Highlighted==ArmCommand.Brace;
	}
}
void Create_EntityMenu(){
	string name="";
	if(EntityMenu!=null&&EntityMenu.Count>0)
		name=EntityMenu.Menu[EntityMenu.Selection].Name();
	EntityMenu=new Menu_Submenu("Targets");
	if(Target_Count>0){
		EntityMenu.Add(new Menu_Display(new EntityInfo(-2,"Forward",MyDetectedEntityType.None,null,Controller.GetShipVelocities().LinearVelocity,MyRelationsBetweenPlayerAndBlock.NoOwnership,MyArm.Motors[0].GetPosition()+20*Forward_Vector), this));
		EntityMenu.Add(new Menu_Display(new EntityInfo(-3,"Up",MyDetectedEntityType.None,null,Controller.GetShipVelocities().LinearVelocity,MyRelationsBetweenPlayerAndBlock.NoOwnership,MyArm.Motors[0].GetPosition()+20*Up_Vector), this));
		EntityMenu.Add(new Menu_Display(new EntityInfo(-4,"Right",MyDetectedEntityType.None,null,Controller.GetShipVelocities().LinearVelocity,MyRelationsBetweenPlayerAndBlock.NoOwnership,MyArm.Motors[0].GetPosition()+20*Right_Vector), this));
		if(MyEntities.Count>0){
			EntityList ValidEntities=new EntityList();
			foreach(EntityInfo Entity in MyEntities){
				if(Small_Ships&&Entity.Type==MyDetectedEntityType.SmallGrid)
					ValidEntities.UpdateEntry(Entity);
				else if(Large_Ships&&Entity.Type==MyDetectedEntityType.LargeGrid)
					ValidEntities.UpdateEntry(Entity);
				else if(Characters&&(Entity.Type==MyDetectedEntityType.CharacterHuman||Entity.Type==MyDetectedEntityType.CharacterOther))
					ValidEntities.UpdateEntry(Entity);
				else if(Objects&&Entity.Type==MyDetectedEntityType.FloatingObject)
					ValidEntities.UpdateEntry(Entity);
				else if(Voxels&&(Entity.Type==MyDetectedEntityType.Asteroid||Entity.Type==MyDetectedEntityType.Planet))
					ValidEntities.UpdateEntry(Entity);
			}
			ValidEntities.Sort(Controller.GetPosition());
			foreach(EntityInfo Entity in ValidEntities)
				EntityMenu.Add(new Menu_Display(Entity,this));
			for(int i=0;i<EntityMenu.Menu.Count;i++){
				if(EntityMenu.Menu[i].Name().Equals(name)){
					EntityMenu.Selection=i;
					break;
				}
			}
		}
	}
	else{
		EntityMenu.Add(new Menu_Text("No Target Needed"));
	}
}

int Defined_Target_Count=0;
//This function should do whatever should happen when a command is selected
bool Command_Menu_Function(string Command){
	if(Target_Count>0&&Selected.ID!=-2){
		if(Defined_Target_Count==0){
			Target_ID=Selected.ID;
			Target_Position=Selected.GetPosition();
			Defined_Target_Count++;
		}
		else if(Defined_Target_Count==1){
			Target_2=Selected.GetPosition();
			Defined_Target_Count++;
		}
	}
	EntityMenu.Selection=0;
	if(Defined_Target_Count>=Target_Count){
		Next_Command=Highlighted;
		if(Next_Command==ArmCommand.Idle)
			Force_End=true;
		return true;
	}
	else
		return false;
}

void Next(){
	if(CommandMenu.IsSelected)
		EntityMenu.Next();
	else{
		CommandMenu.Next();
		Create_EntityMenu();
	}
}

void Prev(){
	if(CommandMenu.IsSelected)
		EntityMenu.Prev();
	else{
		CommandMenu.Prev();
		Create_EntityMenu();
	}
}

void Back(){
	if(Defined_Target_Count==0)
		CommandMenu.Back();
	else{
		Defined_Target_Count--;
		EntityMenu.Selection=0;
	}
}

void Select(){
	CommandMenu.Select();
}

void Create_CommandMenu(){
	CommandMenu=new Menu_Submenu("Arm Commands");
	CommandMenu.Add(new Menu_Command<string>("Idle",Command_Menu_Function,"Ends current Command\nRequires 0 Targets",ArmCommand.Idle.ToString()));
	CommandMenu.Add(new Menu_Command<string>("Punch",Command_Menu_Function,"Punches the target\nRequires 1 Target",ArmCommand.Punch.ToString()));
	CommandMenu.Add(new Menu_Command<string>("Brace",Command_Menu_Function,"Locks arm against target\nRequires 1 Target",ArmCommand.Brace.ToString()));
	CommandMenu.Add(new Menu_Command<string>("Grab",Command_Menu_Function,"Grabs the target and brings it to another target\nRequires 2 Targets",ArmCommand.Grab.ToString()));
	CommandMenu.Add(new Menu_Command<string>("Throw",Command_Menu_Function,"Grabs target and throws at another target\nRequires 2 Targets",ArmCommand.Throw.ToString()));
	CommandMenu.Add(new Menu_Command<string>("Block",Command_Menu_Function,"Shields Cockpit with Arm\nRequires 0 Targets",ArmCommand.Block.ToString()));
	CommandMenu.Add(new Menu_Command<string>("Wave",Command_Menu_Function,"Waves hand\nRequires 0 Targets",ArmCommand.Wave.ToString()));
	CommandMenu.Add(new Menu_Command<string>("Spin",Command_Menu_Function,"Aims hand at target and spins the wrist\nRequires 1 Target",ArmCommand.Spin.ToString()));
	Create_EntityMenu();
}

void DisplayMenus(){
	if(EntityPanel!=null)
		EntityPanel.WriteText(EntityMenu.ToString());
	if(CommandPanel!=null)
		CommandPanel.WriteText(CommandMenu.ToString());
}

//Gets the difference between the motor's current Angle and the closest it can get to its Target angle
float Adjusted_Difference(IMyMotorStator Motor,Angle Target){
	bool can_increase=true;
	bool can_decrease=true;
	Angle Motor_Angle=Angle.FromRadians(Motor.Angle);
	if(Motor.UpperLimitDeg!=float.MaxValue)
		can_increase=Angle.IsBetween(Motor_Angle,Target,new Angle(Motor.UpperLimitDeg));
	if(Motor.LowerLimitDeg!=float.MinValue)
		can_decrease=Angle.IsBetween(new Angle(Motor.LowerLimitDeg),Target,Motor_Angle);
	if(Motor_Angle.Difference_From_Top(Target)<Motor_Angle.Difference_From_Bottom(Target)){
		if(!can_increase)
			Target=new Angle(Motor.UpperLimitDeg);
	}
	else{
		if(!can_decrease)
			Target=new Angle(Motor.LowerLimitDeg);
	}
	return Motor_Angle.Difference(Target);
}

Angle GetTarget(IMyMotorStator Motor,Vector3D Direction){
	float Difference=0;
	Angle Current=Angle.FromRadians(Motor.Angle);
	if(IsRotor(Motor)){
		Vector3D Rotor_Left=LocalToGlobal(new Vector3D(-1,0,0),Motor.Top);
		Difference=(float)(GetAngle(-1*Rotor_Left,Direction)-GetAngle(Rotor_Left,Direction));
	}
	else if(IsHinge(Motor)){
		Vector3D Hinge_Forward=LocalToGlobal(new Vector3D(0,0,-1),Motor.Top);
		Difference=(float)(GetAngle(-1*Hinge_Forward,Direction)-GetAngle(Hinge_Forward,Direction));
	}
	else{
		Write("Invalid Stator: "+Motor.CustomName);
		return Current;
	}
	return Current+Difference;
}

bool SetDirection(IMyMotorStator Motor,Vector3D Direction,float Speed_Multx=1){
	Angle Target=GetTarget(Motor,Direction);
	SetAngle(Motor,Target,Speed_Multx);
	return true;
}

bool SetAngle(IMyMotorStator Motor,Angle Target,float Speed_Multx=1){
	Speed_Multx=Math.Max(0.05f, Math.Min(Math.Abs(Speed_Multx),50));
	bool can_increase=true;
	bool can_decrease=true;
	Vector3D Velocity=GetVelocity(Motor);
	double Speed=Velocity.Length();
	float RPM=GetRPM(Motor);
	float overall_speed=(float)Speed+RPM;
	Speed_Multx*=(float)Math.Min(1,(10*Speed_Multx)/overall_speed);
	
	Angle Motor_Angle=Angle.FromRadians(Motor.Angle);
	Speed_Multx*=Math.Min(1,10/Math.Abs(Motor_Angle.Difference(Target)));
	if(Motor.UpperLimitDeg!=float.MaxValue)
		can_increase=Angle.IsBetween(Motor_Angle,Target,new Angle(Motor.UpperLimitDeg));
	if(Motor.LowerLimitDeg!=float.MinValue)
		can_decrease=Angle.IsBetween(new Angle(Motor.LowerLimitDeg),Target,Motor_Angle);
	if(Motor_Angle.Difference_From_Top(Target)<Motor_Angle.Difference_From_Bottom(Target)){
		if(!can_increase)
			Target=new Angle(Motor.UpperLimitDeg);
	}
	else{
		if(!can_decrease)
			Target=new Angle(Motor.LowerLimitDeg);
	}
	float From_Bottom=Motor_Angle.Difference_From_Bottom(Target);
	if(!can_decrease)
		From_Bottom=float.MaxValue;
	float From_Top=Motor_Angle.Difference_From_Top(Target);
	if(!can_increase)
		From_Top=float.MaxValue;
	float difference=Math.Min(From_Bottom,From_Top);
	if(difference>1){
		Motor.RotorLock=false;
		float target_rpm=0;
		float current_rpm=GetRPM(Motor);
		Speed_Multx*=Math.Max(0.1f,(20-Math.Abs(current_rpm))/10);
		if(From_Bottom<From_Top)
			target_rpm=(float)(-1*From_Bottom*Speed_Multx*.5f);
		else
			target_rpm=(float)(From_Top*Speed_Multx*.5f);
		Motor.TargetVelocityRPM=target_rpm;
	}
	else
		Motor.TargetVelocityRPM=0;
	return true;
}

Vector3D GetVelocity(IMyMotorStator Motor){
	Vector3D Last_Velocity=new Vector3D(0,0,0);
	if(HasBlockData(Motor,"LastVelocity"))
		Vector3D.TryParse(GetBlockData(Motor,"LastVelocity"),out Last_Velocity);
	return Last_Velocity;
}

float GetRPM(IMyMotorStator Motor){
	float Last_RPM=0;
	if(HasBlockData(Motor,"LastRPM"))
		float.TryParse(GetBlockData(Motor,"LastRPM"),out Last_RPM);
	return Last_RPM;
}

void UpdateMotor(IMyMotorStator Motor){
	Angle Current_Angle=Angle.FromRadians(Motor.Angle);
	Angle Last_Angle=Current_Angle;
	if(HasBlockData(Motor,"LastAngle"))
		Angle.TryParse(GetBlockData(Motor,"LastAngle"),out Last_Angle);
	SetBlockData(Motor,"LastAngle",Current_Angle.ToString());
	
	float Last_RPM=GetRPM(Motor);
	float Difference=Current_Angle.Difference(Last_Angle);
	float Current_RPM=(float)(Difference/seconds_since_last_update/6);
	
	Vector3D Current_Position=Motor.GetPosition()-Me.GetPosition();
	Vector3D Last_Position=Current_Position;
	if(HasBlockData(Motor,"LastPosition"))
		Vector3D.TryParse(GetBlockData(Motor,"LastPosition"),out Last_Position);
	SetBlockData(Motor,"LastPosition",Current_Position.ToString());
	Vector3D Current_Velocity=(Current_Position-Last_Position)/seconds_since_last_update;
	Vector3D Last_Velocity=GetVelocity(Motor);
	if(Runtime.UpdateFrequency==UpdateFrequency.Update1){
		Last_RPM=((Last_RPM*99)+Current_RPM)/100;
		Last_Velocity=((Last_Velocity*99)+Current_Velocity)/100;
	}
	else{
		Last_RPM=((Last_RPM*9)+Current_RPM)/10;
		Last_Velocity=((Last_Velocity*9)+Current_Velocity)/10;
	}
	SetBlockData(Motor,"LastRPM",Last_RPM.ToString());
	SetBlockData(Motor,"LastVelocity",Last_Velocity.ToString());
}

enum ArmCommand{
	Idle=0,
	Punch=1,
	Brace=2,
	Grab=3,
	Throw=4,
	Block=5,
	Wave=6,
	Spin=7
}

ArmCommand Current_Command=ArmCommand.Idle;
int Command_Stage=0;

ArmCommand Next_Command=ArmCommand.Idle;

void SetLock(Arm arm, bool LockState){
	foreach(IMyMotorStator Motor in arm.Motors){
		Motor.RotorLock=LockState;
	}
}

void SetLock(Hand hand, bool LockState){
	foreach(Arm Finger in hand)
		SetLock(Finger,LockState);
}

bool Ending_Command=false;
double Command_Timer=0;
bool Force_End=false;
long Target_ID=0;
Vector3D Target_Position=new Vector3D(0,0,0);
Vector3D Hand_Position{
	get{
		Vector3D Avg_Position=new Vector3D(0,0,0);
		foreach(Arm Finger in MyArm.MyHand){
			Avg_Position+=Finger.Motors[0].GetPosition();
		}
		Avg_Position/=MyArm.MyHand.Count;
		return Avg_Position;
	}
}
double Hand_Distance{
	get{
		return (Target_Position-Hand_Position).Length();
	}
}
Vector3D Target_2=new Vector3D(0,0,0);
bool EndCommand(){
	if(!Ending_Command){
		SetLock(MyArm,false);
		SetLock(MyArm.MyHand,false);
	}
	switch(Current_Command){
		case ArmCommand.Idle:
			foreach(IMyMotorStator Motor in MyArm.AllMotors){
				if(HasBlockData(Motor,"DefaultTorque")){
					float torque=Motor.Torque;
					float.TryParse(GetBlockData(Motor,"DefaultTorque"),out torque);
					Motor.Torque=torque;
				}
				Motor.Enabled=true;
				Motor.RotorLock=false;
			}
			break;
	}
	if(!Ending_Command)
		Command_Timer=0;
	Ending_Command=true;
	return Command_Timer>=2;
}

void PerformCommand(){
	if(Force_End||(Next_Command!=ArmCommand.Idle&&Next_Command!=Current_Command)){
		if(!EndCommand())
			return;
		Current_Command=Next_Command;
		Next_Command=ArmCommand.Idle;
		Command_Stage=0;
		Ending_Command=false;
		Force_End=false;
	}
	switch(Current_Command){
		case ArmCommand.Idle:
			Idle();
			break;
		case ArmCommand.Punch:
			Punch();
			break;
		case ArmCommand.Brace:
			Brace();
			break;
		case ArmCommand.Grab:
			Grab();
			break;
		case ArmCommand.Throw:
			Throw();
			break;
		case ArmCommand.Block:
			Block();
			break;
		case ArmCommand.Wave:
			Wave();
			break;
		case ArmCommand.Spin:
			Spin();
			break;
	}
	if(GetTargetCount(Current_Command)>0)
		Write("Distance: "+Math.Round(Hand_Distance,1).ToString()+"M");
}

/*
* Idle
* 	0:	Reduce Torque to 100, disable Motors, lock Motors
*	E:	Revert Torque to default, enable Motors, Unlock Motors
*/
void Idle(){
	if(Command_Stage==0){
		foreach(IMyMotorStator Motor in MyArm.AllMotors){
			if(Motor.Torque!=100){
				SetBlockData(Motor,"DefaultTorque",Motor.Torque.ToString());
				Motor.Torque=100;
			}
			Motor.Enabled=false;
			Motor.RotorLock=true;
		}
		Command_Stage++;
	}
}

/*
* Punch
* 	0:	Wind Arm, Clench Hand
* 	1: Lock Hand, Release Arm at high speed for 2 seconds
* 	E: Unlock Hand
*/
void Punch(){
	if(Command_Stage==0){
		bool ready=true;
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_Position-Motor.GetPosition();
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx=5;
				if(Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			if(Motor==MyArm.FirstHinge)
				Target=new Angle(-90);
			SetAngle(Motor,Target,Speed_Multx);
			ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
		}
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Angle Target=new Angle(90);
				if(IsRotor(Motor))
					Target=new Angle(0);
				SetAngle(Motor,Target);
				ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
			}
		}
		if(ready){
			Command_Stage++;
			Command_Timer=0;
		}
	}
	if(Command_Stage==1){
		SetLock(MyArm.MyHand,true);
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_Position-Motor.GetPosition();
			float Speed_Multx=3;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			SetDirection(Motor,Direction,Speed_Multx);
		}
		if(Command_Timer>=2)
			Command_Stage++;
	}
	if(Command_Stage>1)
		Force_End=true;
}

/*
* Brace
* 	0: Wind Arm, Open Hand
* 	1: Lock Hand, Release Arm at low speed until contact
* 	2: Lock Arm
* 	E: Unlock Hand, Unlock Arm
*/
void Brace(){
	if(Command_Stage==0){
		bool ready=true;
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_Position-Motor.GetPosition();
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			if(Motor==MyArm.FirstHinge)
				Target=new Angle(-90);
			SetAngle(Motor,Target,Speed_Multx);
			ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
		}
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Angle Target=new Angle(5);
				if(IsRotor(Motor))
					Target=new Angle(0);
				SetAngle(Motor,Target);
				ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
			}
		}
		if(ready){
			Command_Stage++;
			Command_Timer=0;
		}
	}
	if(Command_Stage==1){
		SetLock(MyArm.MyHand,true);
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_Position-Motor.GetPosition();
			float Speed_Multx=0.5f;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			SetDirection(Motor,Direction,Speed_Multx);
		}
		if((MyArm.Motors[MyArm.Motors.Count-1].Top.GetPosition()-Target_Position).Length()<1)
			Command_Stage++;
	}
	if(Command_Stage==2){
		SetLock(MyArm,true);
		Command_Stage++;
	}
}

/*
* Grab
* 	0: Wind Arm, Prepare Hand
* 	1: Release Arm
* 	2: Change Arm Target, Lock Hand
* 	E: Unlock Hand
*/
void Grab(){
	if(Command_Stage==0){
		bool ready=true;
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_Position-Motor.GetPosition();
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			if(Motor==MyArm.FirstHinge)
				Target=new Angle(-90);
			SetAngle(Motor,Target,Speed_Multx);
			ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
		}
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Vector3D Direction=Target_Position-Motor.GetPosition();
				Direction.Normalize();
				Angle Target=GetTarget(Motor,Direction);
				if(IsHinge(Motor))
					Target/=2;
				SetAngle(Motor,Target);
				ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
			}
		}
		if(ready)
			Command_Stage++;
	}
	if(Command_Stage==1){
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_Position-Motor.GetPosition();
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			SetDirection(Motor,Direction,Speed_Multx);
		}
		double Distance=Hand_Distance;
		bool ready=Distance<1;
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Vector3D Direction=Target_Position-Motor.GetPosition();
				Direction.Normalize();
				Angle Target=GetTarget(Motor,Direction);
				if(IsHinge(Motor)&&Distance>1)
					Target/=2;
				SetAngle(Motor,Target);
				ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
			}
		}
		if(ready)
			Command_Stage++;
	}
	if(Command_Stage==2){
		SetLock(MyArm.MyHand,true);
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_2-Motor.GetPosition();
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			SetDirection(Motor,Direction,Speed_Multx);
		}
	}
}

/*
* Throw
* 	0: Grab 0
* 	1: Grab 1
* 	2: Lock Hand, Wind Arm
* 	3: Release Arm, Unlock Hand, Open Hand, Wait 2 Seconds
*	E: Unlock Hand
*/
void Throw(){
	if(Command_Stage<2)
		Grab();
	if(Command_Stage==2){
		SetLock(MyArm.MyHand,true);
		bool ready=true;
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_2-Motor.GetPosition();
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			if(Motor==MyArm.FirstHinge)
				Target=new Angle(-90);
			SetAngle(Motor,Target,Speed_Multx);
			ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
		}
		if(ready){
			Command_Stage++;
			Command_Timer=0;
		}
	}
	if(Command_Stage==3){
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_2-Motor.GetPosition();
			float Speed_Multx=3;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			SetDirection(Motor,Direction,Speed_Multx);
		}
		SetLock(MyArm.MyHand,false);
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Angle Target=new Angle(5);
				if(IsRotor(Motor))
					Target=new Angle(0);
				SetAngle(Motor,Target);
			}
		}
		if(Command_Timer>=2)
			Command_Stage++;
	}
	if(Command_Stage>3)
		Force_End=true;
}

/*
* Block
* 	0: Clench Hand, Set Arm Targets, Release Arm
* 	1: Lock Hand, Lock Arm
* 	E: Unlock Hand, Unlock Arm
*/
void Block(){
	if(Command_Stage==0){
		bool ready=true;
		Vector3D My_Target=MyArm.Motors[0].GetPosition()+3*Forward_Vector;
		foreach(IMyMotorStator Motor in MyArm.Motors){
			if(Motor==MyArm.FirstHinge)
				My_Target=Controller.GetPosition()+5*Forward_Vector;
			Vector3D Direction=My_Target-Motor.GetPosition();
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				Direction=Forward_Vector;
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			SetAngle(Motor,Target,Speed_Multx);
			ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
		}
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Angle Target=new Angle(90);
				if(IsRotor(Motor))
					Target=new Angle(0);
				SetAngle(Motor,Target);
				ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
			}
		}
		if(ready)
			Command_Stage++;
	}
	if(Command_Stage==1){
		SetLock(MyArm,true);
		SetLock(MyArm.MyHand,true);
		Command_Stage++;
	}
}

/*
* Wave
* 	0: Change Arm Targets, Open Hand
* 	1: Set Wave Timer, Lock Hand, Alter Last Rotor target slightly
* 	E: Unlock Hand
*/
void Wave(){
	if(Command_Stage==0){
		bool ready=true;
		Vector3D My_Target=Right_Vector;
		foreach(IMyMotorStator Motor in MyArm.Motors){
			if(Motor==MyArm.FirstHinge)
				My_Target=Forward_Vector;
			Vector3D Direction=My_Target;
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			SetAngle(Motor,Target,Speed_Multx);
			ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
		}
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Angle Target=new Angle(0);
				SetAngle(Motor,Target);
				ready=ready&&Math.Abs(Adjusted_Difference(Motor,Target))<5&&GetRPM(Motor)<5;
			}
		}
		if(ready){
			Command_Stage++;
			Command_Timer=0;
		}
	}
	if(Command_Stage==1){
		SetLock(MyArm.MyHand,true);
		Vector3D My_Target=Right_Vector;
		foreach(IMyMotorStator Motor in MyArm.Motors){
			if(Motor==MyArm.FirstHinge)
				My_Target=Forward_Vector;
			Vector3D Direction=My_Target;
			float Speed_Multx=1;
			if(Motor==MyArm.LastRotor){
				Speed_Multx*=5;
				if(Controller!=null&&Controller.GetTotalGravity().Length()>0)
					Direction=Controller.GetTotalGravity();
			}
			Direction.Normalize();
			Angle Target=GetTarget(Motor,Direction);
			if(Motor==MyArm.LastRotor){
				Angle Offset=new Angle(15);
				Command_Timer=Command_Timer%3;
				if(Command_Timer<=1.5)
					Offset*=-1;
				Target+=Offset;
			}
			SetAngle(Motor,Target,Speed_Multx);
		}
	}
}

/*
* Spin
* 	0: Release Hand, Release Arm, Set Last Rotor Velocity
* 	E: Nothing
*/
void Spin(){
	if(Command_Stage==0){
		foreach(IMyMotorStator Motor in MyArm.Motors){
			Vector3D Direction=Target_Position-Motor.GetPosition();
			Direction.Normalize();
			if(Motor==MyArm.LastRotor){
				Motor.TargetVelocityRPM=30;
			}
			else
				SetDirection(Motor,Direction);
		}
		foreach(Arm Finger in MyArm.MyHand){
			foreach(IMyMotorStator Motor in Finger.Motors){
				Vector3D Direction=Target_Position-Motor.GetPosition();
				Direction.Normalize();
				SetDirection(Motor,Direction);
			}
		}
	}
}

void UpdateProgramInfo(){
	cycle_long += ((++cycle)/long.MaxValue)%long.MaxValue;
	cycle = cycle % long.MaxValue;
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
	Echo(Program_Name + " OS " + cycle_long.ToString() + '-' + cycle.ToString() + " (" + loading_char + ")");
	Me.GetSurface(1).WriteText(Program_Name + " OS " + cycle_long.ToString() + '-' + cycle.ToString() + " (" + loading_char + ")",false);
	seconds_since_last_update = Runtime.TimeSinceLastRun.TotalSeconds + (Runtime.LastRunTimeMs / 1000);
	if(seconds_since_last_update<1)
		Echo(Math.Round(seconds_since_last_update*1000, 0).ToString() + " milliseconds\n");
	else if(seconds_since_last_update<60)
		Echo(Math.Round(seconds_since_last_update, 3).ToString() + " seconds\n");
	else if(seconds_since_last_update/60<60)
		Echo(Math.Round(seconds_since_last_update/60, 2).ToString() + " minutes\n");
	else if(seconds_since_last_update/60/60<24)
		Echo(Math.Round(seconds_since_last_update/60/60, 2).ToString() + " hours\n");
	else 
		Echo(Math.Round(seconds_since_last_update/60/60/24, 2).ToString() + " days\n");
	List<IMyMotorStator> All_Motors=new List<IMyMotorStator>();
	GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(All_Motors);
	foreach(IMyMotorStator Motor in All_Motors)
		UpdateMotor(Motor);
	if(Command_Timer<10)
		Command_Timer+=seconds_since_last_update;
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
	MyEntities.UpdatePositions(seconds_since_last_update);
	Scan_Timer+=seconds_since_last_update;
}

double Scan_Timer=10;
void PerformScan(){
	if(Scan_Timer>=0.5){
		Scan_Timer=0;
		List<IMySensorBlock> Sensors=new List<IMySensorBlock>();
		GridTerminalSystem.GetBlocksOfType<IMySensorBlock>(Sensors);
		bool Altered=false;
		foreach(IMySensorBlock Sensor in Sensors){
			List<MyDetectedEntityInfo> Detected=new List<MyDetectedEntityInfo>();
			Sensor.DetectedEntities(Detected);
			foreach(MyDetectedEntityInfo Entity in Detected){
				EntityInfo entity=new EntityInfo(Entity);
				MyEntities.UpdateEntry(entity);
				if(entity.ID==Target_ID||Target_ID==0)
					Target_Position=entity.GetPosition();
				Altered=true;
				IGC.SendBroadcastMessage("Entity Report",entity.ToString(),TransmissionDistance.TransmissionDistanceMax);
			}
		}
		for(int i=0;i<MyEntities.Count;i++){
			if((MyEntities[i].GetPosition()-Controller.GetPosition()).Length()>1000){
				Altered=true;
				MyEntities.RemoveEntry(MyEntities[i]);
			}
		}
		if(Altered&&!CommandMenu.IsSelected)
			Create_EntityMenu();
	}
	Write("Scan: "+Math.Round(Scan_Timer,3).ToString()+"s");
	if(MyEntities.Count==1)
		Write(MyEntities.Count.ToString()+" Entity on Record");
	else
		Write(MyEntities.Count.ToString()+" Entities on Record");
}

public void Main(string argument, UpdateType updateSource)
{
	try{
		UpdateProgramInfo();
		PerformScan();
		if(argument.Length>0){
			if(argument.ToLower().Equals("back"))
				Back();
			else if(argument.ToLower().Equals("prev"))
				Prev();
			else if(argument.ToLower().Equals("next"))
				Next();
			else if(argument.ToLower().Equals("select"))
				Select();
		}
		DisplayMenus();
		if(Target_ID<-1){
			if(Target_ID==-2){
				Target_Position=MyArm.Motors[0].GetPosition()+20*Forward_Vector;
			}
			else if(Target_ID==-3){
				Target_Position=MyArm.Motors[0].GetPosition()+20*Up_Vector;
			}
			else if(Target_ID==-3){
				Target_Position=MyArm.Motors[0].GetPosition()+20*Right_Vector;
			}
		}
		Write("Current:"+Current_Command.ToString()+" "+Command_Stage.ToString());
		if(Current_Command!=Next_Command)
			Write("Next:"+Next_Command.ToString());
		PerformCommand();
		if(Current_Command==ArmCommand.Idle)
			Runtime.UpdateFrequency=UpdateFrequency.Update100;
		else
			Runtime.UpdateFrequency=UpdateFrequency.Update1;
		Write(Runtime.UpdateFrequency.ToString());
	}
	catch(Exception e){
		try{
			foreach(IMyMotorStator Motor in MyArm.AllMotors){
				try{
					Motor.TargetVelocityRPM=0;
				}
				catch(Exception){
					continue;
				}
			}
			List<IMyInteriorLight> Lights=new List<IMyInteriorLight>();
			GridTerminalSystem.GetBlocksOfType<IMyInteriorLight>(Lights);
			foreach(IMyInteriorLight Light in Lights){
				try{
					if(Light.Enabled){
						SetBlockData(Light,"ExceptionState","active");
						Light.Enabled=false;
					}
				}
				catch(Exception){
					continue;
				}
			}
		}
		catch(Exception){
			;
		}
		throw e;
	}
}
