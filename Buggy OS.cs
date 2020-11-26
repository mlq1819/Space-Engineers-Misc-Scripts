const string Program_Name = "Buggy OS"; //Name me!
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
	if(Controller!=null){
		if(new_line)
			Controller.GetSurface(0).WriteText(text+'\n', append);
		else
			Controller.GetSurface(0).WriteText(text, append);
	}
}

long cycle_long = 1;
long cycle = 0;
char loading_char = '|';
double seconds_since_last_update = 0;

Random Rnd;

IMyCockpit Controller;
IMyGyro Gyroscope;
List<IMyLightingBlock> Brakelights;
List<IMyLightingBlock> Headlights;
List<IMyMotorSuspension> Wheels;

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

double Elevation{
	get{
		double output=double.MaxValue;
		Controller.TryGetPlanetElevation(MyPlanetElevation.Surface,out output);
		return output;
	}
}
Vector3D Gravity{
	get{
		return Controller.GetTotalGravity();
	}
}
Vector3D Gravity_Direction{
	get{
		Vector3D Direction=Gravity;
		Direction.Normalize();
		return Direction;
	}
}
Vector3D Current_LinearVelocity{
	get{
		return Controller.GetShipVelocities().LinearVelocity;
	}
}
Vector3D Current_AngularVelocity{
	get{
		return Controller.GetShipVelocities().AngularVelocity;
	}
}
Vector3D Relative_AngularVelocity{
	get{
		return GlobalToLocal(Current_AngularVelocity,Controller);
	}
}

public Program()
{
	Me.CustomName=(Program_Name+" Programmable block").Trim();
	Rnd=new Random();
	for(int i=0;i<Me.SurfaceCount;i++){
		Me.GetSurface(i).FontColor=DEFAULT_TEXT_COLOR;
		Me.GetSurface(i).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
		Me.GetSurface(i).Alignment=TextAlignment.CENTER;
		Me.GetSurface(i).ContentType=ContentType.TEXT_AND_IMAGE;
	}
	Me.GetSurface(1).FontSize=2.2f;
	Me.GetSurface(1).TextPadding=40.0f;
	Echo("Beginning initialization");
	Controller=(new GenericMethods<IMyCockpit>(this)).GetContaining("");
	Gyroscope=(new GenericMethods<IMyGyro>(this)).GetContaining("Control Gyroscope");
	if(Controller==null||Gyroscope==null)
		return;
	Controller.GetSurface(0).FontColor=DEFAULT_TEXT_COLOR;
	Controller.GetSurface(0).BackgroundColor=DEFAULT_BACKGROUND_COLOR;
	Controller.GetSurface(0).Alignment=TextAlignment.CENTER;
	Controller.GetSurface(0).ContentType=ContentType.TEXT_AND_IMAGE;
	Controller.GetSurface(0).WriteText("Hello World",false);
	Headlights=(new GenericMethods<IMyLightingBlock>(this)).GetAllContaining("Headlight");
	Brakelights=(new GenericMethods<IMyLightingBlock>(this)).GetAllContaining("Brake Light");
	Wheels=(new GenericMethods<IMyMotorSuspension>(this)).GetAllContaining("Wheel");
	Runtime.UpdateFrequency=UpdateFrequency.Update1;
}

public void Save()
{
	Gyroscope.GyroOverride=false;
}

//Sets gyroscope outputs from player input, dampeners, gravity, and autopilot
private double Pitch_Time= 1.0f;
private double Yaw_Time=1.0f;
private double Roll_Time=1.0f;
private void SetGyroscopes(){
	Gyroscope.GyroOverride=true;
	float current_pitch=(float) Relative_AngularVelocity.X;
	float current_yaw=(float) Relative_AngularVelocity.Y;
	float current_roll=(float) Relative_AngularVelocity.Z;
	
	float gyro_count = 0;
	List<IMyGyro> AllGyros=new List<IMyGyro>();
	GridTerminalSystem.GetBlocksOfType<IMyGyro>(AllGyros);
	foreach(IMyGyro Gyro in AllGyros){
		if(Gyro.IsWorking)
			gyro_count+=Gyro.GyroPower/100.0f;
	}
	float gyro_multx=(float)Math.Max(0.1f, Math.Min(1, 1.5f/(Controller.CalculateShipMass().PhysicalMass/gyro_count/1000000)));
	float correction_multx=5;
	
	
	float input_pitch=0;
	float input_yaw=0;
	float input_roll=0;
	
	if(Pitch_Time<1)
		Pitch_Time+=seconds_since_last_update;
	if(Yaw_Time<1)
		Yaw_Time+=seconds_since_last_update;
	if(Roll_Time<1)
		Roll_Time+=seconds_since_last_update;
	
	input_pitch=Math.Min(Math.Max(Controller.RotationIndicator.X / 100, -1), 1);
	if(Math.Abs(input_pitch)<0.05f){
		input_pitch=current_pitch*0.99f;
		if(Pitch_Time>0.5){
			double difference=Math.Abs(GetAngle(Gravity,Forward_Vector));
			Write("Pitch: "+Math.Round(difference-90,1).ToString()+"°");
			float Pitch_Multx=1;
			if(Math.Abs(difference-90)>20)
				Pitch_Multx=correction_multx;
			if(difference<85)
				input_pitch-=10*Pitch_Multx*gyro_multx*((float)Math.Min(Math.Abs((90-difference)/90), 1));
			else if(difference>95)
				input_pitch+=10*Pitch_Multx*gyro_multx*((float)Math.Min(Math.Abs((difference-90)/90), 1));
		}
	}
	else{
		Pitch_Time=0;
		input_pitch*=30;
	}
	input_yaw=Math.Min(Math.Max(Controller.RotationIndicator.Y / 100, -1), 1);
	if(Math.Abs(input_yaw)<0.05f){
		input_yaw=current_yaw*0.99f;
	}
	else{
		Yaw_Time=0;
		input_yaw*=30;
	}
	input_roll=Controller.RollIndicator;
	if(Math.Abs(input_roll)<0.05f){
		input_roll=current_roll*0.99f;
		if(Gravity.Length()>0&&Roll_Time>0.5){
			double difference=GetAngle(Left_Vector, Gravity)-GetAngle(Right_Vector, Gravity);
			Write("Roll: "+Math.Round(difference,1).ToString()+"°");
			float Roll_Multx=1;
			if(Math.Abs(difference)>20)
				Roll_Multx=correction_multx;
			if(Math.Abs(difference)>5){
				input_roll-=(float)Math.Min(Math.Max(difference/5,-1),1)*gyro_multx*Roll_Multx;
			}
		}
	}
	else{
		Roll_Time=0;
		input_roll*=10;
	}
	
	Vector3D input=new Vector3D(input_pitch,input_yaw,input_roll);
	Vector3D global=Vector3D.TransformNormal(input,Controller.WorldMatrix);
	Vector3D output=Vector3D.TransformNormal(global,MatrixD.Invert(Gyroscope.WorldMatrix));
	output.Normalize();
	output*=input.Length();
	
	Gyroscope.Pitch=(float)output.X;
	Gyroscope.Yaw=(float)output.Y;
	Gyroscope.Roll=(float)output.Z;
}

private Color ColorParse(string parse){
	parse=parse.Substring(parse.IndexOf('{')+1);
	parse=parse.Substring(0, parse.IndexOf('}')-1);
	string[] args=parse.Split(' ');
	int r, g, b, a;
	r=Int32.Parse(args[0].Substring(args[0].IndexOf("R:")+2).Trim());
	g=Int32.Parse(args[1].Substring(args[1].IndexOf("G:")+2).Trim());
	b=Int32.Parse(args[2].Substring(args[2].IndexOf("B:")+2).Trim());
	a=Int32.Parse(args[3].Substring(args[3].IndexOf("A:")+2).Trim());
	return new Color(r,g,b,a);
}

float Brightness(Color color){
	return 0.2126f*color.R+0.7152f*color.G + 0.0722f*color.B;
}

double Fun_Timer=0;
double Fun_Timer_Limit=1;
bool Fun_State=false;
void Fun(){
	bool Last_State=Fun_State;
	Fun_State=Controller.IsUnderControl;
	//Write("Fun:"+Fun_State.ToString()+":"+Math.Round(Fun_Timer,3).ToString()+"s");
	if(Last_State!=Fun_State){
		List<IMyTextSurfaceProvider> Screens=new List<IMyTextSurfaceProvider>();
		GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(Screens);
		if(!Last_State){
			foreach(IMyTextSurfaceProvider Screen in Screens){
				try{
					if(HasBlockData((IMyTerminalBlock)Screen,"DefaultBackgroundColor")){
						Color background=ColorParse(GetBlockData((IMyTerminalBlock)Screen,"DefaultBackgroundColor"));
						for(int i=0;i<Screen.SurfaceCount;i++){
							Screen.GetSurface(i).BackgroundColor=background;
							Screen.GetSurface(i).ScriptBackgroundColor=background;
						}
					}
				}
				catch(Exception){
					continue;
				}
			}
		}
		else{
			foreach(IMyTextSurfaceProvider Screen in Screens){
				try{
					if(Screen.SurfaceCount>0&&!HasBlockData((IMyTerminalBlock)Screen,"DefaultBackgroundColor")){
						Color color=Screen.GetSurface(0).BackgroundColor;
						if(Screen.GetSurface(0).ContentType==ContentType.SCRIPT)
							color=Screen.GetSurface(0).ScriptBackgroundColor;
						color.A=255;
						SetBlockData((IMyTerminalBlock)Screen,"DefaultBackgroundColor",color.ToString());
					}
				}
				catch(Exception){
					continue;
				}
			}
		}
	}
	if(!Fun_State){
		Fun_Timer+=seconds_since_last_update;
		if(Fun_Timer>Fun_Timer_Limit){
			Fun_Timer=0;
			Fun_Timer_Limit=0.5+(Rnd.Next(0,50)/100.0);
			List<IMyTextSurfaceProvider> Screens=new List<IMyTextSurfaceProvider>();
			GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(Screens);
			foreach(IMyTextSurfaceProvider Screen in Screens){
				try{
					if(HasBlockData((IMyTerminalBlock)Screen,"DefaultBackgroundColor")){
						for(int i=0;i<Screen.SurfaceCount;i++){
							Color color;
							int r;
							int g;
							int b;
							do{
								r=Rnd.Next(0,256);
								g=Rnd.Next(0,256);
								b=Rnd.Next(0,256);
							}
							while(r+g+b==0);
							color=new Color(r,g,b,255);
							float Target=50;
							float Actual=Brightness(color);
							while(Math.Abs(Target-Actual)>5){
								if(Target<Actual){
									r=Math.Max(0,(int)(r/1.2-1));
									g=Math.Max(0,(int)(g/1.2-1));
									b=Math.Max(0,(int)(b/1.2-1));
								}
								else{
									r=Math.Min(255,(int)(r*1.2+1));
									g=Math.Min(255,(int)(g*1.2+1));
									b=Math.Min(255,(int)(b*1.2+1));
								}
								color=new Color(r,g,b,255);
								Actual=Brightness(color);
							}
							Screen.GetSurface(i).BackgroundColor=color;
							Screen.GetSurface(i).ScriptBackgroundColor=color;
						}
					}
				}
				catch(Exception){
					continue;
				}
			}
		}
	}
}

void SetLimit(){
	float Angle=15;
	float Speed=(float)Current_LinearVelocity.Length();
	Angle=Math.Min(15,Math.Max(2.5f,37.5f-Speed));
	Angle=(float)(Angle/180*Math.PI);
	foreach(IMyMotorSuspension Wheel in Wheels)
		Wheel.MaxSteerAngle=Angle;
	Write("Steering Angle: "+Math.Round(Angle,1).ToString()+"°");
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
bool Holding_Down=false;

public void Main(string argument, UpdateType updateSource)
{
	UpdateProgramInfo();
	Write("Elevation: "+Math.Round(Elevation,1).ToString()+"M");
	if(Elevation>10)
		SetGyroscopes();
	else
		Gyroscope.GyroOverride=false;
	Fun();
	SetLimit();
	Write(Headlights.Count.ToString()+" Headlights");
	Write(Brakelights.Count.ToString()+" Brake Lights");
	
	if(Wheels.Count>0){
		float Speed_Limit=Wheels[0].GetValue<float>("Speed Limit");
		if((!Holding_Down)&&Controller.MoveIndicator.Y<0){
			bool set_up=(Speed_Limit<360);
			if(set_up)
				Speed_Limit=360;
			else
				Speed_Limit=72;
			foreach(IMyMotorSuspension Wheel in Wheels)
				Wheels[0].SetValue<float>("Speed Limit",Speed_Limit);
		}
		Write("Speed Limit\n"+Math.Round(Speed_Limit,0)+"kM/h; "+Math.Round(Speed_Limit/3.6f,1)+"M/s");
		Holding_Down=(Controller.MoveIndicator.Y<0);
	}
	
	foreach(IMyLightingBlock Light in Brakelights)
		Light.Enabled=Controller.HandBrake||Controller.MoveIndicator.Y>0;
	if(!Controller.IsUnderControl){
		Controller.HandBrake=true;
		foreach(IMyLightingBlock Light in Headlights)
			Light.Enabled=false;
	}
}
