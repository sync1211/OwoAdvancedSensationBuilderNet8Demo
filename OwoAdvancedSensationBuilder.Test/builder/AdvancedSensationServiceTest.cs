using OwoAdvancedSensationBuilder.builder;
using OWOGame;

namespace OwoAdvancedSensationBuilder.Test.builder
{
    [TestClass]
    public class AdvancedSensationServiceTest
    {
        /*
         * /!\ NOTE IF YOU SEE COMPILER ERRORS /!\
         * 
         * This test expects the following methods to be public:
         * * actualMuscleMergeMax
         * * actualMuscleMergeMin
         * * actualMuscleMergeKeep
         * * actualMuscleMergeOverride
         * * lerp
         */

        [TestMethod]
        public void actualMuscleMergeMaxTest()
        {
            Muscle[] oldMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Arm_R.WithIntensity(25),
                Muscle.Arm_L.WithIntensity(40)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            //NOTE: Expected muscles are sorted by their ID
            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10), // ID = 2
                Muscle.Abdominal_L.WithIntensity(50), // ID = 3
                Muscle.Arm_R.WithIntensity(35),       // ID = 4
                Muscle.Arm_L.WithIntensity(40),       // ID = 5
                Muscle.Lumbar_L.WithIntensity(20)     // ID = 9
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeMax(newMuscles, oldMuscles);
            
            // Sort by ID as we only care about the intensity values
            Muscle[] actualMusclesSorted = actualMuscles.OrderBy(m => m.id).ToArray();

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMusclesSorted[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMusclesSorted[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
            }
        }

        [TestMethod]
        public void actualMuscleMergeMinTest()
        {
            Muscle[] oldMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            //NOTE: Expected muscles are sorted by their ID, see comments in actualMuscleMergeMaxTest for reference
            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20)
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeMin(newMuscles, oldMuscles);

            // Sort by ID as we only care about the intensity values
            Muscle[] actualMusclesSorted = actualMuscles.OrderBy(m => m.id).ToArray();

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMusclesSorted[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMusclesSorted[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
            }
        }

        [TestMethod]
        public void actualMuscleMergeOverrideTest()
        {
            Muscle[] oldMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            //NOTE: Expected muscles are sorted by their ID, see comments in actualMuscleMergeMaxTest for reference
            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(35),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20)
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeOverride(newMuscles, oldMuscles);

            // Sort by ID as we only care about the intensity values
            Muscle[] actualMusclesSorted = actualMuscles.OrderBy(m => m.id).ToArray();

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMusclesSorted[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMusclesSorted[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
            }
        }

        [TestMethod]
        public void actualMuscleMergeKeepTest()
        {
            Muscle[] oldMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            //NOTE: Expected muscles are sorted by their ID, see comments in actualMuscleMergeMaxTest for reference
            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Arm_R.WithIntensity(25),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20)
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeKeep(newMuscles, oldMuscles);

            // Sort by ID as we only care about the intensity values
            Muscle[] actualMusclesSorted = actualMuscles.OrderBy(m => m.id).ToArray();

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMusclesSorted[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMusclesSorted[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
            }
        }

        [TestMethod]
        public void float2snippetsTest_RoundUp()
        {
            const float seconds = 0.46f;
            const int expected = 5;

            int actual = AdvancedSensationService.float2snippets(seconds);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void float2snippetsTest_RoundDown()
        {
            const float seconds = 0.44f;
            const int expected = 4;

            int actual = AdvancedSensationService.float2snippets(seconds);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void lerpTest()
        {
            const float firstFloat = 10;
            const float secondFloat = 110;
            const float by = 0.5f;

            const int expected = 60;

            int actual = AdvancedSensationService.lerp(firstFloat, secondFloat, by);
            Assert.AreEqual(expected, actual);
        }
    }
}
