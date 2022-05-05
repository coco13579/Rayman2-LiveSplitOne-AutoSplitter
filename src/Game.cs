using System.Diagnostics;
using MTK;
using MTK.MemUtils.Pointers;

namespace AutoSplitter;

internal class Game
{
    public bool Hooked;

    private StringPointer _levelName;
    private Pointer<bool> _isLoading;
    private Pointer<float> _posX;
    private Pointer<float> _posY;
    private Pointer<float> _posZ;
    private Pointer<uint> _customBits;
    private Pointer<byte> _finalBossHealth;

    private Stopwatch _inControlTimer;
    private bool _inControl;
    private bool _scanForBossHealth;
    private bool _liveSplitTimerStarted;

    private readonly MemoryManager _memory;
    public Game(UI ui)
    {
        _memory = new("Rayman2")
        {
            OnHook = () =>
            {
                InitPointers();

                Hooked = ui.GameHooked = true;
            },
            OnExit = () => Hooked = ui.GameHooked = false
        };

        _inControlTimer = new Stopwatch();
    }

    public void Check()
    {
        if (_memory.IsHooked)
            _memory.TickUp();
    }

    private void InitPointers()
    {
        _isLoading = new(_memory, 0x11663C);
        _posX = new(_memory, 0x100578, 0x4, 0x0, 0x1C);
        _posY = new(_memory, 0x100578, 0x4, 0x0, 0x20);
        _posZ = new(_memory, 0x100578, 0x4, 0x0, 0x24);
        _levelName = new(_memory, 0x10039F);
        _customBits = new(_memory, 0x100578, 0x4, 0x4, 0x24);
        _finalBossHealth = new(_memory, 0x102D64, 0xE4, 0x0, 0x4, 0x741);
    }

    public void FirstLevelMovement(LSO lso)
    {
        if (!_liveSplitTimerStarted)
        {
            ManageInControlVars();

            if (_levelName.Current == "jail_20"
                && _levelName.Old == "jail_20"
                && !_isLoading.Old
                && !_isLoading.Current
                && _posX.Old != 0 && _posX.Current != 0
                && _posY.Old != 0 && _posY.Current != 0
                && _posZ.Old != 0 && _posZ.Current != 0
                && _inControlTimer.ElapsedMilliseconds > 5
                && (Math.Abs(_posX.Current - _posX.Old) > 0.01 ||
                    Math.Abs(_posY.Current - _posY.Old) > 0.01 ||
                    Math.Abs(_posZ.Current - _posZ.Old) > 0.01))
            {
                lso.Send("start");
                lso.Send("initgametime");
                _inControlTimer.Reset();
                _liveSplitTimerStarted = true;
            }
        }
    }

    public void LoadingScreen(LSO lso)
    {
        if (_isLoading.Current)
        {
            lso.Send("pausegametime");
        }
        else if (_isLoading.Old)
        {
            lso.Send("resumegametime");
        }
    }

    public void LevelChanged(LSO lso)
    {
        if (_levelName.Current == "mapmonde" && _levelName.Old != "mapmonde" && _levelName.Old != "menu")
        {
            lso.Send("split");
        }
    }

    public void FinalBossDead(LSO lso)
    {
        CheckScanFinalBossHealth();

        if (_scanForBossHealth && _levelName.Current == "rhop_10" && _finalBossHealth.Current == 0)
        {
            lso.Send("split");
        }
    }

    public void NewGameStarted(LSO lso)
    {
        if (_levelName.Current == "jail_10" && _levelName.Old != "jail_10")
        {
            lso.Send("reset");
            ResetBooleans();
        }
    }

    public void ResetBooleans()
    {
        _inControlTimer.Reset();
        _liveSplitTimerStarted = false;
        _scanForBossHealth = false;
    }

    private void ManageInControlVars()
    {
        if (_isLoading.Current) _inControlTimer = Stopwatch.StartNew();

        _inControl = (_customBits.Current & 0x10000) == 0;
        _inControlTimer.Start();

        if (!_inControl) _inControlTimer.Reset();
    }

    private void CheckScanFinalBossHealth()
    {
        if (_levelName.Current == "rhop_10" && !_scanForBossHealth && _finalBossHealth.Current == 24)
        {
            _scanForBossHealth = true;
        }
    }
}