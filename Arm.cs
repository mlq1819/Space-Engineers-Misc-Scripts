const string Program_Name = ""; //Name me!
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);

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
		return Math.Abs(a1.Degrees-a2.Degrees)<0.000001f;
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
		return a1.Difference_From_Top(a2)<a1.Difference_From_Bottom(a2);
	}
	
	public static bool operator >=(Angle a1, Angle a2){
		return a1==a2 || a1>a2;
	}
	
	public static bool operator <=(Angle a1, Angle a2){
		return a1==a2 || a1<a2;
	}
	
	public static bool operator <(Angle a1, Angle a2){
		return a1.Difference_From_Top(a2)>a1.Difference_From_Bottom(a2);
	}
	
	public override string ToString(){
		return Degrees.ToString()+'°';
	}
	
	public string ToString(int n){
		n=Math.Min(0,n);
		return Math.Round(Degrees,n).ToString()+'°';
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
}

bool IsHinge(IMyMotorStator Motor){
	return Motor.BlockDefinition.SubtypeName.Contains("Hinge");
}

bool IsRotor(IMyMotorStator Motor){
	return (!IsHinge(Motor))&&Motor.BlockDefinition.SubtypeName.Contains("Stator");
}

class Arm{
	public static MyGridProgram P;
	public static Func<IMyTerminalBlock,string,bool> HasBlockData;
	public static Func<IMyTerminalBlock,string,string> GetBlockData;
	public static Func<IMyTerminalBlock,string,string,bool> SetBlockData;
	public static Func<Vector3D,IMyCubeBlock,Vector3D> GlobalToLocalPosition;
	public static Func<IMyMotorStator,bool> IsHinge;
	public static Func<IMyMotorStator,bool> IsRotor;
	
	public List<IMyMotorStator> Motors;
	
	public double MaxLength{
		get{
			double output=0;
			for(int i=0;i<Motors.Count;i++){
				if(IsHinge(Motors[i]))
					output+=(Motors[i].GetPosition()-Motors[i].Top.GetPosition()).Length();
				if(i>0)
					output+=(Motors[i-1].Top.GetPosition()-Motors[i].GetPosition()).Length();
			}
			return output;
		}
	}
	
	List<IMyMotorStator> FilterByGrid(List<IMyMotorStator> input,IMyCubeGrid Grid){
		List<IMyMotorStator> output=new List<IMyMotorStator>();
		foreach(IMyMotorStator Motor in input){
			if(Motor.CubeGrid==Grid)
				output.Add(Motor);
		}
		return output;
	}
	
	public Arm(IMyMotorStator BaseMotor){
		Motors=new List<IMyMotorStator>();
		Motors.Add(BaseMotor);
		List<IMyMotorStator> allmotors=(new GenericMethods<IMyMotorStator>(P)).GetAllIncluding("");
		List<IMyMotorStator> gridmotors;
		do{
			gridmotors=FilterByGrid(allmotors,Motors[Motors.Count-1].TopGrid);
			if(gridmotors.Count==1)
				Motors.Add(gridmotors[0]);
		} while(gridmotors.Count==1);
	}
	
	public override string ToString(){
		string output="Arm";
		foreach(IMyMotorStator Motor in Motors){
			output+=":"+Motor.CustomName;
		}
		return output;
	}
}

long cycle_long = 1;
long cycle = 0;
char loading_char = '|';
double seconds_since_last_update = 0;
IMyShipController Controller;
List<Arm> Arms;

bool ArmFunction(IMyMotorStator Motor){
	return Motor.CubeGrid==Controller.CubeGrid;
}

public Program()
{
	Arm.P=this;
	Arm.HasBlockData=HasBlockData;
	Arm.GetBlockData=GetBlockData;
	Arm.GlobalToLocalPosition=GlobalToLocalPosition;
	Arm.SetBlockData=SetBlockData;
	Arm.IsHinge=IsHinge;
	Arm.IsRotor=IsRotor;
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
	Write("",false,false);
	Controller=(new GenericMethods<IMyShipController>(this)).GetContaining("");
	List<IMyMotorStator> Motors=(new GenericMethods<IMyMotorStator>(this)).GetAllFunc(ArmFunction);
	Write(Motors.Count.ToString()+" Motors");
	Arms=new List<Arm>();
	foreach(IMyMotorStator Motor in Motors){
		Write('\t'+Motor.CustomName+":"+Motor.BlockDefinition.SubtypeName);
		Arm arm = new Arm(Motor);
		Arms.Add(arm);
		Write('\t'+arm.ToString());
	}
	
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

//Checks whether a particular Motor can move to the given angle
bool CanSetAngle(IMyMotorStator Motor,Angle Next_Angle){
	bool can_increase=true;
	bool can_decrease=true;
	Angle Motor_Angle=Angle.FromRadians(Motor.Angle);
	if(Motor.UpperLimitDeg!=float.MaxValue)
		can_increase=Angle.IsBetween(Motor_Angle,Next_Angle,new Angle(Motor.UpperLimitDeg));
	if(Motor.LowerLimitDeg!=float.MinValue)
		can_decrease=Angle.IsBetween(new Angle(Motor.LowerLimitDeg),Next_Angle,Motor_Angle);
	return can_increase||can_decrease;
}

//Updates the velocity of a particular motor to move to the given angle. Call on every run.
void SetAngle(IMyMotorStator Motor,Angle Next_Angle,float Speed_Multx=1,float Precision=0.1f){
	Speed_Multx=Math.Max(0.1f, Math.Min(Math.Abs(Speed_Multx),10));
	Precision=Math.Max(0.0001f, Math.Min(Math.Abs(Precision),1));
	bool can_increase=true;
	bool can_decrease=true;
	Angle Motor_Angle=Angle.FromRadians(Motor.Angle);
	if(Motor.UpperLimitDeg!=float.MaxValue)
		can_increase=Angle.IsBetween(Motor_Angle,Next_Angle,new Angle(Motor.UpperLimitDeg));
	if(Motor.LowerLimitDeg!=float.MinValue)
		can_decrease=Angle.IsBetween(new Angle(Motor.LowerLimitDeg),Next_Angle,Motor_Angle);
	
	if((!can_increase)&&(!can_decrease)){
		Motor.TargetVelocityRPM=0;
		return;
	}
	
	float From_Bottom=Motor_Angle.Difference_From_Bottom(Next_Angle);
	if(!can_decrease)
		From_Bottom=float.MaxValue;
	float From_Top=Motor_Angle.Difference_From_Top(Next_Angle);
	if(!can_increase)
		From_Top=float.MaxValue;
	float difference=Math.Min(From_Bottom,From_Top);
	//Write(Motor.CustomName+" Difference:"+Math.Round(difference,2)+'°');
	if(difference>Precision){
		Motor.RotorLock=false;
		float target_rpm=0;
		if(From_Bottom<From_Top)
			target_rpm=(float)(-1*From_Bottom*Speed_Multx*Precision*5);
		else
			target_rpm=(float)(From_Top*Speed_Multx*Precision*5);
		Motor.TargetVelocityRPM=target_rpm;
	}
	else{
		Motor.TargetVelocityRPM=0;
		//Motor.RotorLock=true;
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
	UpdateProgramInfo();
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
}
