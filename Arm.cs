const string Program_Name = "Arm"; //Name me!
Color DEFAULT_TEXT_COLOR=new Color(197,137,255,255);
Color DEFAULT_BACKGROUND_COLOR=new Color(44,0,88,255);
const double BLINK_TIMER_LIMIT=2;

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
	public static Func<Vector3D,IMyCubeBlock,Vector3D> LocalToGlobal;
	public static Func<IMyMotorStator,bool> IsHinge;
	public static Func<IMyMotorStator,bool> IsRotor;
	public static Func<Vector3D,Vector3D,double> GetAngle;
	
	public int MaxDepth{
		get{
			int max=0;
			foreach(Arm Finger in Hand)
				max=Math.Max(max,Finger.MaxDepth);
			return max+Motors.Count;
		}
	}
	
	public int MotorCount{
		get{
			int sum=Motors.Count;
			foreach(Arm Finger in Hand)
				sum+=Finger.MotorCount;
			return sum;
		}
	}
	
	public List<IMyMotorStator> AllMotors{
		get{
			List<IMyMotorStator> output=new List<IMyMotorStator>();
			foreach(IMyMotorStator Motor in Motors)
				output.Add(Motor);
			foreach(Arm Finger in Hand){
				foreach(IMyMotorStator Motor in Finger.Motors)
					output.Add(Motor);
			}
			return output;
		}
	}
	
	public List<IMyInteriorLight> AllLights{
		get{
			List<IMyInteriorLight> output=new List<IMyInteriorLight>();
			if(Light!=null)
				output.Add(Light);
			foreach(Arm Finger in Hand){
				foreach(IMyInteriorLight light in Finger.AllLights)
					output.Add(light);
			}
			return output;
		}
	}
	
	public List<IMyMotorStator> Motors;
	public IMyInteriorLight Light;
	public List<Arm> Hand;
	
	public double MaxLength{
		get{
			double output=0;
			for(int i=1;i<Motors.Count;i++){
				if(i>1)
					output+=(Motors[i-1].Top.GetPosition()-Motors[i].GetPosition()).Length();
				output+=(Motors[i].GetPosition()-Motors[i].Top.GetPosition()).Length();
			}
			if(Light!=null)
				output+=(Motors[Motors.Count-1].Top.GetPosition()-Light.GetPosition()).Length();
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
	List<IMyInteriorLight> FilterByGrid(List<IMyInteriorLight> input,IMyCubeGrid Grid){
		List<IMyInteriorLight> output=new List<IMyInteriorLight>();
		foreach(IMyInteriorLight light in input){
			if(light.CubeGrid==Grid)
				output.Add(light);
		}
		return output;
	}
	
	public double MotorLength(int MotorNum){
		IMyMotorStator Motor=Motors[MotorNum];
		if(MotorNum<Motors.Count-1)
			return (Motor.Top.GetPosition()-Motors[MotorNum+1].GetPosition()).Length();
		else if(MotorNum==Motors.Count-1&&Light!=null)
			return (Motor.Top.GetPosition()-Light.GetPosition()).Length();
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
	
	public Arm(IMyMotorStator BaseMotor){
		Motors=new List<IMyMotorStator>();
		Motors.Add(BaseMotor);
		List<IMyMotorStator> allmotors=(new GenericMethods<IMyMotorStator>(P)).GetAllIncluding("",50);
		List<IMyMotorStator> gridmotors;
		do{
			gridmotors=FilterByGrid(allmotors,Motors[Motors.Count-1].TopGrid);
			if(gridmotors.Count==1)
				Motors.Add(gridmotors[0]);
		} while(gridmotors.Count==1);
		Hand=new List<Arm>();
		if(gridmotors.Count>0){
			for(int i=0;i<gridmotors.Count;i++){
				Hand.Add(new Arm(gridmotors[i]));
				foreach(IMyMotorStator Motor in Hand[Hand.Count-1].Motors)
					Motor.CustomName="Finger "+(i+1).ToString()+Motor.CustomName.Substring(3);
				if(Hand[Hand.Count-1].Light!=null)
					Hand[Hand.Count-1].Light.CustomName="Finger "+(i+1).ToString()+" Light";
			}
		}
		for(int i=0;i<Motors.Count;i++){
			if(IsHinge(Motors[i]))
				Motors[i].CustomName="Arm Stator "+(i+1).ToString()+" (Hinge)";
			else if(IsRotor(Motors[i]))
				Motors[i].CustomName="Arm Stator "+(i+1).ToString()+" (Rotor)";
			else
				Motors[i].CustomName="Arm Stator "+(i+1).ToString();
		}
		List<IMyInteriorLight> lights=(new GenericMethods<IMyInteriorLight>(P)).GetAllIncluding("",50);
		lights=FilterByGrid(lights,Motors[Motors.Count-1].TopGrid);
		if(lights.Count>0){
			Light=lights[0];
			Light.CustomName="Arm Light";
		}
		else
			Light=null;
	}
	
	public override string ToString(){
		string output="Arm";
		foreach(IMyMotorStator Motor in Motors)
			output+=":"+Motor.CustomName;
		if(Light!=null)
			output+=':'+Light.CustomName;
		return output;
	}
}

long cycle_long = 1;
long cycle = 0;
char loading_char = '|';
double seconds_since_last_update = 0;
IMyShipController Controller;
List<Arm> Arms;
IMySensorBlock Sensor;
Random Rnd;

bool ArmFunction(IMyMotorStator Motor){
	return Motor.CubeGrid==Controller.CubeGrid;
}

public Program()
{
	string[] strs=this.Storage.Split('\n');
	if(strs.Length>0)
		bool.TryParse(strs[0],out toggle);
	if(strs.Length>1)
		bool.TryParse(strs[1],out toggling);
	Me.CustomName=(Program_Name+" Programmable block").Trim();
	for(int i=0;i<Me.SurfaceCount;i++){
		Me.GetSurface(i).FontColor=DEFAULT_TEXT_COLOR;
		if(toggle||toggling)
			Me.GetSurface(i).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		else
			Me.GetSurface(i).BackgroundColor=new Color(0,0,0,255);
		Me.GetSurface(i).Alignment=TextAlignment.CENTER;
		Me.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
	}
	Rnd=new Random();
	Me.GetSurface(1).FontSize=2.2f;
	Me.GetSurface(1).TextPadding=40.0f;
	Echo("Beginning initialization");
	Write("",false,false);
	Controller=(new GenericMethods<IMyShipController>(this)).GetContaining("");
	Arm.P=this;
	Arm.HasBlockData=HasBlockData;
	Arm.GetBlockData=GetBlockData;
	Arm.GlobalToLocalPosition=GlobalToLocalPosition;
	Arm.SetBlockData=SetBlockData;
	Arm.IsHinge=IsHinge;
	Arm.IsRotor=IsRotor;
	Arm.GetAngle=GetAngle;
	Arm.LocalToGlobal=LocalToGlobal;
	List<IMyMotorStator> Motors=(new GenericMethods<IMyMotorStator>(this)).GetAllFunc(ArmFunction);
	Write(Motors.Count.ToString()+" Motors");
	Arms=new List<Arm>();
	foreach(IMyMotorStator Motor in Motors){
		Write('\t'+Motor.CustomName+":"+Motor.BlockDefinition.SubtypeName);
		Arm arm = new Arm(Motor);
		Arms.Add(arm);
		foreach(IMyInteriorLight Light in arm.AllLights)
			Light.Enabled=toggle&&(!toggling);
		Write('\t'+arm.ToString()+':'+Math.Round(arm.MaxLength,1).ToString()+"M");
	}
	Sensor=(new GenericMethods<IMySensorBlock>(this)).GetContaining("");
	rndarr=new float[10];
	for(int i=0;i<10;i++)
		rndarr[i]=10;
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

public void Save()
{
	foreach(Arm arm in Arms){
		foreach(IMyMotorStator Motor in arm.AllMotors){
			Motor.TargetVelocityRPM=0;
		}
		foreach(IMyInteriorLight Light in arm.AllLights)
			Light.Enabled=false;
	}
	this.Storage=toggle.ToString()+'\n'+toggling.ToString();
}

Angle GetTargetAngle(Arm arm, int MotorNum, Vector3D position){
	IMyMotorStator Motor=arm.Motors[MotorNum];
	float Difference=0;
	Vector3D Direction=position-Motor.Top.GetPosition();
	Direction.Normalize();
	if(IsHinge(Motor)){
		Vector3D Front=LocalToGlobal(new Vector3D(0,0,-1),Motor.Top);
		Front.Normalize();
		Difference=(float)(GetAngle(-1*Front,Direction)-GetAngle(Front,Direction));
	}
	else if(IsRotor(Motor)){
		Vector3D Left=LocalToGlobal(new Vector3D(-1,0,0),Motor.Top);
		Left.Normalize();
		Difference=(float)(GetAngle(-1*Left,Direction)-GetAngle(Left,Direction));
	}
	else
		throw new ArgumentException("Invalid Stator:"+Motor.CustomName);
	return Angle.FromRadians(Motor.Angle)+Difference;
}

Angle GetWindTarget(Arm arm, int MotorNum, Vector3D position){
	IMyMotorStator Motor=arm.Motors[MotorNum];
	Angle Target=GetTargetAngle(arm,MotorNum,position);
	double Distance=(position-arm.Motors[0].GetPosition()).Length();
	if(Distance>=arm.MaxLength)
		return Target;
	double Motor_Length=arm.MotorLength(MotorNum); //Length of adjustable arm segment
	double Arm_Length=arm.SumLength(MotorNum+1); //Length of non-adjustable arm segments after the adjustable one
	double Cover_Length=(arm.Motors[MotorNum+1].Top.GetPosition()-position).Length()*.9-1; //Length of distance to cover with arm
	Angle Degrees=new Angle(0);
	double Covered_Length=0;
	float angle=0;
	while(angle<180&&(CanSetAngle(Motor,Target+angle+1)||CanSetAngle(Motor,Target-angle-1)))
		Covered_Length=Motor_Length-Math.Cos(++angle)*Motor_Length;
	if(CanSetAngle(Motor,Target+angle)){
		if(Runtime.UpdateFrequency==UpdateFrequency.Update1)
			Hinge_Adjustment_Avg=((99*Hinge_Adjustment_Avg)+angle)/100;
		else
			Hinge_Adjustment_Avg=((9*Hinge_Adjustment_Avg)+angle)/10;
		return Target+Hinge_Adjustment_Avg;
	}
	else if(CanSetAngle(Motor,Target-angle)){
		if(Runtime.UpdateFrequency==UpdateFrequency.Update1)
			Hinge_Adjustment_Avg=((99*Hinge_Adjustment_Avg)-angle)/100;
		else
			Hinge_Adjustment_Avg=((9*Hinge_Adjustment_Avg)-angle)/10;
		return Target-Hinge_Adjustment_Avg;
	}
	return Target;
}

bool grabbed=false;
double LightTimer=0;
int LightMultx_R=1;
int LightMultx_G=1;
int LightMultx_B=1;
float Hinge_Adjustment_Avg=0;
bool SetPosition(Arm arm,Vector3D position,float percent=1,bool worry_reset=true){
	//if((arm.Motors[0].GetPosition()-position).Length()>=arm.MaxLength)
		//return false;
	Vector3D actual_position=position;
	Vector3D Gravity=Controller.GetTotalGravity();
	if(Gravity.Length()!=0){
		Gravity.Normalize();
		position-=Gravity*0.5f;
	}
	
	bool moving=false;
	double distance=(position-arm.Motors[0].GetPosition()).Length();
	double distance_from_target=(position-arm.Motors[arm.Motors.Count-1].Top.GetPosition()).Length();
	if(arm.Light!=null)
		distance_from_target=(position-arm.Light.GetPosition()).Length();
	double adjusted_distance=0.9*distance-1;
	bool hinge_1=adjusted_distance>=arm.MaxLength;
	Write("Distance:"+Math.Round(distance,1).ToString()+"M");
	float speed=(float)(distance/arm.MaxLength)/2;
	float max_difference=0;
	float Max_RPM=0;
	float Max_Speed=0;
	double ResetTimer=0;
	bool Resetting=false;
	bool do_main_loop=true;
	if(HasBlockData(arm.Motors[0],"ResetTimer"))
		double.TryParse(GetBlockData(arm.Motors[0],"ResetTimer"),out ResetTimer);
	if(HasBlockData(arm.Motors[0],"Resetting"))
		bool.TryParse(GetBlockData(arm.Motors[0],"Resetting"),out Resetting);
	if(worry_reset&&Resetting&&(!toggling)&&toggle){
		ResetTimer+=seconds_since_last_update;
		Write("Resetting Arm... ("+Math.Round(ResetTimer,3)+"s)");
		if(ResetTimer>=5){
			ResetTimer=0;
			Resetting=false;
		}
		else{
			foreach(IMyMotorStator Motor in arm.AllMotors)
				SetAngle(Motor,new Angle(0));
			do_main_loop=false;
		}
		if(Resetting){
			foreach(IMyInteriorLight Light in arm.AllLights){
				Light.Color=new Color(255,255,255,255);
				Light.Radius=10;
				Light.Intensity=5;
			}
		}
		else{
			foreach(IMyInteriorLight Light in arm.AllLights)
				Light.Color=DEFAULT_TEXT_COLOR;
		}
	}
	
	if(do_main_loop){
		for(int i=0;i<arm.Motors.Count;i++){
			IMyMotorStator Motor=arm.Motors[i];
			Angle Current=Angle.FromRadians(Motor.Angle);
			float speed_multx=(float)Math.Pow(1+((i+1.0)/arm.Motors.Count),2);
			if(arm.Light!=null)
				speed_multx*=(float)Math.Max(0.1,Math.Min(1,(position-arm.Light.GetPosition()).Length()/2));
			speed=(float)(distance/arm.MaxLength*speed_multx)/2;
			Angle Target=GetTargetAngle(arm,i,position);
			float RPM=GetRPM(Motor);
			Vector3D Velocity=GetVelocity(Motor);
			Max_RPM=Math.Max(Max_RPM,Math.Abs(RPM));
			Max_Speed=(float)Math.Max(Max_Speed,Velocity.Length());
			
			if((!hinge_1)&&IsHinge(Motor)){
				Target=GetWindTarget(arm,i,position);
				hinge_1=true;
			}
			
			float angle_difference=Math.Abs(Current.Difference(Target));
			
			if(i==arm.Motors.Count-1&&IsRotor(Motor)&&Gravity.Length()!=0){
				Vector3D Left=LocalToGlobal(new Vector3D(-1,0,0),Motor.Top);
				float Difference=(float)(GetAngle(Left,Gravity)-GetAngle(-1*Left,Gravity));
				Target=Angle.FromRadians(Motor.Angle)+Difference;
				speed*=5;
				Motor.RotorLock=false;
			}
			else{
				max_difference=Math.Max(max_difference,angle_difference);
				Motor.RotorLock=(angle_difference<1);
			}
			
			if((RPM>0)^(Current>=Target))
				speed*=2;
			else if(Math.Abs(RPM)>Math.Abs(Current.Difference(Target)))
				speed/=2;
			if(distance_from_target<2)
				speed*=Math.Max(0.05f,(float)(distance_from_target)/2);
			speed*=(float)Math.Min(1.5,Math.Abs(((max_difference+angle_difference)/2)/RPM));
			
			if(percent<1){
				float deg=Target.Degrees;
				if(deg>=180)
					deg-=360;
				deg*=percent;
				Target=new Angle(deg);
			}
			if(Current.Difference(Target)>1){
				moving=SetClosest(Motor,Target,speed);
			}
			else
				Motor.TargetVelocityRPM=0;
		}
		if(worry_reset&&(!toggling)&&toggle){
			if((Max_RPM<2.5||Max_Speed<0.5f||Max_RPM*Max_Speed<2.5)&&distance_from_target>2){
				ResetTimer+=seconds_since_last_update;
				if(ResetTimer>=5){
					ResetTimer=0;
					Resetting=true;
				}
			}
			else
				ResetTimer=Math.Max(0,ResetTimer-seconds_since_last_update);
			Write("Testing Reset:\n"+Math.Round(Max_RPM,1)+"RPM; "+Math.Round(Max_Speed,1)+"mps; "+Math.Round(ResetTimer,3)+"s");
		}
	}
	
	SetBlockData(arm.Motors[0],"ResetTimer",ResetTimer.ToString());
	SetBlockData(arm.Motors[0],"Resetting",Resetting.ToString());
	
	if(arm.Light!=null){
		if(!Resetting){
			if(LightTimer>=BLINK_TIMER_LIMIT||LightTimer==0){
				if(LightTimer!=0){
					LightTimer=0;
					do{
						LightMultx_R=Rnd.Next(-1,2);
					} while(LightMultx_R==0);
					do{
						LightMultx_G=Rnd.Next(-1,2);
					} while(LightMultx_G==0);
					do{
						LightMultx_B=Rnd.Next(-1,2);
					} while(LightMultx_B==0);
				}
				arm.Light.Intensity=(float)Math.Max(1,Math.Min(10,(distance/(arm.MaxLength*2))*5));
				arm.Light.Radius=10;
				arm.Light.Color=DEFAULT_TEXT_COLOR;
			}
			else{
				int r=arm.Light.Color.R;
				int g=arm.Light.Color.G;
				int b=arm.Light.Color.B;
				switch(Rnd.Next(0,3)){
					case 1:
						try{
							if(Sensor.LastDetectedEntity.Relationship>=MyRelationsBetweenPlayerAndBlock.Neutral)
								r+=Rnd.Next(0,5);
							else
								r+=Rnd.Next(0,3)*LightMultx_R;
						}
						catch(Exception){
							r+=Rnd.Next(0,3)*LightMultx_R;
						}
						break;
					case 2:
						g+=Rnd.Next(0,3)*LightMultx_G;
						break;
					case 3:
						try{
							if(Sensor.LastDetectedEntity.Relationship==MyRelationsBetweenPlayerAndBlock.Owner)
								b+=Rnd.Next(0,5);
							else
								b+=Rnd.Next(0,3)*LightMultx_B;
						}
						catch(Exception){
							b+=Rnd.Next(0,3)*LightMultx_B;
						}
						break;
				}
				arm.Light.Color=new Color(r,g,b,255);
			}
		}
		
		if(arm.Hand.Count>0){
			Vector3D hand_target=(arm.Light.GetPosition()-arm.Motors[arm.Motors.Count-1].Top.GetPosition());
			hand_target.Normalize();
			hand_target=actual_position+1*hand_target;
			double grab_distance=(arm.Light.GetPosition()-hand_target).Length();
			if(grab_distance<1||(grabbed&&grab_distance<2)){
				grabbed=true;
				foreach(Arm Finger in arm.Hand)
					SetPosition(Finger,hand_target,1,false);
			}
			else{
				grabbed=false;
				foreach(Arm Finger in arm.Hand){
					SetPosition(Finger,hand_target,.5f,false);
				}
			}
			
		}
	}
	else if(arm.Hand.Count>0){
		foreach(Arm Finger in arm.Hand){
			SetPosition(Finger,position,1,false);
		}
	}
	return moving;
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

bool SetClosest(IMyMotorStator Motor,Angle Next_Angle,float Speed_Multx=1,float Precision=0.1f){
	bool can_increase=true;
	bool can_decrease=true;
	Angle Motor_Angle=Angle.FromRadians(Motor.Angle);
	if(Motor.UpperLimitDeg!=float.MaxValue)
		can_increase=Angle.IsBetween(Motor_Angle,Next_Angle,new Angle(Motor.UpperLimitDeg));
	if(Motor.LowerLimitDeg!=float.MinValue)
		can_decrease=Angle.IsBetween(new Angle(Motor.LowerLimitDeg),Next_Angle,Motor_Angle);
	if(Motor_Angle.Difference_From_Top(Next_Angle)<Motor_Angle.Difference_From_Bottom(Next_Angle)){
		if(!can_increase)
			Next_Angle=new Angle(Motor.UpperLimitDeg);
	}
	else{
		if(!can_decrease)
			Next_Angle=new Angle(Motor.LowerLimitDeg);
	}
	if(Math.Abs(Motor_Angle.Difference(Next_Angle))<1)
		return false;
	SetAngle(Motor,Next_Angle,Speed_Multx,Precision);
	return true;
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
		float current_rpm=GetRPM(Motor);
		Speed_Multx*=Math.Max(0.1f,(20-Math.Abs(current_rpm))/10);
		if(From_Bottom<From_Top)
			target_rpm=(float)(-1*From_Bottom*Speed_Multx*Precision*5);
		else
			target_rpm=(float)(From_Top*Speed_Multx*Precision*5);
		Motor.TargetVelocityRPM=Math.Max(-10,Math.Min(10,target_rpm));
	}
	else{
		Motor.TargetVelocityRPM=0;
		//Motor.RotorLock=true;
	}
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
	
	Vector3D Current_Position=Motor.GetPosition()-Controller.GetPosition();
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
	if(LightTimer<BLINK_TIMER_LIMIT)
		LightTimer+=seconds_since_last_update;
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

Queue<int> rndqueue=new Queue<int>();
float[] rndarr;
void RndQueue(){
	if(cycle%10==0||Runtime.UpdateFrequency!=UpdateFrequency.Update1){
		int n=Rnd.Next(0,10);
		for(int i=0;i<10;i++){
			rndarr[i]*=.999f;
			if(i==n)
				rndarr[i]+=0.1f;
		}
		rndqueue.Enqueue(n);
		if(rndqueue.Count>20)
			rndqueue.Dequeue();
	}
	
	
	string rndstr="[";
	for(int i=rndqueue.Count;i<20;i++)
		rndstr+=' ';
	foreach(int n in rndqueue)
		rndstr+=n.ToString();
	Write(rndstr+']');
	for(int i=0;i<10;i++){
		Echo(i.ToString()+":"+Math.Round(rndarr[i],1).ToString()+'%');
	}
}

double Toggle_Timer=0;
void Toggle(){
	Toggle_Timer+=seconds_since_last_update;
	int max_length=0;
	foreach(Arm arm in Arms)
		max_length=Math.Max(max_length,arm.MotorCount);
	double arm_timer_count=25.0/max_length;
	Write("Toggling:"+Math.Round(Toggle_Timer,3).ToString()+"s / "+Math.Round(arm_timer_count,3).ToString()+"s");
	if(Toggle_Timer>=arm_timer_count){
		Toggle_Timer=0;
		toggling=false;
		foreach(Arm arm in Arms){
			if(toggle){
				for(int i=0;i<arm.AllMotors.Count;i++){
					if(arm.AllMotors[i].Enabled!=toggle){
						toggling=true;
						arm.AllMotors[i].Enabled=true;
						if(HasBlockData(arm.AllMotors[i],"DefaultBrakingTorque")){
							float temp;
							if(float.TryParse(GetBlockData(arm.AllMotors[i],"DefaultBrakingTorque"),out temp))
								arm.AllMotors[i].BrakingTorque=temp;
						}
						SetAngle(arm.AllMotors[i],new Angle(0),10,10);
						arm.AllMotors[i].TargetVelocityRPM*=6;
						break;
					}
				}
			}
			else{
				foreach(IMyInteriorLight Light in arm.AllLights)
					Light.Enabled=false;
				for(int i=arm.AllMotors.Count-1;i>=0;i--){
					if(arm.AllMotors[i].Enabled!=toggle){
						toggling=true;
						arm.AllMotors[i].Enabled=false;
						SetBlockData(arm.AllMotors[i],"DefaultBrakingTorque",arm.AllMotors[i].BrakingTorque.ToString());
						arm.AllMotors[i].BrakingTorque=1;
						break;
					}
				}
			}
		}
	}
	foreach(Arm arm in Arms){
		if(toggle){
			if(!toggling){
				foreach(IMyInteriorLight Light in arm.AllLights)
					Light.Enabled=true;
			}
			for(int i=0;i<arm.AllMotors.Count;i++){
				if(!arm.AllMotors[i].Enabled){
					Write(Math.Round((i*100.0f)/arm.AllMotors.Count,1).ToString()+"% complete");
					break;
				}
			}
		}
		else{
			for(int i=arm.Motors.Count-1;i>=0;i--){
				if(arm.AllMotors[i].Enabled){
					Write(Math.Round(((arm.AllMotors.Count-i)*100.0f)/arm.AllMotors.Count,1).ToString()+"% complete");
					break;
				}
			}
		}
	}	
	if(toggle^(!toggling)){
		if(toggle){
			Me.GetSurface(0).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
			Me.GetSurface(1).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		}
		else{
			Me.GetSurface(0).BackgroundColor=new Color(0,0,0,255);
			Me.GetSurface(1).BackgroundColor=new Color(0,0,0,255);
		}
	}
}

bool toggle=true;
bool toggling=false;
Vector3D Last_Input=new Vector3D(0,0,0);
double rnd_timer=0;
public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	RndQueue();
	List<IMyMotorStator> all_motors=new List<IMyMotorStator>();
	GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(all_motors);
	foreach(IMyMotorStator Motor in all_motors)
		UpdateMotor(Motor);
	foreach(Arm arm in Arms){
		foreach(IMyInteriorLight Light in arm.AllLights)
			Light.Enabled=toggle&&(!toggling);
	}
	if(toggle||toggling)
		Runtime.UpdateFrequency=UpdateFrequency.Update1;
	else
		Runtime.UpdateFrequency=UpdateFrequency.Update100;
	
	if(argument.Length>0){
		if(argument.ToLower().Equals("toggle")){
			toggling=true;
			toggle=!toggle;
			Toggle_Timer=0;
		}
		else{
			MyWaypointInfo waypoint;
			Vector3D coords;
			bool moving=false;
			if(MyWaypointInfo.TryParse(argument,out waypoint)){
				moving=true;
				coords=waypoint.Coords;
			}
			else if(argument.Length>10&&MyWaypointInfo.TryParse(argument.Substring(0,argument.Length-10),out waypoint)){
				moving=true;
				coords=waypoint.Coords;
			}
			else
				moving=Vector3D.TryParse(argument,out coords);
			if(moving){
				Last_Input=coords;
			}
		}
	}
	bool run_it=false;
	try{
		if(Sensor.IsActive&&Sensor.LastDetectedEntity.Type!=MyDetectedEntityType.None){
			Last_Input=Sensor.LastDetectedEntity.Position;
			run_it=true;
		}
	}
	catch(Exception){
		run_it=false;
	}
	if(toggling){
		Toggle();
		if(toggle)
			Write("Powering Up");
		else
			Write("Powering Down");
	}
	else if(toggle)
		Write("Powered Up");
	else
		Write("Powered Down");
	
	//Write(Last_Input.ToString());
	
	if(run_it){
		foreach(Arm arm in Arms)
			SetPosition(arm,Last_Input);
		rnd_timer=0;
	}
	else{
		if(true){
			rnd_timer+=seconds_since_last_update;
			if(rnd_timer>=5||Last_Input.Length()==0){
				rnd_timer=0;
				do{
					int x=Rnd.Next(-10,11);
					int y=Rnd.Next(-10,11);
					int z=Rnd.Next(-10,11);
					Last_Input=new Vector3D(x,y,z);
				} while(Last_Input.Length()==0);
				Last_Input.Normalize();
				Last_Input*=Rnd.Next((int)(Arms[0].MaxLength/2),20);
				Last_Input+=Arms[0].Motors[0].GetPosition();
			}
			foreach(Arm arm in Arms)
				SetPosition(arm,Last_Input);
		}
		else{
			foreach(Arm arm in Arms){
				for(int i=0;i<arm.Motors.Count;i++){
					IMyMotorStator Motor=arm.Motors[i];
					Motor.TargetVelocityRPM=0;
				}
				if(arm.Light!=null){
					arm.Light.Intensity=2.5f;
					arm.Light.Radius=5;
				}
			}
		}
	}
    
}