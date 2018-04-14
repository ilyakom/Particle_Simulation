using Diploma.Visualization;
using System;
using System.Windows.Forms;
using Diploma.NumericalMethods;
using Diploma.NumericalMethods.ParticlesMethod;
using Diploma.NumericalMethods.SweepMethod;

namespace Diploma
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            graph1d.InitializeContexts(); 
        }

        private readonly VisualizationModel _model = VisualizationModel.GetInstance();
	    private readonly ViewModel _viewModel = ViewModel.GetInstance();

	    private SweepMethod _sweepMethod;
	    private ParticlesMethod _particlesMethod;
        
        //длинна локализации
        //double Localize = 2 * Math.PI * Math.Sqrt(2 * 10);
        //float Colapse = 1;
        //double WindowWidth = 0.3;

        private void StartForm_Load(object sender, EventArgs e)
        {
            Visualizer.Init(graph1d);

			AddBinding("TimeStep", ViewModel.SetTimeStep, TimeStepTextBox);
			AddBinding("SweepDimension", ViewModel.SetSweepDimension, sweepDimensionTextBox);
			AddBinding("ParticlesCount", null, particlesCountTextBox);
			AddBinding("PartitionsCount", ViewModel.SetPartitionsCount, PartitionsCountTextBox);
			AddBinding("Averaging", ViewModel.SetAveraging, AveragingTextBox);
			AddBinding("SweepDimensionStep", null, sweepStepTextBox);
			AddBinding("ParzenWindowWidth", null, parzenWindowWidthTextBox);
			AddBinding("PartitionWidth", null, PartitionWidthTextBox);
			AddBinding("CurrentStep", null, CurrentStepTextBox);
        }

	    private void AddBinding(string modelField, ConvertEventHandler parser, IBindableComponent target)
	    {
			var bind = new Binding("Text", _viewModel, modelField);
		    if( parser != null) bind.Parse += parser;
		    target.DataBindings.Add(bind);
	    }

        private void Initialization()
        {
	        _viewModel.LeftBound = 0;
	        _viewModel.RightBound = _model.ProjectionWidth;
	        _viewModel.SweepDimensionStep = (_viewModel.RightBound - _viewModel.LeftBound) / _viewModel.SweepDimension;
	        _viewModel.PartitionWidth = _model.ProjectionWidth / _viewModel.PartitionsCount;

	        _particlesMethod = new ParticlesMethod( _viewModel.PartitionWidth);
	        _sweepMethod = new SweepMethod(_viewModel.SweepDimension);

            _viewModel.ParticlesCount = _particlesMethod.Particles.Count;
	        _viewModel.TimeStep = _particlesMethod.GetPreferableTimeStep();           
        }

        private void Simulation()
        {   
            _viewModel.CurrentStep++;

            _sweepMethod.DifferenceScheme();

            if (everyStepAveraging.Checked)
            {
                _particlesMethod.SimulateWithEveryStepAveraging();
            }
            else if (wholeWayAveraging.Checked)
            {
				_particlesMethod.SimulateWholeWayAveraging();
            }
            else if (parzenMethod.Checked)
            {
				_particlesMethod.SimulateParzenMethod();
            }     
        }

        private void Button1_Click(object sender, EventArgs e)
        {
	        _viewModel.CurrentStep = 1;
	        Initialization();
			simulateTimer.Start();            
            button1.Enabled = false;
        }

        private void SimulateTimer_Tick(object sender, EventArgs e)
        {
            Visualizer.Draw(_particlesMethod.PartitionsHeights, _particlesMethod.Density, _sweepMethod.Solution);

            Simulation();           
        }                   

        private void Button2_Click(object sender, EventArgs e)
        {
            simulateTimer.Stop();           
            button1.Enabled = true;
        }
	}
}
