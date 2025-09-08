// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("6wMuCButsUBXZeastAxxc5MuYSehIiwjE6EiKSGhIiIjl3fYoySHOekTl9NX/uVKv8dKp7WImYOSYiX4Mwx6o9eJGlzpdwEfjp00cNimYblujb7Tn6bZKwo05ymvByzeKVxSul5cvYShoQS91LvGShV6m9/VhDswj0Mk1aiuthQPKzt+tc1wdGXvmYQoeILycTP5wCc2zzNdLOAJ/nnkpyPpv8/S63alYybhh8uoJrcbelsP4J9sskp1hdQwqQ+N5G3pp3D9V1pBVhR3f2Xqe/C29zQFkHZWQZloHBOhIgETLiUqCaVrpdQuIiIiJiMgwjUgPSmVmDvnQMZ0Bwtl+AXI5SU6LXykkL8Fs2GcgsNK3dd3qRyF4mSV4UpF+0NUmiEgIiMi");
        private static int[] order = new int[] { 9,1,2,4,12,13,10,7,10,13,10,13,12,13,14 };
        private static int key = 35;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
