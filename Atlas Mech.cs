const string ProgramName = "Atlas Walker Mech"; //Name me!
Color DefaultTextColor=new Color(197,137,255,255);
Color DefaultBackgroundColor=new Color(44,0,88,255);

// Runtime Logic and Methods
public void Main(string argument,UpdateType updateSource){
	UpdateProgramInfo();
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
}


// Initialization and Object Definitions
public Program(){
	Echo("Beginning initialization.");
	Prog.P=this;
	Me.CustomName=(ProgramName+" Programmable block").Trim();
	for(int i=0;i<Me.SurfaceCount;i++){
		Me.GetSurface(i).FontColor=DefaultTextColor;
		Me.GetSurface(i).BackgroundColor=DefaultBackgroundColor;
		Me.GetSurface(i).Alignment=TextAlignment.CENTER;
		Me.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
	}
	Me.GetSurface(1).FontSize=2.2f;
	Me.GetSurface(1).TextPadding=40.0f;
	
	// Initialize runtime objects
	Echo("Initializing objects...");
	Controller=CollectionMethods<IMyShipController>.ByName("Mech Main");
	
	
	// Load runtime variables from CustomData
	Echo("Setting variables...");
	
	
	// Load data from Storage
	Echo("Loading data...");
	string storageMode="";
	string[] storageArgs=this.Storage.Trim().Split('\n');
	foreach(string line in storageArgs){
		switch(line){
			case "SampleData":
				storageMode=line;
				break;
			default:
				switch(storageMode){
					case "SampleData":
						// Load Data from line
						break;
				}
				break;
		}
	}
	
	Runtime.UpdateFrequency=UpdateFrequency.None;
	Echo("Completed initialization!");
}

IMyShipController Controller{
	get{
		return Prog.Controller;
	}
	set{
		Prog.Controller=value;
	}
}
MyLeg LeftLeg;
MyLeg RightLeg;


public enum MyParityStatus{
	NotReady,
	Return,
	Testing,
	Reset,
	Ready
}
public interface IMyJointController{
	MyParityStatus ParityStatus();
	bool Parity();
	
	bool GoForward(float multx);
	bool GoForward();
	bool GoBackward(float multx);
	bool GoBackward();
	bool Stop();
	
	float Position();
	
	bool SetParity(Base6Directions.Direction direction);
}
public abstract class MyJointController<T>:IMyJointController where T:class,IMyMechanicalConnectionBlock{
	public T Block;
	protected IMyCubeBlock Connection;
	protected Vector3I LengthVector;
	protected IMyCubeGrid BottomGrid;
	protected IMyCubeGrid TopGrid;
	protected double JointLength{
		get{
			return VectorLength(LengthVector-Connection.Position);
		}
	}
	
	protected MyJointController(T block,IMyCubeBlock connection){
		Block=block;
		Connection=connection;
	}
	
	protected Vector3D GetEndPosition(){
		return GenMethods.GlobalToLocalPosition(
		GenMethods.LocalToGlobalPosition(LengthVector,Connection),
		Prog.Controller);
	}
	
	public MyParityStatus ParityStatus(){
		MyParityStatus output=MyParityStatus.NotReady;
		if(GenMethods.HasBlockData(Block,"ParityStatus")){
			string str=GenMethods.GetBlockData(Block,"ParityStatus");
			output=(MyParityStatus)Enum.Parse(typeof(MyParityStatus),str);
		}
		return output;
	}
	
	public bool Parity(){
		return GenMethods.GetBlockData<bool>(Block,"Parity",bool.Parse);
	}
	
	void LengthVectorHelper(Dictionary<int,List<Vector3I>> positions,Vector3I start,Vector3I pos){
		int distance=(pos-start).Length();
		for(int i=0;i<6;i++){
			int x=0,y=0,z=0;
			switch(i){
				case 0:
					x=1;
					break;
				case 1:
					x=-1;
					break;
				case 2:
					y=1;
					break;
				case 3:
					y=-1;
					break;
				case 4:
					z=1;
					break;
				case 5:
					z=-1;
					break;
			}
			Vector3I next=pos+(new Vector3I(x,y,z));
			int nextDistance=(next-start).Length();
			if(nextDistance<=distance)
				continue;
			if(!TopGrid.CubeExists(next))
				continue;
			if(!positions.ContainsKey(nextDistance))
				positions.Add(nextDistance,new List<Vector3I>());
			if(positions[nextDistance].Contains(next))
				continue;
			positions[nextDistance].Add(next);
		}
	}
	
	bool LengthVectorHelper(Dictionary<int,List<Vector3I>> positions,Vector3I start,int distance){
		foreach(Vector3I pos in positions[distance]){
			LengthVectorHelper(positions,start,pos);
		}
		return positions[distance+1].Count>0;
	}
	
	protected Vector3I GetLengthVector(){
		if(VectorLength(LengthVector)>0)
			return LengthVector;
		if(GenMethods.HasBlockData(Block,"LengthVector")){
			Vector3I output;
			if(Vector3I.TryParseFromString(GenMethods.GetBlockData(Block,"LengthVector"),out output)){
				LengthVector=output;
				return LengthVector;
			}
		}
		Vector3I start=Connection.Position;
		Dictionary<int,List<Vector3I>> positions=new Dictionary<int,List<Vector3I>>();
		positions.Add(0,new List<Vector3I>());
		positions[0].Add(start);
		int distance=0;
		while(LengthVectorHelper(positions,start,distance))
			distance++;
		LengthVector=positions[distance][0]-start;
		return LengthVector;
	}
	
	public static double VectorLength(Vector3I v){
		return (new Vector3D(v.X,v.Y,v.Z)).Length();
	}
	
	
	public bool GoForward(){
		float multx=1;
		if(GenMethods.HasBlockData(Block,"ForwardMultx"))
			multx=GenMethods.GetBlockData<float>(Block,"ForwardMultx",float.Parse);
		else if(GenMethods.HasBlockData(Block,"Multx"))
			multx=GenMethods.GetBlockData<float>(Block,"Multx",float.Parse);
		return GoForward(multx);
	}
	
	public bool GoBackward(){
		float multx=1;
		if(GenMethods.HasBlockData(Block,"BackwardMultx"))
			multx=GenMethods.GetBlockData<float>(Block,"BackwardMultx",float.Parse);
		else if(GenMethods.HasBlockData(Block,"Multx"))
			multx=GenMethods.GetBlockData<float>(Block,"Multx",float.Parse);
		return GoForward(multx);
	}
	
	public abstract bool GoForward(float multx);
	public abstract bool GoBackward(float multx);
	public abstract bool Stop();
	public abstract float Position();
	public abstract bool SetParity(Base6Directions.Direction direction);
	
}
class MyJointRotor:MyJointController<IMyMotorStator>{
	public IMyMotorStator Rotor{
		get{
			return Block;
		}
		set{
			Block=value;
		}
	}
	
	public MyJointRotor(IMyMotorStator rotor):base(rotor,rotor.Top){
		BottomGrid=rotor.CubeGrid;
		TopGrid=rotor.TopGrid;
	}
	
	public override bool GoForward(float multx){
		MyAngle Angle=MyAngle.FromRadians(Rotor.Angle);
		MyAngle Limit=MyAngle.FromRadians(Rotor.UpperLimitRad);
		bool atLimit=Limit.FromBottom(Angle)<0.1;
		Rotor.RotorLock=atLimit;
		Rotor.TargetVelocityRPM=atLimit?0:3*multx*(Parity()?1:-1);
		return !atLimit;
	}
	
	public override bool GoBackward(float multx){
		MyAngle Angle=MyAngle.FromRadians(Rotor.Angle);
		MyAngle Limit=MyAngle.FromRadians(Rotor.LowerLimitRad);
		bool atLimit=Limit.FromTop(Angle)<0.1;
		Rotor.RotorLock=atLimit;
		Rotor.TargetVelocityRPM=atLimit?0:3*multx*(Parity()?-1:1);
		return !atLimit;
	}
	
	public override bool Stop(){
		Rotor.TargetVelocityRPM=0;
		Rotor.RotorLock=true;
		return true;
	}
	
	public override float Position(){
		return (float)(180*Rotor.Angle/Math.PI);
	}
	
	Vector3D DefaultPos;
	Vector3D ForwardPos;
	public override bool SetParity(Base6Directions.Direction direction){
		MyParityStatus status=ParityStatus();
		MyAngle angle=MyAngle.FromRadians(Rotor.Angle);
		if(status==MyParityStatus.NotReady)
			status=MyParityStatus.Return;
		if(status==MyParityStatus.Return){
			Rotor.TargetVelocityRad=Rotor.Angle/-2;
			if(angle.Difference(new MyAngle(0))<1){
				DefaultPos=GetEndPosition();
				status=MyParityStatus.Testing;
				Rotor.TargetVelocityRPM=3;
			}
		}
		if(status==MyParityStatus.Testing){
			Rotor.TargetVelocityRPM=(angle-30).Degrees/-2;
			if(angle>new MyAngle(25)){
				Vector3D ForwardPos=GetEndPosition();
				double difference=GenMethods.DirectionComp(ForwardPos-DefaultPos,direction);
				if(Math.Abs(difference)>=0.5){
					Rotor.TargetVelocityRPM=-3;
					status=MyParityStatus.Reset;
				}
			}
		}
		if(status==MyParityStatus.Reset){
			Rotor.TargetVelocityRad=Rotor.Angle/-2;
			if(angle.Difference(new MyAngle(0))<1){
				Rotor.TargetVelocityRPM=0;
				GenMethods.SetBlockData(Block,"Parity",((ForwardPos-DefaultPos).Z<0).ToString());
				status=MyParityStatus.Ready;
			}
		}
		GenMethods.SetBlockData(Rotor,"ParityStatus",status.ToString());
		return status==MyParityStatus.Ready;
	}
}
class MyJointPneumatic:MyJointController<IMyPistonBase>{
	public IMyPistonBase Piston{
		get{
			return Block;
		}
		set{
			Block=value;
		}
	}
	public IMyMotorStator BaseRotor;
	public IMyMotorStator TopRotor;
	
	protected MyJointPneumatic(IMyPistonBase piston,IMyMotorStator baseRotor,IMyMotorStator topRotor,IMyCubeBlock jointTop):base(piston,jointTop){
		BaseRotor=baseRotor;
		TopRotor=topRotor;
		BottomGrid=baseRotor.CubeGrid;
		TopGrid=topRotor.TopGrid;
	}
	
	
	public override bool GoForward(float multx){
		Piston.Velocity=0.5f*multx*(Parity()?1:-1);
		return Piston.Status==PistonStatus.Extending||Piston.Status==PistonStatus.Retracting;
	}
	
	public override bool GoBackward(float multx){
		Piston.Velocity=0.5f*multx*(Parity()?-1:1);
		return Piston.Status==PistonStatus.Extending||Piston.Status==PistonStatus.Retracting;
	}
	
	public override bool Stop(){
		Piston.Velocity=0;
		return Piston.Status!=PistonStatus.Extending&&Piston.Status!=PistonStatus.Retracting;
	}
	
	public override float Position(){
		float position=Piston.CurrentPosition;
		float high=Piston.HighestPosition;
		float low=Piston.LowestPosition;
		float middle=(high-low)/2+low;
		if(GenMethods.HasBlockData(Piston,"DefaultPosition"))
			middle=GenMethods.GetBlockData<float>(Piston,"DefaultPosition",float.Parse);
		return position-middle;
	}
	
	Vector3D DefaultPos;
	Vector3D ForwardPos;
	public override bool SetParity(Base6Directions.Direction direction){
		MyParityStatus status=ParityStatus();
		float position=Piston.CurrentPosition;
		float high=Piston.HighestPosition;
		float low=Piston.LowestPosition;
		float middle=(high-low)/2+low;
		if(GenMethods.HasBlockData(Piston,"DefaultPosition"))
			middle=GenMethods.GetBlockData<float>(Piston,"DefaultPosition",float.Parse);
		if(status==MyParityStatus.NotReady)
			status=MyParityStatus.Return;
		if(status==MyParityStatus.Return){
			Piston.Velocity=(middle-position)/2;
			if(Math.Abs(position-middle)<.1){
				DefaultPos=GetEndPosition();
				status=MyParityStatus.Testing;
				Piston.Velocity=1;
			}
		}
		if(status==MyParityStatus.Testing){
			Piston.Velocity=(high-position)/2;
			if(position>=(high-middle)*.8f+middle){
				Vector3D ForwardPos=GetEndPosition();
				double difference=GenMethods.DirectionComp(ForwardPos-DefaultPos,direction);
				if(Math.Abs(difference)>=0.5){
					Piston.Velocity=-1;
					status=MyParityStatus.Reset;
				}
			}
		}
		if(status==MyParityStatus.Reset){
			Piston.Velocity=(middle-position)/2;
			if((position-middle)<=.05){
				Piston.Velocity=0;
				GenMethods.SetBlockData(Block,"Parity",((ForwardPos-DefaultPos).Z<0).ToString());
				status=MyParityStatus.Ready;
			}
		}
		GenMethods.SetBlockData(Block,"ParityStatus",status.ToString());
		return status==MyParityStatus.Ready;
	}
	
	public static bool ValidPneumatic(IMyMotorStator baseRotor,IMyMotorStator joint){
		if(baseRotor.CubeGrid!=joint.CubeGrid)
			return false;
		IMyPistonBase piston=CollectionMethods<IMyPistonBase>.ByGrid(baseRotor.TopGrid);
		if(piston==null)
			return false;
		IMyMotorStator topRotor=CollectionMethods<IMyMotorStator>.ByGrid(piston.TopGrid);
		return topRotor!=null&&topRotor.TopGrid==joint.TopGrid;
	}
	
	public static MyJointPneumatic TryGet(IMyMotorStator joint){
		IMyMotorStator baseRotor=CollectionMethods<IMyMotorStator>.ByFunc<IMyMotorStator>(ValidPneumatic,joint,CollectionMethods<IMyMotorStator>.AllByGrid(joint.CubeGrid));
		if(baseRotor==null)
			return null;
		IMyPistonBase piston=CollectionMethods<IMyPistonBase>.ByGrid(baseRotor.TopGrid);
		IMyMotorStator topRotor=CollectionMethods<IMyMotorStator>.ByGrid(piston.TopGrid);
		return new MyJointPneumatic(piston,baseRotor,topRotor,joint.Top);
	}
}
class MyJoint{
	public IMyMotorStator Rotor;
	public IMyJointController Joint;
	
	public MyJoint(IMyMotorStator rotor){
		Rotor=rotor;
		MyJointPneumatic pneumatic=MyJointPneumatic.TryGet(rotor);
		if(pneumatic!=null)
			Joint=new MyJointRotor(Rotor);
		else
			Joint=pneumatic;
	}
	
	
}

class MyLeg{
	public MyJoint Hip;
	public MyJoint Knee;
	public MyJoint Ankle;
	
	
}


// Saving and Data Storage Classes
public void Save(){
    // Reset Storage
	this.Storage="";
	
	// Save Data to Storage
	this.Storage+="\n"+"SampleData";
	this.Storage+="\n"+"lorem ipsum";
	
	// Update runtime variables from CustomData
	
	
	// Reset CustomData
	Me.CustomData="";
	
	// Save Runtime Data to CustomData
	
	
}

public struct MyAngle{
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
	
	public MyAngle(float degrees){
		_Degrees=degrees;
		Degrees=degrees;
	}
	
	public static MyAngle FromRadians(float Rads){
		return new MyAngle((float)(Rads/Math.PI*180));
	}
	
	public float FromTop(MyAngle other){
		if(other.Degrees>=Degrees)
			return other.Degrees-Degrees;
		return other.Degrees-Degrees+360;
	}
	
	public float FromBottom(MyAngle other){
		if(other.Degrees<=Degrees)
			return Degrees-other.Degrees;
		return Degrees-other.Degrees+360;
	}
	
	public float Difference(MyAngle other){
		return Math.Min(FromTop(other),FromBottom(other));
	}
	
	public static bool IsBetween(MyAngle Bottom, MyAngle Middle, MyAngle Top){
		return Bottom.FromTop(Middle)<=Bottom.FromTop(Top);
	}
	
	public static bool TryParse(string parse,out MyAngle output){
		output=new MyAngle(0);
		float d;
		if(!float.TryParse(parse.Substring(0,Math.Max(0,parse.Length-1)),out d))
			return false;
		output=new MyAngle(d);
		return true;
	}
	
	public static MyAngle operator +(MyAngle a1, MyAngle a2){
		return new MyAngle(a1.Degrees+a2.Degrees);
	}
	
	public static MyAngle operator +(MyAngle a1, float a2){
		return new MyAngle(a1.Degrees+a2);
	}
	
	public static MyAngle operator +(float a1, MyAngle a2){
		return a2 + a1;
	}
	
	public static MyAngle operator -(MyAngle a1, MyAngle a2){
		return new MyAngle(a1.Degrees-a2.Degrees);
	}
	
	public static MyAngle operator -(MyAngle a1, float a2){
		return new MyAngle(a1.Degrees-a2);
	}
	
	public static MyAngle operator -(float a1, MyAngle a2){
		return new MyAngle(a1-a2.Degrees);
	}
	
	public static MyAngle operator *(MyAngle a1, float m){
		return new MyAngle(a1.Degrees*m);
	}
	
	public static MyAngle operator *(float m, MyAngle a2){
		return a2*m;
	}
	
	public static MyAngle operator /(MyAngle a1, float m){
		return new MyAngle(a1.Degrees/m);
	}
	
	public static bool operator ==(MyAngle a1, MyAngle a2){
		float degrees=(a1-a2).Degrees;
		if(degrees>180)
			degrees-=360;
		return Math.Abs(degrees)<0.000001f;
	}
	
	public static bool operator !=(MyAngle a1, MyAngle a2){
		return Math.Abs(a1.Degrees-a2.Degrees)>=0.000001f;
	}
	
	public override bool Equals(object o){
		return (o.GetType()==this.GetType()) && this==((MyAngle)o);
	}
	
	public override int GetHashCode(){
		return Degrees.GetHashCode();
	}
	
	public static bool operator >(MyAngle a1, MyAngle a2){
		return a1.FromTop(a2)>a1.FromBottom(a2);
	}
	
	public static bool operator >=(MyAngle a1, MyAngle a2){
		return (a1==a2)||(a1>a2);
	}
	
	public static bool operator <=(MyAngle a1, MyAngle a2){
		return (a1==a2)||(a1<a2);
	}
	
	public static bool operator <(MyAngle a1, MyAngle a2){
		return a1.FromTop(a2)<a1.FromBottom(a2);
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
	
	public string LinedString(MyAngle Comparison){
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

// Core Components
TimeSpan TimeSinceStart=new TimeSpan(0);
long Cycle=0;
char LoadingChar='|';
double SecondsSinceLastUpdate=0;

static class Prog{
	public static MyGridProgram P;
	public static IMyShipController Controller;
}

static class GenMethods{
	private static double GetAngle(Vector3D v1,Vector3D v2,int i){
		v1.Normalize();
		v2.Normalize();
		double output=Math.Round(Math.Acos(v1.X*v2.X+v1.Y*v2.Y+v1.Z*v2.Z)*180/Math.PI,5);
		if(i>0&&output.ToString().Equals("NaN")){
			Random Rnd=new Random();
			Vector3D v3=new Vector3D(Rnd.Next(0,10)-5,Rnd.Next(0,10)-5,Rnd.Next(0,10)-5);
			v3.Normalize();
			if(Rnd.Next(0,1)==1)
				output=GetAngle(v1+v3/360,v2,i-1);
			else
				output=GetAngle(v1,v2+v3/360,i-1);
		}
		return output;
	}
	
	public static double GetAngle(Vector3D v1, Vector3D v2){
		return GetAngle(v1,v2,10);
	}
	
	public static bool HasBlockData(IMyTerminalBlock Block, string Name){
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

	public static string GetBlockData(IMyTerminalBlock Block, string Name){
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
	
	public static T GetBlockData<T>(IMyTerminalBlock Block,string Name,Func<string,T> F){
		return F(GetBlockData(Block,Name));
	}
	
	public static bool SetBlockData(IMyTerminalBlock Block, string Name, string Data){
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

	public static bool WipeBlockData(IMyTerminalBlock Block,string Name){
		if(Name.Contains(':'))
			return false;
		string[] args=Block.CustomData.Split('•');
		for(int i=0; i<args.Count(); i++){
			if(args[i].IndexOf(Name+':')==0){
				Block.CustomData="";
				for(int j=0; j<args.Count(); j++){
					if(j!=i)
						Block.CustomData+='•'+args[j];
				}
				return true;
			}
		}
		return false;
	}

	public static Vector3D GlobalToLocal(Vector3D Global,IMyCubeBlock Ref){
		Vector3D Local=Vector3D.Transform(Global+Ref.GetPosition(),MatrixD.Invert(Ref.WorldMatrix));
		Local.Normalize();
		return Local*Global.Length();
	}

	public static Vector3D GlobalToLocalPosition(Vector3D Global,IMyCubeBlock Ref){
		Vector3D Local=Vector3D.Transform(Global,MatrixD.Invert(Ref.WorldMatrix));
		Local.Normalize();
		return Local*(Global-Ref.GetPosition()).Length();
	}

	public static Vector3D LocalToGlobal(Vector3D Local,IMyCubeBlock Ref){
		Vector3D Global=Vector3D.Transform(Local,Ref.WorldMatrix)-Ref.GetPosition();
		Global.Normalize();
		return Global*Local.Length();
	}

	public static Vector3D LocalToGlobalPosition(Vector3D Local,IMyCubeBlock Ref){
		return Vector3D.Transform(Local,Ref.WorldMatrix);
	}

	public static List<T> Merge<T>(List<T> L1,List<T> L2){
		return L1.Concat(L2).ToList();
	}
	
	public static double DirectionComp(Vector3D v,Base6Directions.Direction d){
		switch(d){
			case Base6Directions.Direction.Forward:
				return -1*v.Z;
			case Base6Directions.Direction.Backward:
				return v.Z;
			case Base6Directions.Direction.Up:
				return v.Y;
			case Base6Directions.Direction.Down:
				return -1*v.Y;
			case Base6Directions.Direction.Left:
				return -1*v.X;
			case Base6Directions.Direction.Right:
				return v.X;
		}
		return 0;
	}
}

static class CollectionMethods<T> where T:class,IMyTerminalBlock{
	public static MyGridProgram P{
		get{
			return Prog.P;
		}
	}
	
	private static List<T> Get_AllBlocks(){
		List<T> output=new List<T>();
		P.GridTerminalSystem.GetBlocksOfType<T>(output);
		return output;
	}
	public static Rool<T> AllBlocks=new Rool<T>(Get_AllBlocks);
	private static List<T> Get_AllConstruct(){
		List<T> output=new List<T>();
		foreach(T Block in AllBlocks){
			if(Block.IsSameConstructAs(P.Me))
				output.Add(Block);
		}
		return output;
	}
	public static Rool<T> AllConstruct=new Rool<T>(Get_AllConstruct);
	
	public static T ByFullName(string name,List<T> blocks){
		foreach(T Block in blocks){
			if(Block?.CustomName.Equals(name)??false)
				return Block;
		}
		return null;
	}
	
	public static T ByFullName(string name){
		return ByName(name,AllConstruct);
	}
	
	public static T ByName(string name,List<T> blocks){
		foreach(T Block in blocks){
			if(Block?.CustomName.Contains(name)??false)
				return Block;
		}
		return null;
	}
	
	public static T ByName(string name){
		return ByName(name,AllConstruct);
	}
	
	public static List<T> AllByName(string name,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block?.CustomName.Contains(name)??false)
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByName(string name){
		return AllByName(name,AllConstruct);
	}
	
	public static T ByDistance(Vector3D Ref,double distance,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&(Block.GetPosition()-Ref).Length()<=distance)
				return Block;
		}
		return null;
	}
	
	public static T ByDistance(Vector3D Ref,double distance){
		return ByDistance(Ref,distance,AllConstruct);
	}
	
	public static List<T> AllByDistance(Vector3D Ref,double distance,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&(Block.GetPosition()-Ref).Length()<=distance)
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByDistance(Vector3D Ref,double distance){
		return AllByDistance(Ref,distance,AllConstruct);
	}
	
	public static T ByClosest(Vector3D Ref,List<T> blocks){
		return (blocks.Count>0?SortByDistance(blocks,Ref)[0]:null);
	}
	
	public static T ByClosest(Vector3D Ref){
		return ByClosest(Ref,AllConstruct);
	}
	
	public static T ByFunc(Func<T,bool> f,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&f(Block))
				return Block;
		}
		return null;
	}
	
	public static T ByFunc(Func<T,bool> f){
		return ByFunc(f,AllConstruct);
	}
	
	public static List<T> AllByFunc(Func<T,bool> f,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&f(Block))
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByFunc(Func<T,bool> f){
		return AllByFunc(f,AllConstruct);
	}
	
	public static T ByFunc<U>(Func<T,U,bool> f,U param,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&f(Block,param))
				return Block;
		}
		return null;
	}
	
	public static T ByFunc<U>(Func<T,U,bool> f,U param){
		return ByFunc<U>(f,param,AllConstruct);
	}
	
	public static List<T> AllByFunc<U>(Func<T,U,bool> f,U param,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&f(Block,param))
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByFunc<U>(Func<T,U,bool> f,U param){
		return AllByFunc(f,param,AllConstruct);
	}
	
	public static T ByGrid(IMyCubeGrid Grid,List<T> blocks){
		foreach(T Block in blocks){
			if(Block!=null&&Block.CubeGrid==Grid)
				return Block;
		}
		return null;
	}
	
	public static T ByGrid(IMyCubeGrid Grid){
		return ByGrid(Grid,AllBlocks);
	}
	
	public static List<T> AllByGrid(IMyCubeGrid Grid,List<T> blocks){
		List<T> output=new List<T>();
		foreach(T Block in blocks){
			if(Block!=null&&Block.CubeGrid==Grid)
				output.Add(Block);
		}
		return output;
	}
	
	public static List<T> AllByGrid(IMyCubeGrid Grid){
		return AllByGrid(Grid,AllBlocks);
	}
	
	public static T ByDefinitionString(string def,List<T> blocks){
		if(def.ToLower().Equals(def)){
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.ToLower().Contains(def)??false)
					return Block;
			}
		}
		else{
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.Contains(def)??false)
					return Block;
			}
		}
		return null;
	}
	
	public static T ByDefinitionString(string def){
		return ByDefinitionString(def,AllConstruct);
	}
	
	public static List<T> AllByDefinitionString(string def,List<T> blocks){
		List<T> output=new List<T>();
		if(def.ToLower().Equals(def)){
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.ToLower().Contains(def)??false)
					output.Add(Block);
			}
		}
		else{
			foreach(T Block in blocks){
				if(Block?.DefinitionDisplayNameText.Contains(def)??false)
					output.Add(Block);
			}
		}
		return output;
	}
	
	public static List<T> AllByDefinitionString(string def){
		return AllByDefinitionString(def,AllConstruct);
	}
	
	public static List<T> SortByDistance(List<T> input,Vector3D Ref){
		if(input.Count<=1)
			return input;
		List<T> output=new List<T>();
		foreach(T block in input)
			output.Add(block);
		SortHelper(output,Ref,0,output.Count-1);
		return output;
	}
	
	private static void Swap(List<T> list,int i1,int i2){
		T temp=list[i1];
		list[i1]=list[i2];
		list[i2]=temp;
	}
	
	private static int SortPartition(List<T> sorting,Vector3D Ref,int low,int high){
		double pivot=(sorting[high].GetPosition()-Ref).Length();
		int i=low-1;
		for(int j=low;j<high;j++){
			if((sorting[j].GetPosition()-Ref).Length()<pivot)
				Swap(sorting,j,++i);
		}
		Swap(sorting,high,++i);
		return i;
	}
	
	private static void SortHelper(List<T> sorting,Vector3D Ref,int low,int high){
		if(low>=high)
			return;
		int pi=SortPartition(sorting,Ref,low,high);
		SortHelper(sorting,Ref,low,pi-1);
		SortHelper(sorting,Ref,pi+1,high);
	}
	
}

abstract class OneDone{
	public static List<OneDone> All;
	
	protected OneDone(){
		if(All==null)
			All=new List<OneDone>();
		All.Add(this);
	}
	
	public static void ResetAll(){
		if(All==null)
			return;
		for(int i=0;i<All.Count;i++)
			All[i].Reset();
	}
	
	public abstract void Reset();
}
class OneDone<T>:OneDone{
	private T Default;
	public T Value;
	
	public OneDone(T value):base(){
		Default=value;
		Value=value;
	}
	
	public override void Reset(){
		Value=Default;
	}
	
	public static implicit operator T(OneDone<T> O){
		return O.Value;
	}
}
class Rool<T>:IEnumerable<T>{
	// Run Only Once
	private List<T> _Value;
	public List<T> Value{
		get{
			if(!Ran.Value){
				_Value=Updater();
				Ran.Value=true;
			}
			return _Value;
		}
	}
	private OneDone<bool> Ran;
	private Func<List<T>> Updater;
	
	public Rool(Func<List<T>> updater){
		Ran=new OneDone<bool>(false);
		Updater=updater;
	}
	
	public IEnumerator<T> GetEnumerator(){
		return Value.GetEnumerator();
	}
	
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
	
	public static implicit operator List<T>(Rool<T> R){
		return R.Value;
	}
}
class Roo<T>{
	// Run Only Once
	private T _Value;
	public T Value{
		get{
			if(!Ran.Value){
				_Value=Updater();
				Ran.Value=true;
			}
			return _Value;
		}
	}
	private OneDone<bool> Ran;
	private Func<T> Updater;
	
	public Roo(Func<T> updater){
		Ran=new OneDone<bool>(false);
		Updater=updater;
	}
	
	public static implicit operator T(Roo<T> R){
		return R.Value;
	}
}

TimeSpan FromSeconds(double seconds){
	return (new TimeSpan(0,0,0,(int)seconds,(int)(seconds*1000)%1000));
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

void Write(string text,bool new_line=true,bool append=true){
	Echo(text);
	if(new_line)
		Me.GetSurface(0).WriteText(text+'\n', append);
	else
		Me.GetSurface(0).WriteText(text, append);
}

void UpdateProgramInfo(){
	OneDone.ResetAll();
	Cycle=(++Cycle)%long.MaxValue;
	switch(LoadingChar){
		case '|':
			LoadingChar='\\';
			break;
		case '\\':
			LoadingChar='-';
			break;
		case '-':
			LoadingChar='/';
			break;
		case '/':
			LoadingChar='|';
			break;
	}
	Write("",false,false);
	Echo(ProgramName+" OS\nCycle-"+Cycle.ToString()+" ("+LoadingChar+")");
	Me.GetSurface(1).WriteText(ProgramName+" OS\nCycle-"+Cycle.ToString()+" ("+LoadingChar+")",false);
	SecondsSinceLastUpdate=Runtime.TimeSinceLastRun.TotalSeconds+(Runtime.LastRunTimeMs/1000);
	Echo(ToString(FromSeconds(SecondsSinceLastUpdate))+" since last Cycle");
	TimeSinceStart=UpdateTimeSpan(TimeSinceStart,SecondsSinceLastUpdate);
	Echo(ToString(TimeSinceStart)+" since last reboot\n");
	Me.GetSurface(1).WriteText("\n"+ToString(TimeSinceStart)+" since last reboot",true);
}

