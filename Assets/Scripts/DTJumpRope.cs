using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DTJumpRope : MonoBehaviour
{
    private void OnEnable()
    {
        playtime = GameControllerScript.current.bcm.playtime;

        rope.SetBool("Hard Mode", false);
        jumpDelay = 1f;
        ropeHit = true;
        jumpStarted = false;
        jumps = 0;
        cs.jumpHeight = 0f;
        playtime.audioDevice.PlayOneShot(playtime.aud_ReadyGo);

        if (GameControllerScript.current.mode == "hard")
            targetJumps = 10;

        jumpCount.text = 0 + $"/{targetJumps}";
    }

    // Token: 0x06000049 RID: 73 RVA: 0x00003418 File Offset: 0x00001818
    private void Update()
    {
        if (jumpDelay > 0f)
            jumpDelay -= Time.deltaTime;
        else if (!jumpStarted)
        {
            jumpStarted = true;
            ropePosition = 1f;
            rope.SetTrigger("ActivateJumpRope");
            ropeHit = false;
        }

        if (ropePosition > 0f)
            ropePosition -= Time.deltaTime;
        else if (!ropeHit)
            RopeHit();
    }

    private void RopeHit()
    {
        ropeHit = true;

        if (cs.jumpHeight <= 0.2f)
            Fail();
        else
            Success();

        jumpStarted = false;
    }

    private void Success()
    {
        playtime.audioDevice.Stop();

        playtime.audioDevice.PlayOneShot(playtime.aud_Numbers[jumps]);

        jumps++;
        jumpCount.text = jumps + $"/{targetJumps}";
        jumpDelay = 0.1f;

        if (jumps >= targetJumps)
        {
            GameControllerScript.current.GiveScore(300);
            playtime.audioDevice.Stop();
            playtime.audioDevice.PlayOneShot(playtime.aud_Congrats);
            ps.DeactivateJumpRope();
        }
    }

    private void Fail()
    {
        playtime.audioDevice.PlayOneShot(fail);
        jumps = 0;
        jumpCount.text = jumps + $"/{targetJumps}";
    }

    public AudioClip fail;

    private int targetJumps = 5;

    // Token: 0x04000058 RID: 88
    public TMP_Text jumpCount;

    // Token: 0x04000059 RID: 89
    public Animator rope;

    // Token: 0x0400005A RID: 90
    public CameraScript cs;

    // Token: 0x0400005B RID: 91
    public PlayerScript ps;

    // Token: 0x0400005C RID: 92
    private DTPlaytimeScript playtime;

    // Token: 0x0400005E RID: 94
    public int jumps;

    // Token: 0x0400005F RID: 95
    public float jumpDelay;

    // Token: 0x04000060 RID: 96
    public float ropePosition;

    // Token: 0x04000061 RID: 97
    public bool ropeHit;

    // Token: 0x04000062 RID: 98
    public bool jumpStarted;
}
