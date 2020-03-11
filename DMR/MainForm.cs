using DMR.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;

namespace DMR
{
	public class MainForm : Form
	{

#if OpenGD77
		private const string DEFAULT_DATA_FILE_NAME = "DefaultOpenGD77.g77";
#elif CP_VER_3_1_X
		private const string DEFAULT_DATA_FILE_NAME = "Default31X.dat";
#endif

		public static byte[] CommsBuffer=null;// = new byte[0x10000];

		private static string PRODUCT_NAME = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false)).Product;

		private const int WM_SETFONT = 48;

		private const int TVM_GETEDITCONTROL = 4367;

		private const int EM_LIMITTEXT = 197;

		private IContainer components;

		private MenuStrip mnsMain;

		private ToolStripMenuItem tsmiFile;

		private ToolStripMenuItem tsmiSetting;

		private ToolStripMenuItem tsmiSignaling;
		private ToolStripMenuItem tsmiSignalingSystem;

		private ToolStripMenuItem tsmiDtmf;

		private ToolStripMenuItem tsmiEmgSystem;

		private ToolStripMenuItem tsmiContact;

		private ToolStripMenuItem tsmiEncrypt;

		private ToolStripMenuItem tsmiTextMsg;

		private ToolStripMenuItem tsmiGerneralSet;

		private ToolStripMenuItem tsmiGrpRxList;

		private ToolStripMenuItem tsmiChannels;

		private ToolStripMenuItem tsmiZone;

		private ToolStripMenuItem tsmiButton;

		private ToolStripMenuItem tsmiScan;
		private ToolStripMenuItem tsmiVfos;
		private ToolStripMenuItem tsmiVfoA;
		private ToolStripMenuItem tsmiVfoB;

		private ToolStripMenuItem tsmiAbout;

		private ContextMenuStrip cmsGroup;

		private ToolStripMenuItem tsmiAdd;

		private ContextMenuStrip cmsSub;

		private ToolStripMenuItem tsmiDel;

		private ToolStripMenuItem tsmiRename;

		private ToolStripMenuItem tsmiWindow;

		private ToolStripMenuItem tsmiCascade;

		private ToolStripMenuItem tsmiTileHor;

		private ToolStripMenuItem tsmiTileVer;

		private ToolStripMenuItem tsmiExit;

		private ToolStripMenuItem tsmiProgram;

		private ToolStripMenuItem tsmiRead;

		private ToolStripMenuItem tsmiWrite;

		private ToolStripMenuItem tsmiNew;

		private ToolStripMenuItem tsmiSave;

		private ToolStripMenuItem tsmiOpen;

		private ToolStripSeparator toolStripSeparator1;

		private ContextMenuStrip cmsGroupContact;

		private ToolStripMenuItem tsmiAddContact;

		private ToolStripMenuItem tsmiGroupCall;

		private ToolStripMenuItem tsmiPrivateCall;

		private ToolStripMenuItem tsmiAllCall;

		private ImageList imgMain;

		private OpenFileDialog ofdMain;

		private SaveFileDialog sfdMain;

		private ToolStripMenuItem tsmiDtmfContact;

		private ToolStripMenuItem tsmiDmrContacts;

		private ToolStripMenuItem tsmiScanBasic;
		private ToolStripMenuItem tsmiScanList;

		private ToolStripMenuItem tsmiZoneBasic;
		private ToolStripMenuItem tsmiZoneList;

		private ToolStripMenuItem tsmiCloseAll;

		private ToolStripMenuItem tsmiDeviceInfo;

		private ContextMenuStrip cmsTree;

		private ToolStripMenuItem tsmiExpandAll;

		private ToolStripMenuItem tsmiCollapseAll;

		private DockPanel dockPanel;

		private Panel pnlTvw;

		private TreeView tvwMain;

		private ToolStripMenuItem tsmiView;

		private ToolStripMenuItem tsmiTree;

		private ToolStripMenuItem tsmiHelp;

		private ToolStripMenuItem tsmiMenu;

		private ToolStripMenuItem tsmiBootItem;

		private ToolStripMenuItem tsmiNumKeyContact;

		private StatusStrip ssrMain;

		private ToolStripStatusLabel slblComapny;

		private ToolStrip tsrMain;

		private ToolStripButton tsbtnNew;

		private ToolStripButton tsbtnOpen;

		private ToolStripButton tsbtnSave;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripButton tsbtnRead;

		private ToolStripButton tsbtnWrite;

		private ToolStripSeparator toolStripSeparator3;

		private ToolStripButton tsbtnAbout;

		private ToolStripMenuItem tsmiClear;

		private ToolStripSeparator toolStripSeparator4;

		private ToolStripMenuItem tsmiCopy;
		private ToolStripMenuItem tsmiMoveUp;
		private ToolStripMenuItem tsmiMoveDown;

		private ToolStripMenuItem tsmiPaste;

		private ToolStripMenuItem tsmiToolBar;

		private ToolStripMenuItem tsmiStatusBar;

		private ToolStripMenuItem tsmiBasic;

		private ToolStripMenuItem tsmiLanguage;

		private ToolStripMenuItem tsmiExtras;

		private ToolStripMenuItem tsmiContactsDownload;
		private ToolStripMenuItem tsmiDMRID;
		private ToolStripMenuItem tsmiCalibration;
		private ToolStripMenuItem tsmiOpenGD77;
		private ToolStripMenuItem tsmiFirmwareLoader;


		private DeserializeDockContent m_deserializeDockContent;

		private static IDisp PreActiveMdiChild;

		private HelpForm frmHelp;

		private TreeForm frmTree;

		private TreeNodeItem CopyItem;

		private List<TreeNode> lstFixedNode;

		private TextBox _TextBox;

		private static Dictionary<string, string> dicHelp;

		private static Dictionary<string, string> dicTree;

		private List<TreeNodeItem> lstTreeNodeItem;

		//private static readonly string[] TREENODE_KEY;

		/*
		public static int CurCbr
		{
			get;
			set;
		}

		public static string CurCom
		{
			get;
			set;
		}
		*/
		/*
		public static string CurModel
		{
			get;
			set;
		}*/

		public static string CurFileName
		{
			get;
			set;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
			DockPanelSkin dockPanelSkin = new DockPanelSkin();
			AutoHideStripSkin autoHideStripSkin = new AutoHideStripSkin();
			DockPanelGradient dockPanelGradient = new DockPanelGradient();
			TabGradient tabGradient = new TabGradient();
			DockPaneStripSkin dockPaneStripSkin = new DockPaneStripSkin();
			DockPaneStripGradient dockPaneStripGradient = new DockPaneStripGradient();
			TabGradient tabGradient2 = new TabGradient();
			DockPanelGradient dockPanelGradient2 = new DockPanelGradient();
			TabGradient tabGradient3 = new TabGradient();
			DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient = new DockPaneStripToolWindowGradient();
			TabGradient tabGradient4 = new TabGradient();
			TabGradient tabGradient5 = new TabGradient();
			DockPanelGradient dockPanelGradient3 = new DockPanelGradient();
			TabGradient tabGradient6 = new TabGradient();
			TabGradient tabGradient7 = new TabGradient();
			this.imgMain = new ImageList(this.components);
			this.mnsMain = new MenuStrip();
			this.tsmiFile = new ToolStripMenuItem();
			this.tsmiNew = new ToolStripMenuItem();
			this.tsmiSave = new ToolStripMenuItem();
			this.tsmiOpen = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.tsmiExit = new ToolStripMenuItem();
			this.tsmiSetting = new ToolStripMenuItem();
			this.tsmiDeviceInfo = new ToolStripMenuItem();
			this.tsmiBootItem = new ToolStripMenuItem();
			this.tsmiMenu = new ToolStripMenuItem();
			this.tsmiNumKeyContact = new ToolStripMenuItem();
			this.tsmiGerneralSet = new ToolStripMenuItem();
			this.tsmiButton = new ToolStripMenuItem();
			this.tsmiTextMsg = new ToolStripMenuItem();
			this.tsmiEncrypt = new ToolStripMenuItem();
			this.tsmiSignaling = new ToolStripMenuItem();
			this.tsmiSignalingSystem = new ToolStripMenuItem();
			this.tsmiDtmf = new ToolStripMenuItem();
			this.tsmiEmgSystem = new ToolStripMenuItem();
			this.tsmiContact = new ToolStripMenuItem();
			this.tsmiDtmfContact = new ToolStripMenuItem();
			this.tsmiDmrContacts = new ToolStripMenuItem();
			this.tsmiGrpRxList = new ToolStripMenuItem();
			this.tsmiZone = new ToolStripMenuItem();
			this.tsmiZoneBasic = new ToolStripMenuItem();
			this.tsmiZoneList = new ToolStripMenuItem();
			this.tsmiChannels = new ToolStripMenuItem();
			this.tsmiScan = new ToolStripMenuItem();
			this.tsmiScanBasic = new ToolStripMenuItem();
			this.tsmiScanList = new ToolStripMenuItem();
			this.tsmiVfos = new ToolStripMenuItem();
			this.tsmiVfoA = new ToolStripMenuItem();
			this.tsmiVfoB = new ToolStripMenuItem();

			this.tsmiProgram = new ToolStripMenuItem();
			this.tsmiRead = new ToolStripMenuItem();
			this.tsmiWrite = new ToolStripMenuItem();
			this.tsmiBasic = new ToolStripMenuItem();
			this.tsmiView = new ToolStripMenuItem();
			this.tsmiTree = new ToolStripMenuItem();
			this.tsmiHelp = new ToolStripMenuItem();
			this.tsmiToolBar = new ToolStripMenuItem();
			this.tsmiStatusBar = new ToolStripMenuItem();
			this.tsmiLanguage = new ToolStripMenuItem();
			this.tsmiExtras = new ToolStripMenuItem();
			this.tsmiContactsDownload = new ToolStripMenuItem();
			this.tsmiDMRID = new ToolStripMenuItem();
			this.tsmiCalibration = new ToolStripMenuItem();
			this.tsmiOpenGD77 = new ToolStripMenuItem();
			this.tsmiFirmwareLoader = new ToolStripMenuItem();
			
			
			
			this.tsmiWindow = new ToolStripMenuItem();
			this.tsmiCascade = new ToolStripMenuItem();
			this.tsmiTileHor = new ToolStripMenuItem();
			this.tsmiTileVer = new ToolStripMenuItem();
			this.tsmiCloseAll = new ToolStripMenuItem();
			this.tsmiAbout = new ToolStripMenuItem();
			this.cmsGroup = new ContextMenuStrip(this.components);
			this.tsmiAdd = new ToolStripMenuItem();
			this.tsmiClear = new ToolStripMenuItem();
			this.cmsSub = new ContextMenuStrip(this.components);
			this.tsmiDel = new ToolStripMenuItem();
			this.tsmiRename = new ToolStripMenuItem();
			this.toolStripSeparator4 = new ToolStripSeparator();
			this.tsmiCopy = new ToolStripMenuItem();

			this.tsmiMoveUp = new ToolStripMenuItem();
			this.tsmiMoveDown = new ToolStripMenuItem();

			this.tsmiPaste = new ToolStripMenuItem();
			this.cmsGroupContact = new ContextMenuStrip(this.components);
			this.tsmiAddContact = new ToolStripMenuItem();
			this.tsmiGroupCall = new ToolStripMenuItem();
			this.tsmiPrivateCall = new ToolStripMenuItem();
			this.tsmiAllCall = new ToolStripMenuItem();
			this.ofdMain = new OpenFileDialog();
			this.sfdMain = new SaveFileDialog();
			this.cmsTree = new ContextMenuStrip(this.components);
			this.tsmiCollapseAll = new ToolStripMenuItem();
			this.tsmiExpandAll = new ToolStripMenuItem();
			this.dockPanel = new DockPanel();
			this.pnlTvw = new Panel();
			this.tvwMain = new TreeView();
			this.ssrMain = new StatusStrip();
			this.slblComapny = new ToolStripStatusLabel();
			this.tsrMain = new ToolStrip();
			this.tsbtnNew = new ToolStripButton();
			this.tsbtnOpen = new ToolStripButton();
			this.tsbtnSave = new ToolStripButton();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.tsbtnRead = new ToolStripButton();
			this.tsbtnWrite = new ToolStripButton();
			this.toolStripSeparator3 = new ToolStripSeparator();
			this.tsbtnAbout = new ToolStripButton();
			this.mnsMain.SuspendLayout();
			this.cmsGroup.SuspendLayout();
			this.cmsSub.SuspendLayout();
			this.cmsGroupContact.SuspendLayout();
			this.cmsTree.SuspendLayout();
			this.pnlTvw.SuspendLayout();
			this.ssrMain.SuspendLayout();
			this.tsrMain.SuspendLayout();
			base.SuspendLayout();
            
			this.imgMain.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imgMain.ImageStream");
			this.imgMain.TransparentColor = Color.Transparent;
			this.imgMain.Images.SetKeyName(0, "46.png");
			this.imgMain.Images.SetKeyName(1, "47.png");
			this.imgMain.Images.SetKeyName(2, "21.png");


            this.mnsMain.Items.AddRange(new ToolStripItem[8]
			{
				this.tsmiFile,
				this.tsmiSetting,
				this.tsmiProgram,
				this.tsmiView,
				this.tsmiExtras,
				this.tsmiLanguage,
				this.tsmiWindow,
				this.tsmiAbout
			});
			this.mnsMain.Location = new Point(234, 0);
			this.mnsMain.MdiWindowListItem = this.tsmiWindow;
			this.mnsMain.Name = "mnsMain";
			this.mnsMain.Padding = new Padding(7, 3, 0, 3);
			this.mnsMain.Size = new Size(865, 27);
			this.mnsMain.TabIndex = 4;
			this.tsmiFile.DropDownItems.AddRange(new ToolStripItem[5]
			{
				this.tsmiNew,
				this.tsmiSave,
				this.tsmiOpen,
				this.toolStripSeparator1,
				this.tsmiExit
			});
			this.tsmiFile.Name = "tsmiFile";
			this.tsmiFile.Size = new Size(39, 21);
			this.tsmiFile.Text = "File";
			this.tsmiNew.Name = "tsmiNew";
			this.tsmiNew.Size = new Size(108, 22);
			this.tsmiNew.Text = "New";
			this.tsmiNew.Click += this.tsbtnNew_Click;
			this.tsmiNew.ShortcutKeys = Keys.Control | Keys.N;

			this.tsmiSave.Name = "tsmiSave";
			this.tsmiSave.Size = new Size(108, 22);
			this.tsmiSave.Text = "Save";
			this.tsmiSave.Click += this.tsbtnSave_Click;
			this.tsmiSave.ShortcutKeys = Keys.Control | Keys.S;
			
			this.tsmiOpen.Name = "tsmiOpen";
			this.tsmiOpen.Size = new Size(108, 22);
			this.tsmiOpen.Text = "Open";
			this.tsmiOpen.Click += this.tsbtnOpen_Click;
			this.tsmiOpen.ShortcutKeys = Keys.Control | Keys.O;

			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(105, 6);
			this.tsmiExit.Name = "tsmiExit";
			this.tsmiExit.Size = new Size(108, 22);
			this.tsmiExit.Text = "Exit";
			this.tsmiExit.Click += this.tsmiExit_Click;
			this.tsmiSetting.DropDownItems.AddRange(new ToolStripItem[15]
			{
				this.tsmiDeviceInfo,
				this.tsmiBootItem,
				this.tsmiMenu,
				this.tsmiNumKeyContact,
				this.tsmiGerneralSet,
				this.tsmiButton,
				this.tsmiTextMsg,
				this.tsmiEncrypt,
				this.tsmiSignaling,
				this.tsmiContact,
				this.tsmiGrpRxList,
				this.tsmiZone,
				this.tsmiChannels,
				this.tsmiScan,
				this.tsmiVfos
			});
			this.tsmiSetting.Name = "tsmiSetting";
			this.tsmiSetting.Size = new Size(60, 21);
			this.tsmiSetting.Text = "Setting";
			this.tsmiSetting.DropDownOpening += this.tsmiSetting_DropDownOpening;
			this.tsmiDeviceInfo.Name = "tsmiDeviceInfo";
			this.tsmiDeviceInfo.Size = new Size(191, 22);
			this.tsmiDeviceInfo.Text = "Basic Information";
			this.tsmiDeviceInfo.Click += this.tsmiDeviceInfo_Click;
			this.tsmiBootItem.Name = "tsmiBootItem";
			this.tsmiBootItem.Size = new Size(191, 22);
			this.tsmiBootItem.Text = "Boot Item";
			this.tsmiBootItem.Click += this.tsmiBootItem_Click;
			this.tsmiMenu.Name = "tsmiMenu";
			this.tsmiMenu.Size = new Size(191, 22);
			this.tsmiMenu.Text = "Menu";
			this.tsmiMenu.Click += this.tsmiMenu_Click;
			this.tsmiNumKeyContact.Name = "tsmiNumKeyContact";
			this.tsmiNumKeyContact.Size = new Size(191, 22);
			this.tsmiNumKeyContact.Text = "Number Key Assign";
			this.tsmiNumKeyContact.Click += this.tsmiNumKeyContact_Click;
			this.tsmiGerneralSet.Name = "tsmiGerneralSet";
			this.tsmiGerneralSet.Size = new Size(191, 22);
			this.tsmiGerneralSet.Text = "General Setting";
			this.tsmiGerneralSet.Click += this.tsmiGerneralSet_Click;
			this.tsmiButton.Name = "tsmiButton";
			this.tsmiButton.Size = new Size(191, 22);
			this.tsmiButton.Text = "Buttons";
			this.tsmiButton.Click += this.tsmiButton_Click;
			this.tsmiTextMsg.Name = "tsmiTextMsg";
			this.tsmiTextMsg.Size = new Size(191, 22);
			this.tsmiTextMsg.Text = "Text Message";
			this.tsmiTextMsg.Click += this.tsmiTextMsg_Click;
			this.tsmiEncrypt.Name = "tsmiEncrypt";
			this.tsmiEncrypt.Size = new Size(191, 22);
			this.tsmiEncrypt.Text = "Privacy";
			this.tsmiEncrypt.Click += this.tsmiEncrypt_Click;

			this.tsmiSignaling.Name = "tsmiSignaling";
			this.tsmiSignaling.Size = new Size(191, 22);
			this.tsmiSignaling.Text = "Signaling";
			this.tsmiSignaling.DropDownItems.AddRange(new ToolStripItem[3]
			{
				this.tsmiSignalingSystem,
				this.tsmiDtmf,
				this.tsmiEmgSystem
			});

			this.tsmiSignalingSystem.Name = "tsmiSignalingSystem";
			this.tsmiSignalingSystem.Size = new Size(191, 22);
			this.tsmiSignalingSystem.Text = "Signaling System";
			this.tsmiSignalingSystem.Click += this.tsmiSignalingSystem_Click;

			this.tsmiDtmf.Name = "tsmiDtmf";
			this.tsmiDtmf.Size = new Size(185, 22);
			this.tsmiDtmf.Text = "DTMF";
			this.tsmiDtmf.Click += this.tsmiDtmf_Click;
			this.tsmiEmgSystem.Name = "tsmiEmgSystem";
			this.tsmiEmgSystem.Size = new Size(185, 22);
			this.tsmiEmgSystem.Text = "Emergency System";
			this.tsmiEmgSystem.Click += this.tsmiEmgSystem_Click;
			this.tsmiContact.DropDownItems.AddRange(new ToolStripItem[2]
			{
				this.tsmiDtmfContact,
				this.tsmiDmrContacts
			});
			this.tsmiContact.Name = "tsmiContact";
			this.tsmiContact.Size = new Size(191, 22);
			this.tsmiContact.Text = "Contacts";
			this.tsmiDtmfContact.Name = "tsmiDtmfContact";
			this.tsmiDtmfContact.Size = new Size(161, 22);
			this.tsmiDtmfContact.Text = "DTMF";
			this.tsmiDtmfContact.Click += this.tsmiDtmfContact_Click;
			this.tsmiDmrContacts.Name = "tsmiDmrContacts";
			this.tsmiDmrContacts.Size = new Size(161, 22);
			this.tsmiDmrContacts.Text = "Digital Contacts";
			this.tsmiDmrContacts.Click += this.tsmiDmrContacts_Click;
			this.tsmiGrpRxList.Name = "tsmiGrpRxList";
			this.tsmiGrpRxList.Size = new Size(191, 22);
			this.tsmiGrpRxList.Text = "Rx Group List";
			this.tsmiGrpRxList.Click += this.tsmiGrpRxList_Click;
			this.tsmiZone.DropDownItems.AddRange(new ToolStripItem[1]
			{
				this.tsmiZoneList
			});
			this.tsmiZone.Name = "tsmiZone";
			this.tsmiZone.Size = new Size(191, 22);
			this.tsmiZone.Text = "Zone";
			this.tsmiZone.DropDownItems.AddRange(new ToolStripItem[2]
			{
				this.tsmiZoneBasic,
				this.tsmiZoneList
			});

			this.tsmiZoneBasic.Name = "tsmiZoneBasic";
			this.tsmiZoneBasic.Size = new Size(124, 22);
			this.tsmiZoneBasic.Text = "Zone Basic";
			this.tsmiZoneBasic.Click += this.tsmiZoneBasic_Click;

			this.tsmiZoneList.Name = "tsmiZoneList";
			this.tsmiZoneList.Size = new Size(124, 22);
			this.tsmiZoneList.Text = "ZoneList";
			this.tsmiZoneList.Click += this.tsmiZoneList_Click;

			this.tsmiChannels.Name = "tsmiChannels";
			this.tsmiChannels.Size = new Size(191, 22);
			this.tsmiChannels.Text = "Channels";
			this.tsmiChannels.Click += this.tsmiChannels_Click;


			this.tsmiScan.Name = "tsmiScan";
			this.tsmiScan.Size = new Size(191, 22);
			this.tsmiScan.Text = "Scan";
			this.tsmiScan.DropDownItems.AddRange(new ToolStripItem[2]
			{
				this.tsmiScanBasic,
				this.tsmiScanList
			});

			this.tsmiScanBasic.Name = "tsmiScanBasic";
			this.tsmiScanBasic.Size = new Size(191, 22);
			this.tsmiScanBasic.Text = "Scan Basic";
			this.tsmiScanBasic.Click += this.tsmiScanBasic_Click;

			this.tsmiScanList.Name = "tsmiScanList";
			this.tsmiScanList.Size = new Size(126, 22);
			this.tsmiScanList.Text = "Scan List";
			this.tsmiScanList.Click += this.tsmiScanList_Click;

			this.tsmiVfos.Name = "tsmiVfos";
			this.tsmiVfos.Size = new Size(191, 22);
			this.tsmiVfos.Text = "VFOs";

			this.tsmiVfoA.Name = "tsmiVfoA";
			this.tsmiVfoA.Size = new Size(191, 22);
			this.tsmiVfoA.Text = "VFO A";
			this.tsmiVfoA.Click += this.tsmiVfoA_Click;


			this.tsmiVfoB.Name = "tsmiVfoB";
			this.tsmiVfoB.Size = new Size(191, 22);
			this.tsmiVfoB.Text = "VFO B";
			this.tsmiVfoB.Click += this.tsmiVfoB_Click;

			this.tsmiVfos.DropDownItems.AddRange(new ToolStripItem[2]
			{
				this.tsmiVfoA,
				this.tsmiVfoB
			});



			this.tsmiProgram.DropDownItems.AddRange(new ToolStripItem[2]// was 3
			{
				this.tsmiRead,
				this.tsmiWrite//,
				//this.tsmiBasic
			});
			this.tsmiProgram.Name = "tsmiProgram";
			this.tsmiProgram.Size = new Size(71, 21);
			this.tsmiProgram.Text = "Program";
			this.tsmiRead.Name = "tsmiRead";
			this.tsmiRead.ShortcutKeys = (Keys)131154;
			this.tsmiRead.Size = new Size(156, 22);
			this.tsmiRead.Text = "Read";
			this.tsmiRead.Click += this.tsbtnRead_Click;
			this.tsmiWrite.Name = "tsmiWrite";
			this.tsmiWrite.ShortcutKeys = (Keys)131159;
			this.tsmiWrite.Size = new Size(156, 22);
			this.tsmiWrite.Text = "Write";
			this.tsmiWrite.Click += this.tsbtnWrite_Click;
			/*
			 * Roger Clark
			 * Removed basic mode select
			this.tsmiBasic.Name = "tsmiBasic";
			this.tsmiBasic.Size = new Size(156, 22);
			this.tsmiBasic.Text = "Basic";
			this.tsmiBasic.Visible = false;
			this.tsmiBasic.Click += this.tsmiBasic_Click;
			 */ 
			this.tsmiView.DropDownItems.AddRange(new ToolStripItem[4]
			{
				this.tsmiTree,
				this.tsmiHelp,
				this.tsmiToolBar,
				this.tsmiStatusBar
			});
			this.tsmiView.Name = "tsmiView";
			this.tsmiView.Size = new Size(47, 21);
			this.tsmiView.Text = "View";
			this.tsmiTree.Name = "tsmiTree";
			this.tsmiTree.Size = new Size(135, 22);
			this.tsmiTree.Text = "TreeView";
			this.tsmiTree.Click += this.tsmiTree_Click;
			this.tsmiHelp.Name = "tsmiHelp";
			this.tsmiHelp.Size = new Size(135, 22);
			this.tsmiHelp.Text = "HelpView";
			this.tsmiHelp.Click += this.tsmiHelp_Click;
			this.tsmiToolBar.Checked = true;
			this.tsmiToolBar.CheckState = CheckState.Checked;
			this.tsmiToolBar.Name = "tsmiToolBar";
			this.tsmiToolBar.Size = new Size(135, 22);
			this.tsmiToolBar.Text = "Toolbar";
			this.tsmiToolBar.Click += this.tsmiToolBar_Click;
			this.tsmiStatusBar.Checked = true;
			this.tsmiStatusBar.CheckState = CheckState.Checked;
			this.tsmiStatusBar.Name = "tsmiStatusBar";
			this.tsmiStatusBar.Size = new Size(135, 22);
			this.tsmiStatusBar.Text = "Status Bar";
			this.tsmiStatusBar.Click += this.tsmiStatusBar_Click;
			this.tsmiLanguage.Name = "tsmiLanguage";
			this.tsmiLanguage.Size = new Size(77, 21);
			this.tsmiLanguage.Text = "Language";
			this.tsmiExtras.Name = "tsmiExtras";
			this.tsmiExtras.Size = new Size(77, 21);
			this.tsmiExtras.Text = "Extras";
			this.tsmiExtras.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.tsmiContactsDownload,
				this.tsmiCalibration,
				this.tsmiDMRID,
				tsmiOpenGD77,
				tsmiFirmwareLoader
			});

			this.tsmiContactsDownload.Name = "tsmiContactsDownload";
			//this.tsmiContactsDownload.ShortcutKeys = (Keys)131154;
			this.tsmiContactsDownload.Size = new Size(156, 22);
			this.tsmiContactsDownload.Text = "Download contacts";
			this.tsmiContactsDownload.Click += this.tsbtnContactsDownload_Click;

			this.tsmiDMRID.Name = "tsmiDMRID";
			//this.tsmiContactsDownload.ShortcutKeys = (Keys)131154;
			this.tsmiDMRID.Size = new Size(156, 22);
			this.tsmiDMRID.Text = "DMR ID";
			this.tsmiDMRID.Enabled = true;
			this.tsmiDMRID.Click += this.tsbtnDMRID_Click;

			this.tsmiCalibration.Name = "tsmiCalibration";
			//this.tsmiCalibration.ShortcutKeys = (Keys)131154;
			this.tsmiCalibration.Size = new Size(156, 22);
			this.tsmiCalibration.Text = "Calibration editor";
			this.tsmiCalibration.Enabled = true;
			this.tsmiCalibration.Click += new EventHandler(this.tsbtnCalibration_Click);

			this.tsmiOpenGD77.Name = "tsmiOpenGD77";
			//this.tsmiCalibration.ShortcutKeys = (Keys)131154;
			this.tsmiOpenGD77.Size = new Size(156, 22);
			this.tsmiOpenGD77.Text = "OpenGD77 support";
			this.tsmiOpenGD77.Enabled = true;
			this.tsmiOpenGD77.Click += new EventHandler(this.tsmiOpenGD77_Click);


			this.tsmiFirmwareLoader.Name = "tsmiFirmwareLoader";
			//this.tsmiCalibration.ShortcutKeys = (Keys)131154;
			this.tsmiFirmwareLoader.Size = new Size(156, 22);
			this.tsmiFirmwareLoader.Text = "Firmware loader";
			this.tsmiFirmwareLoader.Enabled = true;
			this.tsmiFirmwareLoader.Click += new EventHandler(this.tsmiFirmwareLoader_Click);

			this.tsmiWindow.DropDownItems.AddRange(new ToolStripItem[4]
			{
				this.tsmiCascade,
				this.tsmiTileHor,
				this.tsmiTileVer,
				this.tsmiCloseAll
			});
			this.tsmiWindow.Name = "tsmiWindow";
			this.tsmiWindow.Size = new Size(67, 21);
			this.tsmiWindow.Text = "Window";
			this.tsmiCascade.Name = "tsmiCascade";
			this.tsmiCascade.Size = new Size(157, 22);
			this.tsmiCascade.Text = "Cascade";
			this.tsmiCascade.Click += this.tsmiCascade_Click;
			this.tsmiTileHor.Name = "tsmiTileHor";
			this.tsmiTileHor.Size = new Size(157, 22);
			this.tsmiTileHor.Text = "Tile Horzontal";
			this.tsmiTileHor.Click += this.tsmiTileHor_Click;
			this.tsmiTileVer.Name = "tsmiTileVer";
			this.tsmiTileVer.Size = new Size(157, 22);
			this.tsmiTileVer.Text = "Tile Vertical";
			this.tsmiTileVer.Click += this.tsmiTileVer_Click;
			this.tsmiCloseAll.Name = "tsmiCloseAll";
			this.tsmiCloseAll.Size = new Size(157, 22);
			this.tsmiCloseAll.Text = "Close All";
			this.tsmiCloseAll.Click += this.tsmiCloseAll_Click;
			this.tsmiAbout.Name = "tsmiAbout";
			this.tsmiAbout.Size = new Size(55, 21);
			this.tsmiAbout.Text = "About";
			this.tsmiAbout.Click += this.tsbtnAbout_Click;
			this.cmsGroup.Items.AddRange(new ToolStripItem[2]
			{
				this.tsmiAdd,
				this.tsmiClear
			});
			this.cmsGroup.Name = "cmsGroup";
			this.cmsGroup.Size = new Size(139, 48);
			this.cmsGroup.Opening += this.cmsGroup_Opening;
			this.tsmiAdd.Name = "tsmiAdd";
			this.tsmiAdd.ShortcutKeyDisplayString = "Enter";
			this.tsmiAdd.ShortcutKeys = (Keys.LButton | Keys.MButton | Keys.Back | Keys.Control);
			this.tsmiAdd.Size = new Size(138, 22);
			this.tsmiAdd.Text = "Add";
			this.tsmiAdd.Click += this.tsmiAdd_Click;
			this.tsmiClear.Name = "tsmiClear";
			this.tsmiClear.Size = new Size(138, 22);
			this.tsmiClear.Text = "Clear";
			this.tsmiClear.Click += this.tsmiClear_Click;
			this.cmsSub.Items.AddRange(new ToolStripItem[8]
			{
				this.tsmiDel,
				this.tsmiRename,
				this.toolStripSeparator4,
				this.tsmiCopy,
				this.tsmiPaste,
				this.toolStripSeparator4,
				this.tsmiMoveUp,
				this.tsmiMoveDown
			});
			this.cmsSub.Name = "cmsSub";
			this.cmsSub.Size = new Size(159, 98);
			this.cmsSub.Opening += this.cmsSub_Opening;
			this.tsmiDel.Name = "tsmiDel";
			this.tsmiDel.ShortcutKeyDisplayString = "";
			this.tsmiDel.ShortcutKeys = Keys.Delete;
			this.tsmiDel.Size = new Size(158, 22);
			this.tsmiDel.Text = "Delete";
			this.tsmiDel.Click += this.tsmiDel_Click;
			this.tsmiRename.Name = "tsmiRename";
			this.tsmiRename.ShortcutKeys = Keys.F2;
			this.tsmiRename.Size = new Size(158, 22);
			this.tsmiRename.Text = "Rename";
			this.tsmiRename.Click += this.tsmiRename_Click;
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new Size(155, 6);
			this.tsmiCopy.Name = "tsmiCopy";
			this.tsmiCopy.ShortcutKeys = (Keys)131139;
			this.tsmiCopy.Size = new Size(158, 22);
			this.tsmiCopy.Text = "Copy";
			this.tsmiCopy.Click += this.tsmiCopy_Click;
			this.tsmiPaste.Name = "tsmiPaste";
			this.tsmiPaste.ShortcutKeys = (Keys)131158;
			this.tsmiPaste.Size = new Size(158, 22);
			this.tsmiPaste.Text = "Paste";
			this.tsmiPaste.Click += this.tsmiPaste_Click;

			this.tsmiMoveUp.Name = "tsmiMoveUp";
			this.tsmiMoveUp.ShortcutKeys = (Keys)131157;
			this.tsmiMoveUp.Size = new Size(158, 22);
			this.tsmiMoveUp.Text = "Move up";
			this.tsmiMoveUp.Click += this.tsmiMoveUp_Click;

			this.tsmiMoveDown.Name = "tsmiMoveDown";
			this.tsmiMoveDown.ShortcutKeys = (Keys)131140;
			this.tsmiMoveDown.Size = new Size(158, 22);
			this.tsmiMoveDown.Text = "Move down";
			this.tsmiMoveDown.Click += this.tsmiMoveDown_Click;

			this.cmsGroupContact.Items.AddRange(new ToolStripItem[1]
			{
				this.tsmiAddContact
			});
			this.cmsGroupContact.Name = "cmsGroup";
			this.cmsGroupContact.Size = new Size(101, 26);
			this.cmsGroupContact.Opening += this.cmsGroupContact_Opening;
			this.tsmiAddContact.DropDownItems.AddRange(new ToolStripItem[3]
			{
				this.tsmiGroupCall,
				this.tsmiPrivateCall,
				this.tsmiAllCall
			});
			this.tsmiAddContact.Name = "tsmiAddContact";
			this.tsmiAddContact.Size = new Size(100, 22);
			this.tsmiAddContact.Text = "Add";
			this.tsmiGroupCall.Name = "tsmiGroupCall";
			this.tsmiGroupCall.ShortcutKeyDisplayString = "Enter";
			this.tsmiGroupCall.ShortcutKeys = (Keys.LButton | Keys.MButton | Keys.Back | Keys.Control);
			this.tsmiGroupCall.Size = new Size(231, 22);
			this.tsmiGroupCall.Text = "Group Call";
			this.tsmiGroupCall.Click += this.tsmiGroupCall_Click;
			this.tsmiPrivateCall.Name = "tsmiPrivateCall";
			this.tsmiPrivateCall.ShortcutKeyDisplayString = "Ctrl+Alt+Enter";
			this.tsmiPrivateCall.ShortcutKeys = (Keys.LButton | Keys.MButton | Keys.Back | Keys.Control | Keys.Alt);
			this.tsmiPrivateCall.Size = new Size(231, 22);
			this.tsmiPrivateCall.Text = "Private Call";
			this.tsmiPrivateCall.Click += this.tsmiPrivateCall_Click;
			this.tsmiAllCall.Name = "tsmiAllCall";
			this.tsmiAllCall.Size = new Size(231, 22);
			this.tsmiAllCall.Text = "All Call";
			this.tsmiAllCall.Click += this.tsmiAllCall_Click;
			//this.ofdMain.Filter = "GD-77 codeplug (*.dat,*.g77)|*.dat;*.g77";
			this.ofdMain.Filter = "OpenGD77 (*.g77)|*.g77|GD-77 (*.dat,*.g77)|*.dat;*.g77";
			this.sfdMain.Filter = "OpenGD77 (*.g77)|*.g77|GD-77 (*.dat,*.g77)|*.dat;*.g77";
			this.cmsTree.Items.AddRange(new ToolStripItem[2]
			{
				this.tsmiCollapseAll,
				this.tsmiExpandAll
			});
			this.cmsTree.Name = "cmsTree";
			this.cmsTree.Size = new Size(145, 48);
			this.tsmiCollapseAll.Name = "tsmiCollapseAll";
			this.tsmiCollapseAll.Size = new Size(144, 22);
			this.tsmiCollapseAll.Text = "Collapse All";
			this.tsmiCollapseAll.Click += this.tsmiCollapseAll_Click;
			this.tsmiExpandAll.Name = "tsmiExpandAll";
			this.tsmiExpandAll.Size = new Size(144, 22);
			this.tsmiExpandAll.Text = "Expand All";
			this.tsmiExpandAll.Click += this.tsmiExpandAll_Click;
			this.dockPanel.AllowEndUserDocking = false;
			this.dockPanel.Dock = DockStyle.Fill;
			this.dockPanel.DocumentStyle = DocumentStyle.SystemMdi;
			this.dockPanel.Location = new Point(234, 52);
			this.dockPanel.Margin = new Padding(3, 4, 3, 4);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.Size = new Size(865, 635);
			dockPanelGradient.EndColor = SystemColors.ControlLight;
			dockPanelGradient.StartColor = SystemColors.ControlLight;
			autoHideStripSkin.DockStripGradient = dockPanelGradient;
			tabGradient.EndColor = SystemColors.Control;
			tabGradient.StartColor = SystemColors.Control;
			tabGradient.TextColor = SystemColors.ControlDarkDark;
			autoHideStripSkin.TabGradient = tabGradient;
			autoHideStripSkin.TextFont = new Font("Microsoft Yahei", 9f);
			dockPanelSkin.AutoHideStripSkin = autoHideStripSkin;
			tabGradient2.EndColor = SystemColors.ControlLightLight;
			tabGradient2.StartColor = SystemColors.ControlLightLight;
			tabGradient2.TextColor = SystemColors.ControlText;
			dockPaneStripGradient.ActiveTabGradient = tabGradient2;
			dockPanelGradient2.EndColor = SystemColors.Control;
			dockPanelGradient2.StartColor = SystemColors.Control;
			dockPaneStripGradient.DockStripGradient = dockPanelGradient2;
			tabGradient3.EndColor = SystemColors.ControlLight;
			tabGradient3.StartColor = SystemColors.ControlLight;
			tabGradient3.TextColor = SystemColors.ControlText;
			dockPaneStripGradient.InactiveTabGradient = tabGradient3;
			dockPaneStripSkin.DocumentGradient = dockPaneStripGradient;
			dockPaneStripSkin.TextFont = new Font("Microsoft Yahei", 9f);
			tabGradient4.EndColor = SystemColors.ActiveCaption;
			tabGradient4.LinearGradientMode = LinearGradientMode.Vertical;
			tabGradient4.StartColor = SystemColors.GradientActiveCaption;
			tabGradient4.TextColor = SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient.ActiveCaptionGradient = tabGradient4;
			tabGradient5.EndColor = SystemColors.Control;
			tabGradient5.StartColor = SystemColors.Control;
			tabGradient5.TextColor = SystemColors.ControlText;
			dockPaneStripToolWindowGradient.ActiveTabGradient = tabGradient5;
			dockPanelGradient3.EndColor = SystemColors.ControlLight;
			dockPanelGradient3.StartColor = SystemColors.ControlLight;
			dockPaneStripToolWindowGradient.DockStripGradient = dockPanelGradient3;
			tabGradient6.EndColor = SystemColors.InactiveCaption;
			tabGradient6.LinearGradientMode = LinearGradientMode.Vertical;
			tabGradient6.StartColor = SystemColors.GradientInactiveCaption;
			tabGradient6.TextColor = SystemColors.InactiveCaptionText;
			dockPaneStripToolWindowGradient.InactiveCaptionGradient = tabGradient6;
			tabGradient7.EndColor = Color.Transparent;
			tabGradient7.StartColor = Color.Transparent;
			tabGradient7.TextColor = SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient.InactiveTabGradient = tabGradient7;
			dockPaneStripSkin.ToolWindowGradient = dockPaneStripToolWindowGradient;
			dockPanelSkin.DockPaneStripSkin = dockPaneStripSkin;
			//rjc this.dockPanel.Skin = dockPanelSkin;
			this.dockPanel.TabIndex = 6;
			this.pnlTvw.Controls.Add(this.tvwMain);
			this.pnlTvw.Dock = DockStyle.Left;
			this.pnlTvw.Location = new Point(0, 0);
			this.pnlTvw.Margin = new Padding(3, 4, 3, 4);
			this.pnlTvw.Name = "pnlTvw";
			this.pnlTvw.Size = new Size(234, 709);
			this.pnlTvw.TabIndex = 9;
			this.tvwMain.Dock = DockStyle.Fill;
			this.tvwMain.ImageIndex = 0;
			this.tvwMain.ImageList = this.imgMain;
			this.tvwMain.Location = new Point(0, 0);
			this.tvwMain.Margin = new Padding(3, 4, 3, 4);
			this.tvwMain.Name = "tvwMain";
			this.tvwMain.SelectedImageIndex = 0;
			this.tvwMain.Size = new Size(234, 709);
			this.tvwMain.TabIndex = 0;
			this.tvwMain.TabStop = false;
			this.tvwMain.AfterLabelEdit += this.tvwMain_AfterLabelEdit;
			this.tvwMain.DoubleClick += this.tvwMain_DoubleClick;
			this.tvwMain.NodeMouseClick += this.tvwMain_NodeMouseClick;
			this.tvwMain.BeforeLabelEdit += this.tvwMain_BeforeLabelEdit;
			this.tvwMain.KeyDown += this.tvwMain_KeyDown;
			this.ssrMain.Items.AddRange(new ToolStripItem[1]
			{
				this.slblComapny
			});
			this.ssrMain.Location = new Point(234, 687);
			this.ssrMain.Name = "ssrMain";
			this.ssrMain.Padding = new Padding(1, 0, 17, 0);
			this.ssrMain.Size = new Size(865, 22);
			this.ssrMain.TabIndex = 12;
			this.ssrMain.Text = "statusStrip1";
			this.slblComapny.Name = "slblComapny";
			this.slblComapny.Size = new Size(63, 17);
			this.slblComapny.Text = "Prompt：";
			this.tsrMain.Items.AddRange(new ToolStripItem[8]
			{
				this.tsbtnNew,
				this.tsbtnOpen,
				this.tsbtnSave,
				this.toolStripSeparator2,
				this.tsbtnRead,
				this.tsbtnWrite,
				this.toolStripSeparator3,
				this.tsbtnAbout
			});
			this.tsrMain.Location = new Point(234, 27);
			this.tsrMain.Name = "tsrMain";
			this.tsrMain.Size = new Size(865, 25);
			this.tsrMain.TabIndex = 13;
			this.tsrMain.Text = "toolStrip1";
			this.tsbtnNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnNew.Image = (Image)componentResourceManager.GetObject("tsbtnNew.Image");
			this.tsbtnNew.ImageTransparentColor = Color.Magenta;
			this.tsbtnNew.Name = "tsbtnNew";
			this.tsbtnNew.Size = new Size(23, 22);
			this.tsbtnNew.Text = "New";
			this.tsbtnNew.Click += this.tsbtnNew_Click;
			this.tsbtnOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnOpen.Image = (Image)componentResourceManager.GetObject("tsbtnOpen.Image");
			this.tsbtnOpen.ImageTransparentColor = Color.Magenta;
			this.tsbtnOpen.Name = "tsbtnOpen";
			this.tsbtnOpen.Size = new Size(23, 22);
			this.tsbtnOpen.Text = "Open";
			this.tsbtnOpen.Click += this.tsbtnOpen_Click;
			this.tsbtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnSave.Image = (Image)componentResourceManager.GetObject("tsbtnSave.Image");
			this.tsbtnSave.ImageTransparentColor = Color.Magenta;
			this.tsbtnSave.Name = "tsbtnSave";
			this.tsbtnSave.Size = new Size(23, 22);
			this.tsbtnSave.Text = "Save";
			this.tsbtnSave.ToolTipText = "Save";
			this.tsbtnSave.Click += this.tsbtnSave_Click;
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(6, 25);
			this.tsbtnRead.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnRead.Image = (Image)componentResourceManager.GetObject("tsbtnRead.Image");
			this.tsbtnRead.ImageTransparentColor = Color.Magenta;
			this.tsbtnRead.Name = "tsbtnRead";
			this.tsbtnRead.Size = new Size(23, 22);
			this.tsbtnRead.Text = "Read";
			this.tsbtnRead.Click += this.tsbtnRead_Click;
			this.tsbtnWrite.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnWrite.Image = (Image)componentResourceManager.GetObject("tsbtnWrite.Image");
			this.tsbtnWrite.ImageTransparentColor = Color.Magenta;
			this.tsbtnWrite.Name = "tsbtnWrite";
			this.tsbtnWrite.Size = new Size(23, 22);
			this.tsbtnWrite.Text = "Write";
			this.tsbtnWrite.Click += this.tsbtnWrite_Click;
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new Size(6, 25);
			this.tsbtnAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnAbout.Image = (Image)componentResourceManager.GetObject("tsbtnAbout.Image");
			this.tsbtnAbout.ImageTransparentColor = Color.Magenta;
			this.tsbtnAbout.Name = "tsbtnAbout";
			this.tsbtnAbout.Size = new Size(23, 22);
			this.tsbtnAbout.Text = "About";
			this.tsbtnAbout.Click += this.tsbtnAbout_Click;
			base.AutoScaleDimensions = new SizeF(7f, 16f);

			base.ClientSize = new Size(1099, 709);
			base.Controls.Add(this.dockPanel);
			base.Controls.Add(this.tsrMain);
			base.Controls.Add(this.ssrMain);
			base.Controls.Add(this.mnsMain);
			base.Controls.Add(this.pnlTvw);
			this.DoubleBuffered = true;
			base.IsMdiContainer = true;
			base.KeyPreview = true;
			base.MainMenuStrip = this.mnsMain;
			base.Margin = new Padding(3, 4, 3, 4);
			base.Name = "MainForm";
			string 	version = AssemblyName.GetAssemblyName(System.Reflection.Assembly.GetExecutingAssembly().Location).Version.ToString();

			this.Text = MainForm.PRODUCT_NAME;
			base.WindowState = FormWindowState.Maximized;
			base.Load += this.MainForm_Load;
			base.MdiChildActivate += this.MainForm_MdiChildActivate;

			base.KeyDown += this.MainForm_KeyDown;
			this.mnsMain.ResumeLayout(false);
			this.mnsMain.PerformLayout();
			this.cmsGroup.ResumeLayout(false);
			this.cmsSub.ResumeLayout(false);
			this.cmsGroupContact.ResumeLayout(false);
			this.cmsTree.ResumeLayout(false);
			this.pnlTvw.ResumeLayout(false);
			this.ssrMain.ResumeLayout(false);
			this.ssrMain.PerformLayout();
			this.tsrMain.ResumeLayout(false);
			this.tsrMain.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();

			

		}

		private IDockContent method_0(string string_0)
		{
			if (string_0 == typeof(TreeForm).ToString())
			{
				return this.frmTree;
			}
			if (string_0 == typeof(HelpForm).ToString())
			{
				return this.frmHelp;
			}
			return null;
		}

		public static string[] StartupArgs;

		public MainForm(string[] args)
		{
			MainForm.StartupArgs = args;

			this.frmHelp = new HelpForm();
			this.frmTree = new TreeForm();
			this.lstTreeNodeItem = new List<TreeNodeItem>();
			//base._002Ector();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			this._TextBox = new TextBox();
			this._TextBox.Visible = false;
			this._TextBox.LostFocus += this._TextBox_LostFocus;
			this._TextBox.Validating += this._TextBox_Validating;
			this._TextBox.KeyPress += Settings.smethod_57;
			base.Controls.Add(this._TextBox);
			this.m_deserializeDockContent = this.method_0;
			this.initialiseTree();
			this.method_20(this.method_19());
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Settings.dicCommon.Add("None", Settings.SZ_NONE);
			Settings.dicCommon.Add("Selected", Settings.SZ_SELECTED);
			Settings.dicCommon.Add("Read", Settings.SZ_READ);
			Settings.dicCommon.Add("Write", Settings.SZ_WRITE);
			Settings.dicCommon.Add("ReadComplete", Settings.SZ_READ_COMPLETE);
			Settings.dicCommon.Add("WriteComplete", Settings.SZ_WRITE_COMPLETE);
			Settings.dicCommon.Add(Settings.SZ_NAME_EXIST_NAME, "Name exists");
			Settings.dicCommon.Add("FirstChNotDelete", Settings.SZ_FIRST_CH_NOT_DELETE);
			Settings.dicCommon.Add("IdNotEmpty", Settings.SZ_ID_NOT_EMPTY);
			Settings.dicCommon.Add("IdOutOfRange", Settings.SZ_ID_OUT_OF_RANGE);
			Settings.dicCommon.Add("OpenSuccessfully", Settings.SZ_OPEN_SUCCESSFULLY);
			Settings.dicCommon.Add("SaveSuccessfully", Settings.SZ_SAVE_SUCCESSFULLY);
			Settings.dicCommon.Add("TypeNotMatch", Settings.SZ_TYPE_NOT_MATCH);
			Settings.dicCommon.Add("NotSelectItemNotCopyItem", Settings.SZ_NOT_SELECT_ITEM_NOT_COPYITEM);
			Settings.dicCommon.Add("PromptKey1", Settings.SZ_PROMPT_KEY1);
			Settings.dicCommon.Add("PromptKey2", Settings.SZ_PROMPT_KEY2);
			Settings.dicCommon.Add("DataFormatError","Data format error");
			Settings.dicCommon.Add("FirstNotDelete","The first row cannot be deleted");
			Settings.dicCommon.Add("KeyPressDtmf", "");
			Settings.dicCommon.Add("KeyPressDigit", "");
			Settings.dicCommon.Add("KeyPressPrint", "");
			Settings.dicCommon.Add("DeviceNotFound", "");
			Settings.dicCommon.Add("CommError", "");
            Settings.dicCommon.Add("codePlugReadConfirm", Settings.SZ_CODEPLUG_READ_CONFIRM);
            Settings.dicCommon.Add("codePlugWriteConfirm", Settings.SZ_CODEPLUG_WRITE_CONFIRM);
            Settings.dicCommon.Add("pleaseConfirm", Settings.SZ_PLEASE_CONFIRM);

			Settings.dicCommon.Add("userAgreement", Settings.SZ_USER_AGREEMENT);
			Settings.dicCommon.Add("DownloadContactsMessageAdded", Settings.SZ_DOWNLOADCONTACTS_REGION_EMPTY);
			Settings.dicCommon.Add("DownloadContactsRegionEmpty", Settings.SZ_DOWNLOADCONTACTS_MESSAGE_ADDED);
			Settings.dicCommon.Add("DownloadContactsDownloading", Settings.SZ_DOWNLOADCONTACTS_DOWNLOADING);
			Settings.dicCommon.Add("DownloadContactsSelectContactsToImport", Settings.SZ_DOWNLOADCONTACTS_SELECT_CONTACTS_TO_IMPORT);
			Settings.dicCommon.Add("DownloadContactsTooMany", Settings.SZ_DOWNLOADCONTACTS_TOO_MANY);
			Settings.dicCommon.Add("Warning", Settings.SZ_WARNING);
			Settings.dicCommon.Add("UnableDownloadFromInternet", Settings.SZ_UNABLEDOWNLOADFROMINTERNET);
			Settings.dicCommon.Add("DownloadContactsImportComplete", Settings.SZ_IMPORT_COMPLETE);
			Settings.dicCommon.Add("CodeplugUpgradeNotice", Settings.SZ_CODEPLUG_UPGRADE_NOTICE);
			Settings.dicCommon.Add("CodeplugUpgradeWarningToManyRxGroups", Settings.SZ_CODEPLUG_UPGRADE_WARNING_TO_MANY_RX_GROUPS);

			Settings.dicCommon.Add("CodeplugRead", Settings.SZ_CODEPLUG_READ);
			Settings.dicCommon.Add("CodeplugWrite", Settings.SZ_CODEPLUG_WRITE);
			Settings.dicCommon.Add("DMRIDRead", Settings.SZ_DMRID_READ);
			Settings.dicCommon.Add("DMRIDWrite", Settings.SZ_DMRID_WRITE);
			Settings.dicCommon.Add("CalibrationRead", Settings.SZ_CALIBRATION_READ);
			Settings.dicCommon.Add("CalibrationWrite", Settings.SZ_CALIBRATION_WRITE);
			Settings.dicCommon.Add("IdAlreadyExists", Settings.SZ_ID_ALREADY_EXISTS);
			Settings.dicCommon.Add("ContactNameDuplicate", Settings.SZ_CONTACT_DUPLICATE_NAME);


			Settings.dicCommon.Add("EnableMemoryAccessMode", Settings.SZ_EnableMemoryAccessMode);
			Settings.dicCommon.Add("dataRead", Settings.SZ_dataRead);
			Settings.dicCommon.Add("dataWrite", Settings.SZ_dataWrite);
			Settings.dicCommon.Add("DMRIdContcatsTotal", Settings.SZ_DMRIdContcatsTotal);
			Settings.dicCommon.Add("ErrorParsingData", Settings.SZ_ErrorParsingData);
			Settings.dicCommon.Add("DMRIdIntroMessage", Settings.SZ_DMRIdIntroMessage);

			string text = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
			if (File.Exists(text))
			{
				this.dockPanel.LoadFromXml(text, this.m_deserializeDockContent);
			}
			using (Graphics graphics = base.CreateGraphics())
			{
				Settings.smethod_7(new SizeF(graphics.DpiX / 96f, graphics.DpiY / 96f));
			}
			//MainForm.CurCom = IniFileUtils.smethod_4("Setup", "Com", "Com1");
			//MainForm.CurCbr = IniFileUtils.smethod_2("Setup", "Baudrate", 9600);
			//MainForm.CurModel = IniFileUtils.smethod_4("Setup", "Model", "SG");
			//

	/* Roger Clark
	 * No not read Exprert / basic mode from Setup any more is the CPS is now permanently set in Expert mode 2.
	string text2 = IniFileUtils.smethod_4("Setup", "Power", "");

	if (string.IsNullOrEmpty(text2))
	{
		Settings.smethod_9("");
		Settings.smethod_5(Settings.UserMode.Basic);
		Settings.CUR_MODE = 0;
		this.tsmiBasic.Visible = false;
	}
	else
	{
		
		string text3 = Base64Utils.smethod_1(text2);
		if (text3 == "DMR961510")
		{
			this.tsmiBasic.Visible = true;
			Settings.smethod_9(text3);
			Settings.smethod_5(Settings.UserMode.Expert);
			Settings.CUR_MODE = 1;
		}
		else if (text3 == "TYT760")
		{
			this.tsmiBasic.Visible = true;
			Settings.smethod_9(text3);
			Settings.smethod_5(Settings.UserMode.Expert);
			Settings.CUR_MODE = 2;
		}
	}*/

			this.tsmiBasic.Visible = true;
			Settings.smethod_9("TYT760");
			Settings.smethod_5(Settings.UserMode.Expert);
			Settings.CUR_MODE = 2;


			ChannelForm.CurCntCh = 1024;
			this.method_15();
			string lastFileName = String.Empty;

			if (MainForm.StartupArgs.Length > 0)
			{
				if (File.Exists(StartupArgs[0]))
				{
					openCodeplugFile(StartupArgs[0]);
					lastFileName = StartupArgs[0];
				}
				else
				{
					this.loadDefaultOrInitialFile();
					lastFileName = "";
					IniFileUtils.WriteProfileString("Setup", "LastFilePath", "");
				}
			}
			else
			{
				string tmp = IniFileUtils.getProfileStringWithDefault("Setup", "LastFilePath", "");
				if ("" == tmp)
				{
					this.loadDefaultOrInitialFile();
				}
				else
				{
					lastFileName = IniFileUtils.getProfileStringWithDefault("Setup", "LastFilePath", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
					if (lastFileName!=null && lastFileName != "" && File.Exists(lastFileName))
					{
						openCodeplugFile(lastFileName);
					}
					else
					{
						this.loadDefaultOrInitialFile();
						lastFileName = "";
						IniFileUtils.WriteProfileString("Setup", "LastFilePath", "");
					}
				}
			}

			this.frmHelp.Show(this.dockPanel);
			this.frmTree.Show(this.dockPanel);
			this.pnlTvw.Dock = DockStyle.Fill;
			this.frmTree.Controls.Add(this.pnlTvw);
			ChannelForm.DefaultCh = ChannelForm.data[0].Clone();
			NormalScanForm.DefaultScan = NormalScanForm.data[0].Clone();
			ContactForm.DefaultContact = ContactForm.data[0].Clone();
			EmergencyForm.DefaultEmg = EmergencyForm.data[0].Clone();
			BootItemForm.DefaultBootItem = Settings.smethod_65(BootItemForm.data);
			ButtonForm.DefaultSideKey = Settings.smethod_65(ButtonForm.data);
			ScanBasicForm.DefaultScanBasic = Settings.smethod_65(ScanBasicForm.data);
			SignalingBasicForm.DefaultSignalingBasic = Settings.smethod_65(SignalingBasicForm.data);
			DtmfForm.DefaultDtmf = Settings.smethod_65(DtmfForm.data);
			EncryptForm.DefaultEncrypt = Settings.smethod_65(EncryptForm.data);
			GeneralSetForm.DefaultGeneralSet = Settings.smethod_65(GeneralSetForm.data);
			AttachmentForm.DefaultAttachment = Settings.smethod_65(AttachmentForm.data);
			VfoForm.DefaultCh = VfoForm.data[0].Clone();
			MenuForm.DefaultMenu = Settings.smethod_65(MenuForm.data);
			this.imgMain.Images.Clear();
			this.imgMain.Images.AddStrip(Resources.smethod_0());
			base.AutoScaleMode = AutoScaleMode.Font;
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			this.GetAllLang();


			string b = IniFileUtils.getProfileStringWithDefault("Setup", "Language", "English.xml");
			foreach (ToolStripMenuItem dropDownItem in this.tsmiLanguage.DropDownItems)
			{
				string fileName = Path.GetFileName(dropDownItem.Tag.ToString());
				if (!(fileName == b))
				{
					continue;
				}
				dropDownItem.PerformClick();
				break;
			}


			this.Text = getMainTitleStub() + " " + lastFileName;

			if (IniFileUtils.getProfileStringWithDefault("Setup", "agreedToTerms", "no") == "no")
			{
				if (DialogResult.Yes !=  MessageBox.Show(Settings.dicCommon["userAgreement"], Settings.dicCommon["pleaseConfirm"], MessageBoxButtons.YesNo))
				{
					if (System.Windows.Forms.Application.MessageLoop)
					{
						// Use this since we are a WinForms app
						System.Windows.Forms.Application.Exit();
					}
					else
					{
						// Use this since we are a console app
						System.Environment.Exit(1);
					}
				}
				else
				{
					IniFileUtils.WriteProfileString("Setup", "agreedToTerms", "yes");
					base.FormClosing += this.MainForm_FormClosing;
				}
			}
			else
			{
				base.FormClosing += this.MainForm_FormClosing;
			}
		}

		private string getMainTitleStub()
		{
			Version ver = AssemblyName.GetAssemblyName(System.Reflection.Assembly.GetExecutingAssembly().Location).Version;//.ToString();
			DateTime dt = new DateTime(2000, 1, 1, 0, 0, 0).AddDays(ver.Build).AddSeconds(ver.Revision * 2);

			return MainForm.PRODUCT_NAME + " (Build date " + dt.ToString("yyyyMMdd")+ ")";
		}


		private void MainForm_MdiChildActivate(object sender, EventArgs e)
		{
			if (base.ActiveMdiChild != null)
			{
				Type type = base.ActiveMdiChild.GetType();
				if (MainForm.PreActiveMdiChild != null)
				{
					MainForm.PreActiveMdiChild.SaveData();
				}
				MainForm.PreActiveMdiChild = (base.ActiveMdiChild as IDisp);
				this.RefreshForm(type);
			}
			else
			{
				MainForm.PreActiveMdiChild = null;
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			switch (MessageBox.Show(Settings.dicCommon["PromptKey1"], "", MessageBoxButtons.YesNoCancel))
			{
			case DialogResult.Cancel:
				e.Cancel = true;
				break;
			case DialogResult.Yes:
				this.tsmiSave.PerformClick();
				break;
			}
			/* Roger Clark. This function never seems to actually save this value to the ini file, so I'm commenting it out as it doesnt do as advertised
			if (IniFileUtils.smethod_2("Setup", "SaveDockPanel", 0) != 0)
			{
				string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
				this.dockPanel.SaveAsXml(fileName);
			}
			 */
		}

		private void method_1(Form form_0)
		{
			IDisp disp = form_0 as IDisp;
			if (disp != null)
			{
				disp.SaveData();
			}
		}

		private void method_2()
		{
			this.method_1(base.ActiveMdiChild);
		}

		private void method_3()
		{
			this.method_2();
			Form[] mdiChildren = base.MdiChildren;
			Form[] array = mdiChildren;
			foreach (Form form in array)
			{
				if (form != base.ActiveMdiChild)
				{
					this.method_1(form);
				}
			}
		}

		private void closeAllForms()
		{
			Form[] mdiChildren = base.MdiChildren;
			Form[] array = mdiChildren;
			foreach (Form form in array)
			{
				form.Close();
			}
		}

		private void method_5(Type type_0)
		{
			Form[] mdiChildren = base.MdiChildren;
			Form[] array = mdiChildren;
			foreach (Form form in array)
			{
				if (form.GetType() == type_0)
				{
					form.Close();
				}
			}
		}

		private Form method_6(TreeNode treeNode_0)
		{
			if (treeNode_0 == null)
			{
				return null;
			}
			TreeNodeItem treeNodeItem = treeNode_0.Tag as TreeNodeItem;
			if (treeNodeItem != null)
			{
				Form[] mdiChildren = base.MdiChildren;
				foreach (Form form in mdiChildren)
				{
					if (form.GetType() == treeNodeItem.Type)
					{
						return form;
					}
				}
			}
			return null;
		}

		private Form method_7(TreeNode treeNode_0, bool bool_0)
		{
			TreeNodeItem treeNodeItem = treeNode_0.Tag as TreeNodeItem;
			if (treeNodeItem != null)
			{
				Form[] mdiChildren = base.MdiChildren;
				int num = 0;
				while (num < mdiChildren.Length)
				{
					Form form = mdiChildren[num];
					if (form.GetType() != treeNodeItem.Type)
					{
						num++;
						continue;
					}
					form.Activate();
					form.BringToFront();
					IDisp disp = form as IDisp;
					if (disp == null)
					{
						return form;
					}
					disp.SaveData();
					disp.Node = treeNode_0;
					if (bool_0)
					{
						form.Tag = treeNodeItem.Index;
					}
					disp.DispData();
                    return form;
				}
				if (treeNodeItem.Type != null)
				{
					Form form2 = (Form)Activator.CreateInstance(treeNodeItem.Type);
					form2.MdiParent = this;
					IDisp disp2 = form2 as IDisp;
					if (disp2 != null)
					{
						disp2.Node = treeNode_0;
						if (bool_0)
						{
							form2.Tag = treeNodeItem.Index;
						}
					}
					form2.Show();
					form2.Focus();
					form2.BringToFront();
					return form2;
				}
			}
			return null;
		}

		public void RefreshForm(Type type)
		{
			Form[] mdiChildren = base.MdiChildren;
			foreach (Form form in mdiChildren)
			{
				if (form.GetType() == type)
				{
					IDisp disp = form as IDisp;
					if (disp != null)
					{
						disp.DispData();
					}
				}
			}
		}

		public void SaveForm(Type type)
		{
			Form[] mdiChildren = base.MdiChildren;
			foreach (Form form in mdiChildren)
			{
				if (form.GetType() == type)
				{
					this.method_1(form);
				}
			}
		}

		public ISingleRow GetForm(Type type)
		{
			Form[] mdiChildren = base.MdiChildren;
			int num = 0;
			ISingleRow singleRow;
			while (true)
			{
				if (num < mdiChildren.Length)
				{
					Form form = mdiChildren[num];
					if (form.GetType() == type)
					{
						singleRow = (form as ISingleRow);
						if (singleRow != null)
						{
							break;
						}
					}
					num++;
					continue;
				}
				return null;
			}
			return singleRow;
		}

		public void VerifyRelatedForm(Type type)
		{
			if (type == typeof(ContactsForm))
			{
				ButtonForm.data.Verify(ButtonForm.DefaultSideKey);
				ChannelForm.data.Verify();
				RxGroupListForm.data.Verify();
				DigitalKeyContactForm.data.Verify();
			}
			else if (type == typeof(ChannelsForm))
			{
				NormalScanForm.data.Verify();
				ZoneForm.data.Verify();
				ZoneForm.basicData.Verify();
				EmergencyForm.data.Verify();
			}
		}

		public void RefreshRelatedForm(Type type, int index)
		{
			if (type == typeof(ContactForm))
			{
				ISingleRow form = this.GetForm(typeof(ContactsForm));
				if (form != null)
				{
					form.RefreshSingleRow(index);
				}
			}
			else if (type == typeof(ChannelForm))
			{
				ISingleRow form2 = this.GetForm(typeof(ChannelsForm));
				if (form2 != null)
				{
					form2.RefreshSingleRow(index);
				}
			}
		}

		public void RefreshRelatedForm(Type type)
		{
			this.RefreshRelatedForm(type, true);
		}

		public void RefreshRelatedForm(Type type, bool self)
		{
			if (type == typeof(DeviceInfoForm))
			{
				ChannelForm.data.Verify();
				this.RefreshForm(typeof(ChannelForm));
				VfoForm.data.Verify();
				this.RefreshForm(typeof(VfoForm));
			}
			else if (type == typeof(TextMsgForm))
			{
				this.RefreshForm(typeof(ButtonForm));
			}
			else if (type == typeof(EncryptForm))
			{
				this.RefreshForm(typeof(ChannelForm));
			}
			else if (type == typeof(ContactForm))
			{
				this.RefreshForm(typeof(ButtonForm));
				this.RefreshForm(typeof(ChannelForm));
				this.RefreshForm(typeof(RxGroupListForm));
				this.RefreshForm(typeof(DigitalKeyContactForm));
				this.RefreshForm(typeof(ContactsForm));
			}
			else if (type == typeof(ContactsForm))
			{
				this.RefreshForm(typeof(ButtonForm));
				this.RefreshForm(typeof(ChannelForm));
				this.RefreshForm(typeof(RxGroupListForm));
				this.RefreshForm(typeof(DigitalKeyContactForm));
				this.RefreshForm(typeof(ContactForm));
			}
			else if (type == typeof(NormalScanForm))
			{
				this.RefreshForm(typeof(ChannelForm));
			}
			else if (type == typeof(ChannelForm))
			{
				if (self)
				{
					this.RefreshForm(typeof(ChannelForm));
				}
				this.RefreshForm(typeof(NormalScanForm));
				this.RefreshForm(typeof(ZoneForm));
				this.RefreshForm(typeof(ZoneBasicForm));
				this.RefreshForm(typeof(AttachmentForm));
				this.RefreshForm(typeof(EmergencyForm));
				this.RefreshForm(typeof(ChannelsForm));
			}
			else if (type == typeof(ChannelsForm))
			{
				this.RefreshForm(typeof(ChannelForm));
				this.RefreshForm(typeof(NormalScanForm));
				this.RefreshForm(typeof(ZoneForm));
				this.RefreshForm(typeof(ZoneBasicForm));
				this.RefreshForm(typeof(AttachmentForm));
				this.RefreshForm(typeof(EmergencyForm));
			}
			else if (type == typeof(DtmfContactForm))
			{
				this.RefreshForm(typeof(ButtonForm));
			}
			else if (type == typeof(RxGroupListForm))
			{
				this.RefreshForm(typeof(ChannelForm));
			}
			else if (type == typeof(ZoneForm))
			{
				this.RefreshForm(typeof(ZoneBasicForm));
				this.RefreshForm(typeof(AttachmentForm));
				if (self)
				{
					this.RefreshForm(typeof(ZoneForm));
				}
			}
			else if (type == typeof(ZoneBasicForm))
			{
				this.RefreshForm(typeof(NormalScanForm));
			}
			else if (type == typeof(EmergencyForm))
			{
				this.RefreshForm(typeof(ChannelForm));
			}
		}

		public void InitTree()
		{
			this.tvwMain.Nodes.Clear();
			this.initialiseTree();
			this.method_20(this.method_19());
			this.lstFixedNode = this.tvwMain.smethod_5();
			this.lstFixedNode.ForEach(MainForm.smethod_0);
			this.InitDynamicNode();
			Settings.smethod_49(this.tvwMain, 0);
		}

		public void InitRxGroupLists(TreeNode parentNode)
		{
			int num = 0;
			for (num = 0; num < RxListData.CNT_RX_LIST; num++)
			{
				if (RxGroupListForm.data.DataIsValid(num))
				{
					this.AddTreeViewNode(parentNode.Nodes, RxGroupListForm.data[num].Name, new TreeNodeItem(this.cmsSub, typeof(RxGroupListForm), null, RxListData.CNT_RX_LIST, num, 19, RxGroupListForm.data));
				}
			}
		}

		public void InitEmergencySystems(TreeNode parentNode)
		{
			int num = 0;
			for (num = 0; num < 32; num++)
			{
				if (EmergencyForm.data.DataIsValid(num))
				{
					this.AddTreeViewNode(parentNode.Nodes, EmergencyForm.data[num].Name, new TreeNodeItem(this.cmsSub, typeof(EmergencyForm), null, 32, num, 11, EmergencyForm.data));
				}
			}
		}

		public void InitZones(TreeNode parentNode)
		{
			int num = 0;
			try
			{
				for (num = 0; num < ZoneForm.NUM_ZONES; num++)
				{
					if (ZoneForm.data.DataIsValid(num))
					{
						this.AddTreeViewNode(parentNode.Nodes, ZoneForm.data.GetName(num), new TreeNodeItem(this.cmsSub, typeof(ZoneForm), null, ZoneForm.NUM_ZONES, num, 25, ZoneForm.data));
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void InitDigitContacts(TreeNode parentNode)
		{
			int num = 0;
			this.method_5(typeof(ContactForm));
			parentNode.Nodes.Clear();
			for (num = 0; num < 1024; num++)
			{
				if (ContactForm.data.DataIsValid(num))
				{
					if (ContactForm.data.GetCallType(num) == 0)
					{
						this.AddTreeViewNode(parentNode.Nodes, ContactForm.data[num].Name, new TreeNodeItem(this.cmsSub, typeof(ContactForm), null, 1024, num, 8, ContactForm.data));
					}
					else if (ContactForm.data.GetCallType(num) == 1)
					{
						this.AddTreeViewNode(parentNode.Nodes, ContactForm.data[num].Name, new TreeNodeItem(this.cmsSub, typeof(ContactForm), null, 1024, num, 10, ContactForm.data));
					}
					else if (ContactForm.data.GetCallType(num) == 2)
					{
						this.AddTreeViewNode(parentNode.Nodes, ContactForm.data[num].Name, new TreeNodeItem(this.cmsSub, typeof(ContactForm), null, 1024, num, 7, ContactForm.data));
					}
				}
			}
		}

		public void InitChannels(TreeNode parentNode)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.method_5(typeof(ChannelForm));
			parentNode.Nodes.Clear();
			for (num = 0; num < ChannelForm.CurCntCh; num++)
			{
				num3 = num;
				if (ChannelForm.data.DataIsValid(num3))
				{
					switch (ChannelForm.data.GetChMode(num3))
					{
					case 0:
						num2 = 2;
						break;
					case 1:
						num2 = 6;
						break;
					default:
						num2 = 54;
						break;
					}
					this.AddTreeViewNode(parentNode.Nodes, ChannelForm.data.GetName(num3), new TreeNodeItem(this.cmsSub, typeof(ChannelForm), null, ChannelForm.CurCntCh, num, num2, ChannelForm.data));
				}
			}
		}

		public void InitScans(TreeNode parentNode)
		{
			int num = 0;
			for (num = 0; num < 64; num++)
			{
				if (NormalScanForm.data.DataIsValid(num))
				{
					this.AddTreeViewNode(parentNode.Nodes, NormalScanForm.data[num].Name, new TreeNodeItem(this.cmsSub, typeof(NormalScanForm), null, 64, num, 26, NormalScanForm.data));
				}
			}
		}

		public void InsertTreeViewNode(TreeNode parentNode, int index, Type formType, int imageIndex, IData data)
		{
			string name = data.GetName(index);
			this.AddTreeViewNode(parentNode.Nodes, name, new TreeNodeItem(this.cmsSub, formType, null, data.Count, index, imageIndex, data));
		}

		public void DeleteTreeViewNode(TreeNode parentNode, int index)
		{
			parentNode.Nodes.RemoveAt(index);
		}

		public void RefreshTreeNodeText(TreeNode parentNode, int rowIndex, int index)
		{
			parentNode.Nodes[rowIndex].Text = ContactForm.data[index].Name;
		}

		public void RefreshTreeNodeImage(TreeNode parentNode, int rowIndex, int index)
		{
			TreeNode treeNode = parentNode.Nodes[rowIndex];
			TreeNodeItem treeNodeItem = treeNode.Tag as TreeNodeItem;
			treeNode.ImageIndex = treeNodeItem.ImageIndex;
			treeNode.SelectedImageIndex = treeNodeItem.ImageIndex;
		}

		public TreeNode AddTreeViewNode(TreeNodeCollection parentNode, string text, object tag)
		{
			TreeNode treeNode = null;
			TreeNodeItem treeNodeItem = tag as TreeNodeItem;
			if (treeNodeItem == null)
			{
				treeNode.Name = text;
				treeNode = parentNode.Add(text);
				treeNode.ImageIndex = 2;
				treeNode.SelectedImageIndex = 2;
				treeNode.Tag = null;
			}
			else
			{
				treeNode = ((treeNodeItem.Index >= 0) ? parentNode.Insert(treeNodeItem.Index, text) : parentNode.Add(text));
				treeNode.Name = text;
				treeNode.ImageIndex = treeNodeItem.ImageIndex;
				treeNode.SelectedImageIndex = treeNodeItem.ImageIndex;
				treeNode.Tag = tag;
			}
			return treeNode;
		}

		public TreeNode GetTreeNodeByType(Type type)
		{
			return this.method_9(type, this.tvwMain.Nodes);
		}

		public TreeNode GetTreeNodeByType(Type type, int index)
		{
			TreeNode treeNodeByType = this.GetTreeNodeByType(type);
			foreach (TreeNode node in treeNodeByType.Nodes)
			{
				TreeNodeItem treeNodeItem = node.Tag as TreeNodeItem;
				if (treeNodeItem != null && treeNodeItem.Index == index)
				{
					return node;
				}
			}
			return null;
		}

		private TreeNode method_8(Type type_0, TreeNodeCollection treeNodeCollection_0)
		{
			foreach (TreeNode item in treeNodeCollection_0)
			{
				TreeNodeItem treeNodeItem = item.Tag as TreeNodeItem;
				if (treeNodeItem != null && treeNodeItem.SubType == type_0)
				{
					return item;
				}
				TreeNode treeNode2 = this.method_8(type_0, item.Nodes);
				if (treeNode2 != null)
				{
					return treeNode2;
				}
			}
			return null;
		}

		private TreeNode method_9(Type type_0, TreeNodeCollection treeNodeCollection_0)
		{
			foreach (TreeNode item in treeNodeCollection_0)
			{
				TreeNodeItem treeNodeItem = item.Tag as TreeNodeItem;
				if (treeNodeItem != null && treeNodeItem.Type == type_0)
				{
					return item;
				}
				TreeNode treeNode2 = this.method_9(type_0, item.Nodes);
				if (treeNode2 != null)
				{
					return treeNode2;
				}
			}
			return null;
		}

		public TreeNode GetTreeNodeByTypeAndIndex(Type type, int index, TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				TreeNodeItem treeNodeItem = node.Tag as TreeNodeItem;
				if (treeNodeItem != null && treeNodeItem.Type == type && treeNodeItem.Index == index)
				{
					return node;
				}
				TreeNode treeNodeByTypeAndIndex = this.GetTreeNodeByTypeAndIndex(type, index, node.Nodes);
				if (treeNodeByTypeAndIndex != null)
				{
					return treeNodeByTypeAndIndex;
				}
			}
			return null;
		}

		private void _TextBox_LostFocus(object sender, EventArgs e)
		{
			this._TextBox.Visible = false;
		}

		private void _TextBox_Validating(object sender, CancelEventArgs e)
		{
			this.tvwMain.SelectedNode.Text = this._TextBox.Text;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr intptr_0, uint uint_0, IntPtr intptr_1, IntPtr intptr_2);

		private void tvwMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (e.Node.Parent != null && e.Node != null)
				{
					this.tvwMain.SelectedNode = e.Node;
					TreeNodeItem treeNodeItem = e.Node.Tag as TreeNodeItem;
					if (treeNodeItem != null && treeNodeItem.Cms != null)
					{
						this.method_2();
						treeNodeItem.Cms.Show(this.tvwMain, e.X, e.Y);
					}
				}
				else
				{
					this.cmsTree.Show(this.tvwMain, e.X, e.Y);
				}
			}
		}

		private void tvwMain_DoubleClick(object sender, EventArgs e)
		{
			TreeNode treeNode = null;
			TreeNodeItem treeNodeItem = null;
			treeNode = this.tvwMain.SelectedNode;
			if (treeNode != null)
			{
				treeNodeItem = (treeNode.Tag as TreeNodeItem);
				if (treeNodeItem != null)
				{
					this.method_7(treeNode, true);
				}
			}
		}

		private void tvwMain_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			TreeNode node = e.Node;
			IntPtr intPtr = MainForm.SendMessage(this.tvwMain.Handle, 4367u, IntPtr.Zero, IntPtr.Zero);
			if (intPtr != IntPtr.Zero)
			{
				int value = 0;
				TreeNodeItem treeNodeItem = e.Node.Tag as TreeNodeItem;
				if (treeNodeItem != null)
				{
					if (treeNodeItem.Type == typeof(ChannelForm))
					{
						value = 16;
					}
					else if (treeNodeItem.Type == typeof(ContactForm))
					{
						value = 16;
					}
					else if (treeNodeItem.Type == typeof(EmergencyForm))
					{
						value = 8;
					}
					else if (treeNodeItem.Type == typeof(NormalScanForm))
					{
						value = 15;
					}
					else if (treeNodeItem.Type == typeof(ZoneForm))
					{
						value = 16;
					}
					else if (treeNodeItem.Type == typeof(RxGroupListForm))
					{
						value = 16;
					}
				}
				MainForm.SendMessage(intPtr, 197u, new IntPtr(value), IntPtr.Zero);
			}
		}

		private void tvwMain_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			try
			{
				if (e.Label == null)
				{
					return;
				}
				if (e.Node.Text == e.Label)
				{
					return;
				}
				if (Settings.smethod_50(e.Node, e.Label))
				{
					MessageBox.Show("Name exists");
					e.CancelEdit = true;
				}
				else
				{
					TreeNodeItem treeNodeItem = e.Node.Tag as TreeNodeItem;
					if (treeNodeItem != null)
					{
						IData data = treeNodeItem.Data;
						data.SetName(treeNodeItem.Index, e.Label);
						this.RefreshRelatedForm(treeNodeItem.Type);
						e.Node.Text = data.GetName(treeNodeItem.Index);
						e.CancelEdit = true;
						Form form = this.method_6(e.Node);
						if (form != null)
						{
							IDisp disp = form as IDisp;
							if (disp != null && (int)form.Tag == treeNodeItem.Index)
							{
								disp.RefreshName();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			this.tvwMain.LabelEdit = false;
		}

		private void tvwMain_KeyDown(object sender, KeyEventArgs e)
		{
			TreeView treeView = sender as TreeView;
			if (treeView != null)
			{
				TreeNode selectedNode = treeView.SelectedNode;
				if (selectedNode != null)
				{
					TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
					
					if (treeNodeItem != null && treeNodeItem.Cms != null)
					{
						Keys keys = e.KeyData;
						if (e.KeyData == Keys.Return)
						{
							keys |= Keys.Control;
						}
						ToolStripMenuItem toolStripMenuItem = this.method_10(treeNodeItem.Cms.Items, keys);
						if (toolStripMenuItem != null)
						{
							if (treeNodeItem.Type != null)
							{
								switch (treeNodeItem.Type.Name)
								{
									case "ZoneForm":
										if (selectedNode.Index == selectedNode.Parent.Nodes.Count - 1)
										{
											this.tsmiMoveDown.Visible = false;
										}
										else
										{
											this.tsmiMoveDown.Visible = true;
										}
										if (selectedNode.Index == 0)
										{
											this.tsmiMoveUp.Visible = false;
										}
										else
										{
											this.tsmiMoveUp.Visible = true;
										}
										break;
									case "ChannelForm":
									default:
										this.tsmiMoveDown.Visible = false;
										this.tsmiMoveUp.Visible = false;
										break;
								}
							}
							toolStripMenuItem.PerformClick();
						}
					}
				}
			}
		}

		private ToolStripMenuItem method_10(ToolStripItemCollection toolStripItemCollection_0, Keys keys_0)
		{
			foreach (ToolStripItem item in toolStripItemCollection_0)
			{
				ToolStripMenuItem toolStripMenuItem = item as ToolStripMenuItem;
				if (toolStripMenuItem != null)
				{
					if (toolStripMenuItem.ShortcutKeys != keys_0)
					{
						ToolStripMenuItem toolStripMenuItem2 = this.method_10(toolStripMenuItem.DropDownItems, keys_0);
						if (toolStripMenuItem2 == null)
						{
							continue;
						}
						return toolStripMenuItem2;
					}
					return toolStripMenuItem;
				}
			}
			return null;
		}

		private void tsmiCollapseAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in this.tvwMain.Nodes)
			{
				foreach (TreeNode node2 in node.Nodes)
				{
					if (node2.IsExpanded)
					{
						node2.Collapse(false);
					}
				}
			}
		}

		private void tsmiExpandAll_Click(object sender, EventArgs e)
		{
			this.tvwMain.ExpandAll();
		}

		private void cmsGroup_Opening(object sender, CancelEventArgs e)
		{
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null)
				{
					this.tsmiAdd.Visible = (selectedNode.Nodes.Count < treeNodeItem.Count);
					this.tsmiClear.Visible = (selectedNode.Nodes.Count > 1);
				}
			}
		}

		private void tsmiAdd_Click(object sender, EventArgs e)
		{
			int num = -1;
			string text = "";
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null && selectedNode.Nodes.Count < treeNodeItem.Count && treeNodeItem.SubType != null)
				{
					num = treeNodeItem.Data.GetMinIndex();
					text = treeNodeItem.Data.GetMinName(selectedNode);
					treeNodeItem.Data.SetIndex(num, 1);
					treeNodeItem.Data.Default(num);
					if (treeNodeItem.SubType == typeof(NormalScanForm))
					{
						this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, treeNodeItem.SubType, null, 0, num, 26, treeNodeItem.Data));
					}
					else if (treeNodeItem.SubType == typeof(ZoneForm))
					{
						this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, treeNodeItem.SubType, null, 0, num, 25, treeNodeItem.Data));
					}
					else if (treeNodeItem.SubType == typeof(ChannelForm))
					{
						this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, treeNodeItem.SubType, null, 0, num, 2, treeNodeItem.Data));
						ChannelForm.Channel channel = (ChannelForm.Channel)treeNodeItem.Data;
						channel.SetChMode(num, ChannelForm.ChModeE.Analog);
						channel.SetDefaultFreq(num);
					}
					else if (treeNodeItem.SubType == typeof(RxGroupListForm))
					{
						this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, treeNodeItem.SubType, null, 0, num, 19, treeNodeItem.Data));
					}
					else if (treeNodeItem.SubType == typeof(EmergencyForm))
					{
						this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, treeNodeItem.SubType, null, 0, num, 11, treeNodeItem.Data));
					}
					treeNodeItem.Data.SetName(num, text);
					this.slblComapny.Text = string.Format(Settings.SZ_ADD + text);
					if (!selectedNode.IsExpanded)
					{
						selectedNode.Expand();
					}
					IDisp disp = base.ActiveMdiChild as IDisp;
					if (disp != null)
					{
						disp.SaveData();
					}
					this.RefreshRelatedForm(treeNodeItem.SubType);
				}
			}
		}

		private void tsmiClear_Click(object sender, EventArgs e)
		{
			int num = 0;
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null && selectedNode.Nodes.Count > 1 && treeNodeItem.SubType != null)
				{
					string text = string.Format(treeNodeItem.Data.Format, num + 1);
					treeNodeItem.Data.SetName(0, text);
					selectedNode.Nodes[0].Text = text;
					treeNodeItem.Data.Default(0);
					for (num = 1; num < treeNodeItem.Data.Count; num++)
					{
						treeNodeItem.Data.SetIndex(num, 0);
					}
					while (selectedNode.Nodes.Count > 1)
					{
						selectedNode.Nodes.RemoveAt(1);
					}
					if (!selectedNode.IsExpanded)
					{
						selectedNode.Expand();
					}
					Form form = this.method_6(selectedNode.Nodes[0]);
					if (form != null)
					{
						IDisp disp = form as IDisp;
						if (disp != null)
						{
							disp.Node = selectedNode.Nodes[0];
							form.Tag = 0;
							disp.DispData();
						}
					}
					this.RefreshRelatedForm(treeNodeItem.SubType);
				}
			}
		}

		private void cmsSub_Opening(object sender, CancelEventArgs e)
		{
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null)
				{
					switch (treeNodeItem.Type.Name)
					{
						case "ZoneForm":
							if (selectedNode.Index == selectedNode.Parent.Nodes.Count - 1)
							{
								this.tsmiMoveDown.Visible = false;
							}
							else
							{
								this.tsmiMoveDown.Visible = true;
							}
							if (selectedNode.Index == 0)
							{
								this.tsmiMoveUp.Visible = false;
							}
							else
							{
								this.tsmiMoveUp.Visible = true;
							}
							break;
						case "ChannelForm":
						default:
							this.tsmiMoveDown.Visible = false;
							this.tsmiMoveUp.Visible = false;
							break;
					}
					if (treeNodeItem.Type == typeof(ChannelForm))
					{
						if (treeNodeItem.Index + 1 == ZoneForm.data.FstZoneFstCh)
						{
							this.tsmiDel.Visible = true;
							//this.tsmiDel.Visible = false;
						}
						else
						{
							this.tsmiDel.Visible = true;
							//this.tsmiDel.Visible = (selectedNode.Parent.Nodes.Count != 1 && selectedNode.Index != 0);
						}
					}
					else
					{
						this.tsmiDel.Visible = true;
						//this.tsmiDel.Visible = (selectedNode.Parent.Nodes.Count != 1 && selectedNode.Index != 0);
					}
					this.tsmiPaste.Visible = (this.CopyItem != null && this.CopyItem != treeNodeItem && this.CopyItem.Type == treeNodeItem.Type);
				}
			}
		}

		private void tsmiRename_Click(object sender, EventArgs e)
		{
			this.method_2();
			this.tvwMain.LabelEdit = true;
			this.tvwMain.SelectedNode.BeginEdit();
		}

		private void tsmiDel_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = null;
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			TreeNodeItem treeNodeItem = null;
			if (selectedNode != null)
			{
				treeNode = selectedNode.Parent;
				treeNodeItem = (selectedNode.Tag as TreeNodeItem);
				this.method_2();
				if (treeNodeItem != null && treeNode.Nodes.Count > 0 && treeNodeItem.Cms == this.cmsSub)// && selectedNode.Index != 0)
				{
					if (false && treeNodeItem.Type == typeof(ChannelForm) && treeNodeItem.Index + 1 == ZoneForm.data.FstZoneFstCh)
					{
						MessageBox.Show(Settings.dicCommon["FirstChNotDelete"]);
					}
					else
					{
						Form form = this.method_6(selectedNode);
						if (form != null)
						{
							IDisp disp = form as IDisp;
							if (disp != null && selectedNode == disp.Node)
							{
								if (selectedNode.NextNode != null)
								{
									this.tvwMain.SelectedNode = selectedNode.NextNode;
								}
								else if (selectedNode.PrevNode != null)
								{
									this.tvwMain.SelectedNode = selectedNode.PrevNode;
								}
								else
								{
									this.tvwMain.SelectedNode = treeNode;
								}
								disp.Node = this.tvwMain.SelectedNode;
								TreeNodeItem treeNodeItem2 = disp.Node.Tag as TreeNodeItem;
								form.Tag = treeNodeItem2.Index;
								disp.DispData();
							}
						}
						TreeNode parentNode = selectedNode.Parent;// Always get the parent node as it may be needed after the selected node has been deleted
						treeNode.Nodes.Remove(selectedNode);
						if (treeNodeItem != null)
						{
							IDisp disp2 = base.ActiveMdiChild as IDisp;
							if (disp2 != null)
							{
								disp2.SaveData();
							}
							treeNodeItem.Data.ClearIndex(treeNodeItem.Index);
							treeNodeItem.Data.Default(treeNodeItem.Index);
							this.RefreshRelatedForm(treeNodeItem.Type);
						}
						if (treeNodeItem == this.CopyItem)
						{
						    this.CopyItem = null;
						}
                        if (treeNodeItem.Type == typeof(ZoneForm))
                        {
                            ZoneForm.CompactZones();
                        //    parentNode.Nodes.Clear();
                        //    this.InitZones(parentNode);
                        }
                    }
				}
			}
		}

		private void tsmiCopy_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				this.CopyItem = (selectedNode.Tag as TreeNodeItem);
			}
		}

		private void tsmiPaste_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			TreeNodeItem treeNodeItem = null;
			if (selectedNode != null)
			{
				TreeNode parent = selectedNode.Parent;
				treeNodeItem = (selectedNode.Tag as TreeNodeItem);
				if (treeNodeItem != null && this.CopyItem != null)
				{
					if (this.CopyItem.Type != treeNodeItem.Type)
					{
						MessageBox.Show(Settings.dicCommon["TypeNotMatch"]);
					}
					else
					{
						if (treeNodeItem != null)
						{
							IDisp disp = base.ActiveMdiChild as IDisp;
							if (disp != null)
							{
								disp.SaveData();
							}
						}
						treeNodeItem.Paste(this.CopyItem);
						Form form = this.method_6(selectedNode);
						if (form != null)
						{
							IDisp disp2 = form as IDisp;
							if (disp2 != null && disp2.Node == selectedNode)
							{
								disp2.DispData();
							}
						}
						this.RefreshRelatedForm(treeNodeItem.Type);
					}
				}
				else
				{
					MessageBox.Show(Settings.dicCommon["NotSelectItemNotCopyItem"]);
				}
			}
		}

		private void tsmiMoveDown_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = (selectedNode.Tag as TreeNodeItem);
				switch (treeNodeItem.Type.Name)
				{
					case "ZoneForm":
						closeForm(treeNodeItem);
						this.tvwMain.Focus();
						ZoneForm.MoveZoneDown(selectedNode.Index);
						TreeNode parentNode = selectedNode.Parent;
						parentNode.Nodes.Clear();
                        this.InitZones(parentNode);
						if (selectedNode.Index < parentNode.Nodes.Count-1)
						{
							this.tvwMain.SelectedNode = parentNode.Nodes[selectedNode.Index + 1];
						}
						else
						{
							this.tvwMain.SelectedNode = parentNode.Nodes[selectedNode.Index];
						}
						break;
					case "ChannelForm":
					default:
						break;
				}
			}
		}

		private void tsmiMoveUp_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = (selectedNode.Tag as TreeNodeItem);
				switch (treeNodeItem.Type.Name)
				{
					case "ZoneForm":
						closeForm(treeNodeItem);
						this.tvwMain.Focus();
						ZoneForm.MoveZoneUp(selectedNode.Index);
						TreeNode parentNode = selectedNode.Parent;
                        parentNode.Nodes.Clear();
                        this.InitZones(parentNode);
						if(selectedNode.Index>0)
						{
							this.tvwMain.SelectedNode = parentNode.Nodes[selectedNode.Index - 1];
						}
						else
						{
							this.tvwMain.SelectedNode = parentNode.Nodes[selectedNode.Index];
						}
						break;
					case "ChannelForm":
					default:
						break;
				}
			}

		}

		private void closeForm(TreeNodeItem treenodeitem)
		{
			Form[] mdiChildren = base.MdiChildren;
			int num = 0;
			while (num < mdiChildren.Length)
			{
				Form form = mdiChildren[num];
				if (form.GetType() == treenodeitem.Type)
				{
					form.Close();
				}
				num++;
			}
		}


		private void cmsGroupContact_Opening(object sender, CancelEventArgs e)
		{
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null)
				{
					if (selectedNode.Nodes.Count >= treeNodeItem.Count)
					{
						this.tsmiAddContact.Enabled = false;
					}
					else
					{
						this.tsmiAddContact.Enabled = true;
					}
				}
			}
			if (ContactForm.data.HaveAll())
			{
				this.tsmiAllCall.Enabled = false;
			}
			else
			{
				this.tsmiAllCall.Enabled = true;
			}
		}

		private void tsmiGroupCall_Click(object sender, EventArgs e)
		{
			int num = -1;
			string text = "";
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			IDisp disp = base.ActiveMdiChild as IDisp;
			if (disp != null)
			{
				disp.SaveData();
			}
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null && selectedNode.Nodes.Count < treeNodeItem.Count)
				{
					num = treeNodeItem.Data.GetMinIndex();
					text = treeNodeItem.Data.GetMinName(selectedNode);
					treeNodeItem.Data.SetIndex(num, 0);
					this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, typeof(ContactForm), null, 0, num, 8, treeNodeItem.Data));
					ContactForm.Contact contact = (ContactForm.Contact)treeNodeItem.Data;
					treeNodeItem.Data.Default(num);
					contact.SetCallID(num, contact.GetMinCallID(0, num));
					treeNodeItem.Data.SetName(num, text);
					this.RefreshRelatedForm(treeNodeItem.SubType);
					if (!selectedNode.IsExpanded)
					{
						selectedNode.Expand();
					}
				}
			}
		}

		private void tsmiPrivateCall_Click(object sender, EventArgs e)
		{
			int num = -1;
			string text = "";
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null && selectedNode.Nodes.Count < treeNodeItem.Count)
				{
					num = treeNodeItem.Data.GetMinIndex();
					text = treeNodeItem.Data.GetMinName(selectedNode);
					treeNodeItem.Data.SetIndex(num, 1);
					this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, typeof(ContactForm), null, 0, num, 10, treeNodeItem.Data));
					ContactForm.Contact contact = (ContactForm.Contact)treeNodeItem.Data;
					treeNodeItem.Data.Default(num);
					contact.SetCallID(num, contact.GetMinCallID(1, num));
					treeNodeItem.Data.SetName(num, text);
					this.RefreshRelatedForm(treeNodeItem.SubType);
					if (!selectedNode.IsExpanded)
					{
						selectedNode.Expand();
					}
				}
			}
		}

		private void tsmiAllCall_Click(object sender, EventArgs e)
		{
			int num = -1;
			string text = "";
			TreeNode selectedNode = this.tvwMain.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeItem treeNodeItem = selectedNode.Tag as TreeNodeItem;
				if (treeNodeItem != null && selectedNode.Nodes.Count < treeNodeItem.Count)
				{
					num = treeNodeItem.Data.GetMinIndex();
					text = treeNodeItem.Data.GetMinName(selectedNode);
					treeNodeItem.Data.SetIndex(num, 2);
					ContactForm.Contact contact = (ContactForm.Contact)treeNodeItem.Data;
					this.AddTreeViewNode(selectedNode.Nodes, text, new TreeNodeItem(this.cmsSub, typeof(ContactForm), null, 0, num, 7, treeNodeItem.Data));
					treeNodeItem.Data.Default(num);
					treeNodeItem.Data.SetName(num, text);
					contact.SetCallID(num, 16777215.ToString());
					this.RefreshRelatedForm(treeNodeItem.SubType);
					if (!selectedNode.IsExpanded)
					{
						selectedNode.Expand();
					}
				}
			}
		}

		private void tsmiSetting_DropDownOpening(object sender, EventArgs e)
		{
			this.tsmiDmrContacts.Visible = !ContactForm.data.ListIsEmpty;
			this.tsmiZone.Visible = !ZoneForm.data.ListIsEmpty;
			this.tsmiScanList.Visible = !NormalScanForm.data.ListIsEmpty;
			this.tsmiEmgSystem.Visible = !EmergencyForm.data.ListIsEmpty;
			this.tsmiGrpRxList.Visible = !RxGroupListForm.data.ListIsEmpty;
			this.tsmiChannels.Visible = !ChannelForm.data.ListIsEmpty;
		}

		private void loadDefaultOrInitialFile(string overRideWithFile=null)
		{
			string text = Application.StartupPath + "\\" + DEFAULT_DATA_FILE_NAME;
			if (overRideWithFile != null)
			{
				text = overRideWithFile;
			}

			if (!string.IsNullOrEmpty(text) && File.Exists(text))
			{
				byte[] eerom = File.ReadAllBytes(text);
				this.closeAllForms();
				MainForm.ByteToData(eerom);

				this.InitTree();
				this.Text = getMainTitleStub();
			}
		}

		private void tsbtnNew_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Settings.dicCommon["PromptKey2"], "", MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				this.loadDefaultOrInitialFile();
				MainForm.CurFileName = "";
				IniFileUtils.WriteProfileString("Setup", "LastFilePath", "");
			}
		}

		private void tsbtnSave_Click(object sender, EventArgs e)
		{
			string initialDirectory;
			//string text = Application.StartupPath + "\\Data";
		
			string lastFileName = IniFileUtils.getProfileStringWithDefault("Setup", "LastFilePath", "");
			try
			{
				if (lastFileName == "")
				{
					initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				}
				else
				{
					initialDirectory = Path.GetDirectoryName(lastFileName);//Application.StartupPath + "\\Data"); 
				}
			}
			catch (Exception)
			{
				initialDirectory = "";
			}


			try
			{
				if (string.IsNullOrEmpty(MainForm.CurFileName))
				{
					//Console.WriteLine(GeneralSetForm.data.RadioName);
#if OpenGD77
					this.sfdMain.FileName = GeneralSetForm.data.RadioName + "_" + DateTime.Now.ToString("MMdd_HHmmss") + ".g77";
#elif CP_VER_3_1_X
					this.sfdMain.FileName = GeneralSetForm.data.RadioName + "_" +  DateTime.Now.ToString("MMdd_HHmmss") + ".dat";
#endif

					this.sfdMain.InitialDirectory = initialDirectory;
				}
				else
				{
					this.sfdMain.InitialDirectory = Path.GetDirectoryName(MainForm.CurFileName);
					this.sfdMain.FileName = Path.GetFileNameWithoutExtension(MainForm.CurFileName)+".g77";
				}
				this.method_3();
				DialogResult dialogResult = this.sfdMain.ShowDialog();
				if (dialogResult == DialogResult.OK && !string.IsNullOrEmpty(this.sfdMain.FileName))
				{
					byte[] array = MainForm.DataToByte();
					Buffer.BlockCopy(Settings.CUR_MODEL, 0, array, 0, 8);
					File.WriteAllBytes(this.sfdMain.FileName, array);
					MainForm.CurFileName = this.sfdMain.FileName;

					MessageBox.Show(Settings.dicCommon["SaveSuccessfully"]);
					IniFileUtils.WriteProfileString("Setup", "LastFilePath", this.sfdMain.FileName);
					this.Text = getMainTitleStub() + " " + MainForm.CurFileName;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void openCodeplugFile(string fileName)
		{
			int index = 0;
			byte[] array = File.ReadAllBytes(fileName);
			bool test1 = !array.Take(8).All(MainForm.smethod_1);
			bool test2 = !array.Take(8).All((byte x) => x == Settings.CUR_MODEL[index++]);// RC. Note. Had to change preincrement to post increment

			if (test1 && test2)
			{
				MessageBox.Show(Settings.dicCommon["Model does not match"]);
				IniFileUtils.WriteProfileString("Setup", "LastFilePath", "");
			}
			else
			{
				//MessageBox.Show(Settings.dicCommon["OpenSuccessfully"]);
				MainForm.CurFileName = fileName;
				IniFileUtils.WriteProfileString("Setup", "LastFilePath", fileName);
				this.closeAllForms();

#if OpenGD77
				if (!checkZonesFor80Channels(array))
				{
					convertTo80ChannelZoneCodeplug(array);	
				}
#endif    
				MainForm.ByteToData(array,true);
				this.InitTree();
				this.Text = getMainTitleStub() + " " + fileName;
			}
		}

		private void tsbtnOpen_Click(object sender, EventArgs e)
		{
			try
			{
				string lastFileName = IniFileUtils.getProfileStringWithDefault("Setup", "LastFilePath", "");
				try
				{
					if (null == lastFileName || "" == lastFileName)
					{
						this.ofdMain.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					}
					else
					{
						this.ofdMain.InitialDirectory = Path.GetDirectoryName(lastFileName);//Application.StartupPath + "\\Data"); 
					}
				}
				catch (Exception)
				{
					this.ofdMain.InitialDirectory = "";
				}
				DialogResult dialogResult = this.ofdMain.ShowDialog();
				if (dialogResult == DialogResult.OK && !string.IsNullOrEmpty(this.ofdMain.FileName))
				{
					openCodeplugFile(this.ofdMain.FileName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void tsmiExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void tsmiDeviceInfo_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(DeviceInfoForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiGerneralSet_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(GeneralSetForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiButton_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(ButtonForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiMenu_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(MenuForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiBootItem_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(BootItemForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiNumKeyContact_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(DigitalKeyContactForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiTextMsg_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(TextMsgForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, false);
			}
		}

		private void tsmiEncrypt_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(EncryptForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, false);
			}
		}

		private void tsmiSignalingSystem_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(SignalingBasicForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, false);
			}
		}

		private void tsmiDtmf_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(DtmfForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, false);
			}
		}

		private void tsmiEmgSystem_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(EmergencyForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, false);
			}
		}

		private void tsmiDtmfContact_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(DtmfContactForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, false);
			}
		}

		private void method_12(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(ContactsForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiDmrContacts_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(ContactsForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiGrpRxList_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(RxGroupListForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiZoneBasic_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(ZoneBasicForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiZoneList_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(ZoneForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiChannels_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(ChannelsForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiScanBasic_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(ScanBasicForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiScanList_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.method_9(typeof(NormalScanForm), this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiVfoA_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.GetTreeNodeByTypeAndIndex(typeof(VfoForm), 0,this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}

		private void tsmiVfoB_Click(object sender, EventArgs e)
		{
			TreeNode treeNode = this.GetTreeNodeByTypeAndIndex(typeof(VfoForm), 1,this.tvwMain.Nodes);
			if (treeNode != null)
			{
				this.method_7(treeNode, true);
			}
		}



		private void tsbtnContactsDownload_Click(object sender, EventArgs e)
		{
			this.closeAllForms();
			DownloadContactsForm dlf = new DownloadContactsForm();
			dlf.mainForm = this;
			TreeNode treeNode = this.method_9(typeof(ContactsForm), this.tvwMain.Nodes);
			dlf.treeNode = treeNode;
			try
			{
				dlf.ShowDialog();
			}
			catch (Exception)
			{
				Cursor.Current = Cursors.Default;
				MessageBox.Show(Settings.dicCommon["UnableDownloadFromInternet"]);
				return;
			}
		}

		private void tsbtnDMRID_Click(object sender, EventArgs e)
		{
			this.closeAllForms();

			DMRIDForm dmrIdForm = new DMRIDForm();
			dmrIdForm.Show();
			return;
		}


		private void tsbtnCalibration_Click(object sender, EventArgs e)
		{
			this.closeAllForms();

			CalibrationForm cf = new CalibrationForm();
			cf.StartPosition = FormStartPosition.CenterParent;
			cf.ShowDialog();
		}

		private void openGD77Form(OpenGD77Form.CommsAction buttonAction)
		{
			/*
			String gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77");

			// If ths standrd port name can't be found so see if we have as saved one
			if (gd77CommPort == null)
			{
				string lastCommPort = IniFileUtils.getProfileStringWithDefault("Setup", "LastCommPort", "");
				if (lastCommPort != "")
				{
					gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix(lastCommPort);
				}
			}

			// if no port found so far, open the dialog to let the user select the port
			if (gd77CommPort == null)
			{
				CommPortSelector cps = new CommPortSelector();
				if (DialogResult.OK == cps.ShowDialog())
				{
					gd77CommPort = cps.SelectedPort;
					IniFileUtils.WriteProfileString("Setup", "LastCommPort", gd77CommPort);// assume they selected something useful !
				}

				//MessageBox.Show("Please connect the GD-77 running OpenGD77 firmware, and try again.", "OpenGD77 radio not detected.");
			}
			*/
			//if (gd77CommPort != null)
			{
				this.closeAllForms();
				OpenGD77Form cf = new OpenGD77Form(buttonAction);
				cf.StartPosition = FormStartPosition.CenterParent;
				cf.ShowDialog();
				InitTree();
			}
		}
		private void tsmiOpenGD77_Click(object sender, EventArgs e)
		{
			openGD77Form(OpenGD77Form.CommsAction.NONE);
		}

		private void tsmiFirmwareLoader_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "firmware files|*.sgl";
			openFileDialog.InitialDirectory = IniFileUtils.getProfileStringWithDefault("Setup", "LastFirmwareLocation", null);
			if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != null)
			{
				IniFileUtils.WriteProfileString("Setup", "LastFirmwareLocation", Path.GetDirectoryName(openFileDialog.FileName)); 
				FirmwareLoaderUI firmwareLoaderUI = new FirmwareLoaderUI(openFileDialog.FileName);
				firmwareLoaderUI.ShowDialog();
			}
		}
		
		private void tsbtnRead_Click(object sender, EventArgs e)
		{

			if (SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77") != null)
			{
				openGD77Form(OpenGD77Form.CommsAction.READ_CODEPLUG);
			}
			else
			{
				DialogResult result = MessageBox.Show(Settings.dicCommon["codePlugReadConfirm"], Settings.dicCommon["pleaseConfirm"], MessageBoxButtons.YesNo);
				if (result == DialogResult.Yes)
				{
					this.closeAllForms();
					CommPrgForm commPrgForm = new CommPrgForm(false);
					commPrgForm.StartPosition = FormStartPosition.CenterParent;
					//commPrgForm.IsRead = true;
					CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.codeplugRead;
					commPrgForm.ShowDialog();
					if (commPrgForm.IsSucess)
					{
						this.InitTree();
					}
				}
			}
		}

		private void tsbtnWrite_Click(object sender, EventArgs e)
		{
			if (SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77") != null)
			{
				openGD77Form(OpenGD77Form.CommsAction.WRITE_CODEPLUG);
			}
			else
			{
				DialogResult result = MessageBox.Show(Settings.dicCommon["codePlugWriteConfirm"], Settings.dicCommon["pleaseConfirm"], MessageBoxButtons.YesNo);
				if (result == DialogResult.Yes)
				{
					if (base.ActiveMdiChild != null)
					{
						base.ActiveMdiChild.ValidateChildren();
					}
					GeneralSetForm.data.KillState = 0;
					this.method_3();
					CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.codeplugWrite;
					CommPrgForm commPrgForm = new CommPrgForm(false);
					commPrgForm.StartPosition = FormStartPosition.CenterParent;
					//commPrgForm.IsRead = false;

					commPrgForm.ShowDialog();
				}
			}
		}

		/* Roger Clark. 
		 * Basic functionality request has been removed as the CPS will now always be in expert mode
		private void tsmiBasic_Click(object sender, EventArgs e)
		{
			this.closeAllForms();
			this.tsmiBasic.Visible = false;
			Settings.smethod_5(Settings.UserMode.Basic);
			Settings.CUR_MODE = 0;
			Settings.smethod_9("");
			IniFileUtils.WriteProfileString("Setup", "Power", "");
		}*/

		private void tsmiTree_Click(object sender, EventArgs e)
		{
			this.frmTree.Show(this.dockPanel);
		}

		private void tsmiHelp_Click(object sender, EventArgs e)
		{
            this.frmHelp.Show(this.dockPanel);// Roger Clark. Fixed issue where forms can't be shown when set to Dockstate of hidden
		}

		private void tsmiToolBar_Click(object sender, EventArgs e)
		{
			ToolStrip toolStrip = this.tsrMain;
			ToolStripMenuItem toolStripMenuItem = this.tsmiToolBar;
			bool visible = toolStripMenuItem.Checked = !this.tsmiToolBar.Checked;
			toolStrip.Visible = visible;
		}

		private void tsmiStatusBar_Click(object sender, EventArgs e)
		{
			StatusStrip statusStrip = this.ssrMain;
			ToolStripMenuItem toolStripMenuItem = this.tsmiStatusBar;
			bool visible = toolStripMenuItem.Checked = !this.tsmiStatusBar.Checked;
			statusStrip.Visible = visible;
		}

		private void tsmiCascade_Click(object sender, EventArgs e)
		{
			base.LayoutMdi(MdiLayout.Cascade);
		}

		private void tsmiTileHor_Click(object sender, EventArgs e)
		{
			base.LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void tsmiTileVer_Click(object sender, EventArgs e)
		{
			base.LayoutMdi(MdiLayout.TileVertical);
		}

		private void tsmiCloseAll_Click(object sender, EventArgs e)
		{
			this.closeAllForms();
		}

		private void tsbtnAbout_Click(object sender, EventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.StartPosition = FormStartPosition.CenterParent;
			aboutForm.ShowDialog();
		}

		private void method_13()
		{
			ButtonForm.RefreshCommonLang();
			BootItemForm.RefreshCommonLang();
			ChannelForm.RefreshCommonLang();
			ChannelsForm.RefreshCommonLang();
			ContactForm.RefreshCommonLang();
			ContactsForm.RefreshCommonLang();
			DigitalKeyContactForm.RefreshCommonLang();
			DtmfForm.RefreshCommonLang();
			EmergencyForm.RefreshCommonLang();
			EncryptForm.RefreshCommonLang();
			GeneralSetForm.RefreshCommonLang();
			MenuForm.RefreshCommonLang();
			NormalScanForm.RefreshCommonLang();
			VfoForm.RefreshCommonLang();
			Settings.smethod_10();
		}

		private void languageChangeHandler(object sender, EventArgs e)
		{
			this.slblComapny.Text = "";
			this.closeAllForms();
			this.frmHelp.ShowHelp(null);
			ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
			string text = toolStripMenuItem.Tag.ToString();
			IniFileUtils.WriteProfileString("Setup", "Language", Path.GetFileName(text));
			//Settings.smethod_1(text);
			Settings.setLanguageXMLFile(text);
			Settings.smethod_3(Path.ChangeExtension(text, "chm"));
			Settings.smethod_76("Read", ref Settings.SZ_READ);
			Settings.smethod_68(this);
			Settings.smethod_68(this.frmHelp);
			Settings.smethod_68(this.frmTree);
			Settings.smethod_70(this.cmsGroup.smethod_9(), base.Name);
			Settings.smethod_70(this.cmsGroupContact.smethod_9(), base.Name);
			Settings.smethod_70(this.cmsTree.smethod_9(), base.Name);
			Settings.smethod_70(this.cmsSub.smethod_9(), base.Name);
			Settings.smethod_72(Settings.dicCommon);
			this.method_13();
			List<string> list = new List<string>();
			list.Add(Settings.SZ_READ);
			Settings.smethod_75(list, new List<string>
			{
				"Read"
			});
			/*
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(Settings.smethod_0());
			*/

			MainForm.dicTree.Clear();
			string xpath = "/Resource/MainForm/TreeView/TreeNode";
			XmlNodeList xmlNodeList = Settings.languageXML.SelectNodes(xpath);
			foreach (XmlNode item in xmlNodeList)
			{
				string value = item.Attributes["Id"].Value;
				string value2 = item.Attributes["Text"].Value;
				MainForm.dicTree.Add(value, value2);
			}
			this.lstFixedNode.ForEach(MainForm.smethod_2);
			List<ToolStripMenuItem> list2 = this.mnsMain.smethod_7();
			Dictionary<string, string> dicMenuItem = new Dictionary<string, string>();
			xpath = "/Resource/MainForm/MenuStrip/MenuItem";
			xmlNodeList = Settings.languageXML.SelectNodes(xpath);
			foreach (XmlNode item2 in xmlNodeList)
			{
				string value3 = item2.Attributes["Id"].Value;
				string value4 = item2.Attributes["Text"].Value;
				dicMenuItem.Add(value3, value4);
			}
			list2.ForEach(delegate(ToolStripMenuItem x)
			{
				if (dicMenuItem.ContainsKey(x.Name))
				{
					x.Text = dicMenuItem[x.Name];
				}
			});
			List<ToolStripItem> list3 = this.tsrMain.smethod_10();
			Dictionary<string, string> dicToolItem = new Dictionary<string, string>();
			xpath = "/Resource/MainForm/ToolStrip/ToolItem";
			xmlNodeList = Settings.languageXML.SelectNodes(xpath);
			foreach (XmlNode item3 in xmlNodeList)
			{
				string value5 = item3.Attributes["Id"].Value;
				string value6 = item3.Attributes["Text"].Value;
				dicToolItem.Add(value5, value6);
			}
			list3.ForEach(delegate(ToolStripItem x)
			{
				if (dicToolItem.ContainsKey(x.Name))
				{
					string text4 = x.Text = (x.ToolTipText = dicToolItem[x.Name]);
				}
			});
		}

		//  This function converts the internal data structures into the binary data of the codeplug file
		public static byte[] DataToByte()
		{
			byte[] array = new byte[Settings.EEROM_SPACE];
			array.Fill((byte)255);
			MainForm.DataVerify();
			byte[] array2 = Settings.objectToByteArray(GeneralSetForm.data, Marshal.SizeOf(GeneralSetForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_GENERAL_SET, array2.Length);
			array2 = Settings.objectToByteArray(DeviceInfoForm.data, Marshal.SizeOf(DeviceInfoForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_DEVICE_INFO, array2.Length);
			array2 = Settings.objectToByteArray(ButtonForm.data, Marshal.SizeOf(ButtonForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_BUTTON, array2.Length);
			array2 = Settings.objectToByteArray(ButtonForm.data1, Marshal.SizeOf(ButtonForm.data1.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_ONE_TOUCH, array2.Length);
			array2 = Settings.objectToByteArray(TextMsgForm.data, Marshal.SizeOf(TextMsgForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_TEXT_MSG, array2.Length);
			array2 = Settings.objectToByteArray(EncryptForm.data, Marshal.SizeOf(EncryptForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_ENCRYPT, array2.Length);
			array2 = Settings.objectToByteArray(SignalingBasicForm.data, Marshal.SizeOf(SignalingBasicForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_SIGNALING_BASIC, array2.Length);
			array2 = Settings.objectToByteArray(DtmfForm.data, Marshal.SizeOf(DtmfForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_DTMF_BASIC, array2.Length);
			array2 = Settings.objectToByteArray(EmergencyForm.data, Marshal.SizeOf(EmergencyForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_EMG_SYSTEM, array2.Length);
			array2 = Settings.objectToByteArray(ContactForm.data, Marshal.SizeOf(ContactForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_DMR_CONTACT_EX, array2.Length);
			array2 = Settings.objectToByteArray(DtmfContactForm.data, Marshal.SizeOf(DtmfContactForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_DTMF_CONTACT, array2.Length);
			array2 = Settings.objectToByteArray(RxGroupListForm.data, Marshal.SizeOf(RxGroupListForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_RX_GRP_LIST_EX, array2.Length);
			ZoneForm.basicData.Verify();
			array[Settings.ADDR_ZONE_BASIC] = ZoneForm.data.ZoneIndex[0];
			array2 = ZoneForm.basicData.ToEerom();
			Array.Copy(array2, 0, array, Settings.ADDR_ZONE_BASIC + 1, array2.Length);
			array2 = ZoneForm.data.ToEerom(0, 2);
			Array.Copy(array2, 0, array, Settings.ADDR_ZONE_LIST, array2.Length);
			array2 = ChannelForm.data.ToEerom(0);
			Array.Copy(array2, 0, array, Settings.ADDR_CHANNEL, array2.Length);
			array2 = Settings.objectToByteArray(ScanBasicForm.data, Marshal.SizeOf(ScanBasicForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_SCAN, array2.Length);
			array2 = Settings.objectToByteArray(NormalScanForm.data, Marshal.SizeOf(NormalScanForm.data.GetType()));
			Array.Copy(array2, 0, array, Settings.ADDR_SCAN_LIST, array2.Length);
			array2 = Settings.objectToByteArray(BootItemForm.data, Settings.SPACE_BOOT_ITEM);
			Array.Copy(array2, 0, array, Settings.ADDR_BOOT_ITEM, array2.Length);
			array2 = Settings.objectToByteArray(DigitalKeyContactForm.data, Settings.SPACE_DIGITAL_KEY_CONTACT);
			Array.Copy(array2, 0, array, Settings.ADDR_DIGITAL_KEY_CONTACT, Settings.SPACE_DIGITAL_KEY_CONTACT);
			array2 = Settings.objectToByteArray(MenuForm.data, Settings.SPACE_MENU_CONFIG);
			Array.Copy(array2, 0, array, Settings.ADDR_MENU_CONFIG, Settings.SPACE_MENU_CONFIG);
			array2 = Settings.objectToByteArray(BootItemForm.dataContent, Settings.SPACE_BOOT_CONTENT);
			Array.Copy(array2, 0, array, Settings.ADDR_BOOT_CONTENT, Settings.SPACE_BOOT_CONTENT);
			array2 = Settings.objectToByteArray(AttachmentForm.data, Settings.SPACE_ATTACHMENT);
			Array.Copy(array2, 0, array, Settings.ADDR_ATTACHMENT, Settings.SPACE_ATTACHMENT);
			array2 = VfoForm.data.ToEerom();
			Array.Copy(array2, 0, array, Settings.ADDR_VFO, array2.Length);
			if (ChannelForm.CurCntCh > 128)
			{
				array2 = Settings.objectToByteArray(ZoneForm.basicData, Marshal.SizeOf(ZoneForm.basicData));
				Array.Copy(array2, 0, array, Settings.ADDR_EX_ZONE_BASIC, array2.Length);
				array2 = Settings.objectToByteArray(ZoneForm.data, Settings.SPACE_EX_ZONE);
				Array.Copy(array2, 0, array, Settings.ADDR_EX_ZONE_LIST, array2.Length);
				array2 = Settings.objectToByteArray(EmergencyForm.dataEx, Marshal.SizeOf(EmergencyForm.dataEx));
				Array.Copy(array2, 0, array, Settings.ADDR_EX_EMERGENCY, array2.Length);
				for (int i = 1; i < 8; i++)
				{
					array2 = ChannelForm.data.ToEerom(i);
					Array.Copy(array2, 0, array, Settings.ADDR_EX_CH + (i - 1) * ChannelForm.SPACE_CH_GROUP, ChannelForm.SPACE_CH_GROUP);
				}
			}

			Array.Copy(OpenGD77Form.CustomData, 0, array, 0x1EE60, OpenGD77Form.CustomData.Length);

		//	Array.Copy(DMRIDFormNew.DMRIDBuffer, 0, array, 0x20000, 0x20000);// Save the CurrentDMRID to the codeplug.

			return array;
		}



		//CJD  Check Codeplug and try to determine if it is a V3.1.1 type by looking at the Rx Group Entries. 

		public static bool checkCodeplugVersion311(byte[] cplg)
		{
			const UInt16 RxGroupLength306 = 48;				//there are 48 bytes in each Rx Group for V3.0.6
			byte c;

			for(c=0;c<76;c++)
			{
				if (cplg[Settings.ADDR_RX_GRP_LIST_EX + c] > 16)         //if any of the Rx Groups has >15 members it must be V3.1.1
				{
					return true;
				}
			}


			if (cplg[Settings.ADDR_RX_GRP_LIST_EX + 1] > 0)				//if there is a second Rx Group then check where its name is
			{
				c = cplg[Settings.ADDR_RX_GRP_LIST_EX + RxListData.CNT_RX_LIST_INDEX + RxGroupLength306 + 1];   //Get the second character of the second Rx Group Name (Assuming it is 3.0.6) 
				if (c>3)                                        //If it is 3.0.6 it will be an ascii character. If it is 3.1.x then it will be the high byte of a channel number which must be <4. 
				{
					return false;								//If it is >3 then it must be 3.0.6
				}
			}

			return true;							//if it was neither of the above then we can't tell what version it is so we must assume it is 3.1.x
		}

		public static bool checkZonesFor80Channels(byte[] codeplug)
		{
			// Offset 0x51 into the Zones is the second character in the second zone's name
			// If this value is non-ascii then this address must contain a channel number, hence this is a 80 channel zone

			if (codeplug[Settings.ADDR_EX_ZONE_LIST + 0x51] <= 0x04)
			{
				return true;
			}
			return false;
		}

		public static void convertTo80ChannelZoneCodeplug(byte[] cplg)
		{
			MessageBox.Show("Your codeplug uses 16 channel zones.\n\nIt will be automatically updated to 80 channel zones.\n\nPlease check the Zones to ensure the update worked correctly before saving or uploading the codeplug","Codeplug update warning");
			byte[,] oldZones = new byte[68,48];
			const int OLD_ZONE_LEN_BYTES = 16 + (16*2);
			const int NEW_ZONE_LEN_BYTES = 16 + (80*2);
			int ZONES_START_ADDRESS = Settings.ADDR_EX_ZONE_LIST + 32;

			for (int zone = 0; zone < 68; zone++)
			{
				for (int i = 0; i < 48; i++)
				{
					oldZones[zone, i] = cplg[ZONES_START_ADDRESS + (zone * OLD_ZONE_LEN_BYTES) + i];			
				}
			}
			for (int zone = 0; zone < 68; zone++)
			{
				for (int i = 0; i < OLD_ZONE_LEN_BYTES; i++)
				{
					cplg[ZONES_START_ADDRESS + (zone * NEW_ZONE_LEN_BYTES) + i] = oldZones[zone, i];			
				}
				// fill all other channel bytes with zeros
				for (int i = OLD_ZONE_LEN_BYTES; i < NEW_ZONE_LEN_BYTES; i++)
				{
					cplg[ZONES_START_ADDRESS + (zone * NEW_ZONE_LEN_BYTES) + i] = 0x00;
				}
			}
		}


		//CJD New function to convert from V3.1.1 to 3.0.6 format
		public static void convertCodeplug(byte[] cplg)
		{
			MessageBox.Show(Settings.dicCommon["CodeplugUpgradeNotice"]);
			byte[,] rxgroups= new byte[128,48];
			int p;
			int i;
			const UInt32 RxGroupAddOffset = 0x80;

			for (p=0;p<128;p++)
			{
				for(i=0;i<48;i++)
				{
					rxgroups[p, i] = cplg[Settings.ADDR_RX_GRP_LIST_EX + RxGroupAddOffset + p * 48 + i];			//copy each 3.0.6 Rx group into temporary array
				}

			}

			for (p = 0; p < 76; p++)
			{
				for (i = 0; i < 48; i++)
				{
					cplg[Settings.ADDR_RX_GRP_LIST_EX + RxGroupAddOffset + p * 80 + i] = rxgroups[p, i];         //copy each Rx group back into 3.1.x location
				}

				for(i=48;i<80;i++)
				{
					cplg[Settings.ADDR_RX_GRP_LIST_EX + RxGroupAddOffset + p * 80 + i] = 0;						//zero any entries above first 16
				}
			}

			i = 0;
			for(p=76;p<128;p++)
			{
				if (cplg[Settings.ADDR_RX_GRP_LIST_EX + p] > 0) i++;					//Count any rxgroups above 76
				cplg[Settings.ADDR_RX_GRP_LIST_EX + p] = 0;							//Remove any indexes above 76 
			}

			if(i>0)
			{
				MessageBox.Show(Settings.dicCommon["CodeplugUpgradeWarningToManyRxGroups"]);
			}

		}



		// This function reads the binary data e.g codeplug file and stores the data into the internal storage structures
		public static void ByteToData(byte[] eerom, bool isFromFile = false)
		{
			byte[] array = new byte[Settings.SPACE_DEVICE_INFO];
			Array.Copy(eerom, Settings.ADDR_DEVICE_INFO, array, 0, array.Length);
			DeviceInfoForm.data = (DeviceInfoForm.DeviceInfo)Settings.smethod_62(array, DeviceInfoForm.data.GetType());
			Settings.MIN_FREQ[0] = ushort.Parse(DeviceInfoForm.data.MinFreq);
			Settings.MAX_FREQ[0] = ushort.Parse(DeviceInfoForm.data.MaxFreq);
			Settings.MIN_FREQ[1] = ushort.Parse(DeviceInfoForm.data.MinFreq2);
			Settings.MAX_FREQ[1] = ushort.Parse(DeviceInfoForm.data.MaxFreq2);
			array = new byte[Settings.SPACE_GENERAL_SET];
			Array.Copy(eerom, Settings.ADDR_GENERAL_SET, array, 0, array.Length);
			GeneralSetForm.data = (GeneralSetForm.GeneralSet)Settings.smethod_62(array, GeneralSetForm.data.GetType());
			array = new byte[Settings.SPACE_BUTTON];
			Array.Copy(eerom, Settings.ADDR_BUTTON, array, 0, array.Length);
			ButtonForm.data = (ButtonForm.SideKey)Settings.smethod_62(array, ButtonForm.data.GetType());
			array = new byte[Settings.SPACE_ONE_TOUCH];
			Array.Copy(eerom, Settings.ADDR_ONE_TOUCH, array, 0, array.Length);
			ButtonForm.data1 = (ButtonForm.OneTouch)Settings.smethod_62(array, ButtonForm.data1.GetType());
			array = new byte[Settings.SPACE_TEXT_MSG];
			Array.Copy(eerom, Settings.ADDR_TEXT_MSG, array, 0, array.Length);
			TextMsgForm.data = (TextMsgForm.TextMsg)Settings.smethod_62(array, TextMsgForm.data.GetType());
			array = new byte[Settings.SPACE_ENCRYPT];
			Array.Copy(eerom, Settings.ADDR_ENCRYPT, array, 0, array.Length);
			EncryptForm.data = (EncryptForm.Encrypt)Settings.smethod_62(array, EncryptForm.data.GetType());
			array = new byte[Settings.SPACE_SIGNALING_BASIC];
			Array.Copy(eerom, Settings.ADDR_SIGNALING_BASIC, array, 0, array.Length);
			SignalingBasicForm.data = (SignalingBasicForm.SignalingBasic)Settings.smethod_62(array, SignalingBasicForm.data.GetType());
			array = new byte[Settings.SPACE_DTMF_BASIC];
			Array.Copy(eerom, Settings.ADDR_DTMF_BASIC, array, 0, array.Length);
			DtmfForm.data = (DtmfForm.Dtmf)Settings.smethod_62(array, DtmfForm.data.GetType());
			array = new byte[Settings.SPACE_EMG_SYSTEM];
			Array.Copy(eerom, Settings.ADDR_EMG_SYSTEM, array, 0, array.Length);
			EmergencyForm.data = (EmergencyForm.Emergency)Settings.smethod_62(array, EmergencyForm.data.GetType());
			array = new byte[Settings.SPACE_DMR_CONTACT_EX];
			Array.Copy(eerom, Settings.ADDR_DMR_CONTACT_EX, array, 0, array.Length);
			ContactForm.data = (ContactForm.Contact)Settings.smethod_62(array, ContactForm.data.GetType());
			array = new byte[Settings.SPACE_DTMF_CONTACT];
			Array.Copy(eerom, Settings.ADDR_DTMF_CONTACT, array, 0, array.Length);
			DtmfContactForm.data = (DtmfContactForm.DtmfContact)Settings.smethod_62(array, DtmfContactForm.data.GetType());
			array = new byte[Settings.SPACE_RX_GRP_LIST];
			Array.Copy(eerom, Settings.ADDR_RX_GRP_LIST_EX, array, 0, array.Length);
			RxGroupListForm.data = (RxListData)Settings.smethod_62(array, RxGroupListForm.data.GetType());
			ZoneForm.data.ZoneIndex[0] = eerom[Settings.ADDR_ZONE_BASIC];
			ZoneForm.basicData.CurZone = eerom[Settings.ADDR_ZONE_BASIC + 1];
			ZoneForm.basicData.MainCh = eerom[Settings.ADDR_ZONE_BASIC + 2];
			ZoneForm.basicData.SubCh = eerom[Settings.ADDR_ZONE_BASIC + 3];
			ZoneForm.basicData.SubZone = eerom[Settings.ADDR_ZONE_BASIC + 4];
			array = new byte[ChannelForm.SPACE_CH_GROUP];
			Array.Copy(eerom, Settings.ADDR_CHANNEL, array, 0, array.Length);
			ChannelForm.data.FromEerom(0, array);
			array = new byte[Settings.SPACE_SCAN_BASIC];
			Array.Copy(eerom, Settings.ADDR_SCAN, array, 0, array.Length);
			ScanBasicForm.data = (ScanBasicForm.ScanBasic)Settings.smethod_62(array, ScanBasicForm.data.GetType());
			array = new byte[Settings.SPACE_SCAN_LIST];
			Array.Copy(eerom, Settings.ADDR_SCAN_LIST, array, 0, array.Length);
			NormalScanForm.data = (NormalScanForm.NormalScan)Settings.smethod_62(array, NormalScanForm.data.GetType());
			array = new byte[Settings.SPACE_BOOT_ITEM];
			Array.Copy(eerom, Settings.ADDR_BOOT_ITEM, array, 0, array.Length);
			BootItemForm.data = (BootItemForm.BootItem)Settings.smethod_62(array, BootItemForm.data.GetType());
			array = new byte[Settings.SPACE_DIGITAL_KEY_CONTACT];
			Array.Copy(eerom, Settings.ADDR_DIGITAL_KEY_CONTACT, array, 0, Settings.SPACE_DIGITAL_KEY_CONTACT);
			DigitalKeyContactForm.data = (DigitalKeyContactForm.NumKeyContact)Settings.smethod_62(array, DigitalKeyContactForm.data.GetType());
			array = new byte[Settings.SPACE_MENU_CONFIG];
			Array.Copy(eerom, Settings.ADDR_MENU_CONFIG, array, 0, Settings.SPACE_MENU_CONFIG);
			MenuForm.data = (MenuForm.MenuSet)Settings.smethod_62(array, MenuForm.data.GetType());
			array = new byte[Settings.SPACE_BOOT_CONTENT];
			Array.Copy(eerom, Settings.ADDR_BOOT_CONTENT, array, 0, array.Length);
			BootItemForm.dataContent = (BootItemForm.BootContent)Settings.smethod_62(array, typeof(BootItemForm.BootContent));
			array = new byte[Settings.SPACE_ATTACHMENT];
			Array.Copy(eerom, Settings.ADDR_ATTACHMENT, array, 0, array.Length);
			AttachmentForm.data = (AttachmentForm.Attachment)Settings.smethod_62(array, typeof(AttachmentForm.Attachment));
			array = new byte[Settings.SPACE_VFO];
			Array.Copy(eerom, Settings.ADDR_VFO, array, 0, array.Length);
			VfoForm.data.FromEerom(array);
			try
			{
				if (ChannelForm.CurCntCh > 128)
				{
					array = new byte[ZoneForm.SPACE_BASIC_ZONE];
					Array.Copy(eerom, Settings.ADDR_EX_ZONE_BASIC, array, 0, array.Length);
					ZoneForm.basicData = (ZoneForm.BasicZone)Settings.smethod_62(array, ZoneForm.basicData.GetType());
					array = new byte[Settings.SPACE_EX_ZONE];
					Array.Copy(eerom, Settings.ADDR_EX_ZONE_LIST, array, 0, Settings.SPACE_EX_ZONE);
					ZoneForm.data = (ZoneForm.Zone)Settings.smethod_62(array, ZoneForm.data.GetType());
					ZoneForm.CompactZones();
					array = new byte[Settings.SPACE_EX_EMERGENCY];
					Array.Copy(eerom, Settings.ADDR_EX_EMERGENCY, array, 0, array.Length);
					EmergencyForm.dataEx = (EmergencyForm.EmergencyEx)Settings.smethod_62(array, EmergencyForm.dataEx.GetType());
					for (int i = 1; i < 8; i++)
					{
						array = new byte[ChannelForm.SPACE_CH_GROUP];
						Array.Copy(eerom, Settings.ADDR_EX_CH + (i - 1) * array.Length, array, 0, array.Length);
						ChannelForm.data.FromEerom(i, array);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			OpenGD77Form.LoadCustomData(eerom);


/*
			if (isFromFile && eerom.Length == 0x40000)
			{
				Array.Copy(eerom, 0x20000, DMRIDFormNew.DMRIDBuffer, 0, 0x20000);
			}
			else
			{
				DMRIDFormNew.ClearStaticData();
			}
 */
		}

		public static void DataVerify()
		{
			BootItemForm.data.Verify(BootItemForm.DefaultBootItem);
			ButtonForm.data.Verify(ButtonForm.DefaultSideKey);
			ChannelForm.data.Verify();
			ContactForm.data.Verify();
			DigitalKeyContactForm.data.Verify();
			DtmfForm.data.Verify(DtmfForm.DefaultDtmf);
			EmergencyForm.data.Verify();
			EncryptForm.data.Verify(EncryptForm.DefaultEncrypt);
			GeneralSetForm.data.Verify(GeneralSetForm.DefaultGeneralSet);
			AttachmentForm.data.Verify(AttachmentForm.DefaultAttachment);
			VfoForm.data.Verify();
			MenuForm.data.Verify(MenuForm.DefaultMenu);
			NormalScanForm.data.Verify();
			RxGroupListForm.data.Verify();
			ScanBasicForm.data.Verify(ScanBasicForm.DefaultScanBasic);
			SignalingBasicForm.data.Verify(SignalingBasicForm.DefaultSignalingBasic);
			ZoneForm.basicData.Verify();
			ZoneForm.data.Verify();
		}

		public void ShowHelp(string helpId)
		{
			string str = Settings.smethod_2();
			if (MainForm.dicHelp.ContainsKey(helpId) && !string.IsNullOrEmpty(MainForm.dicHelp[helpId].Trim()))
			{
				string text = "mk:@MSITStore:" + str + MainForm.dicHelp[helpId];
				this.frmHelp.ShowHelp(text.Replace("#", "%23"));
			}
			else
			{
				this.frmHelp.ShowHelp("");
			}
		}

		private void method_15()
		{
			MainForm.dicHelp.Clear();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(Application.StartupPath + "/help.xml");
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//Control");
			foreach (XmlNode item in xmlNodeList)
			{
				string value = item.Attributes["id"].Value;
				string value2 = item.Attributes["html"].Value;
				if (!MainForm.dicHelp.ContainsKey(value))
				{
					MainForm.dicHelp.Add(value, value2);
				}
			}
			xmlDocument.Clone();
		}

		private void method_16()
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(Application.StartupPath + "/help.xml", Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("Controls");
			Form[] mdiChildren = base.MdiChildren;
			Form[] array = mdiChildren;
			foreach (Form form in array)
			{
				this.method_17(form.Controls, xmlTextWriter);
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}

		private void method_17(Control.ControlCollection controlCollection_0, XmlTextWriter xmlTextWriter_0)
		{
			foreach (Control item in controlCollection_0)
			{
				if (!(item is TextBox) && !(item is CheckBox) && !(item is ListBox) && !(item is NumericUpDown) && !(item is ComboBox))
				{
					goto IL_0095;
				}
				if (!(item.Parent is NumericUpDown))
				{
					xmlTextWriter_0.WriteStartElement("Control");
					xmlTextWriter_0.WriteAttributeString("id", item.FindForm().Name + "_" + item.Name);
					xmlTextWriter_0.WriteAttributeString("html", " ");
					xmlTextWriter_0.WriteEndElement();
					goto IL_0095;
				}
				continue;
				IL_0095:
				if (item.Controls.Count > 0)
				{
					this.method_17(item.Controls, xmlTextWriter_0);
				}
			}
		}

		public void DispChildForm(Type type, int index)
		{
			TreeNode treeNodeByTypeAndIndex = this.GetTreeNodeByTypeAndIndex(type, index, this.tvwMain.Nodes);
			if (treeNodeByTypeAndIndex != null)
			{
				this.method_7(treeNodeByTypeAndIndex, true);
			}
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			/*
			 * Roger Clark
			 * Disabled secret password entry system as mode is permantly set to "Expert"
			if (e.Alt && e.Control && e.Shift)
			{
				if (e.KeyCode != Keys.D5 && e.KeyCode != Keys.F11)
				{
					return;
				}
				PowerPwdForm powerPwdForm = new PowerPwdForm();
				if (powerPwdForm.ShowDialog() == DialogResult.OK)
				{
					this.closeAllForms();
					this.tsmiBasic.Visible = true;
				}
			}*/
		}

		public void GetAllLang()
		{
			this.tsmiLanguage.DropDownItems.Clear();
			string startupPath = Application.StartupPath;
			string[] array = Directory.GetFiles(startupPath + "\\Language", "*.xml").Where(MainForm.smethod_3).ToArray();
			if (array.Length > 0)
			{
				int num = 0;
				while (true)
				{
					if (num >= array.Length)
					{
						break;
					}
					try
					{
						string text = array[num];
						ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
						toolStripMenuItem.Text = this.GetLangName(text);
						toolStripMenuItem.Name = this.GetLangName(text);
						toolStripMenuItem.Tag = text;
						this.tsmiLanguage.DropDownItems.Add(toolStripMenuItem);
						toolStripMenuItem.Click += this.languageChangeHandler;
					}
					catch
					{
					}
					num++;
				}
			}
		}

		public string GetLangName(string xmlFile)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xmlFile);
			string xpath = "/Resource/Language";
			XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath);
			if (xmlNode != null)
			{
				string value = xmlNode.Attributes["Id"].Value;
				return xmlNode.Attributes["Text"].Value;
			}
			return "";
		}

		private void initialiseTree()
		{
			this.lstTreeNodeItem.Clear();
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, null, null, 0, -1, 18, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(DeviceInfoForm), null, 0, -1, 20, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(BootItemForm), null, 0, -1, 30, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(MenuForm), null, 0, -1, 38, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(DigitalKeyContactForm), null, 0, -1, 15, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(GeneralSetForm), null, 0, -1, 5, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(ButtonForm), null, 0, -1, 4, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(TextMsgForm), null, 0, -1, 22, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(EncryptForm), null, 0, -1, 35, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(SignalingBasicForm), null, 0, -1, 16, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(DtmfForm), null, 0, -1, 39, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(this.cmsGroup, null, typeof(EmergencyForm), 32, -1, 17, EmergencyForm.data));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, null, null, 0, -1, 17, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(DtmfContactForm), null, 0, -1, 49, null));
			this.lstTreeNodeItem.Add(new TreeNodeItem(this.cmsGroupContact, typeof(ContactsForm), typeof(ContactForm), 1024, -1, 17, ContactForm.data));
			this.lstTreeNodeItem.Add(new TreeNodeItem(this.cmsGroup, null, typeof(RxGroupListForm), RxListData.CNT_RX_LIST, -1, 17, RxGroupListForm.data));
			this.lstTreeNodeItem.Add(new TreeNodeItem(this.cmsGroup, typeof(ZoneBasicForm), typeof(ZoneForm), ZoneForm.NUM_ZONES, -1, 16, ZoneForm.data));
			this.lstTreeNodeItem.Add(new TreeNodeItem(this.cmsGroup, typeof(ChannelsForm), typeof(ChannelForm), ChannelForm.CurCntCh, -1, 17, ChannelForm.data));
			this.lstTreeNodeItem.Add(new TreeNodeItem(this.cmsGroup, typeof(ScanBasicForm), typeof(NormalScanForm), 64, -1, 16, NormalScanForm.data));
			this.lstTreeNodeItem.Add(new TreeNodeItem(null, null, null, 0, -1, 17, null));
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				switch (VfoForm.data.GetChMode(i))
				{
				case 0:
					num = 2;
					break;
				case 1:
					num = 6;
					break;
				default:
					num = 54;
					break;
				}
				this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(VfoForm), null, 2, i, num, VfoForm.data));
			}
			// Add new DMR-ID section
//			this.lstTreeNodeItem.Add(new TreeNodeItem(null, typeof(DMRIDForm), null, 0, -1, 19, null));// 19 is the icon number
		}

		private DataTable method_19()
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Id");
			dataTable.Columns.Add("Name");
			dataTable.Columns.Add("ParentId");
			dataTable.Rows.Add("00", "Model", "-1");
			dataTable.Rows.Add("0001", "BasicInfo", "00");
			dataTable.Rows.Add("0002", "BootItem", "00");
			dataTable.Rows.Add("0003", "Menu", "00");
			dataTable.Rows.Add("0004", "NumKeyAssign", "00");
			dataTable.Rows.Add("0005", "GeneralSetting", "00");
			dataTable.Rows.Add("0006", "Buttons", "00");
			dataTable.Rows.Add("0007", "TextMsg", "00");
			dataTable.Rows.Add("0008", "Pivacy", "00");
			dataTable.Rows.Add("0009", "SignalingSys", "00");
			dataTable.Rows.Add("000900", "DtmfSignal", "0009");
			dataTable.Rows.Add("000901", "EmergencySys", "0009");
			dataTable.Rows.Add("0011", "Contact", "00");
			dataTable.Rows.Add("001100", "DtmfContact", "0011");
			dataTable.Rows.Add("001101", "DigitalContact", "0011");
			dataTable.Rows.Add("0012", "RxGroupList", "00");
			dataTable.Rows.Add("0013", "Zone", "00");
			dataTable.Rows.Add("0014", "Channel", "00");
			dataTable.Rows.Add("0015", "Scan", "00");
			dataTable.Rows.Add("0016", "VFO", "00");
			dataTable.Rows.Add("001600", "VFOA", "0016");
			dataTable.Rows.Add("001601", "VFOB", "0016");
//			dataTable.Rows.Add("0017", "DMR-ID", "00");// DMRID
			return dataTable;
		}

		private void method_20(DataTable dataTable_0)
		{
			DataRow[] array = dataTable_0.Select("ParentId='-1'").OrderBy(MainForm.smethod_4).ToArray();
			if (array != null && array.Length > 0)
			{
				TreeNode treeNode_ = this.AddTreeViewNode(this.tvwMain.Nodes, array[0]["Name"].ToString(), this.lstTreeNodeItem[dataTable_0.Rows.IndexOf(array[0])]);
				this.method_21(array[0], treeNode_, dataTable_0);
			}
		}

		private void method_21(DataRow dataRow_0, TreeNode treeNode_0, DataTable dataTable_0)
		{
			if (dataRow_0 != null && treeNode_0 != null)
			{
				try
				{
					DataRow[] array = dataTable_0.Select("ParentId='" + dataRow_0["Id"] + "'").OrderBy(MainForm.smethod_5).ToArray();
					if (array != null || array.Length > 0)
					{
						DataRow[] array2 = array;
						foreach (DataRow dataRow in array2)
						{
							int index = dataTable_0.Rows.IndexOf(dataRow);
							TreeNode treeNode_ = this.AddTreeViewNode(treeNode_0.Nodes, dataRow["Name"].ToString(), this.lstTreeNodeItem[index]);
							this.method_21(dataRow, treeNode_, dataTable_0);
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		public void InitDynamicNode()
		{
			TreeNode parentNode = this.method_8(typeof(EmergencyForm), this.tvwMain.Nodes);
			this.InitEmergencySystems(parentNode);
			parentNode = this.method_9(typeof(ContactsForm), this.tvwMain.Nodes);
			this.InitDigitContacts(parentNode);
			parentNode = this.method_8(typeof(RxGroupListForm), this.tvwMain.Nodes);
			this.InitRxGroupLists(parentNode);
			parentNode = this.method_9(typeof(ZoneBasicForm), this.tvwMain.Nodes);
			this.InitZones(parentNode);
			parentNode = this.method_9(typeof(ChannelsForm), this.tvwMain.Nodes);
			this.InitChannels(parentNode);
			parentNode = this.method_9(typeof(ScanBasicForm), this.tvwMain.Nodes);
			this.InitScans(parentNode);
		}

		public void InitChannelsImportNodes()
		{
			TreeNode parentNode = this.method_9(typeof(ContactsForm), this.tvwMain.Nodes);
			this.InitDigitContacts(parentNode);
			parentNode = this.method_8(typeof(RxGroupListForm), this.tvwMain.Nodes);
			this.InitRxGroupLists(parentNode);
			parentNode = this.method_9(typeof(ChannelsForm), this.tvwMain.Nodes);
			this.InitChannels(parentNode);
			parentNode = this.method_9(typeof(ScanBasicForm), this.tvwMain.Nodes);
			this.InitScans(parentNode);
			parentNode = this.method_9(typeof(ZoneBasicForm), this.tvwMain.Nodes);
			this.InitZones(parentNode);
		}

		public void WriteXml(List<ToolStripItem> lstMenuItem)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(Application.StartupPath + "/test.xml", Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("MenuStrip");
			foreach (ToolStripItem item in lstMenuItem)
			{
				xmlTextWriter.WriteStartElement("MenuItem");
				xmlTextWriter.WriteAttributeString("Id", item.Name);
				xmlTextWriter.WriteAttributeString("Text", item.Text);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}

		public void WriteMenuXml(List<ToolStripMenuItem> lstMenuItem, XmlTextWriter xml)
		{
			xml.WriteStartElement("ContextMenuStrip");
			lstMenuItem.ForEach(delegate(ToolStripMenuItem x)
			{
				xml.WriteStartElement("MenuItem");
				xml.WriteAttributeString("Id", x.Name);
				xml.WriteAttributeString("Text", x.Text);
				xml.WriteEndElement();
			});
			xml.WriteEndElement();
		}

		public void WriteToolStripXml(List<ToolStripItem> lstControls, XmlTextWriter xml)
		{
			lstControls.ForEach(delegate(ToolStripItem x)
			{
				xml.WriteStartElement("ToolStripItem");
				xml.WriteAttributeString("Id", x.Name);
				xml.WriteAttributeString("Text", x.Text);
				xml.WriteEndElement();
			});
		}

		private void method_22()
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(Application.StartupPath + "/test.xml", Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("Resource");
			Form[] mdiChildren = base.MdiChildren;
			Form[] array = mdiChildren;
			foreach (Form form in array)
			{
				xmlTextWriter.WriteStartElement(form.Name);
				this.WriteControlXml(form.smethod_12(), xmlTextWriter);
				xmlTextWriter.WriteEndElement();
			}
			foreach (Form openForm in Application.OpenForms)
			{
				xmlTextWriter.WriteStartElement(openForm.Name);
				this.WriteControlXml(openForm.smethod_12(), xmlTextWriter);
				xmlTextWriter.WriteEndElement();
			}
			this.WriteMenuXml(this.cmsGroup.smethod_9(), xmlTextWriter);
			this.WriteMenuXml(this.cmsSub.smethod_9(), xmlTextWriter);
			this.WriteMenuXml(this.cmsTree.smethod_9(), xmlTextWriter);
			this.WriteMenuXml(this.cmsGroupContact.smethod_9(), xmlTextWriter);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}

		public void WriteControlXml(List<Control> lstControls, XmlTextWriter xml)
		{
			xml.WriteStartElement("Controls");
			foreach (Control lstControl in lstControls)
			{
				xml.WriteStartElement("Control");
				xml.WriteAttributeString("id", lstControl.Name);
				if (lstControl is DataGridView)
				{
					List<string> list = new List<string>();
					DataGridView dataGridView = lstControl as DataGridView;
					foreach (DataGridViewColumn column in dataGridView.Columns)
					{
						list.Add(column.HeaderText);
					}
					xml.WriteAttributeString("Text", string.Join(",", list.ToArray()));
					foreach (DataGridViewColumn column2 in dataGridView.Columns)
					{
						xml.WriteStartElement("ControlItem");
						xml.WriteAttributeString("Id", column2.Name);
						xml.WriteAttributeString("Text", column2.HeaderText);
						xml.WriteEndElement();
					}
				}
				else if (lstControl is ToolStrip)
				{
					xml.WriteAttributeString("Text", lstControl.Text);
					ToolStrip toolStrip_ = lstControl as ToolStrip;
					this.WriteToolStripXml(toolStrip_.smethod_10(), xml);
				}
				else
				{
					xml.WriteAttributeString("Text", lstControl.Text);
				}
				xml.WriteEndElement();
			}
			xml.WriteEndElement();
		}

		[CompilerGenerated]
		private static void smethod_0(TreeNode treeNode_0)
		{
			if (MainForm.dicTree.ContainsKey(treeNode_0.Name))
			{
				if (treeNode_0.Name == "Model")
				{
					treeNode_0.Text = GeneralSetForm.data.RadioName;// MainForm.dicTree[treeNode_0.Name];
				}
				else
				{
					treeNode_0.Text = MainForm.dicTree[treeNode_0.Name];
				}
			}
		}

		[CompilerGenerated]
		private static bool smethod_1(byte byte_0)
		{
			return byte_0 == 255;
		}

		[CompilerGenerated]
		private static void smethod_2(TreeNode treeNode_0)
		{
			if (MainForm.dicTree.ContainsKey(treeNode_0.Name))
			{
				if (treeNode_0.Name == "Model")
				{
					treeNode_0.Text = GeneralSetForm.data.RadioName;// MainForm.dicTree[treeNode_0.Name];
				}
				else
				{
					treeNode_0.Text = MainForm.dicTree[treeNode_0.Name];
				}
			}
		}

		[CompilerGenerated]
		private static bool smethod_3(string string_0)
		{
			return string_0.EndsWith(".xml");
		}

		[CompilerGenerated]
		private static object smethod_4(DataRow dataRow_0)
		{
			return dataRow_0[0];
		}

		[CompilerGenerated]
		private static object smethod_5(DataRow dataRow_0)
		{
			return dataRow_0[0];
		}

		static MainForm()
		{
			
			MainForm.PreActiveMdiChild = null;
			MainForm.dicHelp = new Dictionary<string, string>();
			MainForm.dicTree = new Dictionary<string, string>();
			/*
			MainForm.TREENODE_KEY = new string[21]
			{
				"Model",
				"BasicInfo",
				"BootItem",
				"Menu",
				"NumKeyAssign",
				"GeneralSetting",
				"Buttons",
				"TextMsg",
				"Pivacy",
				"SignalingSys",
				"EmergencySys",
				"Contact",
				"DtmfContact",
				"DigitalContact",
				"RxGroupList",
				"Zone",
				"Channel",
				"Scan",
				"VFO",
				"VFOA",
				"VFOB"
			};*/
		}
	}
}
