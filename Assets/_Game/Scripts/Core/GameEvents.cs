using System;

namespace PixelFlow3D
{
    /// <summary>
    /// 全局事件总线：所有跨组件通信统一走这里，方便查找和管理
    /// 使用方式：触发方调 FireXxx()，监听方 += / -=
    /// </summary>
    public static class GameEvents
    {
        // 发射器被点击
        public static event Action<Shooter> ShooterTapped;
        public static void FireShooterTapped(Shooter s) => ShooterTapped?.Invoke(s);

        // 发射器弹药耗尽
        public static event Action<Shooter> ShooterAmmoDepleted;
        public static void FireShooterAmmoDepleted(Shooter s) => ShooterAmmoDepleted?.Invoke(s);

        // 发射器绕传送带一圈（携带剩余弹药）
        public static event Action<Shooter, int> ShooterCompletedLoop;
        public static void FireShooterCompletedLoop(Shooter s, int ammo) => ShooterCompletedLoop?.Invoke(s, ammo);

        // 发射器放入槽位（槽位索引）
        public static event Action<Shooter, int> ShooterPlacedInSlot;
        public static void FireShooterPlacedInSlot(Shooter s, int i) => ShooterPlacedInSlot?.Invoke(s, i);

        // 发射器离开槽位（原槽位索引）
        public static event Action<Shooter, int> ShooterLeftSlot;
        public static void FireShooterLeftSlot(Shooter s, int i) => ShooterLeftSlot?.Invoke(s, i);

        // 像素被消除（网格坐标）
        public static event Action<int, int> PixelDestroyed;
        public static void FirePixelDestroyed(int x, int y) => PixelDestroyed?.Invoke(x, y);

        // 问号发射器被揭示
        public static event Action<Shooter> ShooterRevealed;
        public static void FireShooterRevealed(Shooter s) => ShooterRevealed?.Invoke(s);

        // 所有像素已清除（通关）
        public static event Action AllPixelsCleared;
        public static void FireAllPixelsCleared() => AllPixelsCleared?.Invoke();
    }
}
