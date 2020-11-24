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

long cycle_long = 1;
long cycle = 0;
char loading_char = '|';
double seconds_since_last_update = 0;

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
	if(Motors.Count==0)
		return;
	MyArm=new Arm(Motors[0]);
	Controller=(new GenericMethods<IMyShipController>(this)).GetContaining("",50);
	if(Controller==null)
		return;
	
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
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
Vector3D Target_Position=new Vector3D(0,0,0);
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
}

/*
* Idle
* 	0:	Reduce Torque to 100
*	E:	Revert Torque to default
*/
void Idle(){
	if(Command_Stage==0){
		foreach(IMyMotorStator Motor in MyArm.AllMotors){
			if(Motor.Torque!=100){
				SetBlockData(Motor,"DefaultTorque",Motor.Torque.ToString());
				Motor.Torque=100;
			}
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
		Vector3D Avg_Position=new Vector3D(0,0,0);
		foreach(Arm Finger in MyArm.MyHand)
			Avg_Position+=Finger.Motors[0].GetPosition();
		Avg_Position/=MyArm.MyHand.Count;
		double Distance=(Target_Position-Avg_Position).Length();
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
}

public void Main(string argument, UpdateType updateSource)
{
	try{
		UpdateProgramInfo();
		Write("Current:"+Current_Command.ToString());
		Write("Next:"+Next_Command.ToString());
		PerformCommand();
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
		}
		catch(Exception){
			;
		}
		throw e;
	}
}
