using OwoAdvancedSensationBuilder.builder;
using OwoAdvancedSensationBuilder.Demo.DemoSections;
using OwoAdvancedSensationBuilder.manager;
using OWOGame;
using System.Collections.Specialized;

namespace OwoAdvancedSensationBuilder.Demo {
    public partial class DemoForm : Form {
        public DemoForm() {
            InitializeComponent();
        }

        OrderedDictionary sections = new OrderedDictionary();

        private void Form1_Load(object sender, EventArgs e) {
            OWO.AutoConnect();

            initIntro();
            initCompare();

            flowFinale.Controls.Clear();
            flowFinale.Controls.Add(new ExperienceSection());
        }

        private void initIntro() {
            flowIntro.Controls.Clear();

            flowIntro.Controls.Add(new HeaderSection("Level up the Capabilities of the OWO"));
            flowIntro.Controls.Add(new TextSection("Hello everyone. I would like to present you a way to use the OWO more effectivly. " +
                "I worked on a solution to solve a few of the limitations that the OWO has."));
            flowIntro.Controls.Add(new TextSection("First, that the OWO doesn't allow to play multiple Sensations at once."));
            flowIntro.Controls.Add(new TextSection("Second, that the OWO doesn't have have a feature to update Sensations at runtime."));
            flowIntro.Controls.Add(new TextSection("Third, that the OWO doesn't have the most options to create more complex Sensations."));
            flowIntro.Controls.Add(new TextSection("My Solution to these problems are the the AdvancedSensationBuilder, " +
                "the AdvancedSensationManager and the AdvancedSensationService (in the following just \"Builder\", \"Manager\" or \"Service\"). But first..."));

            flowIntro.Controls.Add(new HeaderSection("What are advanced Sensations"));
            flowIntro.Controls.Add(new TextSection("Advanced Sensations (or in code AdvancedStreamingSensation) are a special new Type of SensationSequences and have two main Characteristics."));
            flowIntro.Controls.Add(new TextSection("They are split into very short snippets which each represent a timeframe of 0.1 Seconds (even though the " +
             "duration of each snippet is slightly longer)."));
            flowIntro.Controls.Add(new TextSection("Every Sensation Snippet additionally has no Ramp and a 100% intensity, while the actual intensity is set in a " +
                "per Muscle basis. This unified Model allows to compare Sensations and treat each Muscle on its own. The only information besides the Muscles that " +
                "is relevant is the frequency (and maybe the name)."));

            flowIntro.Controls.Add(new HeaderSection("The Builder"));
            flowIntro.Controls.Add(new TextSection("The Builder is the part that transforms a Sensation into an advanced Sensation. This includes " +
                "transforming the actual intensity and the per Muscle intensity into the per muscle intensity as well as calculating the intensity in case of " +
                "RampUp and RampDown."));
            flowIntro.Controls.Add(new TextSection("Additionally it also allows to modify and combine Sensations."));

            flowIntro.Controls.Add(new HeaderSection("The Manager"));
            flowIntro.Controls.Add(new TextSection("The Manager is the part that plays the Sensations. It keeps a reference to all Sensations and streams " +
                " every 0.1 seconds the calculated Sensation based on all managed Sensations."));
            flowIntro.Controls.Add(new TextSection("The manager plays a combination of all Sensations, but can only calculate the intensities. " +
                "To decide the overall frequency it respects the Priority of the Sensations. In case of the same Priority, " +
                "the oldest inserted Sensation has priority."));
            flowIntro.Controls.Add(new TextSection("The Manager also allows to update Sensations. This allows to change Sensations at runtime and the " +
                "Manager will simply continue to play the Sensation with the updated Values, without the need to start the whole Sensation from the beginning. " +
                "This allows to dynamically factor in Variables. A simple event management is also provided to help keep Track of Sensations."));
            flowIntro.Controls.Add(new TextSection("All Sensations in the Manager are AdvancedStreamingSensations, but its possible to call the Manager with " +
                "regular OWO Sensations. The Manager transforms them if needed by itself."));

            flowIntro.Controls.Add(new HeaderSection("The Service"));
            flowIntro.Controls.Add(new TextSection("The Service is the part that actually does all the math. This is where Sensations actually get combined, cut " +
                "or what else. Additionally the Service has a few Methods that allow to create a new Builder in ways a normal Sensation can't do, like e.g. " +
                "a Ramp up that doesn't start at 0."));

            flowIntro.Controls.Add(new HeaderSection("Drawbacks"));
            flowIntro.Controls.Add(new TextSection("The Main drawback is that by using the Manager it is (currently) not really possible to use " +
                "Baked Sensations. OWO doesn't allow to load Baked Sensations by Code at the moment. Once that is a feature, this should be resolvable. " +
                "There already is Code that would be working with Baked Sensations (as the Class already exists), " +
                "but there is no inbuilt way to fill this Class with the User calibrated Values."));
            flowIntro.Controls.Add(new TextSection("You may say now \"Why not use the Baked Sensations the regular way and the advanced Sensations " +
                "the new Way?\". Well that is in theory possible, but leads to the minor issue that generally the two Methods don't have great compatibility. " +
                "The regular way sends the Sensation once while the Manager sends a Sensation every 0.1 seconds while processing Sensations, " +
                "which would then cancel the non-managed Sensations. This could be designed and worked around, but would require some work."));
            flowIntro.Controls.Add(new TextSection("Lastly there is minor issue number two. As mentioned above the Manager works in 0.1 Second Ticks. " +
                "This means two things. First that there could be a delay of up to 0.1 Seconds if you try to add a new Sensation to a running Manager and " +
                "second, (especially very short) RampUps or RampDowns may feel slightly stiffer. I have no information on how smoothly OWO ramps its Sensations, " +
                "but the advanced Sensations doesn't use the OWO ramping. Instead the ramping steps are getting calculated in 0.1 second steps."));

            flowIntro.Controls.Add(new HeaderSection("Regarding this Demo"));
            flowIntro.Controls.Add(new TextSection("As this is a Demo for OWO Code, there are multiple opportunities to feel stuff yourself. " +
                "There are multiple Triggers such as this Buttons here."));
            Sensation tap = SensationsFactory.Create(100, 0.1f, 15, 0.1f, 0, 0.2f).WithMuscles(Muscle.Dorsal_R);
            tap = tap.Append(tap);
            flowIntro.Controls.Add(new SensationSection(this, "Tap on back", tap, false));
            flowIntro.Controls.Add(new TextSection("You probably also already noticed the \"Managed Sensations\" list on the right. " +
                "This list displays the Sensations in the Manager and hopefully makes a few things easier to understand. " +
                "Also If there is a Sensation you want to stop, such as a looping Sensation started from a different Tab or a Sensation where you forgot " +
                "how you started it, simply double click it at any time."));
            flowIntro.Controls.Add(new TextSection("I also reccomend to check out the \"Finale\" Tab once you finished trying everything out ^^ "));
        }

        private void initCompare() {
            flowComparisson.Controls.Clear();

            flowComparisson.Controls.Add(new HeaderSection("Comparissions"));
            flowComparisson.Controls.Add(new TextSection("In this Section of the Demo I present to you a few Comparissons of official prebuild Sensations " +
                "by OWO and how these can be enhanced with the Builder or even just by sending these through the Manager."));
            flowComparisson.Controls.Add(new TextSection("Simpler Sensations, such as \"Dart\" are designed to be simple, so there are no advanced alternatives, " +
                "but even in these cases the Manager can help to create additional benefits. By sending them through the manager, we can play multiple " +
                "Sensations at once. Depending on how we name the Sensation Instance we can decide if the same Sensation can be played multiple times or " +
                "replaces itself. Unique names stack on top of each other, while universal names cancel each other."));
            flowComparisson.Controls.Add(new TextSection("The \"Original\" Button sends the Original Sensation in the normal OWO way."));
            flowComparisson.Controls.Add(new TextSection("The \"Manager\" Buttons send the Original Sensation through the Manager, which in isolation should " +
                "feel very similar (if not identical) to the OWO way, but allows to play multiple Sensations at once."));
            flowComparisson.Controls.Add(new TextSection("The \"Advanced\" Button, if available, sends an alternative Sensation that utilizes Features " +
                "of the Builder."));

            Sensation hugOrig = SensationsFactory.Create(1, 1, 1, 0, 0, 0);
            Sensation hugAdvanced = SensationsFactory.Create(1, 1, 1, 0, 0, 0);
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, null));

            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, null));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, null));
            flowComparisson.Controls.Add(new HeaderSection("Comparissions"));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, hugAdvanced));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, hugAdvanced));
            flowComparisson.Controls.Add(new CompareSection(this, "hug", hugOrig, hugAdvanced));
        }

        public void setupManagedSensation(AdvancedSensationStreamInstance instance) {
            instance.AfterStateChanged += Instance_AfterStateChanged;
        }
        
        public void playManagedSensation(AdvancedSensationStreamInstance instance) {
            AdvancedSensationManager.getInstance().play(instance);
        }
        public void toggleManagedSensation(AdvancedSensationStreamInstance instance) {
            if (AdvancedSensationManager.getInstance().getPlayingSensationInstances(true).ContainsKey(instance.name)) {
                AdvancedSensationManager.getInstance().stopSensation(instance.name);
            } else {
                playManagedSensation(instance);
            }
        }

        private void Instance_AfterStateChanged(AdvancedSensationStreamInstance instance, AdvancedSensationManager.ProcessState state) {

            if (lbManager.InvokeRequired) {
                Action doInvoke = delegate { Instance_AfterStateChanged(instance, state); };
                lbManager.Invoke(doInvoke);
                return;
            }

            if (state == AdvancedSensationManager.ProcessState.ADD) {
                lbManager.Items.Add(instance.name);
            } else if (state == AdvancedSensationManager.ProcessState.REMOVE) {
                lbManager.Items.Remove(instance.name);
            }
        }

        private void lbManager_MouseDoubleClick(object sender, MouseEventArgs e) {
            int index = lbManager.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches) {
                AdvancedSensationManager.getInstance().stopSensation(lbManager.Items[index].ToString());
            }
        }

    }
}
