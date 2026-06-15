using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixKineticistElements
{
    public class BasicKinesis
    {
        public static void MakeMain()
        {
            FeatureSelectionConfigurator.New("BasicKinesisFeatureSelection", "9B6962F7-D65E-4A86-8C85-34C09F503BF0", Kingmaker.Blueprints.Classes.FeatureGroup.KineticWildTalent)
                .SetDisplayName("BasicKinesisFeatureSelection.Name")
                .SetDescription("BasicKinesisFeatureSelection.Desc")
                
                .SetIsClassFeature(true)
                
            .Configure();

            FeatureConfigurator.New("BasicPyrokinesisFeature", "FF316118-5138-45F0-AEA5-14A0E0E28EEC")
                .SetDisplayName("BasicPyrokinesis")
                .SetDescription("BasicPyrokinesis.Desc")
                .AddFireFocusPrerequisite()
                .SetIsClassFeature(true)
                .Configure();

            FeatureConfigurator.New("BasicAerokinesisFeature", "10C7BC64-74CE-4735-92EB-5C85EE851375")
                .SetDisplayName("BasicAerokinesis")
                .SetDescription("BasicAerokinesis.Desc")
                .AddAirFocusPrerequisite()
                 .SetIsClassFeature(true)
                .Configure();

            FeatureConfigurator.New("BasicGeokinesisFeature", "04B76A3B-EF96-43EB-81B9-D8CA8DF45CD3")
                .SetDisplayName("BasicGeokinesis")
                .SetDescription("BasicGeokinesis.Desc")
                .AddEarthFocusPrerequisite()
                 .SetIsClassFeature(true)
                .Configure();

            FeatureConfigurator.New("BasicHydrokinesisFeature", "41755AB3-482F-4AC1-A646-85AE92F6EFDF")
                .SetDisplayName("BasicHydrokinesis")
                .SetDescription("BasicHydrokinesis.Desc")
                .AddWaterFocusPrerequisite()
                 .SetIsClassFeature(true)
                .Configure();

            FeatureConfigurator.New("BasicPhotokinesisFeature", "7ACE301F-8173-4FCB-A6A8-0A30C1429595")
                .SetDisplayName("BasicPhotokinesis")
                .SetDescription("BasicPhotokinesis.Desc")
                .AddLightFocusPrerequisite()
                 .SetIsClassFeature(true)
                .Configure();

            FeatureSelectionConfigurator.For("BasicKinesisFeatureSelection").AddToAllFeatures("BasicPyrokinesisFeature", "BasicAerokinesisFeature", "BasicGeokinesisFeature", "BasicHydrokinesisFeature", "BasicPhotokinesisFeature")
                .Configure();

            ProgressionConfigurator.For("b79e92dd495edd64e90fb483c504b8df").AddToLevelEntry(1, "BasicKinesisFeatureSelection").Configure();

            

        }

        internal static void Patch()
        {
            if (Main.IsKineticistArchetypesInstalled())
            {
                ArchetypeConfigurator.For("022742CC-A0CD-414C-98EE-D87F24AE5607").AddToRemoveFeatures(1, "9B6962F7-D65E-4A86-8C85-34C09F503BF0").Configure();
            }
        }
    }
}
