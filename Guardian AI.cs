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

public enum LegStatus{
	Raising = 0,
	Lowering = 1,
	Raised = 3,
	Lowered = 4
}

public enum StrideStatus{
	Rushing = 0,
	Reversing = 2,
	Forward = 2,
	Backward = 4
}

public class Leg{
	private IMyMotorStator Rotor1;
	private IMyMotorStator Hinge1;
	private IMyMotorStator Hinge2;
	private IMyMotorStator Hinge3;
	private IMyMotorStator Hinge4;
	private IMyMotorStator Rotor2;
	private IMyLandingGear LandingGear;
	private Base6Directions.Direction Side;
	protected MyGridProgram Program;
	
	private LegStatus TargetLeg = LegStatus.Lowered;
	private StrideStatus TargetStride = StrideStatus.Forward;
	
	private Base6Direction.Direction Direction = Base6Direction.Direction.Forward;
	
	private bool Stopped = true;
	
	public LegStatus Status{
		get{
			if(TargetLeg == LegStatus.Lowered){
				if(Foot.LockMode == LandingGearMode.Locked)
					return LegStatus.Lowered;
				bool lowered = true;
				lowered = lowered && Hinge1.Angle <= Hinge1.LowerLimitRad + .02f;
				lowered = lowered && Hinge2.Angle <= Hinge2.LowerLimitRad + .02f;
				lowered = lowered && Hinge3.Angle <= Hinge3.LowerLimitRad + .02f;
				lowered = lowered && Hinge4.Angle <= Hinge4.LowerLimitRad + .02f;
				if(Lowered)
					return LegStatus.Lowered;
				else
					return LegStatus.Lowering;
			}
			else {
				bool raised = true;
				raised = raised && Hinge1.Angle >= Hinge1.LowerLimitRad - .02f;
				raised = raised && Hinge2.Angle >= Hinge2.LowerLimitRad - .02f;
				raised = raised && Hinge3.Angle >= Hinge3.LowerLimitRad - .02f;
				raised = raised && Hinge4.Angle >= Hinge4.LowerLimitRad - .02f;
				if(Raised)
					return LegStatus.Raised;
				else
					return LegStatus.Raising;
			}
		}
	}
	
	public StrideStatus Stride{
		get{
			if(TargetStride == StrideStatus.Forward){
				
				if((Side == Base6Directions.Direction.Right && Rotor1.Angle >= Rotor1.UpperLimitRad - 0.02f) || (Side == Base6Directions.Direction.Left && Rotor1.Angle <= Rotor1.LowerLimitRad + 0.02f))
					return LegStatus.Forward;
				else
					return LegStatus.Rushing;
			}
			else {
				if((Side == Base6Directions.Direction.Right && Rotor1.Angle <= Rotor1.LowerLimitRad + 0.02f) || (Side == Base6Directions.Direction.Left && Rotor1.Angle >= Rotor1.UpperLimitRad - 0.02f))
				if(Raised)
					return LegStatus.Backward;
				else
					return LegStatus.Reversing;
			}
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
	
	public static bool TryGet(MyGridProgram Prog, Base6Directions.Direction S, IMyMotorStator R1, out Leg output){
		if(S != Base6Directions.Direction.Left && S != Base6Directions.Direction.Right)
			return false;
		List<IMyMotorStator> MotorList = (new GenericMethods<IMyMotorStator>(Program)).GetAllContaining("Leg Hinge 1");
		IMyMotorStator H1 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(R1.TopGrid == Hinge.CubeGrid){
				H1 = Hinge;
				break;
			}
		}
		if(H1 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Program)).GetAllContaining("Leg Hinge 2");
		IMyMotorStator H2 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(H1.TopGrid == Hinge.CubeGrid){
				H2 = Hinge;
				break;
			}
		}
		if(H2 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Program)).GetAllContaining("Leg Hinge 3");
		IMyMotorStator H3 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(H2.TopGrid == Hinge.CubeGrid){
				H3 = Hinge;
				break;
			}
		}
		if(H3 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Program)).GetAllContaining("Leg Hinge 4");
		IMyMotorStator H4 = null;
		foreach(IMyMotorStator Hinge in MotorList){
			if(H3.TopGrid == Hinge.CubeGrid){
				H4 = Hinge;
				break;
			}
		}
		if(H4 == null)
			return false;
		MotorList = (new GenericMethods<IMyMotorStator>(Program)).GetAllContaining("Ankle Rotor");
		IMyMotorStator R2 = null;
		foreach(IMyMotorStator Rotor in MotorList){
			if(H4.TopGrid == Rotor.CubeGrid){
				R2 = Rotor;
				break;
			}
		}
		if(R2 == null)
			return false;
		List<IMyLandingGear> GearList = (new GenericMethods<IMyLandingGear>(Program)).GetAllContaining("Foot");
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
		return true;
	}
	
	private void Raise(){
		float t = -1.5f;
		TargetStatus = LegStatus.Raised;
		Hinge1.TargetVelocityRPM = t;
		Hinge2.TargetVelocityRPM = t;
		Hinge3.TargetVelocityRPM = t;
		Hinge4.TargetVelocityRPM = t;
		Stopped = false;
	}
	
	private void Lower(){
		float t = 1.5f;
		TargetStatus = LegStatus.Lowered;
		Hinge1.TargetVelocityRPM = t;
		Hinge2.TargetVelocityRPM = t;
		Hinge3.TargetVelocityRPM = t;
		Hinge4.TargetVelocityRPM = t;
		Stopped = false;
	}
	
	private void UpdateMotor(IMyMotorStator Motor){
		if(Motor.TargetVelocityRPM > 0 && Motor.Angle >= Motor.UpperLimit - 0.02f){
			Motor.TargetVelocityRPM = 0;
		}
		else if(Motor.TargetVelocityRPM < 0 && Motor.Angle <= Motor.UpperLimit + 0.02f){
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
	
	private void UpdateHinge(IMyMotorStator Motor){
		if(Status == LegStatus.Lowered)
			Motor.TargetVelocityRPM = 0;
		else
			UpdateMotor(Motor);
	}
	
	public void Forward(){
		Direction = Base6Direction.Direction.Forward;
	}
	
	public void Reverse(){
		Direction = Base6Direction.Direction.Backward;
	}
	
	
	
	public void Update(){
		if(!Stopped && Status == LegStatus.Raised){
			Lower();
		}
		else if(!Stopped && Status == LegStatus.Lowered && Stride == StrideStatus.Backward){
			if(Side == Base6Directions.Direction.Right)
				Rotor1.TargetVelocityRPM = 1.5f;
			else if(Side == Base6Directions.Direction.Left)
				Rotor1.TargetVelocityRPM = -1.5f;
			Raise();
		}
		if(TargetLeg == LegStatus.Lowered){
			Foot.Lock();
			if(!Stopped){
				if(Side == Base6Directions.Direction.Right)
					Rotor1.TargetVelocityRPM = 1.5f;
				else if(Side == Base6Directions.Direction.Left)
					Rotor1.TargetVelocityRPM = -1.5f;
			}
		}
		else {
			Foot.Unlock();
		}
		UpdateMotor(Rotor1);
		UpdateHinge(Hinge1);
		UpdateHinge(Hinge2);
		UpdateHinge(Hinge3);
		UpdateHinge(Hinge4);
		if(Foot.LockMode != LandingGearMode.Locked){
			Rotor2.Enabled = true;
			Rotor2.TargetVelocityRad = Rotor2.Angle / -2;
		}
		else{
			Rotor2.Enabled = false;
		}
	}
}



private long cycle_long = 1;
private long cycle = 0;
private char loading_char = '|';
private const string Program_Name = ""; //Name me!
double seconds_since_last_update = 0;


private IMyShipController Controller = null;
private IMyGyro Gyro = null;

public Program()
{
    Me.CustomName = (Program_Name + " Programmable block").Trim();
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
	Echo(Program_Name + " OS " + cycle_long.ToString() + '-' + cycle.ToString() + " (" + loading_char + ")");
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
