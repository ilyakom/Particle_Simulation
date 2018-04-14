using System;

namespace Diploma.Visualization
{
    public class VisualizationModel
    {
        public float YOffset;
        public float XOffset;

		public float ProjectionWidth;
		public float ProjectionHeight;

		private static VisualizationModel _model;
        private VisualizationModel()
        {
            if (_model != null)
                throw new Exception("Only one SystemModel should exists");
        }

        public static VisualizationModel GetInstance()
        {
	        return _model ?? (_model = new VisualizationModel());
        }
    }
}
