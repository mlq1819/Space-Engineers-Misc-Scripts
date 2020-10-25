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

public enum LegStatus{
	Raising = 0,
	Lowering = 1,
	Raised = 2,
	Lowered = 3
}

public enum StrideStatus{
	Rushing = 0,
	Reversing = 1,
	Forward = 2,
	Backward = 3
}

public enum LegState{
	Stopped = 0,
	Pushing = 1,
	Lowering = 2,
	Returning = 3
}

public class Leg{
	private const float MARGIN = 5f;
	private const float RAISE_SPEED = 9.0f;
	private const float ROTATE_SPEED = 15.0f;
	private IMyMotorStator Rotor1;
	private IMyMotorStator Hinge1;
	private IMyMotorStator Hinge2;
	private IMyMotorStator Hinge3;
	private IMyMotorStator Hinge4;
	private IMyMotorStator Rotor2;
	private IMyLandingGear LandingGear;
	public Base6Directions.Direction Side;
	private Base6Directions.Direction Direction = Base6Directions.Direction.Forward;
	protected MyGridProgram Program;
	private float _Speed_Multx = 1.0f;
	public float Speed_Multx{
		get{
			return _Speed_Multx;
		}
		set{
			if(value>=0.1f && value <= 2.0f){
				_Speed_Multx = value;
			}
		}
	}
	
	public string DebugString = "";
	private LegState _State = LegState.Stopped;
	public LegState State{
		get{
			return _State;
		}
	}
	
	private LegStatus _TargetLeg = LegStatus.Lowered;
	public LegStatus TargetLeg{
		get{
			return _TargetLeg;
		}
		set{
			if(value == LegStatus.Lowered || value == LegStatus.Raised)
				_TargetLeg = value;
		}
	}
	private StrideStatus _TargetStride = StrideStatus.Forward;
	public StrideStatus TargetStride{
		get{
			return _TargetStride;
		}
		set{
			if(value == StrideStatus.Forward || value == StrideStatus.Backward)
				_TargetStride = value;
		}
	}
	
	public float FromTarget{
		get{
			if((TargetStride == StrideStatus.Forward) ^ (Direction == Base6Directions.Direction.Backward)){
				return StridePercent;
			}
			else{
				return 100.0f - StridePercent;
			}
		}
	}
	
	public float LiftPercent{
		get{
			float total_range = 0;
			float sum_range = 0;
			total_range += Hinge1.UpperLimitRad - Hinge1.LowerLimitRad;
			sum_range += Hinge1.Angle - Hinge1.LowerLimitRad;
			total_range += Hinge2.UpperLimitRad - Hinge2.LowerLimitRad;
			sum_range += Hinge2.Angle - Hinge2.LowerLimitRad;
			total_range += Hinge3.UpperLimitRad - Hinge3.LowerLimitRad;
			sum_range += Hinge3.Angle - Hinge3.LowerLimitRad;
			total_range += Hinge4.UpperLimitRad - Hinge4.LowerLimitRad;
			sum_range += Hinge4.Angle - Hinge4.LowerLimitRad;
			return (1-(sum_range / total_range)) * 100.0f;
		}
	}
	
	public float StridePercent{
		get{
			if((Side == Base6Directions.Direction.Right) ^ (Direction == Base6Directions.Direction.Backward)){
				return Rotor1.Angle / ((Rotor1.UpperLimitRad - Rotor1.LowerLimitRad)) * 100.0f;
			}
			else {
				return (1-(Rotor1.Angle / ((Rotor1.UpperLimitRad - Rotor1.LowerLimitRad)))) * 100.0f;
			}
		}
	}
	
	private bool Stopped = true;
	
	private LegStatus _Status;
	private bool UpdatedStatus = false;
	public LegStatus Status{
		get{
			if(!UpdatedStatus){
				if(TargetLeg == LegStatus.Lowered){
					if(LandingGear.LockMode == LandingGearMode.Locked){
						_Status = LegStatus.Lowered;
					}
					else {
						bool lowered = true;
						lowered = lowered && Hinge1.Angle >= Hinge1.UpperLimitRad - .01f;
						lowered = lowered && Hinge2.Angle >= Hinge2.UpperLimitRad - .01f;
						lowered = lowered && Hinge3.Angle >= Hinge3.UpperLimitRad - .01f;
						lowered = lowered && Hinge4.Angle >= Hinge4.UpperLimitRad - .01f;
						if(lowered)
							_Status = LegStatus.Lowered;
						else
							_Status = LegStatus.Lowering;
						}
				}
				else {
					bool raised = true;
					raised = raised && Hinge1.Angle <= Hinge1.LowerLimitRad + .02f;
					raised = raised && Hinge2.Angle <= Hinge2.LowerLimitRad + .02f;
					raised = raised && Hinge3.Angle <= Hinge3.LowerLimitRad + .02f;
					raised = raised && Hinge4.Angle <= Hinge4.LowerLimitRad + .02f;
					if(raised)
						_Status = LegStatus.Raised;
					else
						_Status = LegStatus.Raising;
				}
				UpdatedStatus = true;
			}
			return _Status;
		}
	}
	
	private StrideStatus _Stride;
	private bool UpdatedStride = false;
	public StrideStatus Stride{
		get{
			if(!UpdatedStride){
				if(TargetStride == StrideStatus.Forward){
					if((Side == Base6Directions.Direction.Right && Rotor1.Angle >= Rotor1.UpperLimitRad - 0.02f) || (Side == Base6Directions.Direction.Left && Rotor1.Angle <= Rotor1.LowerLimitRad + 0.02f))
						_Stride = StrideStatus.Forward;
					else
						_Stride = StrideStatus.Rushing;
				}
				else {
					if((Side == Base6Directions.Direction.Right && Rotor1.Angle <= Rotor1.LowerLimitRad + 0.02f) || (Side == Base6Directions.Direction.Left && Rotor1.Angle >= Rotor1.UpperLimitRad - 0.02f))
						_Stride = StrideStatus.Backward;
					else
						_Stride = StrideStatus.Reversing;
				}
				UpdatedStride = true;
			}
			return _Stride;
		}
	}
	
	private Leg(IMyMotorStator R1, IMyMotorStator H1, IMyMotorStator H2, IMyMotorStator H3, IMyMotorStator H4, IMyMotorStator R2, IMyLandingGear LG, Base6Directions.Direction S){
		Rotor1 = R1;
		Hinge1 = H1;
		Hinge2 = H2;
		Hinge3 = H3;
		Hinge4 = H4;
		Rotor2 = R2;
		LandingGear = LG;
		Side = S;
	}
	
	public Vector3D GetPosition(){
		return Rotor1.GetPosition();
	}
	
	public static bool TryGet(MyGridProgram Prog, Base6Directions.Direction S, IMyMotorStator R1, out Leg output){
		output = null;
		if(R1 == null)
			return false;
		if(S != Base6Directions.Direction.Left && S != Base6Directions.Direction.Right)
			return false;
		List<IMyMotorStator> MotorList = (new GenericMethods<IMyMotorStator>(Prog)).GetAllIncluding("Leg Hinge 1");
		IMyMotorStator H1 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(R1.TopGrid == Hinge.CubeGrid){
				H1 = Hinge;
				break;
			}
		}
		if(H1 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Prog)).GetAllIncluding("Leg Hinge 2");
		IMyMotorStator H2 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(H1.TopGrid == Hinge.CubeGrid){
				H2 = Hinge;
				break;
			}
		}
		if(H2 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Prog)).GetAllIncluding("Leg Hinge 3");
		IMyMotorStator H3 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(H2.TopGrid == Hinge.CubeGrid){
				H3 = Hinge;
				break;
			}
		}
		if(H3 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Prog)).GetAllIncluding("Leg Hinge 4");
		IMyMotorStator H4 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(H3.TopGrid == Hinge.CubeGrid){
				H4 = Hinge;
				break;
			}
		}
		if(H4 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Prog)).GetAllIncluding("Ankle Rotor");
		IMyMotorStator R2 = null;
		foreach(IMyMotorStator Rotor in MotorList){
			if(H4.TopGrid == Rotor.CubeGrid){
				R2 = Rotor;
				break;
			}
		}
		if(R2 == null)
			return false;
		List<IMyLandingGear> GearList = (new GenericMethods<IMyLandingGear>(Prog)).GetAllIncluding("Foot");
		IMyLandingGear LG = null;
		foreach(IMyLandingGear LandingGear in GearList){
			if(R2.TopGrid == LandingGear.CubeGrid){
				LG = LandingGear;
				break;
			}
		}
		if(LG == null)
			return false;
		output = new Leg(R1, H1, H2, H3, H4, R2, LG, S);
		output.Program = Prog;
		return true;
	}
	
	public void Raise(){
		float t = -1 * RAISE_SPEED * Speed_Multx;
		TargetLeg = LegStatus.Raised;
		Hinge1.TargetVelocityRPM = t;
		Hinge2.TargetVelocityRPM = t;
		Hinge3.TargetVelocityRPM = t;
		Hinge4.TargetVelocityRPM = t;
		Stopped = false;
		LandingGear.Unlock();
		LandingGear.AutoLock=false;
	}
	
	public void Push(){
		Lower();
		if(Direction == Base6Directions.Direction.Forward){
			Reverse();
		}
		else {
			Rush();
		}
		_State = LegState.Pushing;
	}
	
	public void Return(){
		Raise();
		if(Direction == Base6Directions.Direction.Forward){
			Rush();
		}
		else {
			Reverse();
		}
		_State = LegState.Returning;
	}
	
	public void Drop(){
		Lower();
		if(Direction == Base6Directions.Direction.Forward){
			Rush();
		}
		else {
			Reverse();
		}
		_State = LegState.Lowering;
	}
	
	public void Lower(){
		float t = RAISE_SPEED * Speed_Multx;
		TargetLeg = LegStatus.Lowered;
		Hinge1.TargetVelocityRPM = t;
		Hinge2.TargetVelocityRPM = t;
		Hinge3.TargetVelocityRPM = t;
		Hinge4.TargetVelocityRPM = t;
		Stopped = false;
	}
	
	public void Rush(){
		TargetStride = StrideStatus.Forward;
		if(Side == Base6Directions.Direction.Right)
			Rotor1.TargetVelocityRPM = ROTATE_SPEED * Speed_Multx;
		if(Side == Base6Directions.Direction.Left)
			Rotor1.TargetVelocityRPM = -1 * ROTATE_SPEED * Speed_Multx;
	}
	
	public void Reverse(){
		TargetStride = StrideStatus.Backward;
		if(Side == Base6Directions.Direction.Right)
			Rotor1.TargetVelocityRPM = -1 * ROTATE_SPEED * Speed_Multx;
		if(Side == Base6Directions.Direction.Left)
			Rotor1.TargetVelocityRPM = ROTATE_SPEED * Speed_Multx;
	}
	
	private void UpdateMotor(IMyMotorStator Motor){
		if(Motor.TargetVelocityRPM > 0 && Motor.Angle >= Motor.UpperLimitRad - 0.02f){
			Motor.TargetVelocityRPM = 0;
		}
		else if(Motor.TargetVelocityRPM < 0 && Motor.Angle <= Motor.LowerLimitRad + 0.02f){
			Motor.TargetVelocityRPM = 0;
		}
	}
	
	public void Stop(){
		Rotor1.TargetVelocityRPM = 0;
		Hinge1.TargetVelocityRPM = 0;
		Hinge2.TargetVelocityRPM = 0;
		Hinge3.TargetVelocityRPM = 0;
		Hinge4.TargetVelocityRPM = 0;
		Stopped = true;
	}
	
	public void Continue(){
		if(Stride != TargetStride){
			if(TargetStride == StrideStatus.Forward){
				Rush();
			}
			else {
				Reverse();
			}
		}
		if(Status != TargetLeg){
			if(TargetLeg == LegStatus.Raised){
				Raise();
			}
			else {
				Lower();
			}
		}
	}
	
	private void UpdateHinge(IMyMotorStator Motor){
		if(Status == LegStatus.Lowered)
			Motor.TargetVelocityRPM = 1;
		else
			UpdateMotor(Motor);
	}
	
	public void Forward(){
		UpdatedStatus = false;
		UpdatedStride = false;
		Direction = Base6Directions.Direction.Forward;
		bool was_stopped = Stopped;
		Rush();
		Lower();
		if(was_stopped)
			Stop();
		Update();
	}
	
	public void Backward(){
		UpdatedStatus = false;
		UpdatedStride = false;	
		Direction = Base6Directions.Direction.Backward;
		bool was_stopped = Stopped;
		Reverse();
		Lower();
		if(was_stopped)
			Stop();
		Update();
	}
	
	private bool MovingTowardsDirection(){
		if(Direction == Base6Directions.Direction.Forward)
			return TargetStride == StrideStatus.Forward;
		if(Direction == Base6Directions.Direction.Backward)
			return TargetStride == StrideStatus.Backward;
		return false;
	}
	
	public void Update(){
		UpdatedStatus = false;
		UpdatedStride = false;
		if(Stopped){
			_State = LegState.Stopped;
		}
		
		if(TargetLeg == LegStatus.Lowered){
			LandingGear.Lock();
		}
		UpdateMotor(Rotor1);
		UpdateHinge(Hinge1);
		UpdateHinge(Hinge2);
		UpdateHinge(Hinge3);
		UpdateHinge(Hinge4);
		if(LandingGear.LockMode != LandingGearMode.Locked){
			Rotor2.Enabled = true;
			Rotor2.TargetVelocityRad = Rotor2.Angle / -2;
		}
		else{
			Rotor2.Enabled = false;
		}
	}
}

public enum LegCommand{
	Stop = 0,
	Forward = 1,
	Backward = 2,
	Left = 3,
	Right = 4
}

public class LegPair{
	public Leg Left;
	public Leg Right;
	protected MyGridProgram Program;
	private LegCommand _Command = LegCommand.Stop;
	public LegCommand Command{
		get{
			return _Command;
		}
		set{
			_Command = value;
			Update(0);
		}
	}
	private Base6Directions.Direction Preferance;
	
	public static int PairCount = 0;
	public static int LeftLockCount = 0;
	public static int RightLockCount = 0;
	private bool _LeftLock = false;
	private bool LeftLock{
		get{
			return _LeftLock;
		}
		set{
			if(value!=_LeftLock){
				if(value){
					LeftLockCount++;
				}
				else {
					LeftLockCount--;
				}
			}
			_LeftLock = value;
		}
	}
	private bool _RightLock = false;
	private bool RightLock{
		get{
			return _RightLock;
		}
		set{
			if(value!=_RightLock){
				if(value){
					RightLockCount++;
				}
				else {
					RightLockCount--;
				}
			}
			_RightLock = value;
		}
	}
	private bool _NextLockLeft = false;
	private bool NextLockLeft{
		get{
			return _NextLockLeft;
		}
		set{
			_NextLockLeft = value;
			if(_NextLockLeft)
				_NextLockRight = false;
		}
	}
	private bool _NextLockRight = false;
	private bool NextLockRight{
		get{
			return _NextLockRight;
		}
		set{
			_NextLockRight = value;
			if(_NextLockRight)
				_NextLockLeft = false;
		}
	}
	private bool _NextUnlockLeft = false;
	private bool NextUnlockLeft{
		get{
			return _NextUnlockLeft;
		}
		set{
			_NextUnlockLeft = value;
			if(value)
				_NextUnlockRight = false;
		}
	}
	private bool _NextUnlockRight = false;
	private bool NextUnlockRight{
		get{
			return _NextUnlockRight;
		}
		set{
			_NextUnlockRight = value;
			if(value)
				_NextUnlockLeft = false;
		}
	}
	
	private Leg MovingLeg;
	private Leg LockedLeg;
	private double LastSwitch = double.MaxValue;
	
	private LegPair(Leg L, Leg R){
		Left = L;
		Right = R;
		Random rnd = new Random();
		if(rnd.Next(0,1)==0){
			Preferance = Base6Directions.Direction.Right;
		}
		else {
			Preferance = Base6Directions.Direction.Left;
		}
	}
	
	public static bool TryGet(MyGridProgram Prog, Leg LeftLeg, Leg RightLeg, out LegPair output){
		output = null;
		if(LeftLeg==null || LeftLeg.Side!=Base6Directions.Direction.Left)
			return false;
		if(RightLeg==null || RightLeg.Side!=Base6Directions.Direction.Right)
			return false;
		output = new LegPair(LeftLeg, RightLeg);
		output.Program = Prog;
		return true;
	}
	
	private LegCommand LastCommand = LegCommand.Stop;
	public void Update(double Seconds){
		Program.Echo('\n' + "Update()");
		
		LeftLock = (Left.Status == LegStatus.Lowered);
		RightLock = (Right.Status == LegStatus.Lowered);
		
		if(LastCommand != Command){
			switch(Command){
				case LegCommand.Stop:
					Left.Stop();
					Right.Stop();
					break;
				case LegCommand.Forward:
					Left.Forward();
					Right.Forward();
					break;
				case LegCommand.Backward:
					Left.Backward();
					Right.Backward();
					break;
				case LegCommand.Left:
					Left.Backward();
					Right.Forward();
					break;
				case LegCommand.Right:
					Left.Forward();
					Right.Backward();
					break;
			}
		}
		
		if(MovingLeg == null || LockedLeg == null){
			if(LeftLock && RightLock){
				if(NextUnlockRight || (Left.StridePercent < Right.StridePercent && !NextUnlockLeft)){
					MovingLeg = Right;
					LockedLeg = Left;
				}
				else {
					MovingLeg = Left;
					LockedLeg = Right;
				}
				MovingLeg.Raise();
			}
			else if(LeftLock){
				MovingLeg = Right;
				LockedLeg = Left;
			}
			else if(RightLock){
				MovingLeg = Left;
				LockedLeg = Right;
			}
			else{
			if(Preferance == Base6Directions.Direction.Left){
				MovingLeg = Right;
				LockedLeg = Left;
			}
			else {
				MovingLeg = Left;
				LockedLeg = Right;
			}
		}
		}
		
		
		if(MovingLeg == Left)
			Program.Echo("ML, SR");
		else 
			Program.Echo("MR, SL");
		if(NextLockLeft)
			Program.Echo("NextLockLeft");
		else if(NextLockRight)
			Program.Echo("NextLockRight");
		else
			Program.Echo("NextLockNone");
		if(Command!=LegCommand.Stop){
			//MovingLeg.Continue();
			if(LockedLeg.State != LegState.Pushing)
				LockedLeg.Push();
			if(MovingLeg.State != LegState.Returning && MovingLeg.State != LegState.Lowering){
				MovingLeg.Return();
			}
			else{
				if(MovingLeg.StridePercent < 50){
					if(MovingLeg.State != LegState.Returning){
						MovingLeg.Return();
						if(MovingLeg == Left)
							NextLockLeft = true;
						else
							NextLockRight = true;
					}
				}
				else if(MovingLeg.StridePercent >= 50){
					if(MovingLeg.State != LegState.Lowering){
						MovingLeg.Drop();
						if(MovingLeg == Left)
							NextUnlockRight = true;
						else
							NextUnlockLeft = true;
					}
				}
				if(MovingLeg.Status == LegStatus.Raised){
					MovingLeg.Lower();
				}
			}
			LastSwitch+=Seconds;
		}
		else{
			MovingLeg.Stop();
			LockedLeg.Stop();
		}
		LockedLeg.Update();
		MovingLeg.Update();
		LastCommand = Command;
		if(LeftLock && RightLock && LastSwitch>0.5 || LastSwitch>5){
			Leg temp = MovingLeg;
			MovingLeg = LockedLeg;
			LockedLeg = temp;
			MovingLeg.Raise();
			LastSwitch=0;
		}
	}
	
}

private long cycle_long = 1;
private long cycle = 0;
private char loading_char = '|';
private const string Program_Name = "Guardian AI"; //Name me!
double seconds_since_last_update = 0;


private IMyShipController Controller = null;
private List<LegPair> LegPairs = new List<LegPair>();

public Program()
{
    Me.CustomName = (Program_Name + " Programmable block").Trim();
	Write("Beginning initialization", true, false);
	Controller = (new GenericMethods<IMyShipController>(this)).GetContaining("");
	List<Leg> LeftLegs = new List<Leg>();
	List<Leg> RightLegs = new List<Leg>();
	foreach(IMyMotorStator Rotor in (new GenericMethods<IMyMotorStator>(this)).GetAllIncluding("Left Leg Rotor")){
		Leg LeftLeg = null;
		if(Leg.TryGet(this, Base6Directions.Direction.Left, Rotor, out LeftLeg)){
			LeftLegs.Add(LeftLeg);
		}
	}
	foreach(IMyMotorStator Rotor in (new GenericMethods<IMyMotorStator>(this)).GetAllIncluding("Right Leg Rotor")){
		Leg RightLeg = null;
		if(Leg.TryGet(this, Base6Directions.Direction.Right, Rotor, out RightLeg)){
			RightLegs.Add(RightLeg);
		}
	}
	while(LeftLegs.Count > 0 && RightLegs.Count > 0){
		double min_distance = double.MaxValue;
		foreach(Leg RightLeg in RightLegs){
			double distance = (LeftLegs[0].GetPosition()-RightLeg.GetPosition()).Length();
			min_distance = Math.Min(min_distance, distance);
		}
		for(int i=0; i<RightLegs.Count; i++){
			double distance = (LeftLegs[0].GetPosition()-RightLegs[i].GetPosition()).Length();
			if(distance <= min_distance + 0.1){
				LegPair Pair = null;
				if(LegPair.TryGet(this, LeftLegs[0], RightLegs[i], out Pair)){
					Write("Found Leg Pair");
					LegPairs.Add(Pair);
					RightLegs.RemoveAt(i);
					break;
				}
			}
		}
		LeftLegs.RemoveAt(0);
	}
	
	LegPair.PairCount = LegPairs.Count;
	
	if(Controller!=null && LegPairs.Count > 0)
		Runtime.UpdateFrequency = UpdateFrequency.Update1;
	else{
		if(Controller==null)
			Write("No controller");
		else
			Write("No Leg Pairs");
	}
}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
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
	Write(Program_Name + " OS " + cycle_long.ToString() + '-' + cycle.ToString() + " (" + loading_char + ")", true, false);
	seconds_since_last_update = Runtime.TimeSinceLastRun.TotalSeconds + (Runtime.LastRunTimeMs / 1000);
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

private string last_input = "";
public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	if(Controller.MoveIndicator.Z < 0){
		last_input = "Forward";
		foreach(LegPair Pair in LegPairs){
			Pair.Command = LegCommand.Forward;
		}
	}
	else if(Controller.MoveIndicator.Z > 0){
		last_input = "Backward";
		foreach(LegPair Pair in LegPairs){
			Pair.Command = LegCommand.Backward;
		}
	}
	else if(Controller.MoveIndicator.X > 0){
		last_input = "Right";
		foreach(LegPair Pair in LegPairs){
			Pair.Command = LegCommand.Right;
		}
	}
	else if(Controller.MoveIndicator.X < 0){
		last_input = "Left";
		foreach(LegPair Pair in LegPairs){
			Pair.Command = LegCommand.Left;
		}
	}
	else if(Controller.MoveIndicator.Y != 0){
		last_input = "Stop";
		foreach(LegPair Pair in LegPairs){
			Pair.Command = LegCommand.Stop;
		}
		if(Controller.MoveIndicator.Y < 0){
			List<IMyLandingGear> AllGear = new List<IMyLandingGear>();
			GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(AllGear);
			foreach(IMyLandingGear Gear in AllGear){
				Gear.Unlock();
			}
		}
	}
	Write(last_input);
	Write("Left Locks:" + LegPair.LeftLockCount);
	Write("Right Locks:" + LegPair.RightLockCount);
	foreach(LegPair Pair in LegPairs){
		Pair.Update(seconds_since_last_update);
	}
	
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
}
