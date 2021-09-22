/*
* Nova Printer
* Built by mlq1616
* https://github.com/mlq1819/Space-Engineers-Misc-Scripts
* This script is meant for auto-printing.
* Intended for use only in space.
* Not guaranteed to work 100% of the time.
*/

const string ProgramName = "Nova Printing";
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




// Initialization and Objects
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
	Assembler=CollectionMethods<IMyAssembler>.ByFullName("Nova Printer Assembler");
	if(Assembler==null)
		throw new ArgumentNullException("Assembler cannot be null; must be named \"Nova Printer Assembler\"");
	AllAssemblers=CollectionMethods<IMyAssembler>.AllConstruct;
	Dock=CollectionMethods<IMyShipConnector>.ByFullName("Nova Printer Connector");
	if(Dock==null)
		throw new ArgumentNullException("Docking connector cannot be null; must be named \"Nova Printer Connector\"");
	Projector=CollectionMethods<IMyProjector>.ByFullName("Nova Printer Projector",CollectionMethods<IMyProjector>.AllBlocks);
	if(Dock==null)
		throw new ArgumentNullException("Projector cannot be null; must be named \"Nova Printer Connector\"");
	Container=CollectionMethods<IMyCargoContainer>.ByFullName("Nova Printer Cargo Container");
	if(Container==null)
		throw new ArgumentNullException("Cargo Container cannot be null; must be named \"Nova Printer Cargo Container\"");
	
	// Load runtime variables from CustomData
	Echo("Setting variables...");
	if(GenMethods.HasBlockData(Me,"AutoRun")&&GenMethods.GetBlockData<bool>(Me,"AutoRun",bool.Parse))
		Me.Enabled=true;
	
	// Load data from Storage
	Echo("Loading data...");
	List<ModdedItem> moddedItems=new List<ModdedItem>();
	string storageMode="";
	string[] storageArgs=this.Storage.Trim().Split('\n');
	foreach(string line in storageArgs){
		switch(line){
			case "ModdedItems":
				storageMode=line;
				break;
			default:
				switch(storageMode){
					case "ModdedItems":
						ModdedItem? moddedItem;
						if(ModdedItem.TryParse(line,out moddedItem))
							moddedItems.Add((ModdedItem)moddedItem);
						break;
				}
				break;
		}
	}
	MyItem.InitializeModdedItems(moddedItems);
	
	List<IMyCargoContainer> mainCargos=GenMethods.Merge<IMyCargoContainer>(CollectionMethods<IMyCargoContainer>.AllByName("Main"),CollectionMethods<IMyCargoContainer>.AllByName("Deep"));
	foreach(IMyCargoContainer cargo in mainCargos){
		List<MyInventoryItem> items=new List<MyInventoryItem>();
		cargo.GetInventory().GetItems(items);
		foreach(MyInventoryItem item in items)
			MyItem.CheckExists(item);
	}
	
	//Attempt to load drone components
	Echo("Loading Welding Drone... "+(SetupDrone()?"success":"failed"));
	
	
	Runtime.UpdateFrequency=UpdateFrequency.Update10;
	Echo("Completed initialization!");
}

bool SetupDrone(){
	if(Drone!=null)
		return true;
	if(Dock.Status==MyShipConnectorStatus.Connectable)
		Dock.Connect();
	if(Dock.Status==MyShipConnectorStatus.Connected)
		Drone=MyDrone.TryCreate(Dock.OtherConnector);
	return Drone!=null;
}

IMyAssembler Assembler;
List<IMyAssembler> AllAssemblers;
IMyProjector Projector;
IMyCargoContainer Container;
IMyShipConnector Dock;
MyDrone Drone;


public enum MyDroneStatus{
	Docked,
	Idle,
	Stocking,
	Traveling,
	Welding,
	Docking
}
class MyDrone{
	public IMyRemoteControl Remote;
	public List<IMyShipWelder> Welders;
	public List<IMyCargoContainer> Containers;
	public List<IMyCameraBlock> Cameras;
	public IMyShipConnector Connector;
	public List<IMyBatteryBlock> Batteries;
	
	public Roo<float> ChargePercent;
	
	public MyDroneStatus Status;
	
	protected MyDrone(IMyShipConnector connector,IMyRemoteControl remote,List<IMyShipWelder> welders,List<IMyCargoContainer> containers,List<IMyCameraBlock> cameras,List<IMyBatteryBlock> batteries){
		Remote=remote;
		Welders=welders;
		Containers=containers;
		Cameras=cameras;
		Connector=connector;
		Batteries=batteries;
		ChargePercent=new Roo<float>(Get_ChargePercent);
		if(ChargePercent>=0.99f)
			Status=MyDroneStatus.Idle;
		else
			Status=MyDroneStatus.Docked;
	}
	
	public static MyDrone TryCreate(IMyShipConnector connector){
		if(!(connector?.CustomName.Contains("Nova Printer Drone")??false))
			return null;
		IMyRemoteControl remote=CollectionMethods<IMyRemoteControl>.ByName("Nova Printer Drone",CollectionMethods<IMyRemoteControl>.AllByGrid(connector.CubeGrid));
		if(remote==null)
			return null;
		List<IMyShipWelder> welders=CollectionMethods<IMyShipWelder>.AllByName("Nova Printer Drone",CollectionMethods<IMyShipWelder>.AllByGrid(remote.CubeGrid));
		if(welders.Count==0)
			return null;
		List<IMyCargoContainer> containers=CollectionMethods<IMyCargoContainer>.AllByName("Nova Printer Drone",CollectionMethods<IMyCargoContainer>.AllByGrid(remote.CubeGrid));
		List<IMyCameraBlock> cameras=CollectionMethods<IMyCameraBlock>.AllByName("Nova Printer Drone",CollectionMethods<IMyCameraBlock>.AllByGrid(remote.CubeGrid));
		List<IMyBatteryBlock> cameras=CollectionMethods<IMyBatteryBlock>.AllByName("Nova Printer Drone",CollectionMethods<IMyBatteryBlock>.AllByGrid(remote.CubeGrid));
		if(containers.Count==0||cameras.Count==0||connector==null||batteries.Count==0)
			return null;
		return new MyDrone(connector,remote,welders,containers,cameras,batteries);
	}
	
	public void Redock(IMyShipConnector Dock){
		
	}
	
	public void Undock(Vector3D destination){
		foreach(IMyBatteryBlock battery in Batteries)
			battery.ChargeMode=ChargeMode.Auto;
		Connector.Disconnect();
		Remote.ClearWaypoints();
		Remote.AddWaypoint(destination,"Welder Destination");
		Remote.SetCollisionAvoidance(true);
		Remote.SetDockingMode(false);
		Remote.SetAutoPilotEnabled(true);
	}
	
	protected float Get_ChargePercent(){
		float current=0;
		float max=0;
		foreach(IMyBatteryBlock battery in Batteries){
			current+=battery.CurrentStoredPower;
			max+=battery.MaxStoredPower;
		}
		return (max>0?current/max:0);
	}
}




// Saving and Data Storage
public void Save(){
    // Reset Storage
	this.Storage="";
	
	// Save Data to Storage
	this.Storage+="\n"+"ModdedItems";
	foreach(ModdedItem moddedItem in MyItem.Modded)
		this.Storage+="\n"+moddedItem.ToString();
	
	// Update runtime variables from CustomData
	bool autoRun=false;
	if(GenMethods.HasBlockData(Me,"AutoRun"))
		autoRun=GenMethods.GetBlockData<bool>(Me,"AutoRun",bool.Parse);
	if(autoRun)
		Me.Enabled=true;
	
	// Reset CustomData
	Me.CustomData="";
	
	// Save Runtime Data to CustomData
	GenMethods.SetBlockData(Me,"AutoRun",autoRun.ToString());
	
}


public enum MyRarity{
	VeryRare=1,
	Rare=2,
	Uncommon=3,
	Common=4,
	VeryCommon=5
}
public struct ModdedItem{
	public MyItemType Type;
	public MyRarity Rarity;
	
	public ModdedItem(MyItemType type,MyRarity rarity){
		Type=type;
		Rarity=rarity;
	}
	
	public ModdedItem(MyInventoryItem item){
		Type=item.Type;
		float amount=item.Amount.ToIntSafe();
		float multx=1;
		switch(Type.TypeId){
			case "MyObjectBuilder_Ore":
			case "MyObjectBuilder_Ingot":
				multx=100;
				break;
			case "MyObjectBuilder_Component":
				multx=10;
				break;
			case "MyObjectBuilder_PhysicalGunObject":
				multx=0.1f;
				break;
		}
		if(amount<=25*multx)
			Rarity=MyRarity.VeryRare;
		else if(amount<=2000*multx)
			Rarity=MyRarity.Rare;
		else if(amount<10000*multx)
			Rarity=MyRarity.Uncommon;
		else if(amount<20000*multx)
			Rarity=MyRarity.Common;
		else 
			Rarity=MyRarity.VeryCommon;
	}
	
	public override string ToString(){
		return Type.ToString()+";"+Rarity.ToString();
	}
	
	public static ModdedItem Parse(string input){
		int index=input.IndexOf(';');
		return new ModdedItem(MyItemType.Parse(input.Substring(0,index)),(MyRarity)Enum.Parse(typeof(MyRarity),input.Substring(index+1)));
	}
	
	public static bool TryParse(string input,out ModdedItem? output){
		output=null;
		try{
			output=Parse(input);
			return output!=null;
		}
		catch{
			return false;
		}
	}
}

public static class MyItem{
	public static List<ModdedItem> MiscModded=new List<ModdedItem>();
	
	public static List<MyItemType> All{
		get{
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType Type in Vanilla)
				output.Add(Type);
			foreach(ModdedItem Type in Modded)
				output.Add(Type.Type);
			return output;
		}
	}
	public static List<MyItemType> Vanilla{
		get{
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType I in Raw.Vanilla)
				output.Add(I);
			foreach(MyItemType I in Ingot.Vanilla)
				output.Add(I);
			foreach(MyItemType I in Comp.Vanilla)
				output.Add(I);
			foreach(MyItemType I in Ammo.Vanilla)
				output.Add(I);
			foreach(MyItemType I in Tool.Vanilla)
				output.Add(I);
			foreach(MyItemType I in Cons.Vanilla)
				output.Add(I);
			output.Add(Datapad);
			output.Add(Package);
			output.Add(Credit);
			return output;
		}
	}
	public static List<ModdedItem> Modded{
		get{
			List<ModdedItem> output=new List<ModdedItem>();
			foreach(ModdedItem I in Raw.Modded)
				output.Add(I);
			foreach(ModdedItem I in Ingot.Modded)
				output.Add(I);
			foreach(ModdedItem I in Comp.Modded)
				output.Add(I);
			foreach(ModdedItem I in Ammo.Modded)
				output.Add(I);
			foreach(ModdedItem I in Tool.Modded)
				output.Add(I);
			foreach(ModdedItem I in Cons.Modded)
				output.Add(I);
			foreach(ModdedItem Type in Modded)
				output.Add(Type);
			return output;
		}
	}
	
	public static bool CheckExists(MyInventoryItem item){
		string TypeId=item.Type.TypeId;
		List<MyItemType> VanillaList;
		List<ModdedItem> ModdedList;
		switch(TypeId){
			case "MyObjectBuilder_Ore":
				VanillaList=Raw.Vanilla;
				ModdedList=Raw.Modded;
				break;
			case "MyObjectBuilder_Ingot":
				VanillaList=Ingot.Vanilla;
				ModdedList=Raw.Modded;
				break;
			case "MyObjectBuilder_Component":
				VanillaList=Comp.Vanilla;
				ModdedList=Comp.Modded;
				break;
			case "MyObjectBuilder_AmmoMagazine":
				VanillaList=Ammo.Vanilla;
				ModdedList=Ammo.Modded;
				break;
			case "MyObjectBuilder_PhysicalGunObject":
				VanillaList=Tool.Vanilla;
				ModdedList=Tool.Modded;
				break;
			case "MyObjectBuilder_ConsumableItem":
				VanillaList=Cons.Vanilla;
				ModdedList=Cons.Modded;
				break;
			default:
				VanillaList=Vanilla;
				ModdedList=MiscModded;
				break;
		}
		foreach(MyItemType Type in VanillaList){
			if(item.Type.Equals(Type))
				return true;
		}
		ModdedItem MyItem=new ModdedItem(item);
		for(int i=0;i<ModdedList.Count;i++){
			if(item.Type.Equals(ModdedList[i].Type)){
				if(ModdedList[i].Rarity<MyItem.Rarity)
					ModdedList[i]=MyItem;
				return true;
			}
		}
		ModdedList.Add(MyItem);
		return false;
	}
	
	public static List<MyItemType> ByString(string name){
		List<MyItemType> output=new List<MyItemType>();
		int index=name.Trim().IndexOf(' ');
		string subtype="";
		if(index==-1)
			index=name.Length;
		else
			subtype=name.Substring(index+1).ToLower();
		string type=name.Substring(0,index).ToLower();
		if(type.Equals("raw")||type.Equals("ore"))
			return output.Concat(Raw.ByString(subtype)).ToList();
		if(type.Equals("ingot")||type.Equals("wafer")||type.Equals("powder"))
			return output.Concat(Ingot.ByString(subtype)).ToList();
		if(type.Equals("component")||type.Equals("comp"))
			return output.Concat(Comp.ByString(subtype)).ToList();
		if(type.Equals("ammo")||type.Equals("ammunition"))
			return output.Concat(Ammo.ByString(subtype)).ToList();
		if(type.Equals("tool")||type.Equals("gun")||type.Equals("weapon"))
			return output.Concat(Tool.ByString(subtype)).ToList();
		if(type.Equals("consumable")||type.Equals("cons"))
			return output.Concat(Cons.ByString(subtype)).ToList();
		if(type.Equals("data")||type.Equals("datapad")){
			output.Add(Datapad);
			return output;
		}
		if(type.Equals("package")){
			output.Add(Package);
			return output;
		}
		if(type.Equals("credit")||type.Equals("sc"))
			output.Add(Credit);
		return output;
	}
	
	public static List<MyItemType> Search(string name){
		string[] args=name.Trim().ToLower().Split(' ');
		for(int i=0;i<args.Length;i++){
			if(args[i][args[i].Length-1]=='s')
				args[i]=args[i].Substring(0,args[i].Length-1);
		}
		List<MyItemType> output=new List<MyItemType>();
		foreach(MyItemType Type in All){
			bool match=true;
			string type=Type.TypeId.ToLower();
			string subtype=Type.SubtypeId.ToLower();
			foreach(string arg in args){
				if(type.Contains(arg)||arg.Contains(type))
					continue;
				else if(subtype.Contains(arg)||arg.Contains(subtype))
					continue;
				else{
					match=false;
					break;
				}
			}
			if(match)
				output.Add(Type);
		}
		return output;
	}
	
	public static class Raw{
		public static string B_O="MyObjectBuilder_Ore";
		public static List<ModdedItem> Modded=new List<ModdedItem>();
		public static List<MyItemType> All{
			get{
				List<MyItemType> output=new List<MyItemType>();
				foreach(MyItemType Type in Vanilla)
					output.Add(Type);
				foreach(ModdedItem Type in Modded)
					output.Add(Type.Type);
				return output;
			}
		}
		public static List<MyItemType> Vanilla{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Ice);
				output.Add(Stone);
				output.Add(Iron);
				output.Add(Nickel);
				output.Add(Silicon);
				output.Add(Cobalt);
				output.Add(Uranium);
				output.Add(Magnesium);
				output.Add(Silver);
				output.Add(Gold);
				output.Add(Platinum);
				output.Add(Scrap);
				output.Add(Organic);
				return output;
			}
		}
		public static List<MyItemType> ByString(string subtype){
			if(subtype.Trim().Length==0)
				return All;
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType item in All){
				if(item.SubtypeId.ToLower().Equals(subtype))
					output.Add(item);
			}
			if(output.Count==0){
				foreach(MyItemType item in All){
					if(item.SubtypeId.ToLower().Contains(subtype)||subtype.Contains(item.SubtypeId.ToLower()))
						output.Add(item);
				}
			}
			return output;
		}
		public static MyItemType Ice=new MyItemType(B_O,"Ice");
		public static MyItemType Stone=new MyItemType(B_O,"Stone");
		public static MyItemType Iron=new MyItemType(B_O,"Iron");
		public static MyItemType Nickel=new MyItemType(B_O,"Nickel");
		public static MyItemType Silicon=new MyItemType(B_O,"Silicon");
		public static MyItemType Cobalt=new MyItemType(B_O,"Cobalt");
		public static MyItemType Uranium=new MyItemType(B_O,"Uranium");
		public static MyItemType Magnesium=new MyItemType(B_O,"Magnesium");
		public static MyItemType Silver=new MyItemType(B_O,"Silver");
		public static MyItemType Gold=new MyItemType(B_O,"Gold");
		public static MyItemType Platinum=new MyItemType(B_O,"Platinum");
		public static MyItemType Scrap=new MyItemType(B_O,"Scrap");
		public static MyItemType Organic=new MyItemType(B_O,"Organic");
	}
	public static class Ingot{
		public static string B_I="MyObjectBuilder_Ingot";
		public static List<ModdedItem> Modded=new List<ModdedItem>();
		public static List<MyItemType> All{
			get{
				List<MyItemType> output=new List<MyItemType>();
				foreach(MyItemType Type in Vanilla)
					output.Add(Type);
				foreach(ModdedItem Type in Modded)
					output.Add(Type.Type);
				return output;
			}
		}
		public static List<MyItemType> Vanilla{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Stone);
				output.Add(Iron);
				output.Add(Nickel);
				output.Add(Silicon);
				output.Add(Cobalt);
				output.Add(Uranium);
				output.Add(Magnesium);
				output.Add(Silver);
				output.Add(Gold);
				output.Add(Platinum);
				output.Add(Scrap);
				return output;
			}
		}
		public static List<MyItemType> ByString(string subtype){
			if(subtype.Trim().Length==0)
				return All;
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType item in All){
				if(item.SubtypeId.ToLower().Equals(subtype))
					output.Add(item);
			}
			if(output.Count==0){
				foreach(MyItemType item in All){
					if(item.SubtypeId.ToLower().Contains(subtype)||subtype.Contains(item.SubtypeId.ToLower()))
						output.Add(item);
				}
			}
			return output;
		}
		public static MyItemType Stone=new MyItemType(B_I,"Stone");
		public static MyItemType Iron=new MyItemType(B_I,"Iron");
		public static MyItemType Nickel=new MyItemType(B_I,"Nickel");
		public static MyItemType Silicon=new MyItemType(B_I,"Silicon");
		public static MyItemType Cobalt=new MyItemType(B_I,"Cobalt");
		public static MyItemType Uranium=new MyItemType(B_I,"Uranium");
		public static MyItemType Magnesium=new MyItemType(B_I,"Magnesium");
		public static MyItemType Silver=new MyItemType(B_I,"Silver");
		public static MyItemType Gold=new MyItemType(B_I,"Gold");
		public static MyItemType Platinum=new MyItemType(B_I,"Platinum");
		public static MyItemType Scrap=new MyItemType(B_I,"Scrap");
	}
	public static class Comp{
		public static string B_C="MyObjectBuilder_Component";
		public static List<ModdedItem> Modded=new List<ModdedItem>();
		public static List<MyItemType> All{
			get{
				List<MyItemType> output=new List<MyItemType>();
				foreach(MyItemType Type in Vanilla)
					output.Add(Type);
				foreach(ModdedItem Type in Modded)
					output.Add(Type.Type);
				return output;
			}
		}
		public static List<MyItemType> Vanilla{	
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Steel);
				output.Add(Construction);
				output.Add(Interior);
				output.Add(Motor);
				output.Add(Computer);
				output.Add(Small);
				output.Add(Large);
				output.Add(Grid);
				output.Add(Display);
				output.Add(Girder);
				output.Add(Glass);
				output.Add(Thrust);
				output.Add(Reactor);
				output.Add(Super);
				output.Add(Power);
				output.Add(Detector);
				output.Add(Grav);
				output.Add(Medical);
				output.Add(Radio);
				output.Add(Solar);
				output.Add(Explosive);
				output.Add(Zone);
				output.Add(Canvas);
				return output;
			}
		}
		public static List<MyItemType> ByString(string subtype){
			if(subtype.Trim().Length==0)
				return All;
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType item in All){
				if(item.SubtypeId.ToLower().Equals(subtype))
					output.Add(item);
			}
			if(output.Count==0){
				foreach(MyItemType item in All){
					if(item.SubtypeId.ToLower().Contains(subtype)||subtype.Contains(item.SubtypeId.ToLower()))
						output.Add(item);
				}
			}
			return output;
		}
		public static List<MyItemType> VeryCommon{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Steel);
				foreach(ModdedItem MyItem in Modded){
					if(MyItem.Rarity==MyRarity.VeryCommon)
						output.Add(MyItem.Type);
				}
				return output;
			}
		}
		public static List<MyItemType> Common{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Construction);
				output.Add(Interior);
				output.Add(Small);
				output.Add(Grid);
				output.Add(Glass);
				foreach(ModdedItem MyItem in Modded){
					if(MyItem.Rarity==MyRarity.Common)
						output.Add(MyItem.Type);
				}
				return output;
			}
		}
		public static List<MyItemType> Uncommon{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Motor);
				output.Add(Girder);
				output.Add(Thrust);
				foreach(ModdedItem MyItem in Modded){
					if(MyItem.Rarity==MyRarity.Uncommon)
						output.Add(MyItem.Type);
				}
				return output;
			}
		}
		public static List<MyItemType> Rare{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Computer);
				output.Add(Large);
				output.Add(Display);
				output.Add(Reactor);
				output.Add(Super);
				output.Add(Power);
				foreach(ModdedItem MyItem in Modded){
					if(MyItem.Rarity==MyRarity.Rare)
						output.Add(MyItem.Type);
				}
				return output;
			}
		}
		public static List<MyItemType> VeryRare{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Medical);
				output.Add(Grav);
				output.Add(Radio);
				output.Add(Solar);
				output.Add(Detector);
				output.Add(Explosive);
				output.Add(Zone);
				output.Add(Canvas);
				foreach(ModdedItem MyItem in Modded){
					if(MyItem.Rarity==MyRarity.VeryRare)
						output.Add(MyItem.Type);
				}
				return output;
			}
		}
		
		public static MyItemType Steel=new MyItemType(B_C,"SteelPlate");
		public static MyItemType Construction=new MyItemType(B_C,"Construction");
		public static MyItemType Interior=new MyItemType(B_C,"InteriorPlate");
		public static MyItemType Motor=new MyItemType(B_C,"Motor");
		public static MyItemType Computer=new MyItemType(B_C,"Computer");
		public static MyItemType Small=new MyItemType(B_C,"SmallTube");
		public static MyItemType Large=new MyItemType(B_C,"LargeTube");
		public static MyItemType Grid=new MyItemType(B_C,"MetalGrid");
		public static MyItemType Display=new MyItemType(B_C,"Display");
		public static MyItemType Girder=new MyItemType(B_C,"Girder");
		public static MyItemType Glass=new MyItemType(B_C,"BulletproofGlass");
		public static MyItemType Thrust=new MyItemType(B_C,"Thrust");
		public static MyItemType Reactor=new MyItemType(B_C,"Reactor");
		public static MyItemType Super=new MyItemType(B_C,"Superconductor");
		public static MyItemType Power=new MyItemType(B_C,"PowerCell");
		public static MyItemType Detector=new MyItemType(B_C,"Detector");
		public static MyItemType Grav=new MyItemType(B_C,"GravityGenerator");
		public static MyItemType Medical=new MyItemType(B_C,"Medical");
		public static MyItemType Radio=new MyItemType(B_C,"RadioCommunication");
		public static MyItemType Solar=new MyItemType(B_C,"SolarCell");
		public static MyItemType Explosive=new MyItemType(B_C,"Explosives");
		public static MyItemType Zone=new MyItemType(B_C,"ZoneChip");
		public static MyItemType Canvas=new MyItemType(B_C,"Canvas");
	}
	public static class Ammo{
		public static string B_A="MyObjectBuilder_AmmoMagazine";
		public static List<ModdedItem> Modded=new List<ModdedItem>();
		public static List<MyItemType> All{
			get{
				List<MyItemType> output=new List<MyItemType>();
				foreach(MyItemType Type in Vanilla)
					output.Add(Type);
				foreach(ModdedItem Type in Modded)
					output.Add(Type.Type);
				return output;
			}
		}
		public static List<MyItemType> Vanilla{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Missile);
				output.Add(Container);
				output.Add(Magazine);
				output.Add(RifleB);
				output.Add(RifleP);
				output.Add(RifleA);
				output.Add(RifleE);
				output.Add(PistolB);
				output.Add(PistolA);
				output.Add(PistolE);
				return output;
			}
		}
		public static List<MyItemType> ByString(string subtype){
			if(subtype.Trim().Length==0)
				return All;
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType item in All){
				if(item.SubtypeId.ToLower().Equals(subtype))
					output.Add(item);
			}
			if(output.Count==0){
				foreach(MyItemType item in All){
					if(item.SubtypeId.ToLower().Contains(subtype)||subtype.Contains(item.SubtypeId.ToLower()))
						output.Add(item);
				}
			}
			return output;
		}
		public static MyItemType Missile=new MyItemType(B_A,"Missile200mm");
		public static MyItemType Container=new MyItemType(B_A,"NATO_25x184mm");
		public static MyItemType Magazine=new MyItemType(B_A,"NATO_5p56x45mm");
		public static MyItemType RifleB=new MyItemType(B_A,"AutomaticRifleGun_Mag_20rd");
		public static MyItemType RifleP=new MyItemType(B_A,"PreciseAutomaticRifleGun_Mag_5rd");
		public static MyItemType RifleA=new MyItemType(B_A,"RapidFireAutomaticRifleGun_Mag_50rd");
		public static MyItemType RifleE=new MyItemType(B_A,"UltimateAutomaticRifleGun_Mag_30rd");
		public static MyItemType PistolB=new MyItemType(B_A,"SemiAutoPistolMagazine");
		public static MyItemType PistolA=new MyItemType(B_A,"FullAutoPistolMagazine");
		public static MyItemType PistolE=new MyItemType(B_A,"ElitePistolMagazine");
	}
	public static class Tool{
		public static string B_T="MyObjectBuilder_PhysicalGunObject";
		public static List<ModdedItem> Modded=new List<ModdedItem>();
		public static List<MyItemType> All{
			get{
				List<MyItemType> output=new List<MyItemType>();
				foreach(MyItemType Type in Vanilla)
					output.Add(Type);
				foreach(ModdedItem Type in Modded)
					output.Add(Type.Type);
				return output;
			}
		}
		public static List<MyItemType> Vanilla{	
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(H2);
				output.Add(O2);
				output.Add(Welder1);
				output.Add(Welder2);
				output.Add(Welder3);
				output.Add(Welder4);
				output.Add(Grinder1);
				output.Add(Grinder2);
				output.Add(Grinder3);
				output.Add(Grinder4);
				output.Add(Drill1);
				output.Add(Drill2);
				output.Add(Drill3);
				output.Add(Drill4);
				output.Add(RifleB);
				output.Add(RifleP);
				output.Add(RifleA);
				output.Add(RifleE);
				output.Add(PistolB);
				output.Add(PistolA);
				output.Add(PistolE);
				output.Add(RocketB);
				output.Add(RocketP);
				return output;
			}
		}
		public static List<MyItemType> ByString(string subtype){
			if(subtype.Trim().Length==0)
				return All;
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType item in All){
				if(item.SubtypeId.ToLower().Equals(subtype))
					output.Add(item);
			}
			if(output.Count==0){
				foreach(MyItemType item in All){
					if(item.SubtypeId.ToLower().Contains(subtype)||subtype.Contains(item.SubtypeId.ToLower()))
						output.Add(item);
				}
			}
			return output;
		}
		public static MyItemType H2=new MyItemType("MyObjectBuilder_GasContainerObject","HydrogenBottle");
		public static MyItemType O2=new MyItemType("MyObjectBuilder_OxygenContainerObject","OxygenBottle");
		public static MyItemType Welder1=new MyItemType(B_T,"WelderItem");
		public static MyItemType Welder2=new MyItemType(B_T,"Welder2Item");
		public static MyItemType Welder3=new MyItemType(B_T,"Welder3Item");
		public static MyItemType Welder4=new MyItemType(B_T,"Welder4Item");
		public static MyItemType Grinder1=new MyItemType(B_T,"AngleGrinderItem");
		public static MyItemType Grinder2=new MyItemType(B_T,"AngleGrinder2Item");
		public static MyItemType Grinder3=new MyItemType(B_T,"AngleGrinder3Item");
		public static MyItemType Grinder4=new MyItemType(B_T,"AngleGrinder4Item");
		public static MyItemType Drill1=new MyItemType(B_T,"HandDrillItem");
		public static MyItemType Drill2=new MyItemType(B_T,"HandDrill2Item");
		public static MyItemType Drill3=new MyItemType(B_T,"HandDrill3Item");
		public static MyItemType Drill4=new MyItemType(B_T,"HandDrill4Item");
		public static MyItemType RifleB=new MyItemType(B_T,"AutomaticRifleItem");
		public static MyItemType RifleP=new MyItemType(B_T,"PreciseAutomaticRifleItem");
		public static MyItemType RifleA=new MyItemType(B_T,"RapidFireAutomaticRifleItem");
		public static MyItemType RifleE=new MyItemType(B_T,"UltimateAutomaticRifleItem");
		public static MyItemType PistolB=new MyItemType(B_T,"SemiAutoPistolItem");
		public static MyItemType PistolA=new MyItemType(B_T,"FullAutoPistolItem");
		public static MyItemType PistolE=new MyItemType(B_T,"ElitePistolItem");
		public static MyItemType RocketB=new MyItemType(B_T,"BasicHandHeldLauncherItem");
		public static MyItemType RocketP=new MyItemType(B_T,"AdvancedHandHeldLauncherItem");
	}
	public static class Cons{
		public static string B_C="MyObjectBuilder_ConsumableItem";
		public static List<ModdedItem> Modded=new List<ModdedItem>();
		public static List<MyItemType> All{
			get{
				List<MyItemType> output=new List<MyItemType>();
				foreach(MyItemType Type in Vanilla)
					output.Add(Type);
				foreach(ModdedItem Type in Modded)
					output.Add(Type.Type);
				return output;
			}
		}
		public static List<MyItemType> Vanilla{
			get{
				List<MyItemType> output=new List<MyItemType>();
				output.Add(Power);
				output.Add(Medical);
				output.Add(Clang);
				output.Add(Cosmic);
				return output;
			}
		}
		public static List<MyItemType> ByString(string subtype){
			if(subtype.Trim().Length==0)
				return All;
			List<MyItemType> output=new List<MyItemType>();
			foreach(MyItemType item in All){
				if(item.SubtypeId.ToLower().Equals(subtype))
					output.Add(item);
			}
			if(output.Count==0){
				foreach(MyItemType item in All){
					if(item.SubtypeId.ToLower().Contains(subtype)||subtype.Contains(item.SubtypeId.ToLower()))
						output.Add(item);
				}
			}
			return output;
		}
		public static MyItemType Power=new MyItemType(B_C,"Powerkit");
		public static MyItemType Medical=new MyItemType(B_C,"");
		public static MyItemType Clang=new MyItemType(B_C,"ClangCola");
		public static MyItemType Cosmic=new MyItemType(B_C,"CosmicCoffee");
	}
	
	public static MyItemType Datapad=new MyItemType("MyObjectBuilder_Datapad","Datapad");
	public static MyItemType Package=new MyItemType("MyObjectBuilder_Package","Package");
	public static MyItemType Credit=new MyItemType("MyObjectBuilder_PhysicalObject","SpaceCredit");
	
	public static void InitializeModdedItems(List<ModdedItem> ModdedItems){
		foreach(ModdedItem MyItem in ModdedItems){
			switch(MyItem.Type.TypeId){
				case "MyObjectBuilder_Ore":
					Raw.Modded.Add(MyItem);
					break;
				case "MyObjectBuilder_Ingot":
					Ingot.Modded.Add(MyItem);
					break;
				case "MyObjectBuilder_Component":
					Comp.Modded.Add(MyItem);
					break;
				case "MyObjectBuilder_AmmoMagazine":
					Ammo.Modded.Add(MyItem);
					break;
				case "MyObjectBuilder_PhysicalGunObject":
					Tool.Modded.Add(MyItem);
					break;
				case "MyObjectBuilder_ConsumableItem":
					Cons.Modded.Add(MyItem);
					break;
				default:
					MiscModded.Add(MyItem);
					break;
			}
		}
	}
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

