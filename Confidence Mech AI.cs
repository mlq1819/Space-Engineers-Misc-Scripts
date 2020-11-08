const string Program_Name = "Confidence Mech AI"; //Name me!
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

Vector3D TransformBetweenGrids(Vector3D Input_Local, IMyCubeBlock Input_Reference, IMyCubeBlock Output_Reference){
	Vector3D Global=Vector3D.TransformNormal(Input_Local,Input_Reference.WorldMatrix);
	return Vector3D.TransformNormal(Global,MatrixD.Invert(Output_Reference.WorldMatrix));
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

string VectorString(Vector3D vector,int n=0, bool IsGyro=false){
	if(IsGyro)
		return "P:"+Math.Round(vector.X,n).ToString()+" Y:"+Math.Round(vector.Y,n).ToString()+" R:"+Math.Round(vector.Z,n).ToString();
	else
		return "X:"+Math.Round(vector.X,n).ToString()+" Y:"+Math.Round(vector.Y,n).ToString()+" Z:"+Math.Round(vector.Z,n).ToString();
}

class Leg{
	public static Func<IMyTerminalBlock,string,bool> HasBlockData;
	public static Func<IMyTerminalBlock,string,string> GetBlockData;
	public static Func<IMyTerminalBlock,string,string,bool> SetBlockData;
	public static IMyShipController Controller;
	
	public IMyMotorStator Thigh;
	public IMyMotorStator Knee;
	public IMyMotorStator Ankle;
	public IMyLandingGear Foot;
	private Angle _Target_Angle;
	public Angle Target_Angle{
		get{
			return _Target_Angle;
		}
		set{
			if(value!=_Target_Angle){
				_Target_Angle=value;
				SetBlockData(Thigh,"TargetAngle",_Target_Angle.ToString());
			}
		}
	}
	public Angle Current_Angle{
		get{
			if(IsLeft)
				return Angle.FromRadians(Thigh.Angle);
			else
				return Angle.FromRadians(Thigh.Angle)*-1;
		}
	}
	public float Difference{
		get{
			return Current_Angle.Difference(Target_Angle);
		}
	}
	
	private Angle _Ankle_Target;
	public Angle Ankle_Target{
		get{
			return _Ankle_Target;
		}
		set{
			if(value!=_Ankle_Target){
				_Ankle_Target=value;
				SetBlockData(Ankle,"TargetAngle",_Ankle_Target.ToString());
			}
		}
	}
	public float Ankle_Difference{
		get{
			if(!IsLeft)
				return Angle.FromRadians(Ankle.Angle).Difference(Ankle_Target);
			else
				return Angle.FromRadians(Ankle.Angle).Difference(Ankle_Target*-1);
		}
	}
	
	public bool IsLeft{
		get{
			return Controller.Orientation.Left==Thigh.Orientation.Up;
		}
	}
	public string Side{
		get{
			if(IsLeft)
				return "Left";
			return "Right";
		}
	}
	
	private Leg(IMyMotorStator Thigh, IMyMotorStator Knee, IMyMotorStator Ankle, IMyLandingGear Foot){
		this.Thigh=Thigh;
		this.Knee=Knee;
		this.Ankle=Ankle;
		this.Foot=Foot;
		this._Target_Angle=new Angle(0);
		this._Ankle_Target=new Angle(0);
		if(HasBlockData(Thigh,"TargetAngle"))
			Angle.TryParse(GetBlockData(Thigh,"TargetAngle"),out _Target_Angle);
		if(HasBlockData(Ankle,"TargetAngle"))
			Angle.TryParse(GetBlockData(Ankle,"TargetAngle"),out _Ankle_Target);
	}
	
	public static bool TryGet(MyGridProgram prog, IMyMotorStator Thigh, out Leg output){
		output=null;
		
		List<IMyMotorStator> knees=(new GenericMethods<IMyMotorStator>(prog)).GetAllContaining("Knee Hinge");
		List<IMyMotorStator> good_knees=new List<IMyMotorStator>();
		foreach(IMyMotorStator knee in knees){
			if(knee.CubeGrid==Thigh.TopGrid)
				good_knees.Add(knee);
		}
		if(good_knees.Count==0)
			return false;
		good_knees=GenericMethods<IMyMotorStator>.SortByDistance(good_knees,Thigh);
		
		List<IMyMotorStator> ankles=(new GenericMethods<IMyMotorStator>(prog)).GetAllContaining("Ankle Rotor");
		List<IMyMotorStator> good_ankles=new List<IMyMotorStator>();
		foreach(IMyMotorStator ankle in ankles){
			if(ankle.CubeGrid==good_knees[0].TopGrid)
				good_ankles.Add(ankle);
		}
		if(good_ankles.Count==0)
			return false;
		good_ankles=GenericMethods<IMyMotorStator>.SortByDistance(good_ankles,Thigh);
		
		List<IMyLandingGear> feet=(new GenericMethods<IMyLandingGear>(prog)).GetAllContaining("Foot Gear");
		List<IMyLandingGear> good_feet=new List<IMyLandingGear>();
		foreach(IMyLandingGear foot in feet){
			if(foot.CubeGrid==good_ankles[0].TopGrid)
				good_feet.Add(foot);
		}
		if(good_feet.Count==0)
			return false;
		good_feet=GenericMethods<IMyLandingGear>.SortByDistance(good_feet,Thigh);
		
		output=new Leg(Thigh,good_knees[0],good_ankles[0],good_feet[0]);
		return true;
	}
}

long cycle_long = 1;
long cycle = 0;
char loading_char = '|';
double seconds_since_last_update = 0;

List<Leg> Legs;
IMyShipController Controller;
IMyGyro Gyroscope;

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

Vector3D Target_Down;

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
	Leg.HasBlockData=HasBlockData;
	Leg.GetBlockData=GetBlockData;
	Leg.SetBlockData=SetBlockData;
	List<IMyMotorStator> Thighs=(new GenericMethods<IMyMotorStator>(this)).GetAllContaining("Thigh Rotor");
	Legs=new List<Leg>();
	foreach(IMyMotorStator Thigh in Thighs){
		Leg leg;
		if(Leg.TryGet(this,Thigh,out leg)){
			Legs.Add(leg);
		}
	}
	Controller=(new GenericMethods<IMyShipController>(this)).GetContaining("");
	Leg.Controller=Controller;
	Gyroscope=(new GenericMethods<IMyGyro>(this)).GetContaining("");
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
	Runtime.UpdateFrequency=UpdateFrequency.Update10;
}

public void Save()
{
	Gyroscope.GyroOverride=false;
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
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

void UpdatePositionalInfo(){
	Vector3D base_vector=new Vector3D(0,0,-1);
	Forward_Vector=LocalToGlobal(base_vector,Controller);
	Forward_Vector.Normalize();
	
	base_vector=new Vector3D(0,1,0);
	Up_Vector=LocalToGlobal(base_vector,Controller);
	Up_Vector.Normalize();
	
	base_vector=new Vector3D(-1,0,0);
	Left_Vector=LocalToGlobal(base_vector,Controller);
	Left_Vector.Normalize();
	
	if(Controller.GetTotalGravity().Length()>0)
		Target_Down=Controller.GetTotalGravity();
	else
		Target_Down=Down_Vector;
	Target_Down.Normalize();
}

void SetAngle(IMyMotorStator Motor,Angle Next_Angle,float Precision=0.1f,double Speed_Multx=1){
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
		//Write(Motor.CustomName+": Invalid Angle");
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
		if(From_Bottom<From_Top)
			Motor.TargetVelocityRPM=(float)(-1*From_Bottom*Speed_Multx*Precision*5);
		else
			Motor.TargetVelocityRPM=(float)(From_Top*Speed_Multx*Precision*5);
	}
	else
		Motor.TargetVelocityRPM=0;
}

Vector3D GetForward_Rotor(IMyMotorStator Rotor){
	return LocalToGlobalPosition(new Vector3D(0,0,-1),Rotor.Top);
}

Vector3D GetForward_Hinge(IMyMotorStator Hinge){
	return LocalToGlobalPosition(new Vector3D(0,1,0),Hinge.Top);
}

void SetGyroscopes(){
	Gyroscope.GyroOverride=(Controller.GetShipVelocities().AngularVelocity.Length()<3);
	Vector3D Relative_AngularVelocity=GlobalToLocal(Controller.GetShipVelocities().AngularVelocity, Controller);
	float current_pitch=(float) Relative_AngularVelocity.X;
	float current_yaw=(float) Relative_AngularVelocity.Y;
	float current_roll=(float) Relative_AngularVelocity.Z;
	
	float gyro_multx=2;
	
	float input_pitch=current_pitch*0.99f;
	float input_yaw=current_yaw*0.99f;
	float input_roll=current_roll*0.99f;
	
	if(GetAngle(Target_Down,Down_Vector)>2.5){
		double Pitch_Difference=GetAngle(Backward_Vector,Target_Down)-GetAngle(Forward_Vector,Target_Down);
		double Roll_Difference=GetAngle(Left_Vector,Target_Down)-GetAngle(Right_Vector,Target_Down);
		if(Math.Abs(Pitch_Difference)>1){
			input_pitch-=(float)(Pitch_Difference*gyro_multx);
		}
		if(Math.Abs(Roll_Difference)>1){
			input_roll-=(float)Math.Min(Math.Max(Roll_Difference/5,-1),1)*gyro_multx;
		}
	}
	Vector3D input=new Vector3D(input_pitch,input_yaw,input_roll);
	Vector3D output=TransformBetweenGrids(input,Controller,Gyroscope);
	output.Normalize();
	output*=input.Length();
	
	Write("Gyroscope: "+VectorString(input,2,true));
	Gyroscope.Pitch=(float)output.X;
	Gyroscope.Yaw=(float)output.Y;
	Gyroscope.Roll=(float)output.Z;
}

void PerformWalk(){
	Leg Forward;
	Leg Backward;
	if(Legs[0].Target_Angle>Legs[1].Target_Angle){
		Forward=Legs[0];
		Backward=Legs[1];
	}
	else{
		Forward=Legs[1];
		Backward=Legs[0];
	}
	Leg Leading;
	Leg Holding;
	if(Controller.MoveIndicator.Z>0){
		Leading=Forward;
		Holding=Backward;
		if(Leading.Foot.LockMode==LandingGearMode.Locked)
			Write("Leading:"+Leading.Side+":Forward:Locked");
		else
			Write("Leading:"+Leading.Side+":Forward:Unlocked");
		if(Holding.Foot.LockMode==LandingGearMode.Locked)
			Write("Holding:"+Holding.Side+":Backward:Locked");
		else
			Write("Holding:"+Holding.Side+":Backward:Unlocked");
	}
	else{
		Leading=Backward;
		Holding=Forward;
		if(Leading.Foot.LockMode==LandingGearMode.Locked)
			Write("Leading:"+Leading.Side+":Backward:Locked");
		else
			Write("Leading:"+Leading.Side+":Backward:Unlocked");
		if(Holding.Foot.LockMode==LandingGearMode.Locked)
			Write("Holding:"+Holding.Side+":Forward:Locked");
		else
			Write("Holding:"+Holding.Side+":Forward:Unlocked");
	}
	Holding.Ankle.Displacement=-0.11f;
	Leading.Foot.AutoLock=(Leading.Difference<Math.Abs(Move_Angle.Degrees));
	if(Leading.Foot.LockMode==LandingGearMode.ReadyToLock||Leading.Foot.AutoLock){
		Write("Locking Leading...");
		Leading.Ankle.Displacement=0.11f;
		Leading.Foot.Lock();
	}
	else{
		Write("Waiting to Lock Leading...");
	}
	if(Leading.Foot.LockMode==LandingGearMode.Locked||(Leading.Difference<1&&Holding.Difference<1)){
		Write("Leading Locked");
		Holding.Foot.AutoLock=false;
		Holding.Foot.Unlock();
		if(Controller.MoveIndicator.X!=0){
			if(Controller.MoveIndicator.X>0^Leading.IsLeft)
				Leading.Ankle_Target=new Angle(-30);
			else
				Leading.Ankle_Target=new Angle(30);
		}
		if(Leading.Ankle_Difference<1){
			Write("Swapping");
			Angle target=Move_Angle;
			if(Controller.MoveIndicator.Z>1)
				target*=-1;
			Leading.Target_Angle=target*-1;
			Holding.Target_Angle=target;
		}
		else{
			Write("Waiting Ankle");
		}
	}
	else{
		Write("Leading Unlocked\n");
	}
}

Angle Move_Angle=new Angle(60);
public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	UpdatePositionalInfo();
	Vector3D Thigh_Direction=LocalToGlobal(new Vector3D(0,0,-1),Legs[0].Thigh.Top);
	if(argument.ToLower().IndexOf("set:")==0){
		string word=argument.Substring("set:".Length).Trim();
		float angle=0;
		Angle a2;
		if(float.TryParse(word,out angle)){
			Move_Angle=new Angle(angle);
		}
		else if(Angle.TryParse(word, out a2)){
			Move_Angle=a2;
		}
	}
	if(Legs.Count>=2 && Controller.MoveIndicator.Z!=0)
		PerformWalk();
	if(Gyroscope!=null)
		SetGyroscopes();
	
	foreach(Leg leg in Legs){
		if(Controller.Orientation.Left==Base6Directions.GetOppositeDirection(leg.Thigh.Orientation.Up))
			SetAngle(leg.Thigh,leg.Target_Angle*-1);
		else
			SetAngle(leg.Thigh,leg.Target_Angle);
		if(leg.Thigh.Orientation.Up!=leg.Knee.Orientation.Forward)
			SetAngle(leg.Knee,leg.Target_Angle*-1);
		else
			SetAngle(leg.Knee,leg.Target_Angle);
		if(leg.Foot.LockMode!=LandingGearMode.Locked){
			leg.Ankle_Target=new Angle(0);
		}
		SetAngle(leg.Ankle,leg.Ankle_Target);
		Write("");
	}
	
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
}