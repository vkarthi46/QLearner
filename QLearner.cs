using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using QLearner.QStates;
using QLearner.QAlgos;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QLearner
{
    public partial class QLearner : Form
    {
        private int version = 1, subversion = 2;
        private QAgent agent;
        private QState state;
        private QAlgo algo;
        private BackgroundWorker learnProcess, awakenProcess;

        private Dictionary<QStateActionPair, DataGridViewRow> LearningTableQStateKeys;
        private Dictionary<string, DataGridViewRow> LearningTableStringKeys;
        private Dictionary<QState, QStateActionPair> BestActions;
        private DateTime lastRowUpdate = DateTime.Now, lastOutputUpdate = DateTime.Now;
        private List<DataGridViewRow> displayRowQueue;
        private List<string> OutputQueue;

        private decimal tempExploreRate, tempLearnRate;

        public QLearner()
        {
            try
            {
                InitializeComponent();

                Reset();

                learnProcess = new BackgroundWorker();
                learnProcess.DoWork += (x, y) =>
                {
                    agent.Learn(learnProcess, Convert.ToInt32(LearningTrials.Value), LearningRate.Value, DiscountFactor.Value, ExploreRate.Value);
                };
                learnProcess.WorkerSupportsCancellation = true;

                awakenProcess = new BackgroundWorker();
                awakenProcess.DoWork += (x, y) =>
                {
                    agent.Awaken(awakenProcess, Convert.ToInt32(LearningTrials.Value), LearningRate.Value, DiscountFactor.Value, ExploreRate.Value);
                };
                awakenProcess.WorkerSupportsCancellation = true;

                BootPlugins(true);
                foreach (string file in Properties.Settings.Default.Plugins.Split('|'))
                {
                    if (file != "")
                    {
                        try
                        {
                            LoadPlugin(file);
                        }
                        catch (Exception e)
                        {
                            WriteOutput("Error loading plugin: " + file + "\n" + e, true);
                            ResetPlugins(false);
                        }
                    }
                }

                InitializeUI();
            }
            catch (Exception e)
            {
                popup("Exception: " + e);
            }
        }

        private void InitializeUI()
        {
            Text = "QLearner v" + version + "." + subversion;
            AboutBox.Text = Properties.Resources.about;

            QStatePlugins.SelectedIndexChanged += new EventHandler(BrowseQStatePlugin);
            QAlgoPlugins.SelectedIndexChanged += new EventHandler(BrowseQAlgoPlugin);
            Settings.Click += (o, s) => { state.Settings(); };
            ClearPlugins.Click += (o, s) =>
            {
                ResetPlugins();
            };
            LearningRate.ValueChanged += (o, s) => { Properties.Settings.Default.LearningRate = LearningRate.Value; Properties.Settings.Default.Save(); };
            ToolTip t = new ToolTip();
            string tip = "Weight of how much new information affects pre-existing knowledge.  The higher the number, the faster information is learned but less likely to be retained if situation changes.";
            t.SetToolTip(LearningRateLabel, tip);
            t.SetToolTip(LearningRate, tip);
            DiscountFactor.ValueChanged += (o, s) => { Properties.Settings.Default.DiscountFactor = DiscountFactor.Value; Properties.Settings.Default.Save(); };
            tip = "How much to value longer term gains in favor of short-term ones.  The higher the number, the more valuable long term gains are over shorter-term ones.";
            t.SetToolTip(DiscountFactorLabel, tip);
            t.SetToolTip(DiscountFactor, tip);
            ExploreRate.ValueChanged += (o, s) => { Properties.Settings.Default.ExploreRate = ExploreRate.Value; Properties.Settings.Default.Save(); };
            tip = "Learn mode only. How often to make random decisions and 'explore' away from current best.  The higher the number, the more likely random decisions will be used instead of optimal ones.";
            t.SetToolTip(ExploreRateLabel, tip);
            t.SetToolTip(ExploreRate, tip);
            LearningTrials.ValueChanged += (o, s) => { Properties.Settings.Default.LearningTrials = LearningTrials.Value;  Properties.Settings.Default.Save(); };
            tip = "The number of times to practice at the QState before Learning is complete.";
            t.SetToolTip(LearningTrialsLabel, tip);
            t.SetToolTip(LearningTrials, tip);
            HideOutput.CheckedChanged += (o, s) =>
            {
                LearningTable.Visible = !HideOutput.Checked;
            };
            Learn.Click += (o, s) => {
                Learn.Enabled = false;
                if (!learnProcess.IsBusy&&!learnProcess.CancellationPending)
                {
                    CurrentMode = LEARN;
                    Learn.Text = "Abort";
                    Awaken.Enabled = false;
                    EnableControls(false);
                    displayRowQueue.Clear();
                    OutputQueue.Clear();
                    output.Text = "";
                    learnProcess.RunWorkerAsync();
                }
                else if (!learnProcess.CancellationPending)
                {
                    learnProcess.CancelAsync();
                }
                Learn.Enabled = true;
            };
            learnProcess.RunWorkerCompleted += (o, s) =>
            {
                //FlushDisplayRowQueue(true, true);
                FlushOutputQueue(true, true);
                Learn.Enabled = false;
                Learn.Text = "Learn";
                EnableControls();
                Awaken.Enabled = true;
                Learn.Enabled = true;
            };
            Awaken.Click += (o, s) =>
            {
                Awaken.Enabled = false;
                if (!awakenProcess.IsBusy && !awakenProcess.CancellationPending)
                {
                    CurrentMode = AWAKEN;
                    tempExploreRate = ExploreRate.Value;
                    tempLearnRate = LearningRate.Value;
                    
                    Awaken.Text = "Abort";
                    Learn.Enabled = false;
                    EnableControls(false);
                    output.Text = "";
                    displayRowQueue.Clear();
                    OutputQueue.Clear();
                    awakenProcess.RunWorkerAsync();
                }
                else if (!awakenProcess.CancellationPending)
                {
                    awakenProcess.CancelAsync();
                }
                Awaken.Enabled = true;
            };
            awakenProcess.RunWorkerCompleted += (o, s) =>
            {
                //FlushDisplayRowQueue(true, true);
                FlushOutputQueue(true, true);
                Awaken.Enabled = false;
                Awaken.Text = "Awaken";

                EnableControls();
                Learn.Enabled = true;
                Awaken.Enabled = true;
            };
            Open.MouseEnter += (o, s) =>
            {
                if (Open.Enabled) Open.BorderStyle = BorderStyle.FixedSingle;
            };
            Open.MouseLeave += (o, s) =>
            {
                if (Open.Enabled) Open.BorderStyle = BorderStyle.None;
            };
            Open.Click += (o, s) =>
            {
                if (Open.Enabled)
                {
                    Open.BorderStyle = BorderStyle.None;
                    OpenFile();
                }
            };
            Save.MouseEnter += (o, s) =>
            {
                if (Save.Enabled) Save.BorderStyle = BorderStyle.FixedSingle;
            };
            Save.MouseLeave += (o, s) =>
            {
                if (Save.Enabled) Save.BorderStyle = BorderStyle.None;
            };
            Save.Click += (o, s) =>
            {
                if (Save.Enabled)
                {
                    Save.BorderStyle = BorderStyle.None;
                    SaveFile();
                }
            };
        }

        private void EnableControls(bool on = true)
        {
            LearningRate.Enabled = DiscountFactor.Enabled = ExploreRate.Enabled = LearningTrials.Enabled= QStatePlugins.Enabled =
                QAlgoPlugins.Enabled = ClearPlugins.Enabled = Open.Enabled = Save.Enabled = on;
            Settings.Enabled = on ? state.HasSettings : false;
        }

        public void Abort()
        {
            if (CurrentMode == AWAKEN)
            {
                if(awakenProcess.IsBusy && !awakenProcess.CancellationPending)
                    awakenProcess.CancelAsync();
            }
            else
            {
                if (learnProcess.IsBusy && !learnProcess.CancellationPending)
                    learnProcess.CancelAsync();
            }
        }

        private void Reset()
        {
            OutputQueue = new List<string>();
            ClearLearningTable();
        }

        private void LoadPlugin(string file, bool newlyLoaded = false)
        {
            if (newlyLoaded)
            {
                if (!Properties.Settings.Default.Plugins.Contains(file))
                {
                    Properties.Settings.Default.Plugins += file + "|";
                    Properties.Settings.Default.Save();
                }
            }
            Assembly a;
            try
            {
                a = Assembly.LoadFrom(file);
            }
            catch (Exception e)
            {
                Properties.Settings.Default.Plugins = Properties.Settings.Default.Plugins.Replace(file + "|", "");
                Properties.Settings.Default.Save();
                throw new Exception("Unable to load plugin: " + file + "\n" + e);
            }

            LoadAssembly(a, !newlyLoaded);
        }

        private void LoadAssembly(Assembly a, bool updateSelectedItem = false)
        {
            foreach (Type item in a.GetTypes().
            Where(t => String.Equals(t.Namespace, "QLearner.QAlgos", StringComparison.Ordinal) || String.Equals(t.Namespace, "QLearner.QStates", StringComparison.Ordinal)).
            Where(t => t.IsSubclassOf(typeof(QAlgo)) || t.IsSubclassOf(typeof(QState))).
            Where(t => t.IsVisible).
            Where(t => !t.IsAbstract).
            OrderBy(t => t.Name))
            {
                if (item.IsSubclassOf(typeof(QAlgo)))
                {
                    if (!QAlgoPlugins.Items.Contains(item.Name))
                    {
                        QAlgoPlugins.Items.Add(item.Name);
                        if(!updateSelectedItem) WriteOutput("QAlgo Plugin Loaded: " + item.Name, true);
                        if (Properties.Settings.Default.QAlgoPlugin == item.Name)
                        {
                            LoadQAlgoPlugin(item.Name);
                            if (updateSelectedItem) QAlgoPlugins.SelectedItem = item.Name;
                        }
                    }
                }
                else if (item.IsSubclassOf(typeof(QState)))
                {
                    if (!QStatePlugins.Items.Contains(item.Name))
                    {
                        QStatePlugins.Items.Add(item.Name);
                        if (!updateSelectedItem) WriteOutput("QState Plugin Loaded: " + item.Name, true);
                        if (Properties.Settings.Default.QStatePlugin == item.Name)
                        {
                            LoadQStatePlugin(item.Name);
                            if (updateSelectedItem) QStatePlugins.SelectedItem = item.Name;
                        }
                    }
                }
            }
        }

        private void BrowsePlugin(Object o = null, object s = null)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Title = "Browse to Plugin File...";
            f.Filter = "DLL File (*.dll)|*.dll";
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadPlugin(f.FileName, true);
            }
        }

        private void BrowseQStatePlugin(Object o = null, object s = null)
        {
            if (QStatePlugins.SelectedItem.ToString() == "*Browse to Plugin File...*")
                BrowsePlugin();
            else LoadQStatePlugin(QStatePlugins.SelectedItem.ToString());
        }

        private void LoadQStatePlugin(string f)
        {
            try
            {
                bool loaded = false;
                foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type qstate in a.GetTypes().
                Where(t => String.Equals(t.Namespace, "QLearner.QStates", StringComparison.Ordinal)).
                Where(t => t.IsSubclassOf(typeof(QState))).
                Where(t => t.IsVisible).
                Where(t => !t.IsAbstract).
                Where(t => t.Name==f))
                {
                    state = (QState)Activator.CreateInstance(qstate);
                    WriteOutput("Loaded QState: " + qstate.Name, true);
                    state.setQLearner(this);
                    Settings.Enabled = state.HasSettings;

                    if (state != null && algo != null)
                    {
                        agent = new QAgent(this, algo, state);
                        Reset();
                        Awaken.Enabled = Learn.Enabled = true;
                    }

                    Properties.Settings.Default.QStatePlugin = qstate.Name;
                    Properties.Settings.Default.Save();
                    loaded = true;
                    break;
                }
                if (!loaded)
                {
                    WriteOutput("QState Not Found: " + f, true);
                    ResetPlugins(false);
                }
            }
            catch (Exception e)
            {
                WriteOutput("Unable to load QState: " + f +"\n"+e, true);
                ResetPlugins(false);
            }
        }

        private void BrowseQAlgoPlugin(Object o = null, object s = null)
        {
            if (QAlgoPlugins.SelectedItem.ToString() == "*Browse to Plugin File...*")
                BrowsePlugin();
            else LoadQAlgoPlugin(QAlgoPlugins.SelectedItem.ToString());
        }

        private void LoadQAlgoPlugin(string f)
        {
            try
            {
                bool loaded = false;
                foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type qalgo in a.GetTypes().
                Where(t => String.Equals(t.Namespace, "QLearner.QAlgos", StringComparison.Ordinal)).
                Where(t => t.IsSubclassOf(typeof(QAlgo))).
                Where(t => t.IsVisible).
                Where(t => !t.IsAbstract).
                Where(t => t.Name == f))
                {
                    algo = (QAlgo)Activator.CreateInstance(qalgo);
                    WriteOutput("Loaded QAlgo: " + qalgo.Name, true);
                    algo.setQLearner(this);

                    if (algo != null && state != null)
                    {
                        agent = new QAgent(this, algo, state);
                        Reset();
                        Awaken.Enabled = Learn.Enabled = true;
                    }

                    Properties.Settings.Default.QAlgoPlugin = qalgo.Name;
                    Properties.Settings.Default.Save();

                    loaded = true;
                    break;
                }
                if (!loaded)
                {
                    WriteOutput("QAlgo Not Found: " + f, true);
                    ResetPlugins(false);
                }
            }
            catch (Exception e)
            {
                WriteOutput("Unable to load QAlgo: " + f + "\n" + e, true);
                ResetPlugins(false);
            }
        }

        private void ResetPlugins(bool restart = true)
        {
            Awaken.Enabled = Learn.Enabled = Settings.Enabled = false;
            Properties.Settings.Default.QStatePlugin = "";
            Properties.Settings.Default.QAlgoPlugin = "";
            Properties.Settings.Default.Save();
            WriteOutput("Plugins cleared.", true, true);
            if (restart) Application.Restart();
            else
            {
                BootPlugins();
            }
        }

        private void BootPlugins(bool updateSelections = false)
        {
            QAlgoPlugins.Items.Clear();
            QStatePlugins.Items.Clear();
            Awaken.Enabled = Learn.Enabled = Settings.Enabled = false;
            QStatePlugins.Items.Add("*Browse to Plugin File...*");
            QAlgoPlugins.Items.Add("*Browse to Plugin File...*");
            LoadAssembly(Assembly.GetExecutingAssembly(), true);
        }

        private delegate void RenameLearningTableD(string n, string s, string a, string qv);
        public void RenameLearningTable(string n, string s, string a, string qv)
        {
            if (InvokeRequired)
            {
                Invoke(new RenameLearningTableD(RenameLearningTable), n, s, a, qv);
            }
            else
            {
                LearningTable.Columns[0].HeaderText = n;
                LearningTable.Columns[1].HeaderText = s;
                LearningTable.Columns[2].HeaderText = a;
                LearningTable.Columns[3].HeaderText = qv;
            }
        }

        private delegate void UpdateLearningTableD(int n, QState s, string a, decimal qv);
        public void UpdateLearningTable(int n, QState s, string a, decimal qv)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateLearningTableD(UpdateLearningTable), n, s, a, qv);
            }
            else
            {
                QStateActionPair p = new QStateActionPair(s, a);
                if (LearningTableQStateKeys.ContainsKey(p))
                {
                    LearningTableQStateKeys[p].Cells["Num"].Value = n;
                    LearningTableQStateKeys[p].Cells["QValue"].Value = qv;
                }
                else
                {
                    LearningTable.Rows.Add(n, s.ToString(), a, qv);
                    LearningTableQStateKeys[p] = LearningTable.Rows[LearningTable.Rows.Count - 1]; ;
                }
            }
        }
        private delegate void UpdateLearningTableDD(int n, string s, string a, decimal qv);
        public void UpdateLearningTable(int n, string s, string a, decimal qv)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateLearningTableDD(UpdateLearningTable), n, s, a, qv);
            }
            else
            {
                if (LearningTableStringKeys.ContainsKey(s))
                {
                    LearningTableStringKeys[s].Cells["Num"].Value = n;
                    LearningTableStringKeys[s].Cells["Action"].Value = a;
                    LearningTableStringKeys[s].Cells["QValue"].Value = qv;
                }
                else
                {
                    LearningTable.Rows.Add(n, s, a, qv);
                    LearningTableStringKeys[s] = LearningTable.Rows[LearningTable.Rows.Count - 1];
                }
            }
        }
        private delegate void ClearLearningTableD();
        public void ClearLearningTable()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearLearningTableD(ClearLearningTable));
            }
            else
            {
                LearningTable.Rows.Clear();
                LearningTableQStateKeys = new Dictionary<QStateActionPair, DataGridViewRow>();
                LearningTableStringKeys = new Dictionary<string, DataGridViewRow>();
                BestActions = new Dictionary<QState, QStateActionPair>();
                displayRowQueue = new List<DataGridViewRow>();
            }
        }

        private delegate void UpdateTrialD(decimal t);
        public void UpdateTrial(decimal t)
        {
            if (InvokeRequired) Invoke(new UpdateTrialD(UpdateTrial), t);
            else trialNum.Text = t.ToString();
        }

        private delegate void UpdateScoreD(decimal t);
        public void UpdateScore(decimal newScore)
        {
            if (InvokeRequired) Invoke(new UpdateScoreD(UpdateScore), newScore);
            else
            {
                decimal avg = 0, trial = 1;
                decimal.TryParse(avgScore.Text, out avg);
                decimal.TryParse(trialNum.Text, out trial);
                score.Text = newScore.ToString();
                avgScore.Text = ((avg * (trial - 1) + newScore) / trial).ToString();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Awaken.Text == "Abort")
            {
                ExploreRate.Enabled = true;
                ExploreRate.Value = tempExploreRate;
                LearningRate.Enabled = true;
                LearningRate.Value = tempLearnRate;
            }
            base.OnFormClosing(e);
        }

        private void FlushOutputQueue(bool flush = false, bool force = false)
        {
            if (!force && HideOutput.Checked) return;
            if ((flush || (DateTime.Now - lastOutputUpdate).TotalMilliseconds > 100) && OutputQueue.Count > 0)
            {
                WriteOutput(string.Join("\n", OutputQueue), true);
                OutputQueue.Clear();
                lastOutputUpdate = DateTime.Now;
            }
        }

        private delegate void WriteOutputD(string s, bool flush, bool force);
        public void WriteOutput(string s = "", bool flush = true, bool force=true)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new WriteOutputD(WriteOutput), s, flush, force);
            }
            else
            {
                if (!force && HideOutput.Checked) return;
                if (!flush && (awakenProcess.IsBusy || learnProcess.IsBusy))
                {
                    OutputQueue.Add(DateTime.Now + ": " + s);
                    FlushOutputQueue(false, force);
                }
                else
                {
                    output.AppendText((!s.Contains(": ") || !s.Contains(DateTime.Now.ToShortDateString())? DateTime.Now + ": ":"") + s + "\n");
                    output.SelectionStart = output.Text.Length;
                    output.ScrollToCaret();
                }
            }
        }

        public void popup(string s)
        {
            WriteOutput(s, true);
            MessageBox.Show(s);
        }

        public bool CurrentMode { get; private set; }
        public static bool AWAKEN { get { return true; } }
        public static bool LEARN { get { return false; } }
        public bool OutputHidden { get { return HideOutput.Checked; } }

        public Dictionary<string, string> NewSettingsForm(Dictionary<string, string> SettingNames)
        {
            Dictionary<string, string> vals = new Dictionary<string, string>();
            Form f = new Form();
            int rowHeight = 25;
            f.Width = 400; f.Height = SettingNames.Count * rowHeight + 4* rowHeight;
            int x = 10; int y = 10;
            f.Text = "Settings";
            Dictionary<string, TextBox> fields = new Dictionary<string,TextBox>();

            Button OK = new Button();
            Button Cancel = new Button();

            foreach (KeyValuePair<string, string> kv in SettingNames)
            {
                Label l = new Label();
                l.Text = kv.Key;
                l.Width = 150;
                f.Controls.Add(l);
                l.Location = new Point(x, y);
                l.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                TextBox t = new TextBox();
                t.Text = kv.Value;
                t.Location = new Point(x+l.Width+20, y);
                t.Width = f.Width - 60 - l.Width;
                t.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
                f.Controls.Add(t);
                y += rowHeight;
                fields.Add(kv.Key, t);
                t.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        foreach (string key in SettingNames.Keys)
                            vals[key] = fields[key].Text;
                        f.DialogResult = DialogResult.OK;
                    }
                    else if (e.KeyCode == Keys.Escape)
                    {
                        vals = SettingNames;
                        f.DialogResult = DialogResult.Cancel;
                    }
                };
            }
            y += rowHeight/2;
            
            OK.Text = "Save";
            f.Controls.Add(OK);
            OK.Location = new Point(x + 120, y);
            OK.Anchor = AnchorStyles.Bottom;
            OK.Width = 50;
            OK.Height = rowHeight;
            
            Cancel.Text = "Cancel";
            f.Controls.Add(Cancel);
            Cancel.Location = new Point(x+180, y);
            Cancel.Anchor = AnchorStyles.Bottom;
            Cancel.Width = 50;
            Cancel.Height = rowHeight;

            f.ControlBox = false;
            OK.Click += (o, s) =>
            {
                foreach (string key in SettingNames.Keys)
                    vals[key] = fields[key].Text;
                f.DialogResult = DialogResult.OK;
            };
            Cancel.Click += (o, s) =>
            {
                vals = SettingNames;
                f.DialogResult = DialogResult.Cancel;
            };
            f.ShowDialog();
            return vals;
        }

        private void OpenFile()
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Title = "Load previously learned data...";
            f.Filter = "QLearned Files (*.qlearned)|*.qlearned";

            QLearned o=null;
            if (f.ShowDialog() == DialogResult.OK)
            {
                WriteOutput("Opening " + f.FileName + "...");
                EnableControls(false);
                using (Stream stream = File.Open(f.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    try
                    {

                        BinaryFormatter bFormatter = new BinaryFormatter();
                        o = (QLearned)bFormatter.Deserialize(stream);
                        stream.Close();

                        if (o != null)
                        {
                            agent.Open(o);
                            WriteOutput("Learning data loaded from " + f.FileName);
                        }
                        else popup("Unable to open data from " + f.FileName);
                    }
                    catch (Exception e)
                    {
                        stream.Close();
                        popup("Unable to open data from " + f.FileName+"\nDetails: "+e);
                    }
                }
            }
            EnableControls(true);
        }

        private void SaveFile()
        {
            QLearned o = agent.Save();
            if (o == null)
            {
                popup("Nothing to save.");
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save learned data to...";
                dialog.Filter = "QLearned Files (*.qlearned)|*.qlearned";
                dialog.FileName = o.state + "_" + o.algo + "_Learn"+o.learn+"_Discount"+o.discount+"_Explore"+o.explore+"_"+o.trials+"Trial"+(o.trials==1? "":"s");
                dialog.AddExtension = true;

                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WriteOutput("Saving to " + dialog.FileName);
                    EnableControls(false);

                    using (Stream stream = File.Open(dialog.FileName, FileMode.Create))
                    {
                        try
                        {
                            
                            BinaryFormatter bFormatter = new BinaryFormatter();
                            bFormatter.Serialize(stream, o);
                            stream.Close();
                            WriteOutput("Learning data saved to " + dialog.FileName);
                        }
                        catch (Exception e)
                        {
                            stream.Close();
                            WriteOutput("Error saving data: " + e);
                        }
                    }
                }
                EnableControls(true);
            }
        }
    }
}
