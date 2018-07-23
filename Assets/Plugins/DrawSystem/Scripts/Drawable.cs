using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;


// 1. Attach this to a read/write enabled sprite image
// 2. Set the drawing_layers  to use in the raycast
// 3. Attach a 2D collider (like a Box Collider 2D) to this sprite
// 4. Hold down left mouse to draw on this texture!
public class Drawable : MonoBehaviour
{
    public UnityEvent OnDrawEvent;
	public bool CanDraw;
    // PEN COLOUR
    public static Color Pen_Colour = Color.black;     // Change these to change the default drawing settings
    // PEN WIDTH (actually, it's a radius, in pixels)
    public static int Pen_Width = 6;
	public static int Actual_Pen_Width;
    public LayerMask Drawing_Layers;
	public SpriteRenderer SpriteShowed;
    public bool Reset_Canvas_On_Play = true;

    // MUST HAVE READ/WRITE enabled set in the file editor of Unity
    Sprite drawable_sprite;
	public Sprite reset_sprite;
    public Texture2D drawable_texture;

    Vector2 previous_drag_position;
    Color[] clean_colours_array;
    Color32[] cur_colors;
    bool mouse_was_previously_held_down = false;
    bool no_drawing_on_current_drag = false;
	bool erase = false;
    Vector3 lastMousePos = new Vector3(0,0,0);

    void Awake()
    {
		SpriteShowed = GetComponent<SpriteRenderer>();
		// Initialize clean pixels to use
		int x = Mathf.FloorToInt(reset_sprite.rect.x);
		int y = Mathf.FloorToInt(reset_sprite.rect.y);
		int width = Mathf.FloorToInt(reset_sprite.rect.width);
		int height = Mathf.FloorToInt(reset_sprite.rect.height);
		clean_colours_array = reset_sprite.texture.GetPixels(x, y, width, height);

		// Should we reset our canvas image when we hit play in the editor?
		if (Reset_Canvas_On_Play)
            ResetCanvas();
    }

	public void InitDrawer (Sprite TargetSprite, Texture2D TargetTexture)
	{
        drawable_texture = TargetTexture;
        SpriteShowed.sprite = TargetSprite;
    }

    void Update()
    {
		if (!CanDraw)
		{
			return;
		}

		// Is the user holding down the left mouse button?
		bool mouse_held_down = (Input.GetMouseButton(0) || Input.GetMouseButton(1));
		erase = Input.GetMouseButton(1);
        if (mouse_held_down && !no_drawing_on_current_drag)
        {
            // Convert mouse coordinates to world coordinates
            Vector2 mouse_world_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the current mouse position overlaps our image
            Collider2D hit = Physics2D.OverlapPoint(mouse_world_position, Drawing_Layers.value);
            if (hit != null && hit.transform != null)
                // We're over the texture we're drawing on! Change them pixel colours
                ChangeColourAtPoint(mouse_world_position);
            else
            {
                // We're not over our destination texture
                previous_drag_position = Vector2.zero;
                if (!mouse_was_previously_held_down)
                {
                    // This is a new drag where the user is left clicking off the canvas
                    // Ensure no drawing happens until a new drag is started
                    no_drawing_on_current_drag = true;
                }
            }
        }
        // Mouse is released
        else if (!mouse_held_down)
        {
            previous_drag_position = Vector2.zero;
            no_drawing_on_current_drag = false;
        }
        mouse_was_previously_held_down = mouse_held_down;

        ChangeSizeBySpeed();
    }

        
    // Pass in a point in WORLD coordinates
    // Changes the surrounding pixels of the world_point to the static pen_colour
    public void ChangeColourAtPoint(Vector2 world_point)
    {
        OnDrawEvent.Invoke();
        // Change coordinates to local coordinates of this image
        Vector3 local_pos = transform.InverseTransformPoint(world_point);

        // Change these to coordinates of pixels
        float pixelWidth = drawable_sprite.rect.width;
        float pixelHeight = drawable_sprite.rect.height;
        float unitsToPixels = pixelWidth / drawable_sprite.bounds.size.x * transform.localScale.x;
        unitsToPixels *= 1.2f;

        // Need to center our coordinates
        float centered_x = local_pos.x * unitsToPixels + pixelWidth / 2f;
        float centered_y = local_pos.y * unitsToPixels + pixelHeight / 2f;

        // Round current mouse position to nearest pixel
        Vector2 pixel_pos = new Vector2(Mathf.RoundToInt(centered_x), Mathf.RoundToInt(centered_y));

        cur_colors = drawable_texture.GetPixels32();

        if (previous_drag_position == Vector2.zero)
        {
            // If this is the first time we've ever dragged on this image, simply colour the pixels at our mouse position
            MarkPixelsToColour(pixel_pos, Actual_Pen_Width, Pen_Colour);
        }
        else
        {
            // Colour in a line from where we were on the last update call
            ColourBetween(previous_drag_position, pixel_pos);
        }
        ApplyMarkedPixelChanges();

        //Debug.Log("Dimensions: " + pixelWidth + "," + pixelHeight + ". Units to pixels: " + unitsToPixels + ". Pixel pos: " + pixel_pos);
        previous_drag_position = pixel_pos;
    }


    // Set the colour of pixels in a straight line from start_point all the way to end_point, to ensure everything inbetween is coloured
    public void ColourBetween(Vector2 start_point, Vector2 end_point)
    {
        // Get the distance from start to finish
        float distance = Vector2.Distance(start_point, end_point);
        Vector2 direction = (start_point - end_point).normalized;

        Vector2 cur_position = start_point;

        // Calculate how many times we should interpolate between start_point and end_point based on the amount of time that has passed since the last update
        float lerp_steps = 1 / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerp_steps)
        {
            cur_position = Vector2.Lerp(start_point, end_point, lerp);
            MarkPixelsToColour(cur_position, Actual_Pen_Width, Pen_Colour);
        }
    }


	public void MarkPixelsToColour(Vector2 center_pixel, int pen_thickness, Color color_of_pen)
    {
        // Figure out how many pixels we need to colour in each direction (x and y)
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;
        int extra_radius = Mathf.Min(0, pen_thickness - 2);

		int x, y, px, nx, py, ny, d;

		for (x = 0; x <= pen_thickness; x++)
		{
			d = (int)Mathf.Ceil(Mathf.Sqrt(pen_thickness * pen_thickness - x * x));
			for (y = 0; y <= d; y++)
			{
				px = center_x + x;
				nx = center_x - x;
				py = center_y + y;
				ny = center_y - y;

				MarkPixelToChange(px, py, color_of_pen);
				MarkPixelToChange(nx, py, color_of_pen);
				MarkPixelToChange(px, ny, color_of_pen);
				MarkPixelToChange(nx, ny, color_of_pen);
			}
		}

		/*for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++)
        {
            // Check if the X wraps around the image, so we don't draw pixels on the other side of the image
            if (x >= (int)drawable_sprite.rect.width
                || x < 0)
                continue;



            for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++)
            {
                MarkPixelToChange(x, y, color_of_pen);
            }
        }*/
    }
    public void MarkPixelToChange(int x, int y, Color color)
    {
        // Need to transform x and y coordinates to flat coordinates of array
        int array_pos = y * (int)drawable_sprite.rect.width + x;

        // Check if this is a valid position
        if (array_pos > cur_colors.Length || array_pos < 0)
            return;

		if (erase)
		{
			cur_colors[array_pos] = reset_sprite.texture.GetPixel(x,y);
		}
		else
		{
			cur_colors[array_pos] = color;
		}
	}

    public void ApplyMarkedPixelChanges()
    {
        drawable_texture.SetPixels32(cur_colors);
        drawable_texture.Apply();
    }


    // Directly colours pixels. This method is slower than using MarkPixelsToColour then using ApplyMarkedPixelChanges
    // SetPixels32 is far faster than SetPixel
    // Colours both the center pixel, and a number of pixels around the center pixel based on pen_thickness (pen radius)
    public void ColourPixels(Vector2 center_pixel, int pen_thickness, Color color_of_pen)
    {
        // Figure out how many pixels we need to colour in each direction (x and y)
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;
        int extra_radius = Mathf.Min(0, pen_thickness - 2);

        for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++)
        {
            for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++)
            {
                drawable_texture.SetPixel(x, y, color_of_pen);
            }
        }

        drawable_texture.Apply();
    }

    // Changes every pixel to be the reset colour
    public void ResetCanvas()
    {
        Texture2D ResetTexture = new Texture2D(reset_sprite.texture.width, reset_sprite.texture.height, TextureFormat.ARGB32, false);
        ResetTexture.SetPixels32(reset_sprite.texture.GetPixels32());
        ResetTexture.Apply();

        drawable_sprite = Sprite.Create(ResetTexture, new Rect(0.0f, 0.0f, reset_sprite.texture.width, reset_sprite.texture.height), new Vector2(0.5f, 0.5f));
        drawable_texture = ResetTexture;
        SpriteShowed.sprite = drawable_sprite;
    }

    public void ChangeSizeBySpeed()
    {
		if (erase)
		{
			Actual_Pen_Width = Pen_Width * 2;
		}
		else
		{
			float dist = 0;

			if (lastMousePos != Vector3.zero)
			{
				dist = Vector3.Distance(Input.mousePosition, lastMousePos);
			}

			//Debug.Log(dist);

			if ((int)((float)dist) < 2)
			{
				Actual_Pen_Width = Pen_Width;
			} 
			else
			{
				Actual_Pen_Width = (int)((float)Pen_Width * (1 - (float)dist / 2f * 0.05f));
				if (Actual_Pen_Width < Pen_Width / 5) Actual_Pen_Width = Pen_Width / 5;
			}
		}

		lastMousePos = Input.mousePosition;
    }
}