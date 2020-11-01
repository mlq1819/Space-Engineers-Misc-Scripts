/*
* Vigilance Targeting System 
* Built by mlq1616
* https://github.com/mlq1819
*/
//Name me!
private const string Program_Name="Vigilance AI";
//Sets the maximum firing distance
private const double FIRING_DISTANCE=10000; //Recommended between 5k and 20k; works best within range
//The distance the scanners will default to during AutoScan; the lower the faster it scans
private const double AUTOSCAN_DISTANCE=5000;//Recommended between 2k and 10k
//Sets whether the AI starts out scanning or whether it has to wait to be told to autoscan
private const bool DEFAULT_AUTOSCAN=true;

public class GenericMethods<T> where T : class, IMyTerminalBlock{
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
	
	public T GetContaining(string name, double max_distance, Vector3D Reference){
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
	
	public T GetContaining(string name, double max_distance, IMyTerminalBlock Reference){
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

public void Write(string text, bool new_line=true, bool append=true){
	Echo(text);
	if(new_line)
		Me.GetSurface(0).WriteText(text+'\n', append);
	else
		Me.GetSurface(0).WriteText(text, append);
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

private enum CannonTask{
	None=0,
	Scan=1,
	Search=2,
	Fire3=3,
	Fire2=4,
	Fire1=5
}

private enum FireStatus{
	Idle=0,
	Printing=1
	Aiming=2,
	WaitingClear=3,
	Firing=4
}

private enum PrintStatus{
	StartingPrint=0,
	Printing=1,
	WaitingResources=2,
	EndingPrint=3,
	Ready=4
}

private long cycle_long = 1;
private long cycle = 0;
private char loading_char = '|';
private double seconds_since_last_update = 0;

private IMyRemoteControl Controller;
private IMyProjector Projector;
private IMyMotorStator YawRotor;
private IMyMotorStator PitchRotor;
private IMyMotorStator ShellRotor;
private IMyShipWelder Welder;
private IMySensorBlock Sensor;
private IMyShipMergeBlock Merge;
private List<IMyCameraBlock> Cameras;

private List<IMyInteriorLight> FiringLights;
private List<List<IMyInteriorLight>> StatusLights;

private bool AutoFire=false;
private bool AutoScan=DEFAULT_AUTOSCAN;
private Vector3D Target_Position=new Vector3D(0,0,0);
private Vector3D Target_Velocity=new Vector3D(0,0,0);
private Vector3D Aim_Position=new Vector3D(0,0,0);
private double Target_Distance{
	get{
		if(Projector==null)
			return double.MaxValue;
		return (Target_Position-Projector.GetPosition()).Length();
	}
}

private Vector3D Forward_Vector;
private Vector3D Backward_Vector{
	get{
		return -1*Forward_Vector;
	}
}
private Vector3D Up_Vector;
private Vector3D Down_Vector{
	get{
		return -1*Up_Vector;
	}
}
private Vector3D Left_Vector;
private Vector3D Right_Vector{
	get{
		return -1*Left_Vector;
	}
}

private Queue<CannonTask> TaskQueue=new Queue<CannonTask>();
private CannonTask CurrentTask{
	get{
		if(TaskQueue.Count==0)
			return CannonTask.None;
		return TaskQueue.Peek();
	}
}

private PrintStatus CurrentPrintStatus;
private FireStatus CurrentFireStatus;

public Vector3D GlobalToLocal(Vector3D Global){
	Vector3D Local=Vector3D.Transform(Global+Controller.GetPosition(), MatrixD.Invert(Controller.WorldMatrix));
	Local.Normalize();
	return Local*Global.Length();
}

public Vector3D GlobalToLocalPosition(Vector3D Global){
	Vector3D Local=Vector3D.Transform(Global, MatrixD.Invert(Controller.WorldMatrix));
	Local.Normalize();
	return Local*(Global-Controller.GetPosition()).Length();
}

public Vector3D LocalToGlobal(Vector3D Local){
	Vector3D Global=Vector3D.Transform(Local, Controller.WorldMatrix)-Controller.GetPosition();
	Global.Normalize();
	return Global*Local.Length();
}

public Vector3D LocalToGlobalPosition(Vector3D Local){
	return Vector3D.Transform(Local,Controller.WorldMatrix);
}

private double GetAngle(Vector3D v1, Vector3D v2){
	return GenericMethods<IMyTerminalBlock>.GetAngle(v1,v2);
}

private bool Operational=false;
public Program(){
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
	Me.Enabled=true;
	
	Operational=false;
	
	Controller=(new GenericMethods<IMyRemoteControl>(this)).GetFull("Driver Controller");
	if(Controller==null)
		return;
	Projector=(new GenericMethods<IMyProjector>(this)).GetFull("Shell Projector");
	if(Projector==null)
		return;
	YawRotor=(new GenericMethods<IMyMotorStator>(this)).GetFull("Yaw Rotor");
	if(YawRotor==null)
		return;
	PitchRotor=(new GenericMethods<IMyMotorStator>(this)).GetFull("Pitch Rotor");
	if(PitchRotor==null)
		return;
	ShellRotor=(new GenericMethods<IMyMotorStator>(this)).GetFull("Shell Rotor");
	if(ShellRotor==null)
		return;
	Sensor=(new GenericMethods<IMySensorBlock>(this)).GetFull("Driver Sensor");
	if(Sensor==null)
		return;
	Welder=(new GenericMethods<IMyShipWelder>(this)).GetFull("Driver Shell Welder");
	if(Welder==null)
		return;
	Merge=(new GenericMethods<IMyShipMergeBlock>(this)).GetFull("Shell Printer Merge Block");
	if(Merge==null)
		return;
	Cameras=(new GenericMethods<IMyCameraBlock>(this)).GetAllContaining("Driver Camera ");
	if(Cameras.Count==0)
		return;
	
	FiringLights=(new GenericMethods<IMyInteriorLight>(this)).GetAllContaining("Driver Firing Light ");
	StatusLights=new List<List<IMyInteriorLight>>();
	for(int i=1;i<=3;i++){
		StatusLights.Add((new GenericMethods<IMyInteriorLight>(this)).GetAllContaining("Driver Status Light "+i.ToString()));
	}
	
	TaskQueue=new Queue<CannonTask>();
	string[] args=this.Storage.Split('•');
	foreach(string arg in args){
		try{
			if(arg.IndexOf("Task:")==0){
				string word=arg.Substring("Task:".Length);
				int output=0;
				if(Int32.TryParse(word,output))
					TaskQueue.Enqueue((CannonTask)output);
			}
			else if(arg.IndexOf("Status:")==0){
				string word=arg.Substring("Status:".Length);
				int output=0;
				if(Int32.TryParse(word,output))
					CurrentStatus=(CannonStatus)output;
			}
			else if(arg.IndexOf("AutoScan:")==0){
				string word=arg.Substring("AutoScan:".Length);
				bool.TryParse(word,AutoScan);
			}
		}
		catch(Exception){
			continue;
		}
	}
	
	
	if(TaskQueue.Count==0&&AutoScan)
		TaskQueue.Add(CannonTask.Scan);
	
	Operational=true;
	if(((int)CurrentTask)>=((int)CannonTask.Fire3))
		Runtime.UpdateFrequency=UpdateFrequency.Update10;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}

public void Save(){
    this.Storage="Status:"+((int)CurrentStatus).ToString();
	this.Storage="•AutoScan:"+AutoScan.ToString();
	while(TaskQueue.Count>0){
		this.Storage+="•Task:"+((int)CurrentTask).ToString();
		TaskQueue.Dequeue();
	}
	Me.CustomData=this.Storage;
}

private void AddTask(CannonTask Task){
	if(Task==CannonTask.None||TaskQueue.Contains(Task))
		return;
	if(((int)Task)>((int)CurrentTask)){
		Queue<CannonTask> old=new Queue<CannonTask>();
		while(TaskQueue.Count>0)
			old.Enqueue(TaskQueue.Dequeue());
		TaskQueue.Enqueue(Task);
		while(old.Count>0)
			TaskQueue.Enqueue(old.Dequeue());
	}
	else
		TaskQueue.Enqueue(Task);
}

private void NextTask(){
	if(TaskQueue.Count>0)
		TaskQueue.Dequeue();
	if(AutoScan&&TaskQueue.Count==0)
		TaskQueue.Enqueue(CannonTask.Scan);
}

private void UpdateProgramInfo(){
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
	Echo(Program_Name+" OS "+cycle_long.ToString()+'-'+cycle.ToString()+" ("+loading_char+")");
	Me.GetSurface(1).WriteText(Program_Name+" OS "+cycle_long.ToString()+'-'+cycle.ToString()+" ("+loading_char+")", false);
	Me.GetSurface(0).WriteText("", false);
	if(seconds_since_last_update<1){
		Echo(Math.Round(seconds_since_last_update*1000, 0).ToString() + " milliseconds\n");
	}
	else if(seconds_since_last_update<60){
		Echo(Math.Round(seconds_since_last_update, 3).ToString() + " seconds\n");
	}
	else if(seconds_since_last_update/60<60){
		Echo(Math.Round(seconds_since_last_update/60, 2).ToString() + " minutes\n");
	}
	else if(seconds_since_last_update/60/60<24){
		Echo(Math.Round(seconds_since_last_update/60/60, 2).ToString() + " hours\n");
	}
	else {
		Echo(Math.Round(seconds_since_last_update/60/60/24, 2).ToString() + " days\n");
	}
}

private void UpdatePositionalInfo(){
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector);
	Forward_Vector.Normalize();
	
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector);
	Up_Vector.Normalize();
	
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector);
	Left_Vector.Normalize();
}

private double Print_Timer=0.0;
private void Print(){
	double rotor_angle=ShellRotor.Angle/Math.PI*180;
	bool is_forward=Math.Abs(rotor_angle-180)<1;
	bool is_backward=Math.Abs((rotor_angle+360)%360)<1;
	bool is_printed=Projector.RemainingBlocks==0;
	
	ShellRotor.RotorLock=false;
	if(is_printed){
		Welder.Enabled=false;
		ShellRotor.TargetVelocityRPM=30.0f;
		if(is_forward){
			ShellRotor.TargetVelocityRPM=0;
			ShellRotor.RotorLock=true;
			CurrentPrintStatus=PrintStatus.Ready;
		}
		else{
			CurrentPrintStatus=PrintStatus.EndingPrint;
		}
	}
	else{
		Welder.Enabled=true;
		Projector.Enabled=true;
		ShellRotor.TargetVelocityRPM=-30.0f;
		else if(is_backward){
			ShellRotor.TargetVelocityRPM=0;
			ShellRotor.RotorLock=true;
			Merge.Enabled=true;
			if(Print_Timer>30){
				CurrentPrintStatus=PrintStatus.Printing;
			}
			else{
				CurrentPrintStatus=PrintStatus.WaitingResources;
			}
		}
		else {
			Print_Timer=0.0;
			CurrentPrintStatus=PrintStatus.StartingPrint;
		}
	}
	for(int i=0;i<StatusLights.Count;i++){
		if(i==0){
			foreach(IMyInteriorLight Light in StatusLights[i]){
				Light.Enabled=(CurrentPrintStatus>=PrintStatus.Printing);
			}
		}
		else if(i==1){
			foreach(IMyInteriorLight Light in StatusLights[i]){
				Light.Enabled=(CurrentPrintStatus>=PrintStatus.WaitingResources);
			}
		}
		else if(i==2){
			foreach(IMyInteriorLight Light in StatusLights[i]){
				Light.Enabled=(CurrentPrintStatus>=PrintStatus.Ready);
			}
		}
	}
}

public void Scan(){
	
}

public void Search(){
	
}

public void Fire(){
	
}

private void TimerUpdate(){
	if(CurrentPrintStatus!=PrintStatus.Printing)
		Print_Timer+=seconds_since_last_update;
	
}

public void Main(string argument, UpdateType updateSource)
{
	if(!Operational){
		Runtime.UpdateFrequency=UpdateFrequency.None;
		return;
	}
	UpdateProgramInfo();
	UpdatePositionalInfo();
	TimerUpdate();
	if(argument.Length>0){
		argument=argument.ToLower();
		if(argument.Equals("scan")){
			AddTask(CannonTask.Scan);
		}
		else if(argument.IndexOf("autoscan")==0){
			string word=argument.Substring("autoscan".Length);
			if(word.Length==0||word.Contains("toggle")||word.Contains("switch"))
				Autoscan=!Autoscan;
			else if(word.Contains("on")||word.Contains("enabled")||word.Contains("true"))
				Autoscan=true;
			else if(word.Contains("off")||word.Contains("disabled")||word.Contains("false"))
				Autoscan=false;
		}
		else if(argument.IndexOf("search:")==0){
			string words=argument.Substring("search:".Length).Split(';');
			if(words.Count>0){
				bool set=false;
				if(!Vector3D.TryParse(words[0],Target_Position)){
					MyWaypointInfo Waypoint;
					if(!MyWaypointInfo.TryParse(words[0],Waypoint)){
						if(MyWaypointInfo.TryParse(words[0].Substring(0,words[0].Length-10),Waypoint))
							set=true;
					}
					else
						set=true;
					if(set){
						Target_Position=Waypoint.Coords;
					}
				}
				else
					set=true;
				if(set){
					Target_Velocity=new Vector3D(0,0,0);
					if(words.Count>1){
						Vector3D.TryParse(words[1],Target_Velocity);
					}
					AddTask(CannonTask.Search);
				}
			}
		}
		else if(argument.Equals("dumbfire")){
			Target_Position=Forward_Vector*Math.Min(FIRING_DISTANCE,3000)+Controller.GetPosition();
			Target_Velocity=new Vector3D(0,0,0);
			if(CurrentPrintStatus==PrintStatus.Ready)
				CurrentFireStatus=FireStatus.Aiming;
			else
				CurrentFireStatus=FireStatus.Printing;
			SetAimed();
		}
		else if(argument.IndexOf("fire:")==0){
			string words=argument.Substring("fire:".Length).Split(';');
			if(words.Count>0){
				bool set=false;
				if(!Vector3D.TryParse(words[0],Target_Position)){
					MyWaypointInfo Waypoint;
					if(!MyWaypointInfo.TryParse(words[0],Waypoint)){
						if(MyWaypointInfo.TryParse(words[0].Substring(0,words[0].Length-10),Waypoint))
							set=true;
					}
					else
						set=true;
					if(set){
						Target_Position=Waypoint.Coords;
						SetAimed();
					}
				}
				else
					set=true;
				if(set){
					Target_Velocity=new Vector3D(0,0,0);
					if(words.Count>1){
						Vector3D.TryParse(words[1],Target_Velocity);
					}
					AddTask(CannonTask.Fire1);
					if(CurrentPrintStatus==PrintStatus.Ready)
						CurrentFireStatus=FireStatus.Aiming;
					else
						CurrentFireStatus=FireStatus.Printing;
					SetAimed();
				}
			}
		}
	}
	Print();
	switch(CurrentTask){
		case CannonTask.Scan:
			Scan();
			break;
		case CannonTask.Target:
			Search();
			break;
		case CannonTask.Fire:
			Fire();
			break;
	}
	if(((int)CurrentTask)>=((int)CannonTask.Fire3))
		Runtime.UpdateFrequency=UpdateFrequency.Update10;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
}
