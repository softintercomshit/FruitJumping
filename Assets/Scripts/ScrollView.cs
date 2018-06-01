using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScrollAxis
{
	public bool isAxis;
	public Vector2 deltaScroll;
	public float deltaAxis;
	public float deltaFix;
}

public class ScrollView : MonoBehaviour
{
	public enum ScrollViewType
	{
		fix,
		fixLerp,
		lerp
	}

	public ScrollViewType typeScroll = ScrollViewType.lerp;
	public Transform _transform;
	public ScrollAxis scrollX;
	public ScrollAxis scrollY;
	[Range (0, 1)]
	public float speedLerpEndTouch;
	[Range (0, 1)]
	public float speedLerpOnTouch;
	public float speedInputLimit = 1;
	public bool touchOn = false;
	public Vector3 velocityInput = Vector3.zero;

	Vector3 velocity = Vector3.zero;
	Vector3 targetPosition;

	void Update ()
	{
		Scroll ();
	}

	[ContextMenu ("TouchEnd")]
	public void TouchEnd ()
	{
		touchOn = false;
		if (typeScroll == ScrollViewType.fixLerp) {
			targetPosition = _transform.position;
			if (scrollX.isAxis) {
				if (velocity.x < -speedInputLimit) {
					targetPosition = new Vector3 (Mathf.Clamp (Mathf.FloorToInt ((_transform.localPosition.x - scrollX.deltaScroll.x) / scrollX.deltaFix) * scrollX.deltaFix + scrollX.deltaScroll.x, scrollX.deltaScroll.x, scrollX.deltaScroll.y), targetPosition.y, targetPosition.z);
				} else if (velocity.x > speedInputLimit) {
					targetPosition = new Vector3 (Mathf.Clamp (Mathf.CeilToInt ((_transform.localPosition.x - scrollX.deltaScroll.x) / scrollX.deltaFix) * scrollX.deltaFix + scrollX.deltaScroll.x, scrollX.deltaScroll.x, scrollX.deltaScroll.y), targetPosition.y, targetPosition.z);
				} else {
					targetPosition = new Vector3 (Mathf.Clamp (Mathf.RoundToInt ((_transform.localPosition.x - scrollX.deltaScroll.x) / scrollX.deltaFix) * scrollX.deltaFix + scrollX.deltaScroll.x, scrollX.deltaScroll.x, scrollX.deltaScroll.y), targetPosition.y, targetPosition.z);
				}
			}
			if (scrollX.isAxis) {
				if (velocity.y < -speedInputLimit) {
					targetPosition = new Vector3 (targetPosition.x, Mathf.Clamp (Mathf.FloorToInt ((_transform.localPosition.y - scrollY.deltaScroll.x) / scrollY.deltaFix) * scrollY.deltaFix + scrollY.deltaScroll.x, scrollY.deltaScroll.x, scrollY.deltaScroll.y), targetPosition.z);
				} else if (velocity.y > speedInputLimit) {
					targetPosition = new Vector3 (targetPosition.x, Mathf.Clamp (Mathf.CeilToInt ((_transform.localPosition.y - scrollY.deltaScroll.x) / scrollY.deltaFix) * scrollY.deltaFix + scrollY.deltaScroll.x, scrollY.deltaScroll.x, scrollY.deltaScroll.y), targetPosition.z);
				} else {
					targetPosition = new Vector3 (targetPosition.x, Mathf.Clamp (Mathf.RoundToInt ((_transform.localPosition.y - scrollY.deltaScroll.x) / scrollY.deltaFix) * scrollY.deltaFix + scrollY.deltaScroll.x, scrollY.deltaScroll.x, scrollY.deltaScroll.y), targetPosition.z);
				}
			}
		}
	}

	void Scroll ()
	{
		if (touchOn) {
			velocity = Vector3.Lerp (velocity, velocityInput, speedLerpOnTouch);
			if (typeScroll == ScrollViewType.fix) {
				targetPosition = new Vector3 ((scrollX.isAxis) ? Mathf.Clamp (_transform.localPosition.x + velocity.x, scrollX.deltaScroll.x, scrollX.deltaScroll.y) : _transform.localPosition.x, (scrollY.isAxis) ? Mathf.Clamp (_transform.localPosition.y + velocity.y, scrollY.deltaScroll.x, scrollY.deltaScroll.y) : _transform.localPosition.y, _transform.localPosition.z);
			} else {
				targetPosition = new Vector3 ((scrollX.isAxis) ? Mathf.Clamp (_transform.localPosition.x + velocity.x, scrollX.deltaScroll.x - scrollX.deltaAxis, scrollX.deltaScroll.y + scrollX.deltaAxis) : _transform.localPosition.x, (scrollY.isAxis) ? Mathf.Clamp (_transform.localPosition.y + velocity.y, scrollY.deltaScroll.x - scrollY.deltaAxis, scrollY.deltaScroll.y + scrollY.deltaAxis) : _transform.localPosition.y, _transform.localPosition.z);
			}
			_transform.localPosition = Vector3.Lerp (_transform.localPosition, targetPosition, speedLerpOnTouch);
		} else { 
			if (typeScroll == ScrollViewType.fix) {
				return;
			} else if (typeScroll == ScrollViewType.fixLerp) {
				if (scrollX.isAxis) {
					if (_transform.localPosition.x > scrollX.deltaScroll.x && _transform.localPosition.x < scrollX.deltaScroll.y) {
						_transform.localPosition = Vector3.Lerp (_transform.localPosition, targetPosition, speedLerpEndTouch);
					} else {
						_transform.localPosition = Vector3.Lerp (_transform.localPosition, new Vector3 (Mathf.Clamp (_transform.localPosition.x, scrollX.deltaScroll.x, scrollX.deltaScroll.y), _transform.localPosition.y, _transform.localPosition.z), speedLerpEndTouch);
					}
				}
				if (scrollY.isAxis) {
					if (_transform.localPosition.y > scrollY.deltaScroll.x && _transform.localPosition.y < scrollY.deltaScroll.y) {
						transform.localPosition = Vector3.Lerp (transform.localPosition, targetPosition, speedLerpEndTouch);
					} else {
						_transform.localPosition = Vector3.Lerp (_transform.localPosition, new Vector3 (_transform.localPosition.x, Mathf.Clamp (_transform.localPosition.y, scrollY.deltaScroll.x, scrollY.deltaScroll.y), transform.localPosition.z), speedLerpEndTouch);
					}
				}
			} else if (typeScroll == ScrollViewType.lerp) {
				if (scrollX.isAxis) {
					if (_transform.localPosition.x > scrollX.deltaScroll.x && _transform.localPosition.x < scrollX.deltaScroll.y) {
						velocity = new Vector3 (Mathf.Lerp (velocity.x, 0, speedLerpEndTouch), velocity.y, velocity.z);
						_transform.localPosition = new Vector3 (Mathf.Clamp (_transform.localPosition.x + velocity.x, scrollX.deltaScroll.x, scrollX.deltaScroll.y), _transform.localPosition.y, _transform.localPosition.z);
					} else {
						_transform.localPosition = Vector3.Lerp (_transform.localPosition, new Vector3 (Mathf.Clamp (_transform.localPosition.x, scrollX.deltaScroll.x, scrollX.deltaScroll.y), _transform.localPosition.y, _transform.localPosition.z), speedLerpEndTouch);
					}
				}
				if (scrollY.isAxis) {
					if (_transform.localPosition.y > scrollY.deltaScroll.x && _transform.localPosition.y < scrollY.deltaScroll.y) {
						velocity = new Vector3 (velocity.x, Mathf.Lerp (velocity.y, 0, speedLerpEndTouch), velocity.z);
						_transform.localPosition = new Vector3 (_transform.localPosition.x, Mathf.Clamp (_transform.localPosition.y + velocity.y, scrollY.deltaScroll.x, scrollY.deltaScroll.y), _transform.localPosition.z);
					} else {
						_transform.localPosition = Vector3.Lerp (_transform.localPosition, new Vector3 (_transform.localPosition.x, Mathf.Clamp (_transform.localPosition.y, scrollY.deltaScroll.x, scrollY.deltaScroll.y), _transform.localPosition.z), speedLerpEndTouch);
					}
				}
			}
		}
	}

}
