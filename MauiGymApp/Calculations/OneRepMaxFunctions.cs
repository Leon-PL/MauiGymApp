namespace MauiGymApp.Calculations
{
    public static class OneRepMaxFunctions
    {
        public static OneRepMaxFunction Epley
            => new((w, r) => w * (1 + ((double)r / 30)),
                   (orm, r) => orm / (1 + ((double)r / 30)));

        public static OneRepMaxFunction Bryzki
            => new((w, r) => w * (36 / (37 - (double)r)),
                   (orm, r) => orm * (37 - (double)r) / 36);

        public static OneRepMaxFunction Lombardi
            => new((w, r) => w * Math.Pow(r, 0.1),
                   (orm, r) => orm / Math.Pow(r, 0.1));

        public static OneRepMaxFunction Mayhew
            => new((w, r) => 100 * w / (52.2 + (41.9 * Math.Exp(-0.055 * r))),
                   (orm, r) => orm * (52.2 + (41.9 * Math.Exp(-0.055 * r))) / 100);

        public static OneRepMaxFunction McGlothin
            => new((w, r) => 100 * w / (101.3 - (2.67123 * r)),
                (orm, r) => (orm * (101.3 - (2.67123 * r))) / 100);

        public static OneRepMaxFunction OConnor
            => new((w, r) => w * (1 + ((double)r / 40)),
                   (orm, r) => orm / (1 + ((double)r / 40)));

        public static OneRepMaxFunction Wathan
            => new((w, r) => 100 * w / (48.8 + (53.8 * Math.Exp(-0.075 * r))),
                   (orm, r) => orm * (48.8 + (53.8 * Math.Exp(-0.075 * r))) / 100); // WRONG
    }
}
