/*
*   This file is part of xFF
*   Copyright (C) 2017 Fabio Attard
*
*   This program is free software: you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation, either version 3 of the License, or
*   (at your option) any later version.
*
*   This program is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*   Additional Terms 7.b and 7.c of GPLv3 apply to this file:
*       * Requiring preservation of specified reasonable legal notices or
*         author attributions in that material or in the Appropriate Legal
*         Notices displayed by works containing it.
*       * Prohibiting misrepresentation of the origin of that material,
*         or requiring that modified versions of such material be marked in
*         reasonable ways as different from the original version.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Test : MonoBehaviour
{
    public enum DisplayRotation
    {
        Standard,
        RotatedLeft,
        RotatedRight,
        UpsideDown
    }

    public GameObject screenPivot;

    [Header("Display Config")]
    public int displayWidth = 320;
    public int displayHeight = 240;
    public int displayZoomFactor = 1;
    public DisplayRotation displayRotation = DisplayRotation.Standard;


    Vector3 anchorTopLeft;
    Vector3 anchorTopRight;
    Vector3 anchorBottomLeft;
    Vector3 anchorBottomRight;


    void Awake( )
    {
        switch (displayRotation)
        {
            case DisplayRotation.RotatedLeft:
                SetScreenRotatedLeft();
                break;

            case DisplayRotation.RotatedRight:
                SetScreenRotatedRight();
                break;

            case DisplayRotation.UpsideDown:
                SetScreenUpsideDown();
                break;

            case DisplayRotation.Standard:
            default:
                SetScreenStandard();
                break;
        }
    }

    void SetScreenStandard( )
    {
        ChangeResolution(displayWidth, displayHeight, displayZoomFactor);
        RecalcAnchors(displayWidth, displayHeight);

        screenPivot.transform.position = anchorTopLeft;
    }


    void SetScreenRotatedRight( )
    {
        ChangeResolution(displayHeight, displayWidth, displayZoomFactor);
        RecalcAnchors(displayHeight, displayWidth);

        screenPivot.transform.position = anchorTopRight;
        screenPivot.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
    }


    void SetScreenRotatedLeft( )
    {
        ChangeResolution(displayHeight, displayWidth, displayZoomFactor);
        RecalcAnchors(displayHeight, displayWidth);

        screenPivot.transform.position = anchorBottomLeft;
        screenPivot.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
    }


    void SetScreenUpsideDown( )
    {
        ChangeResolution(displayWidth, displayHeight, displayZoomFactor);

        RecalcAnchors(displayWidth, displayHeight);
        
        screenPivot.transform.position = anchorBottomRight;
        screenPivot.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
    }



    void RecalcAnchors(int aWidth, int aHeight)
    {
        anchorTopLeft = new Vector3(-(aWidth / 2), (aHeight / 2)) + (Vector3.forward * 10.0f);
        anchorTopRight = new Vector3((aWidth / 2), (aHeight / 2)) + (Vector3.forward * 10.0f);
        anchorBottomLeft = new Vector3(-(aWidth / 2), -(aHeight / 2)) + (Vector3.forward * 10.0f);
        anchorBottomRight = new Vector3((aWidth / 2), -(aHeight / 2)) + (Vector3.forward * 10.0f);
    }


    void ChangeResolution(int aWidth, int aHeight, int aZoomFactor)
    {
#if UNITY_EDITOR
        UnityEditor.PlayerSettings.defaultScreenWidth = aWidth * aZoomFactor;
        UnityEditor.PlayerSettings.defaultScreenHeight = aHeight * aZoomFactor;
#elif UNITY_STANDALONE
        Screen.SetResolution(aWidth * aZoomFactor, aHeight * aZoomFactor, Screen.fullScreen);
#endif

        // Adjust ortographic size for pixel perfect
        Camera.main.orthographicSize = (aHeight / 2);
    }
}
