//Name me!
private const string Program_Name = "Pluton Ship AI"; 
//Decides whether SPEED_CAP is relevant
private const bool DO_SPEED_CAP = true; 
//should be between 0.0f and 1.0f; recommended between 0.7f and 0.95f
private const float SPEED_CAP = 0.9f;
//Double the angle of what the ship will accept
private const double ACCEPTABLE_ANGLE = 20; 
//how long after you stop spinning before it registers it's not spinning
private const double WAIT_DURATION = 1.0f; 
//Decides whether the ships should attempt to pitch to be aligned with gravity
private const bool GRAVITY_PITCH_ADJUST = false;
//Affects how quickly the gravity pitch/roll adjust moves
private const float GRAV_ADJUST_MULTX = 0.9f;
//Keep between 0 and 1; the closer to 1, the faster the ship stops spinning but the more likely it will over-correct
private const float ROTATIONAL_DAMPENER_MULTX = 0.9f;
//Affects how quickly the target pitch/roll/yaw adjust moves
private const float TARGET_ADJUST_MULTX = 0.5f;
//Time to wait between scans
private const double SCAN_FREQUENCY = 2.5f;
//Set this to the distance you want lights and sound blocks to update on an alert
private const double ALERT_DISTANCE = 25;

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
		return GetContaining(name, max_distance, Reference.GetPosition());
	}
	
	public T GetContaining(string name, double max_distance){
		return GetContaining(name, max_distance, Prog);
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
	
	public List<T> SortByDistance(List<T> unsorted){
		List<T> output = new List<T>();
		while(unsorted.Count > 0){
			double min_distance = double.MaxValue;
			foreach(T Block in unsorted){
				double distance = (Prog.GetPosition() - Block.GetPosition()).Length();
				min_distance = Math.Min(min_distance, distance);
			}
			for(int i=0; i<unsorted.Count; i++){
				double distance = (Prog.GetPosition() - unsorted[i].GetPosition()).Length();
				if(distance <= min_distance + 0.1){
					output.Add(unsorted[i]);
					unsorted.RemoveAt(i);
					break;
				}
			}
		}
		return output;
	}
	
	public static double GetAngle(Vector3D v1, Vector3D v2){
		v1.Normalize();
		v2.Normalize();
		return Math.Round(Math.Acos(v1.X*v2.X + v1.Y*v2.Y + v1.Z*v2.Z) * 57.295755, 5);
	}
}

private class EntityInfo{
	public long ID;
	public string Name;
	public MyDetectedEntityType Type;
	private Vector3D? _hitposition;
	public Vector3D? HitPosition{
		get{
			return _hitposition;
		}
		set{
			_hitposition = value;
			if(_hitposition!=null){
				Size = Math.Max(Size, (Position - ((Vector3D)_hitposition)).Length());
			}
		}
	}
	private Vector3D _velocity;
	public Vector3D Velocity{
		get{
			return _velocity;
		}
		set{
			_velocity = value;
			Age = TimeSpan.Zero;
		}
	}
	public MyRelationsBetweenPlayerAndBlock Relationship;
	public Vector3D Position;
	public double Size = 0;
	public TimeSpan Age = TimeSpan.Zero;
	
	public EntityInfo(long id, string name, MyDetectedEntityType type, Vector3D? hitposition, Vector3D velocity, MyRelationsBetweenPlayerAndBlock relationship, Vector3D position){
		ID = id;
		Name = name;
		Type = type;
		HitPosition = hitposition;
		Velocity = velocity;
		Relationship = relationship;
		Position = position;
		Age = TimeSpan.Zero;
	}
	
	public EntityInfo(long id, string name, MyDetectedEntityType type, Vector3D? hitposition, Vector3D velocity, MyRelationsBetweenPlayerAndBlock relationship, Vector3D position, double size) : this(id, name, type, hitposition, velocity, relationship, position){
		this.Size = size;
	}
	
	public EntityInfo(EntityInfo o){
		ID = o.ID;
		Name = o.Name;
		Type = o.Type;
		Position = o.Position;
		HitPosition = o.HitPosition;
		Velocity = o.Velocity;
		Relationship = o.Relationship;
		Size = o.Size;
		Age = o.Age;
	}
	
	public EntityInfo(MyDetectedEntityInfo entity_info){
		ID = entity_info.EntityId;
		Name = entity_info.Name;
		Type = entity_info.Type;
		Position = entity_info.Position;
		if(entity_info.HitPosition != null){
			HitPosition = entity_info.HitPosition;
		}
		else {
			HitPosition = (Vector3D?) null;
		}
		Velocity = entity_info.Velocity;
		Relationship = entity_info.Relationship;
		Age = TimeSpan.Zero;
	}
	
	public static bool TryParse(string Parse, out EntityInfo Entity){
		Entity = new EntityInfo(-1,"bad", MyDetectedEntityType.None, null, new Vector3D(0,0,0), MyRelationsBetweenPlayerAndBlock.NoOwnership, new Vector3D(0,0,0));
		try{
			string[] args = Parse.Split('\n');
			long id;
			if(!Int64.TryParse(args[0], out id)){
				return false;
			}
			string name = args[1];
			MyDetectedEntityType type = (MyDetectedEntityType) Int32.Parse(args[2]);
			Vector3D? hitposition;
			if(args[3].Equals("null")){
				hitposition = (Vector3D?) null;
			}
			else {
				Vector3D temp;
				if(!Vector3D.TryParse(args[3], out temp)){
					return false;
				}
				else {
					hitposition = (Vector3D?) temp;
				}
			}
			Vector3D velocity;
			if(!Vector3D.TryParse(args[4], out velocity)){
				return false;
			}
			MyRelationsBetweenPlayerAndBlock relationship = (MyRelationsBetweenPlayerAndBlock) Int32.Parse(args[5]);
			Vector3D position;
			if(!Vector3D.TryParse(args[6], out position)){
				return false;
			}
			double size = 0;
			if(!double.TryParse(args[7], out size)){
				size = 0;
			}
			TimeSpan age;
			if(!TimeSpan.TryParse(args[8], out age)){
				return false;
			}
			Entity = new EntityInfo(id, name, type, hitposition, velocity, relationship, position, size);
			Entity.Age = age;
			return true;
		}
		catch(Exception){
			return false;
		}
	}
	
	public override string ToString(){
		string entity_info = "";
		entity_info += ID.ToString() + '\n';
		entity_info += Name.ToString() + '\n';
		entity_info += ((int)Type).ToString() + '\n';
		if(HitPosition != null){
			entity_info += ((Vector3D) HitPosition).ToString() + '\n';
		}
		else {
			entity_info += "null" + '\n';
		}
		entity_info += Velocity.ToString() + '\n';
		entity_info += ((int)Relationship).ToString() + '\n';
		entity_info += Position.ToString() + '\n';
		entity_info += Size.ToString() + '\n';
		entity_info += Age.ToString() + '\n';
		return entity_info;
	}
	
	public string NiceString(){
		string entity_info = "";
		entity_info += "ID: " + ID.ToString() + '\n';
		entity_info += "Name: " + Name.ToString() + '\n';
		entity_info += "Type: " + Type.ToString() + '\n';
		if(HitPosition != null){
			entity_info += "HitPosition: " + NeatVector((Vector3D) HitPosition) + '\n';
		}
		else {
			entity_info += "HitPosition: " + "null" + '\n';
		}
		entity_info += "Velocity: " + NeatVector(Velocity) + '\n';
		entity_info += "Relationship: " + Relationship.ToString() + '\n';
		entity_info += "Position: " + NeatVector(Position) + '\n';
		entity_info += "Size: " + ((int)Size).ToString() + '\n';
		entity_info += "Data Age: " + Age.ToString() + '\n';
		return entity_info;
	}
	
	public static string NeatVector(Vector3D vector){
		return "X:" + ((long)vector.X).ToString() + " Y:" + ((long)vector.Y).ToString() + " Z:" + ((long)vector.Z).ToString();
	}
	
	public void Update(double seconds){
		TimeSpan time = new TimeSpan((int)(seconds/60/60/24), ((int)(seconds/60/60))%24, ((int)(seconds/60))%60, ((int)(seconds))%60, ((int)(seconds*1000))%1000);
		Age.Add(time);
		Position += seconds * Velocity;
		if(HitPosition != null){
			HitPosition = (Vector3D?) (((Vector3D)HitPosition) + seconds * Velocity);
		}
	}
	
}

private long cycle_long = 1;
private long cycle = 0;
private char loading_char = '|';
double seconds_since_last_update = 0;

private Random Rnd;

private bool match_direction = false;
private Vector3D target_direction = new Vector3D(0,0,0);
private Vector3D actual_target_direction = new Vector3D(0,0,0);
private bool match_position = false;
private Vector3D target_position = new Vector3D(0,0,0);
private Vector3D actual_target_position = new Vector3D(0,0,0);
private bool update_position = false;
private Vector3D target_velocity = new Vector3D(0,0,0);
private Vector3D actual_target_velocity = new Vector3D(0,0,0);
private bool detected_target = false;
private EntityInfo target_info = null;
private Vector3D position_offset = new Vector3D(0,0,0);

private double Speed_Limit = 100.0;

private bool HasNearestPlanet = false;
private Vector3D NearestPlanet = new Vector3D(0,0,0);

private double MySize = 2.5;

private List<EntityInfo> AsteroidList = new List<EntityInfo>();
private List<EntityInfo> PlanetList = new List<EntityInfo>();
private List<EntityInfo> SmallShipList = new List<EntityInfo>();
private List<EntityInfo> LargeShipList = new List<EntityInfo>();
private List<EntityInfo> CharacterList = new List<EntityInfo>();

private IMyShipController Controller = null;
private IMyGyro Gyroscope = null;
private List<IMyThrust> Forward_Thrusters = new List<IMyThrust>();
private List<IMyThrust> Backward_Thrusters = new List<IMyThrust>();
private List<IMyThrust> Up_Thrusters = new List<IMyThrust>();
private List<IMyThrust> Down_Thrusters = new List<IMyThrust>();
private List<IMyThrust> Left_Thrusters = new List<IMyThrust>();
private List<IMyThrust> Right_Thrusters = new List<IMyThrust>();
private List<IMyThrust> Unknown_Thrusters = new List<IMyThrust>();

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

private float Forward_Thrust = 0.0f;
private float Backward_Thrust = 0.0f;
private float Up_Thrust = 0.0f;
private float Down_Thrust = 0.0f;
private float Left_Thrust = 0.0f;
private float Right_Thrust = 0.0f;

private bool ControllerFunction(IMyShipController ctrlr){
	IMyRemoteControl Remote = ctrlr as IMyRemoteControl;
	if(Remote!=null)
		return ctrlr.ControlThrusters;
	else
		return (ctrlr.ControlThrusters && ctrlr.IsMainCockpit);
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

private void Setup(){
	Echo("Beginning initialization");
	List<IMyTerminalBlock> AllTerminalBlocks = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(AllTerminalBlocks);
	foreach(IMyTerminalBlock Block in AllTerminalBlocks){
		MySize = Math.Max(MySize, (Me.CubeGrid.GetPosition() - Block.GetPosition()).Length());
	}
	Controller = (new GenericMethods<IMyShipController>(this)).GetClosestFunc(ControllerFunction);
	if(Controller!=null){
		Echo("Found Controller: " + Controller.CustomName);
		SetControllerDirections();
		SetDirections();
	}
	else {
		Echo("Failed to initialize Controller");
		return;
	}
	Gyroscope = (new GenericMethods<IMyGyro>(this)).GetContaining("Control Gyroscope");
	if(Gyroscope!=null){
		Echo("Found Gyroscope: " + Gyroscope.CustomName);
		Gyroscope.GyroOverride = Controller.IsUnderControl;
	}
	else {
		Echo("Failed to initialize Gyroscope");
		return;
	}
	List<IMyThrust> MyThrusters = (new GenericMethods<IMyThrust>(this)).GetAllContaining("");
	foreach(IMyThrust Thruster in MyThrusters){
		Base6Directions.Direction ThrustDirection = Thruster.Orientation.Forward;
		
		if(ThrustDirection == Backward){
			Forward_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection == Forward){
			Backward_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection == Down){
			Up_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection == Up){
			Down_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection == Right){
			Left_Thrusters.Add(Thruster);
		}
		else if(ThrustDirection == Left){
			Right_Thrusters.Add(Thruster);
		}
		else{
			Unknown_Thrusters.Add(Thruster);
		}
	}
	
	
	Runtime.UpdateFrequency = UpdateFrequency.Update1;
}

public Program()
{
	Rnd = new Random();
    Me.CustomName = (Program_Name + " Programmable block").Trim();
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
	Setup();
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
		Thruster.ThrustOverridePercentage = 0.0f;
	}
	foreach(IMyThrust Thruster in Backward_Thrusters){
		Thruster.ThrustOverridePercentage = 0.0f;
	}
	foreach(IMyThrust Thruster in Up_Thrusters){
		Thruster.ThrustOverridePercentage = 0.0f;
	}
	foreach(IMyThrust Thruster in Down_Thrusters){
		Thruster.ThrustOverridePercentage = 0.0f;
	}
	foreach(IMyThrust Thruster in Left_Thrusters){
		Thruster.ThrustOverridePercentage = 0.0f;
	}
	foreach(IMyThrust Thruster in Right_Thrusters){
		Thruster.ThrustOverridePercentage = 0.0f;
	}
}

private void SetDirections(){
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
			break;
		case Base6Directions.Direction.Backward:
			Controller_Forward = Backward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Forward = Up_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Forward = Down_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Forward = Left_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Forward = Right_Vector;
			break;
	}
	switch(Backward){
		case Base6Directions.Direction.Forward:
			Controller_Backward = Forward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Backward = Backward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Backward = Up_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Backward = Down_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Backward = Left_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Backward = Right_Vector;
			break;
	}
	switch(Up){
		case Base6Directions.Direction.Forward:
			Controller_Up = Forward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Up = Backward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Up = Up_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Up = Down_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Up = Left_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Up = Right_Vector;
			break;
	}
	switch(Down){
		case Base6Directions.Direction.Forward:
			Controller_Down = Forward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Down = Backward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Down = Up_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Down = Down_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Down = Left_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Down = Right_Vector;
			break;
	}
	switch(Left){
		case Base6Directions.Direction.Forward:
			Controller_Left = Forward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Left = Backward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Left = Up_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Left = Down_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Left = Left_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Left = Right_Vector;
			break;
	}
	switch(Right){
		case Base6Directions.Direction.Forward:
			Controller_Right = Forward_Vector;
			break;
		case Base6Directions.Direction.Backward:
			Controller_Right = Backward_Vector;
			break;
		case Base6Directions.Direction.Up:
			Controller_Right = Up_Vector;
			break;
		case Base6Directions.Direction.Down:
			Controller_Right = Down_Vector;
			break;
		case Base6Directions.Direction.Left:
			Controller_Right = Left_Vector;
			break;
		case Base6Directions.Direction.Right:
			Controller_Right = Right_Vector;
			break;
	}
	
}

private int Glitch_Counter = 0;
private int Glitch = 0;
private long Target_Glitch = -1;
private float GlitchFloat{
	get{
		if(Glitch > 1){
			if((Glitch+Glitch_Counter)%2 == 0){
				Glitch_Counter += 2;
			}
			else if((Glitch+Glitch_Counter)%3 == 0){
				Glitch_Counter += 3;
			}
			else if((Glitch+Glitch_Counter)%5 == 0){
				Glitch_Counter += 5;
			}
			else {
				Glitch_Counter++;
			}
			return (((Glitch * Glitch_Counter * Glitch_Counter) + Glitch_Counter) % 100) / 100.0f;
		}
		else {
			return 0.0f;
		}
	}
}


private void SetTarget(){
	target_direction = actual_target_direction;
	target_position = actual_target_position;
	target_velocity = actual_target_velocity;
	if(Controller.TryGetPlanetElevation(MyPlanetElevation.Sealevel, out elevation)){
		Controller.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation);
		Vector3D center = new Vector3D(0,0,0);
		if(Controller.TryGetPlanetPosition(out center)){
			Vector3D target_angle = actual_target_position - center;
			Vector3D current_angle = Me.CubeGrid.GetPosition() - center;
			target_angle.Normalize();
			current_angle.Normalize();
			double actual_target_distance = (actual_target_position - Me.CubeGrid.GetPosition()).Length();
			double elevation_difference = (Me.CubeGrid.GetPosition() - center).Length() - elevation;
			double difference = GetAngle(current_angle, target_angle);
			double angle_difference = Math.Min(Math.Max(1, 2500 / (elevation_difference * 2 * Math.PI) * 360), 15);
			Echo("angle_difference: " + Math.Round(angle_difference, 2) .ToString() + '°');
			if(actual_target_distance > 2500 || difference > (angle_difference * 1.5)){
				target_angle = ((angle_difference * target_angle) + (difference - angle_difference) * current_angle) / difference;
				target_angle.Normalize();
				double target_elevation = Math.Max(Math.Min(elevation + 10, 500) * (1-GlitchFloat), 100);
				target_position = target_angle * (elevation_difference + target_elevation);
				double target_speed = target_velocity.Length();
				target_velocity = ((angle_difference * target_velocity) + (difference - angle_difference) * Speed_Limit) / difference;
				target_direction = target_position - Me.CubeGrid.GetPosition();
				target_direction.Normalize();
			}
		}
	}
}

public double GetAngle(Vector3D v1, Vector3D v2){
	v1.Normalize();
	v2.Normalize();
	return Math.Round(Math.Acos(v1.X*v2.X + v1.Y*v2.Y + v1.Z*v2.Z) * 57.295755, 5);
}

private double Pitch_Time = WAIT_DURATION;
private double Yaw_Time = WAIT_DURATION;
private double Roll_Time = WAIT_DURATION;

private double Scan_Time = SCAN_FREQUENCY;

private string LastError = "";

private void ArgumentProcessor(string argument, UpdateType updateSource){
	try{
		if(argument.IndexOf("GoTo") == 0){
			MyWaypointInfo Waypoint;
			string[] args = argument.Substring("GoTo".Length + 1).Trim().Split('•');
			if(MyWaypointInfo.TryParse(args[0], out Waypoint)){
				actual_target_position = Waypoint.Coords;
				match_position = true;
				match_direction = true;
				Controller.DampenersOverride = true;
			}
			else{
				if(args[0].Length > 10 && MyWaypointInfo.TryParse(args[0].Substring(0, args[0].Length-10), out Waypoint)){
					actual_target_position = Waypoint.Coords;
					match_position = true;
					match_direction = true;
					Controller.DampenersOverride = true;
				}
				else {
					if(Vector3D.TryParse(args[0], out actual_target_position)){
						match_position = true;
						match_direction = true;
						Controller.DampenersOverride = true;
					}
					else{
						LastError = "Failed to parse \"" + args[0] + "\"";
						match_position = false;
						match_direction = false;
					}
				}
			}
			if(args.Count() > 1){
				update_position = Vector3D.TryParse(args[1], out actual_target_velocity);
			}
			else {
				update_position = false;
				actual_target_velocity = new Vector3D(0,0,0);
			}
		}
		else if(argument.ToLower().Equals("glitch")){
			Glitch = Rnd.Next(1, 100);
			Target_Glitch = (cycle + ((long)Math.Pow(10, Rnd.Next(200, 3699))))%long.MaxValue;
			Echo("Glitching");
		}
	}
	catch(Exception e){
		Echo(e.Message);
		LastError = e.Message;
	}
}

private double AsteroidDistance = double.MaxValue;
private double ClosestAsteroidSize = 0;
private double PlanetDistance = double.MaxValue;
private double ShipDistance = double.MaxValue;
private double ClosestShipSize = 0;
private double CharacterDistance = double.MaxValue;

private bool HasBlockData(IMyTerminalBlock Block, string Name){
	if(Name.Contains(':'))
		return false;
	string[] args = Block.CustomData.Split('•');
	foreach(string argument in args){
		if(argument.IndexOf(Name + ':') == 0){
			return true;
		}
	}
	return false;
}

private string GetBlockData(IMyTerminalBlock Block, string Name){
	if(Name.Contains(':'))
		return "";
	string[] args = Block.CustomData.Split('•');
	foreach(string argument in args){
		if(argument.IndexOf(Name + ':') == 0){
			return argument.Substring((Name + ':').Length);
		}
	}
	return "";
}

private bool SetBlockData(IMyTerminalBlock Block, string Name, string Data){
	if(Name.Contains(':'))
		return false;
	string[] args = Block.CustomData.Split('•');
	for(int i=0; i<args.Count(); i++){
		if(args[i].IndexOf(Name + ':') == 0){
			Block.CustomData = Name + ':' + Data;
			for(int j=0; j<args.Count(); j++){
				if(j!=i){
					Block.CustomData += '•' + args[j];
				}
			}
			return true;
		}
	}
	Block.CustomData += '•' + Name + ':' + Data;
	return true;
}

private void UpdateClosestDistance(){
	AsteroidDistance = double.MaxValue;
	PlanetDistance = double.MaxValue;
	ShipDistance = double.MaxValue;
	CharacterDistance = double.MaxValue;
	foreach(EntityInfo Entity in AsteroidList){
		double distance = (Entity.Position - Me.CubeGrid.GetPosition()).Length() - Entity.Size - MySize;
		if(distance < AsteroidDistance){
			AsteroidDistance = distance;
			ClosestAsteroidSize = Entity.Size;
		}
	}
	if(Controller.TryGetPlanetElevation(MyPlanetElevation.Sealevel, out elevation)){
		Controller.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation);
		PlanetDistance = Math.Min(PlanetDistance, elevation - MySize);
	}
	foreach(EntityInfo Entity in SmallShipList){
		double adjusted_distance = (Entity.Position - Me.CubeGrid.GetPosition()).Length() - Entity.Size - MySize;
		adjusted_distance *= Math.Max(Math.Log10(Entity.Age.TotalSeconds), 1);
		if(adjusted_distance < ShipDistance){
			ShipDistance = adjusted_distance;
			ClosestShipSize = Entity.Size;
		}
	}
	foreach(EntityInfo Entity in LargeShipList){
		double adjusted_distance = (Entity.Position - Me.CubeGrid.GetPosition()).Length() - Entity.Size - MySize;
		adjusted_distance *= Math.Max(Math.Log10(Entity.Age.TotalSeconds), 1);
		ShipDistance = Math.Min(ShipDistance, adjusted_distance);
	}
	
	foreach(EntityInfo Entity in CharacterList){
		CharacterDistance = Math.Min(CharacterDistance, (Entity.Position - Me.CubeGrid.GetPosition()).Length() - MySize);
	}
	
}

private string ScanString = "";

private double elevation = double.MaxValue;

private void SetThrusters(){
	
	Forward_Thrust = 0.0f;
	foreach(IMyThrust Thruster in Forward_Thrusters){
		if(Thruster.IsWorking)
			Forward_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Backward_Thrust = 0.0f;
	foreach(IMyThrust Thruster in Backward_Thrusters){
		if(Thruster.IsWorking)
			Backward_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Up_Thrust = 0.0f;
	foreach(IMyThrust Thruster in Up_Thrusters){
		if(Thruster.IsWorking)
			Up_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Down_Thrust = 0.0f;
	foreach(IMyThrust Thruster in Down_Thrusters){
		if(Thruster.IsWorking)
			Down_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Left_Thrust = 0.0f;
	foreach(IMyThrust Thruster in Left_Thrusters){
		if(Thruster.IsWorking)
			Left_Thrust+=Thruster.MaxEffectiveThrust;
	}
	Right_Thrust = 0.0f;
	foreach(IMyThrust Thruster in Right_Thrusters){
		if(Thruster.IsWorking)
			Right_Thrust+=Thruster.MaxEffectiveThrust;
	}
	
	float input_forward = 0.0f;
	float input_up = 0.0f;
	float input_right = 0.0f;
	
	
	float damp_multx = 1.0f - GlitchFloat;
	elevation = double.MaxValue;
	double effective_speed_limit = Speed_Limit;
	if(Controller.TryGetPlanetElevation(MyPlanetElevation.Sealevel, out elevation)){
		if(elevation < 2000){
			if(Controller.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation)){
				if(elevation < 100){
					damp_multx = (float) (1.0f + (100 - elevation)/50);
					effective_speed_limit = Math.Min(effective_speed_limit, elevation);
				}
			}
		}
	}
	UpdateClosestDistance();
	if(Controller.DampenersOverride){
		Me.GetSurface(0).WriteText("Dampeners: On\n", true);
		input_right -= (float) (Relative_Velocity.X * Mass_Accomodation * damp_multx);
		input_up -= (float) (Relative_Velocity.Y * Mass_Accomodation * damp_multx);
		input_forward += (float) (Relative_Velocity.Z * Mass_Accomodation * damp_multx);
	}
	else {
		Me.GetSurface(0).WriteText("Dampeners: Off\n", true);
	}
	
	double Target_Distance = (actual_target_position - Controller.GetPosition()).Length();
	Echo("Full Target Distance: " + Math.Round(Target_Distance, 1).ToString());
	Echo("Partial Target Distance: " + Math.Round((target_position - Me.CubeGrid.GetPosition()).Length(), 1).ToString());
	if(Target_Distance <= 0.5f){
		match_direction = false;
		match_position = false;
	}
	effective_speed_limit = Math.Min(effective_speed_limit, AsteroidDistance / 4);
	effective_speed_limit = Math.Min(effective_speed_limit, PlanetDistance);
	effective_speed_limit = Math.Min(effective_speed_limit, ShipDistance / 2);
	if(match_position){
		effective_speed_limit = Math.Min(effective_speed_limit, ((100-Controller.GetShipSpeed()) + Math.Sqrt(Target_Distance * 10)) / 2);
	}
	effective_speed_limit = Math.Max(effective_speed_limit, 5) * (1 + GlitchFloat);
	
	Echo("Effective Speed Limit: " + Math.Round(effective_speed_limit, 1).ToString());
	
	if(Gravity.Length() > 0){
		if(Adjusted_Gravity.X!=0){
			input_right -= (float) Adjusted_Gravity.X;
		}
		if(Adjusted_Gravity.Y!=0){
			input_up -= (float) Adjusted_Gravity.Y;
		}
		if(Adjusted_Gravity.Z!=0){
			input_forward += (float) Adjusted_Gravity.Z;
		}
	}
	
	float speed_multx = 1.0f;
	if(DO_SPEED_CAP)
		speed_multx = Math.Max(Math.Min(speed_multx * SPEED_CAP, 1), 0);
	
	Vector3D Relative_Target_Position = Vector3D.Transform(target_position, MatrixD.Invert(Controller.WorldMatrix));
	if(match_position){
		Me.GetSurface(0).WriteText("Relative Position:\n", true);
		if(Relative_Target_Position.X > 0){
			Me.GetSurface(0).WriteText(" R:" + Math.Round(Math.Abs(Relative_Target_Position.X), 1).ToString(), true);
		}
		else if(Relative_Target_Position.X < 0){
			Me.GetSurface(0).WriteText(" L:" + Math.Round(Math.Abs(Relative_Target_Position.X), 1).ToString(), true);
		}
		if(Relative_Target_Position.Y > 0){
			Me.GetSurface(0).WriteText(" U:" + Math.Round(Math.Abs(Relative_Target_Position.Y), 1).ToString(), true);
		}
		else if(Relative_Target_Position.Y < 0){
			Me.GetSurface(0).WriteText(" D:" + Math.Round(Math.Abs(Relative_Target_Position.Y), 1).ToString(), true);
		}
		if(Relative_Target_Position.Z > 0){
			Me.GetSurface(0).WriteText(" B:" + Math.Round(Math.Abs(Relative_Target_Position.Z), 1).ToString(), true);
		}
		else if(Relative_Target_Position.Z < 0){
			Me.GetSurface(0).WriteText(" F:" + Math.Round(Math.Abs(Relative_Target_Position.Z), 1).ToString(), true);
		}
		Me.GetSurface(0).WriteText("\n", true);
	}
	Vector3D Relative_Target_Velocity = Vector3D.Transform(target_velocity - Controller.GetShipVelocities().LinearVelocity + Controller.GetPosition(), MatrixD.Invert(Controller.WorldMatrix));
	if(match_position){
		Echo("Relative_Target_Position: " + Relative_Target_Position.ToString());
		Echo("Relative_Target_Velocity: " + Relative_Target_Velocity.ToString());
	}
	Vector3D Relative_Current_Velocity = Vector3D.Transform(Controller.GetShipVelocities().LinearVelocity, MatrixD.Invert(Controller.WorldMatrix));
	Relative_Current_Velocity.Normalize();
	Relative_Current_Velocity *= Controller.GetShipSpeed();
	Echo("Relative_Current_Velocity: " + Relative_Current_Velocity.ToString());
	
	bool matched_direction = !match_direction;
	if(match_direction){
		if(Gravity.Length() > 0){
			matched_direction = Math.Abs(GetAngle(target_direction, Controller_Left) - GetAngle(target_direction, Controller_Right)) <= ACCEPTABLE_ANGLE;
			Vector3D gravity_direction = Gravity;
			gravity_direction.Normalize();
			double difference = GetAngle(target_direction, gravity_direction);
			if(Math.Abs(difference - 90) > ACCEPTABLE_ANGLE){
				matched_direction = matched_direction && (GetAngle(target_direction, Controller_Forward) < GetAngle(target_direction, Controller_Backward));
				if(difference > 90 + ACCEPTABLE_ANGLE){
					matched_direction = matched_direction && (GetAngle(gravity_direction, Controller_Forward) >= 90 + (ACCEPTABLE_ANGLE - 5));
				}
				else if(difference < 90 - ACCEPTABLE_ANGLE){
					matched_direction = matched_direction && (GetAngle(gravity_direction, Controller_Forward) <= 90 - (ACCEPTABLE_ANGLE - 5));
				}
			}
			else{
				matched_direction = matched_direction && (GetAngle(Controller_Forward, target_direction) <= ACCEPTABLE_ANGLE);
			}
			matched_direction = matched_direction || (GlitchFloat > 0.5f);
		}
		else {
			matched_direction = (GetAngle(Controller_Forward, target_direction) <= ACCEPTABLE_ANGLE);
		}
	}
	
	if(Math.Abs(Controller.MoveIndicator.X)>0.5f){
		if(Controller.MoveIndicator.X > 0){
			if((Controller.GetShipVelocities().LinearVelocity + Controller_Right).Length() <= effective_speed_limit)
				input_right = speed_multx * Right_Thrust;
			else
				input_right = Math.Min(input_right, 0);
		} else {
			if((Controller.GetShipVelocities().LinearVelocity + Controller_Left).Length() <= effective_speed_limit)
				input_right = -1 * speed_multx * Left_Thrust;
			else
				input_right = Math.Max(input_right, 0);
		}
	}
	else if(match_position){
		double Relative_Speed = Relative_Velocity.X;
		double Relative_Target_Speed = Relative_Target_Velocity.X;
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
			if(time > 0 && (!match_direction || matched_direction)){
				if(difference > 0){
					if((Controller.GetShipVelocities().LinearVelocity + Controller_Left).Length() <= Math.Min(elevation, Math.Min(effective_speed_limit, Target_Distance)) && Math.Abs(Relative_Current_Velocity.X) <= Math.Abs(Relative_Target_Position.X) + 0.05)
						input_right = -1 * speed_multx * Left_Thrust;
					else
						Echo("Ignoring Left Autopilot");
				}
				else {
					if((Controller.GetShipVelocities().LinearVelocity + Controller_Right).Length() <= Math.Min(elevation, Math.Min(effective_speed_limit, Target_Distance)) && Math.Abs(Relative_Current_Velocity.X) <= Math.Abs(Relative_Target_Position.X) + 0.05)
						input_right = speed_multx * Right_Thrust;
					else
						Echo("Ignoring Right Autopilot");
				}
			}
		}
	}
	if(Math.Abs(Controller.MoveIndicator.Y)>0.5f){
		if(Controller.MoveIndicator.Y > 0){
			if((Controller.GetShipVelocities().LinearVelocity + Controller_Up).Length() <= effective_speed_limit)
				input_up = speed_multx * Up_Thrust;
			else
				input_up = Math.Min(input_up, 0);
		} else {
			if((Controller.GetShipVelocities().LinearVelocity + Controller_Down).Length() <= effective_speed_limit)
				input_up = -1 * speed_multx * Down_Thrust;
			else
				input_up = Math.Max(input_up, 0);
		}
	}
	else if(match_position){
		double Relative_Speed = Relative_Velocity.Y;
		double Relative_Target_Speed = Relative_Target_Velocity.Y;
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
			if(time > 0 && (!match_direction || matched_direction)){
				if(difference > 0){
					if((Controller.GetShipVelocities().LinearVelocity + Controller_Down).Length() <= Math.Min(elevation, Math.Min(effective_speed_limit, Math.Sqrt(Target_Distance*10))) || Math.Abs(Relative_Current_Velocity.Y) < Math.Abs(Relative_Target_Position.Y))
						input_up = -1 * speed_multx * Down_Thrust;
					else
						Echo("Ignoring Down Autopilot");
				}
				else {
					if((Controller.GetShipVelocities().LinearVelocity + Controller_Up).Length() <= Math.Min(elevation, Math.Min(effective_speed_limit, Math.Sqrt(Target_Distance*10))) || Math.Abs(Relative_Current_Velocity.Y) < Math.Abs(Relative_Target_Position.Y))
						input_up = speed_multx * Up_Thrust;
					else
						Echo("Ignoring Up Autopilot");
				}
			}
		}
	}
	if(Math.Abs(Controller.MoveIndicator.Z)>0.5f){
		if(Controller.MoveIndicator.Z < 0){
			if((Controller.GetShipVelocities().LinearVelocity + Controller_Forward).Length() <= effective_speed_limit)
				input_forward = speed_multx * Forward_Thrust;
			else
				input_forward = Math.Min(input_forward, 0);
		} else {
			if((Controller.GetShipVelocities().LinearVelocity + Controller_Backward).Length() <= effective_speed_limit)
				input_forward = -1 * speed_multx * Backward_Thrust;
			else
				input_forward = Math.Max(input_forward, 0);
		}
	}
	else if(match_position){
		double Relative_Speed = Relative_Velocity.Z;
		double Relative_Target_Speed = Relative_Target_Velocity.Z;
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
			if(time > 0 && (!match_direction || matched_direction)){
				if(difference > 0){
					if((Controller.GetShipVelocities().LinearVelocity + Controller_Backward).Length() <= Math.Min(elevation, Math.Min(effective_speed_limit, Math.Sqrt(Target_Distance*10))) && Math.Abs(Relative_Current_Velocity.Z) <= Math.Abs(Relative_Target_Position.Z) + 0.05)
						input_forward = -1 * speed_multx * Backward_Thrust;
					else
						Echo("Ignoring Backward Autopilot");
				}
				else {
					if((Controller.GetShipVelocities().LinearVelocity + Controller_Forward).Length() <= Math.Min(elevation, Math.Min(effective_speed_limit, Math.Sqrt(Target_Distance*10))) && Math.Abs(Relative_Current_Velocity.Z) <= Math.Abs(Relative_Target_Position.Z) + 0.05)
						input_forward = speed_multx * Forward_Thrust;
					else
						Echo("Ignoring Forward Autopilot");
				}
			}
		}
	}
	float output_forward = 0.0f;
	float output_backward = 0.0f;
	if(input_forward / Forward_Thrust > 0.05f){
		output_forward = Math.Min(Math.Abs(input_forward / Forward_Thrust), 1);
		Me.GetSurface(0).WriteText("Forward: " + Math.Round(output_forward*100, 1).ToString() + '%' + '\n', true);
	}
	else if(input_forward / Backward_Thrust < -0.05f){
		output_backward = Math.Min(Math.Abs(input_forward / Backward_Thrust), 1);
		Me.GetSurface(0).WriteText("Backward: " + Math.Round(output_backward*100, 1).ToString() + '%' + '\n', true);
	}
	float output_up = 0.0f;
	float output_down = 0.0f;
	if(input_up / Up_Thrust > 0.05f){
		output_up = Math.Min(Math.Abs(input_up / Up_Thrust), 1);
		Me.GetSurface(0).WriteText("Up: " + Math.Round(output_up*100, 1).ToString() + '%' + '\n', true);
	}
	else if(input_up / Down_Thrust < -0.05f){
		output_down = Math.Min(Math.Abs(input_up / Down_Thrust), 1);
		Me.GetSurface(0).WriteText("Down: " + Math.Round(output_down*100, 1).ToString() + '%' + '\n', true);
	}
	float output_right = 0.0f;
	float output_left = 0.0f;
	if(input_right / Right_Thrust > 0.05f){
		output_right = Math.Min(Math.Abs(input_right / Right_Thrust), 1);
		Me.GetSurface(0).WriteText("Right: " + Math.Round(output_right*100, 1).ToString() + '%' + '\n', true);
	}
	else if(input_right / Left_Thrust < -0.05f){
		output_left = Math.Min(Math.Abs(input_right / Left_Thrust), 1);
		Me.GetSurface(0).WriteText("Left: " + Math.Round(output_left*100, 1).ToString() + '%' + '\n', true);
	}
	Echo("\nOutput:");
	Echo("F: " + Math.Round(output_forward, 3).ToString());
	Echo("B: " + Math.Round(output_backward, 3).ToString());
	Echo("U: " + Math.Round(output_up, 3).ToString());
	Echo("D: " + Math.Round(output_down, 3).ToString());
	Echo("R: " + Math.Round(output_right, 3).ToString());
	Echo("L: " + Math.Round(output_left, 3).ToString());
	Echo("");
	float g_float = GlitchFloat - GlitchFloat;
	foreach(IMyThrust Thruster in Forward_Thrusters){
		Thruster.ThrustOverridePercentage = output_forward * (1+g_float);
		if(GlitchFloat > 0.95f)
			Thruster.ThrustOverridePercentage = 1.0f;
	}
	foreach(IMyThrust Thruster in Backward_Thrusters){
		Thruster.ThrustOverridePercentage = output_backward * (1-g_float);
		if(GlitchFloat > 0.95f)
			Thruster.ThrustOverridePercentage = 1.0f;
	}
	g_float = GlitchFloat - GlitchFloat;
	foreach(IMyThrust Thruster in Up_Thrusters){
		Thruster.ThrustOverridePercentage = output_up * (1+g_float);
		if(GlitchFloat > 0.95f)
			Thruster.ThrustOverridePercentage = 1.0f;
	}
	foreach(IMyThrust Thruster in Down_Thrusters){
		Thruster.ThrustOverridePercentage = output_down * (1-g_float);
		if(GlitchFloat > 0.95f)
			Thruster.ThrustOverridePercentage = 1.0f;
	}
	g_float = GlitchFloat - GlitchFloat;
	foreach(IMyThrust Thruster in Right_Thrusters){
		Thruster.ThrustOverridePercentage = output_right * (1+g_float);
		if(GlitchFloat > 0.95f)
			Thruster.ThrustOverridePercentage = 1.0f;
	}
	foreach(IMyThrust Thruster in Left_Thrusters){
		Thruster.ThrustOverridePercentage = output_left * (1+g_float);
		if(GlitchFloat > 0.95f)
			Thruster.ThrustOverridePercentage = 1.0f;
	}
	
}

private Vector3D AngularVelocity = new Vector3D(0,0,0);

private void SetGyroscopes(){
	AngularVelocity = Vector3D.TransformNormal(Controller.GetShipVelocities().AngularVelocity, MatrixD.Transpose(Controller.WorldMatrix)) * (1 - GlitchFloat);
	
	float current_pitch = (float) AngularVelocity.X;
	float current_yaw = (float) AngularVelocity.Y;
	float current_roll = (float) AngularVelocity.Z;
	
	Echo("Current Relative Angular Velocity:");
	Echo("Pitch: " + Math.Round(current_pitch, 3).ToString());
	Echo("Yaw: " + Math.Round(current_yaw, 3).ToString());
	Echo("Roll: " + Math.Round(current_roll, 3).ToString());
	Echo("");
	
	bool launching = false;
	if(Gravity.Length() > 0 && Forward_Thrust > 1.2 * Up_Thrust){
		if(GetAngle(Controller_Backward, Gravity) < 90 && GetAngle(Controller_Down, Gravity) < GetAngle(Controller_Up, Gravity)){
			launching = true;
		}
	}
	
	float input_pitch = 0;
	float input_yaw = 0;
	float input_roll = 0;
	
	if(Gravity.Length() > 0){
		Echo("Gravity Angles:");
		Echo("Forward: " + Math.Round(GetAngle(Forward_Vector, Gravity), 2) + "°");
		Echo("Backward: " + Math.Round(GetAngle(Backward_Vector, Gravity), 2) + "°");
		Echo("Up: " + Math.Round(GetAngle(Up_Vector, Gravity), 2) + "°");
		Echo("Down: " + Math.Round(GetAngle(Down_Vector, Gravity), 2) + "°");
		Echo("Left: " + Math.Round(GetAngle(Left_Vector, Gravity), 2) + "°");
		Echo("Right: " + Math.Round(GetAngle(Right_Vector, Gravity), 2) + "°");
		Echo("");
	}
	
	bool adjusting_target = false;
	
	input_pitch = Math.Min(Math.Max(Controller.RotationIndicator.X / 200, -1), 1);
	if(Math.Abs(input_pitch) < 0.1f){
		input_pitch = current_pitch * -1 * ROTATIONAL_DAMPENER_MULTX;
		if(Gravity.Length() > 0 && !launching && Pitch_Time >= WAIT_DURATION && GRAVITY_PITCH_ADJUST){
			double difference = (GetAngle(Backward_Vector, Gravity) - GetAngle(Forward_Vector, Gravity)) + GlitchFloat - GlitchFloat;
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE){
				if(AngularVelocity.Length() < 1){
					if(difference>0){
						input_pitch += GRAV_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
					else {
						input_pitch -= GRAV_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
				}
			}
		}
		if(elevation<Controller.GetShipSpeed()*2 && elevation<50 && GetAngle(Gravity, Controller_Down) < 90){
			double difference = Math.Abs(GetAngle(Gravity, Controller_Forward));
			if(difference < 90){
				input_pitch -= ((float)Math.Min(Math.Abs((90-difference)/90), 1));
			}
		}
		if(match_direction){
			double difference = GetAngle(Controller_Down, target_direction) - GetAngle(Controller_Up, target_direction) + GlitchFloat - GlitchFloat;
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE){
				adjusting_target = true;
				if(AngularVelocity.Length() < 1){
					if(difference>0){
						input_pitch -= TARGET_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
					else {
						input_pitch += TARGET_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
				}
			}
		}
	}
	else{
		Pitch_Time = 0;
		input_pitch *= 30;
	}
	input_yaw = Math.Min(Math.Max(Controller.RotationIndicator.Y / 200, -1), 1);
	if(Math.Abs(input_yaw) < 0.1f){
		input_yaw = current_yaw * -1 * ROTATIONAL_DAMPENER_MULTX;
		if(match_direction){
			double difference = GetAngle(Controller_Right, target_direction) - GetAngle(Controller_Left, target_direction) + GlitchFloat - GlitchFloat;
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE || GetAngle(Controller_Forward, target_direction) > ACCEPTABLE_ANGLE){
				adjusting_target = true;
				if(AngularVelocity.Length() < 1){
					if(difference>0 || difference==0 && GetAngle(Controller_Forward, target_direction) > ACCEPTABLE_ANGLE){
						input_yaw -= TARGET_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
					else {
						input_yaw += TARGET_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
				}
			}
		}
	}
	else{
		Yaw_Time = 0;
		input_yaw *= 30;
	}
	input_roll = Controller.RollIndicator;
	if(Math.Abs(input_roll) < 0.1f){
		input_roll = current_roll * -1 * ROTATIONAL_DAMPENER_MULTX;
		if(Gravity.Length() > 0  && Roll_Time >= WAIT_DURATION && !adjusting_target){
			double difference = (GetAngle(Left_Vector, Gravity) - GetAngle(Right_Vector, Gravity)) + GlitchFloat - GlitchFloat;
			if(Math.Abs(difference) > ACCEPTABLE_ANGLE){
				if(AngularVelocity.Length() < 1){
					if(difference>0){
						input_roll += GRAV_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
					else {
						input_roll -= GRAV_ADJUST_MULTX * ((float)Math.Min(Math.Abs(difference/(ACCEPTABLE_ANGLE*2)), 1)) * (1 + GlitchFloat);
					}
				}
			}
		}
	}
	else{
		Roll_Time = 0;
		input_roll *= 20;
	}
	
	Gyro_Tuple output = Transform(new Gyro_Tuple(input_pitch, input_yaw, input_roll));
	
	Gyroscope.Pitch = output.Pitch + GlitchFloat / 100;
	Gyroscope.Yaw = output.Yaw + GlitchFloat / 100;
	Gyroscope.Roll = output.Roll + GlitchFloat / 100;
	
	if(Math.Abs(Gyroscope.Pitch) > 0.1f)
		Me.GetSurface(0).WriteText("Pitch: " + Math.Round(Gyroscope.Pitch*100, 3).ToString() + " RPM" + '\n', true);
	if(Math.Abs(Gyroscope.Yaw) > 0.1f)
		Me.GetSurface(0).WriteText("Yaw: " + Math.Round(Gyroscope.Yaw*100, 3).ToString() + " RPM" + '\n', true);
	if(Math.Abs(Gyroscope.Roll) > 0.1f)
		Me.GetSurface(0).WriteText("Roll: " + Math.Round(Gyroscope.Roll*100, 3).ToString() + " RPM" + '\n', true);
}

private Vector3D Gravity = new Vector3D(0,0,0);
private Vector3D Adjusted_Gravity = new Vector3D(0,0,0);
private float Mass_Accomodation = 0.0f;
private Vector3D Relative_Velocity = new Vector3D(0,0,0);

public void Main(string argument, UpdateType updateSource)
{
	Glitch_Counter = 0;
	long last_long = cycle_long;
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
	Echo(Program_Name + " OS " + cycle_long.ToString() + '-' + cycle.ToString() + " (" + loading_char + ")\n");
	seconds_since_last_update = Runtime.TimeSinceLastRun.TotalSeconds + (Runtime.LastRunTimeMs / 1000);
	Me.GetSurface(0).WriteText(Program_Name + " OS " + cycle_long.ToString() + '-' + cycle.ToString() + " (" + loading_char + ")\n" + Controller.CustomName + "\n" + Gyroscope.CustomName + "\n\n", false);
	if(Glitch > 0){
		Me.GetSurface(0).BackgroundColor = new Color((int)(150 * GlitchFloat), (int)(75 * GlitchFloat), 0, 255);
		if(Glitch<10){
			Echo("ERROR: Glitch code " + Glitch.ToString() + "00");
			Me.GetSurface(0).WriteText("ERROR: Glitch code " + Glitch.ToString() + "00" + '\n', true);
		}
		else{
			Echo("ERROR: Glitch code " + Glitch.ToString().Substring(0,1) + "0" + Glitch.ToString().Substring(1,1));
			Me.GetSurface(0).WriteText("ERROR: Glitch code " + Glitch.ToString().Substring(0,1) + "0" + Glitch.ToString().Substring(1,1) + '\n', true);
		}
		
		Echo("Potential next glitch in " + (Target_Glitch - cycle + Rnd.Next(0,50) - 25).ToString() + " cycles");
		Me.GetSurface(0).WriteText("Potential next glitch in " + (Target_Glitch - cycle + Rnd.Next(0,50) - 25).ToString() + " cycles" + '\n', true);
	}
	else {
		Me.GetSurface(0).BackgroundColor = new Color(0, 88, 151, 255);
		Glitch = 0;
	}
	if(Glitch > 0 && cycle >= Target_Glitch){
		Target_Glitch = (cycle + ((long)Math.Pow(10, Rnd.Next(200, 369)/100.0)))%long.MaxValue;
		Glitch = Rnd.Next(1, 100);
	}
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
    if(last_long != cycle_long){
		Runtime.UpdateFrequency = UpdateFrequency.None;
		Echo("Performing regular Reinitialization...");
		Controller = null;
		Gyroscope = null;
		Setup();
	}
	if(Controller == null || Gyroscope == null || !Controller.IsFunctional || !Gyroscope.IsFunctional){
		Runtime.UpdateFrequency = UpdateFrequency.None;
		if(Gyroscope!=null){
			Gyroscope.GyroOverride = false;
		}
		Echo("Null components detected");
		Controller = null;
		Gyroscope = null;
		Setup();
		return;
	}
	if(Pitch_Time < WAIT_DURATION){
		Pitch_Time += seconds_since_last_update;
	}
	if(Yaw_Time < WAIT_DURATION){
		Yaw_Time += seconds_since_last_update;
	}
	if(Roll_Time < WAIT_DURATION){
		Roll_Time += seconds_since_last_update;
	}
	if(Scan_Time < SCAN_FREQUENCY){
		Scan_Time += seconds_since_last_update;
	}
	ArgumentProcessor(argument, updateSource);
	SetDirections();
	SetTarget();
	Gyroscope.GyroOverride = true;
	
	if(LastError.Length>0)
		Echo("LastError:\n" + LastError + '\n');
	
	HasNearestPlanet = Controller.TryGetPlanetPosition(out NearestPlanet);
	
	if(Scan_Time >= SCAN_FREQUENCY){
		Echo("Running scan...");
		ScanString = "";
		foreach(EntityInfo Entity in AsteroidList){
			Entity.Update(Scan_Time);
		}
		foreach(EntityInfo Entity in PlanetList){
			Entity.Update(Scan_Time);
		}
		foreach(EntityInfo Entity in SmallShipList){
			Entity.Update(Scan_Time);
		}
		foreach(EntityInfo Entity in LargeShipList){
			Entity.Update(Scan_Time);
		}
		foreach(EntityInfo Entity in CharacterList){
			Entity.Update(Scan_Time);
		}
		List<MyDetectedEntityInfo> Entities = new List<MyDetectedEntityInfo>();
		List<IMySensorBlock> AllSensors = new List<IMySensorBlock>();
		List<IMyLargeTurretBase> AllTurrets = new List<IMyLargeTurretBase>();
		GridTerminalSystem.GetBlocksOfType<IMySensorBlock>(AllSensors);
		GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(AllTurrets);
		foreach(IMySensorBlock Sensor in AllSensors){
			MyDetectedEntityInfo LastEntity = Sensor.LastDetectedEntity;
			if(LastEntity.Type != MyDetectedEntityType.None){
				bool found = false;
				for(int i=0; i<Entities.Count; i++){
					if(Entities[i].EntityId == LastEntity.EntityId){
						found = true;
						if(LastEntity.TimeStamp > Entities[i].TimeStamp)
							Entities[i] = LastEntity;
						break;
					}
				}
				if(!found){
					Entities.Add(LastEntity);
				}
			}
			List<MyDetectedEntityInfo> SomeEntities = new List<MyDetectedEntityInfo>();
			Sensor.DetectedEntities(SomeEntities);
			foreach(MyDetectedEntityInfo Entity in SomeEntities){
				if(Entity.Type != MyDetectedEntityType.None){
					bool found = false;
					for(int i=0; i<Entities.Count; i++){
						if(Entities[i].EntityId == Entity.EntityId){
							found = true;
							if(Entity.TimeStamp > Entities[i].TimeStamp)
								Entities[i] = Entity;
							break;
						}
					}
					if(!found){
						Entities.Add(Entity);
					}
				}
			}
		}
		foreach(IMyLargeTurretBase Turret in AllTurrets){
			MyDetectedEntityInfo Target = Turret.GetTargetedEntity();
			if(Target.Type != MyDetectedEntityType.None){
				bool found = false;
				for(int i=0; i<Entities.Count; i++){
					if(Entities[i].EntityId == Target.EntityId){
						found = true;
						if(Target.TimeStamp > Entities[i].TimeStamp)
							Entities[i] = Target;
						break;
					}
				}
				if(!found){
					Entities.Add(Target);
				}
			}
		}
		
		ScanString += "Retrieved updated data on " + Entities.Count + " relevant entities" + '\n';
		
		foreach(MyDetectedEntityInfo entity in Entities){
			EntityInfo Entity = new EntityInfo(entity);
			bool found = false;
			switch(Entity.Type){
				case MyDetectedEntityType.Asteroid:
					for(int i=0; i<AsteroidList.Count; i++){
						if(AsteroidList[i].ID == Entity.ID){
							AsteroidList[i] = Entity;
							found = true;
							break;
						}
					}
					if(!found){
						AsteroidList.Add(Entity);
					}
					break;
				case MyDetectedEntityType.Planet:
					for(int i=0; i<PlanetList.Count; i++){
						if(PlanetList[i].ID == Entity.ID){
							PlanetList[i] = Entity;
							found = true;
							break;
						}
					}
					if(!found){
						PlanetList.Add(Entity);
					}
					break;
				case MyDetectedEntityType.SmallGrid:
					for(int i=0; i<SmallShipList.Count; i++){
						if(SmallShipList[i].ID == Entity.ID){
							SmallShipList[i] = Entity;
							found = true;
							break;
						}
					}
					if(!found){
						SmallShipList.Add(Entity);
					}
					break;
				case MyDetectedEntityType.LargeGrid:
					for(int i=0; i<LargeShipList.Count; i++){
						if(LargeShipList[i].ID == Entity.ID){
							LargeShipList[i] = Entity;
							found = true;
							break;
						}
					}
					if(!found){
						LargeShipList.Add(Entity);
					}
					break;
				case MyDetectedEntityType.CharacterHuman:
					for(int i=0; i<CharacterList.Count; i++){
						if(CharacterList[i].ID == Entity.ID){
							CharacterList[i] = Entity;
							found = true;
							break;
						}
					}
					if(!found){
						CharacterList.Add(Entity);
					}
					break;
				case MyDetectedEntityType.CharacterOther:
					for(int i=0; i<CharacterList.Count; i++){
						if(CharacterList[i].ID == Entity.ID){
							CharacterList[i] = Entity;
							found = true;
							break;
						}
					}
					if(!found){
						CharacterList.Add(Entity);
					}
					break;
			}
		}
		
		ScanString += "Completed updating data" + '\n';
		Scan_Time = 0;
	}
	else {
		Echo("Last Scan:");
	}
	Echo(ScanString);
	
	
	if(detected_target){
		if(target_info != null){
			target_position = target_info.Position + position_offset;
			target_velocity = target_info.Velocity;
		}
		else{
			detected_target = false;
		}
	}
	
	target_direction = (target_position - Controller.GetPosition());
	target_direction.Normalize();
	
	if(update_position){
		target_position += target_velocity * seconds_since_last_update;
	}
	
	
	if(match_position){
		Echo("AutoPilot: On");
		Me.GetSurface(0).WriteText("AutoPilot: On" + '\n', true);
	}
	else if(match_direction){
		Echo("AutoTarget: On");
		Me.GetSurface(0).WriteText("AutoTarget: On" + '\n', true);
	}
	
	Me.GetSurface(0).WriteText("Speed Limit: " + Math.Round(Speed_Limit, 1).ToString() + '\n', true);
	Echo("Speed Limit: " + Math.Round(Speed_Limit, 1).ToString());
	
	Gravity = Controller.GetNaturalGravity();
	Adjusted_Gravity = Gravity;
	Mass_Accomodation = (float) (Controller.CalculateShipMass().PhysicalMass * 9.81f) * (1 + GlitchFloat);
	
	Relative_Velocity = Vector3D.Transform(Controller.GetShipVelocities().LinearVelocity+Controller.GetPosition(), MatrixD.Invert(Controller.WorldMatrix));
	Relative_Velocity.Normalize();
	Relative_Velocity *= Controller.GetShipSpeed() * (1 + GlitchFloat);
	
	Echo("Current Relative Velocity:");
	Echo("X: " + Math.Round(Relative_Velocity.X, 2).ToString());
	Echo("Y: " + Math.Round(Relative_Velocity.Y, 2).ToString());
	Echo("Z: " + Math.Round(Relative_Velocity.Z, 2).ToString());
	
	if(Gravity.Length() > 0){
		Gravity.Normalize();
		Adjusted_Gravity = Vector3D.Transform(Gravity+Controller.GetPosition(), MatrixD.Invert(Controller.WorldMatrix));
		Adjusted_Gravity.Normalize();
		Adjusted_Gravity *= Mass_Accomodation;
	}
	
	SetThrusters();
	
	if(match_position){
		Me.GetSurface(0).WriteText("Distance to Target: " + Math.Round((Me.CubeGrid.GetPosition() - actual_target_position).Length(), 0) + " meters" + '\n', true);
		Me.GetSurface(0).WriteText("Partial Distance: " + Math.Round((Me.CubeGrid.GetPosition() - target_position).Length(), 0) + " meters" + '\n', true);
	}
	
	SetGyroscopes();
	
	if(Controller.IsUnderControl || AngularVelocity.Length() > .1f){
		Runtime.UpdateFrequency = UpdateFrequency.Update1;
	}
	else{
		Runtime.UpdateFrequency = UpdateFrequency.Update10;
	}
	if(GlitchFloat != 0 && GlitchFloat*Rnd.Next(1,3) > 0.75f){
		string text = Me.GetSurface(0).GetText();
		for(int i=0; i<text.Length; i++){
			if(text[i]!='\n' && text[i]!=' ' && GlitchFloat > 0.5f)
				text = text.Substring(0,i) + (char)(((int)text[i])+1) + text.Substring(i+1);
		}
		Me.GetSurface(0).WriteText(text, false);
	}
}
