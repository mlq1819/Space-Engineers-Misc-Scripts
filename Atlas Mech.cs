const string ProgramName = "Atlas Mech"; //Name me!
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


// Core Components
TimeSpan TimeSinceStart=new TimeSpan(0);
long Cycle=0;
char LoadingChar='|';
double SecondsSinceLastUpdate=0;

static class Prog{
	public static MyGridProgram P;
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

