using System;
using WhimsyEarlierLiteracy.ViewModel;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhimsyEarlierLiteracy
{
    public sealed partial class MainPage : Page
    {
        private readonly Accelerometer _accelerometerSensor;
        private readonly LightSensorReading _baseReading;
        private readonly Compass _compassSensor;
        private readonly Gyrometer _gyroSensor;
        private readonly Inclinometer _inclineSensor;
        private readonly LightSensor _lightSensor;
        private readonly Geolocator _locator;
        private readonly OrientationSensor _orientationSensor;

        public MainPage()
        {
            InitializeComponent();
            _locator = new Geolocator();
            Loaded += OnControlLoaded;

            _lightSensor = LightSensor.GetDefault();
            if (_lightSensor != null)
            {
                //For now we get a base reading to use to compare later
                _baseReading = _lightSensor.GetCurrentReading();
                //Register for the reading change
                _lightSensor.ReadingChanged += OnLightReadingChanged;
            }

            _accelerometerSensor = Accelerometer.GetDefault();
            if (_accelerometerSensor != null)
            {
                _accelerometerSensor.Shaken += OnShaken;
            }
            _compassSensor = Compass.GetDefault();
            if (_compassSensor != null)
            {
                _compassSensor.ReadingChanged += OnCompassReadingChanged;
            }
            _gyroSensor = Gyrometer.GetDefault();
            if (_gyroSensor != null)
            {
                _gyroSensor.ReadingChanged += OnGyroReadingChanged;
            }
            _inclineSensor = Inclinometer.GetDefault();
            if (_inclineSensor != null)
            {
                _inclineSensor.ReadingChanged += OnInclineReadingChanged;
            }
            _orientationSensor = OrientationSensor.GetDefault();
            if (_orientationSensor != null)
            {
                _orientationSensor.ReadingChanged += OnOrientationReadingChanged;
            }
        }

        private void OnOrientationReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
        }

        private void OnInclineReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
        {
        }

        private void OnGyroReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
        {
        }

        private void OnCompassReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
        }

        private void OnShaken(Accelerometer sender, AccelerometerShakenEventArgs args)
        {
            ((MainPageVM) DataContext).WasShaken();
            Path[] paths;

            if (zoom.IsZoomedInViewActive)
            {
                paths = GetVisiblePaths();
            }
            else
            {
                paths = GetAllPaths();
            }

            int MaxParts = 100;

            foreach (Path geo in paths)
            {
                for (int i = 0; i < MaxParts; i++)
                {
                    // The geometry allows us to get the position and  
                    // the tangent at a given fraction length.
                    Geometry flattened = GetFlattenedPathGeometry(geo.Data);
                    Point tangent;
                    Point point;
                    GetPointAtFractionLength(flattened, (i/MaxParts), out point, out tangent);

                    // Create the visual representation of the broken part.  
                    var rectangle = new Rectangle
                                        {
                                            Width = 8,
                                            Height = 3,
                                            Fill = new SolidColorBrush(Colors.Black),
                                            RadiusX = 1.5,
                                            RadiusY = 1.5
                                        };

                    // Add the rectangle to the explosion area and set the position  
                    Canvas.SetLeft(rectangle, point.X - (rectangle.Width/2));
                    Canvas.SetTop(rectangle, point.Y - (rectangle.Height/2));
                    ((Canvas) geo.Parent).Children.Add(rectangle);

                    // For a good look, the rectangle shall be rotated,  
                    // based on the tangent of the point.  
                    var v = new Vector(tangent.X, tangent.Y);
                    v.Normalize();

                    double angle = Vector.AngleBetween(new Vector(1, 0), v);
                    rectangle.RenderTransform = new RotateTransform
                                                    {
                                                        Angle = angle,
                                                        CenterX = rectangle.Width/2,
                                                        CenterY = rectangle.Height/2
                                                    };
                }
            }
        }

        private void GetPointAtFractionLength(Geometry data, int i, out Point point, out Point tangent)
        {
            //throw new NotImplementedException();
        }

        private Geometry GetFlattenedPathGeometry(Geometry geometry)
        {
            return geometry;
        }

        private Path[] GetAllPaths()
        {
            return null;
        }

        private Path[] GetVisiblePaths()
        {
            return null;
        }

        private void OnLightReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            //Read the local settings and see if AutoQuietTime has been setup
            if ((bool?) ApplicationData.Current.LocalSettings.Values["AutoQuietTime"] == true)
            {
                //If so check if the light in the last reading has dropped below our darkness threshold, could be configured for sensativity
                //This will also turn the application noise back up, when the lights are back on
                //We could also look at the time of the last reading and determine if the lights have been off for a period of time

                //_lastReadingTime = args.Reading.Timestamp;
                bool shouldBeQuietTime = args.Reading.IlluminanceInLux < (_baseReading.IlluminanceInLux - 100);
                ApplicationData.Current.LocalSettings.Values["ShouldBeQuietTime"] = shouldBeQuietTime;
            }
        }

        private async void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Geoposition position = await _locator.GetGeopositionAsync();
                ((MainPageVM) DataContext).LoadSymbolsFor(position);
            }
            catch (Exception exception)
            {
                //NOTE: Couldn't find GeoCoordinates, don't attempt to load geo based symbols.
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }

    internal class Vector
    {
        public Vector(double x, double y)
        {
        }

        public void Normalize()
        {
        }

        public static double AngleBetween(Vector v1, Vector v2)
        {
            return 0;
        }
    }
}