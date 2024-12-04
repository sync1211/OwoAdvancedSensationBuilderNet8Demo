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
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_R.WithIntensity(25),
                Muscle.Arm_L.WithIntensity(40)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_R.WithIntensity(35),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Lumbar_L.WithIntensity(20)
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeMax(newMuscles, oldMuscles);

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMuscles[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMuscles[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
            }
        }

        [TestMethod]
        public void actualMuscleMergeMinTest()
        {
            Muscle[] oldMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25),
                Muscle.Lumbar_L.WithIntensity(20)
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeMin(newMuscles, oldMuscles);

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMuscles[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMuscles[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
            }
        }

        [TestMethod]
        public void actualMuscleMergeOverrideTest()
        {
            Muscle[] oldMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(35),
                Muscle.Lumbar_L.WithIntensity(20)
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeOverride(newMuscles, oldMuscles);

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMuscles[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMuscles[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
            }
        }

        [TestMethod]
        public void actualMuscleMergeKeepTest()
        {
            Muscle[] oldMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25)
            ];

            Muscle[] newMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(40),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Lumbar_L.WithIntensity(20),
                Muscle.Arm_R.WithIntensity(35)
            ];

            Muscle[] expectedMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(50),
                Muscle.Abdominal_R.WithIntensity(10),
                Muscle.Arm_L.WithIntensity(40),
                Muscle.Arm_R.WithIntensity(25),
                Muscle.Lumbar_L.WithIntensity(20)
            ];

            Muscle[] actualMuscles = AdvancedSensationService.actualMuscleMergeKeep(newMuscles, oldMuscles);

            Assert.AreEqual(expectedMuscles.Length, actualMuscles.Length);
            for (int i = 0; i < expectedMuscles.Length; i++)
            {
                Assert.AreEqual(expectedMuscles[i].id, actualMuscles[i].id);
                Assert.AreEqual(expectedMuscles[i].intensity, actualMuscles[i].intensity, $"Muscle at index {i} has an intensity of {actualMuscles[i].intensity}, expected: {expectedMuscles[i].intensity}");
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
