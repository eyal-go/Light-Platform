using UnityEngine;

public class LightPlatform : MonoBehaviour
{
    private BoxCollider2D myCollider;
    private SpriteRenderer myRenderer;

    protected bool isActivated = false; //to be able to apply logic on the differnet states of the platform.

    protected virtual void Start()
    {
        // GetComponent = "תביא לי את הרכיב הזה מהאובייקט הנוכחי"
        // אנחנו שואבים את הקוליידר (הגוף הפיזי) ושומרים במשתנה
        myCollider = GetComponent<BoxCollider2D>();
        
        // כנ"ל לגבי הצייר (האחראי על הצבע והתמונה)
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // פונקציה ציבורית (Public Method)
    // המילה פאבליק קריטית פה: היא מאפשרת לסקריפטים *אחרים* (כמו הכדור) לקרוא לפונקציה הזו
    public void ActivatePlatform()
    {
        isActivated = true; 
        // שינוי הפיזיקה:
        // isTrigger = true אומר "רוח רפאים" (אפשר לעבור דרכו)
        // isTrigger = false אומר "קיר מוצק" (אי אפשר לעבור דרכו)
        // אנחנו מכבים את הטריגר -> הפלטפורמה הופכת למוצקה
        myCollider.isTrigger = false;

        // שינוי המראה:
        // משנים את הצבע ללבן מלא (ללא שקיפות) כדי לתת פידבק לשחקן
        myRenderer.color = Color.white; 

        gameObject.tag = "Ground";
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    public void DeactivatePlatform()
    {
        isActivated = false;

        myCollider.isTrigger = true;

        myRenderer.color = new Color32(176, 155, 155, 255);

        gameObject.tag = "Untagged";

        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}