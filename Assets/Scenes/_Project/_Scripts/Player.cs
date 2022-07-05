using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryGetIClickOnRaycast(out IClick click))
            {
                click.Click();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (TryGetIClickOnRaycast(out IClick click))
            {
                click.AlternativeClick();
            }
        }
    }

    private bool TryGetIClickOnRaycast(out IClick click)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D == false)
        {
            click = null;
            return false;
        }
        
        return hit2D.collider.TryGetComponent(out click);
    }
}