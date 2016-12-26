using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Line2D
{
	public class HeartBeat : MonoBehaviour {
		public GameObject m_petal;

		private IEnumerator m_heartBeating;

		public static HeartBeat s_instance;

		private const float c_minWaitTime = 1.0f;
		private const float c_maxWaitTime = 10.0f;

		private const int c_minPoints = 10;
		private const int c_maxPoints = 20;

		private const float c_width = .2f;

		private const float c_speedModifier = 10.0f;
		public Color m_color;

		private const float c_maxTransparencyTime = .5f;
		private Color noAlpha;

		[SerializeField]private Line2DRenderer m_lineRenderer;
		// Use this for initialization
		void Start () {
			m_heartBeating = PulseHeartBeat ();
			s_instance = this;
			noAlpha = new Color (m_color.r, m_color.g, m_color.b, 0.0f);
		}

		private IEnumerator OscillateTransparencyAtPoint(Line2DRenderer lr, int index){
			float transparencyTime = 0.0f;
			float maxTime = Random.Range (0, c_maxTransparencyTime);
			while (transparencyTime < maxTime) {
				if (index >= lr.points.Count) {
                    yield break;
				}

				transparencyTime += Time.deltaTime;
				m_lineRenderer.points [index].color = Color.Lerp (m_color, noAlpha, transparencyTime / c_maxTransparencyTime);
				yield return new WaitForEndOfFrame ();
			}


			while (transparencyTime > 0) {
				if (index >= lr.points.Count) {
                    yield break;
				}

				transparencyTime -= Time.deltaTime;
				m_lineRenderer.points [index].color = Color.Lerp (m_color, noAlpha, transparencyTime / c_maxTransparencyTime);
				yield return new WaitForEndOfFrame ();
			}
		}

		public static void MakeHeartMonitorBeat(){
			s_instance.StopCoroutine (s_instance.m_heartBeating);
			s_instance.StartCoroutine (s_instance.m_heartBeating);
		}

		private IEnumerator PulseHeartBeat(){
			Vector3 petalCoords = m_petal.transform.position;

			List<Line2DPoint> points = new List<Line2DPoint> ();

			//randomly generate nPoints the number
			int nPoints = Random.Range(c_minPoints,c_maxPoints);

			Line2DPoint lastPoint = new Line2DPoint ( new Vector3 (0, 0, 0),c_width,m_color);


			m_lineRenderer.points.Clear ();
			float opacityTime = 0.0f;
			//generate nPoints points
			for (int i = 0; i < nPoints; i++) {
				m_lineRenderer.points.Add(new Line2DPoint (lastPoint.pos,lastPoint.width,lastPoint.color));
				//TODO: work out actual spacing relative to petal
				StartCoroutine(OscillateTransparencyAtPoint(m_lineRenderer,i));
				Line2DPoint newPoint = new Line2DPoint (new Vector3(m_petal.transform.position.x + 9 + i * -2.0f,m_petal.transform.position.y + Random.Range(-3.0f,3.0f)) ,c_width,m_color);
				float tPassed = 0.0f;
				while (Vector3.Distance (m_lineRenderer.points [i].pos, newPoint.pos) > 0.1f) {
					tPassed += Time.deltaTime;
					opacityTime += Time.deltaTime;
					m_lineRenderer.points [i].pos = Vector3.Lerp (lastPoint.pos, newPoint.pos, tPassed * c_speedModifier);



					yield return new WaitForEndOfFrame ();
				}



				lastPoint = m_lineRenderer.points [i];
			}
			opacityTime = 0.0f;
			//TODO: fold the points into eachother until they dissapear offcreen
			for(int i = 0; i < m_lineRenderer.points.Count -1 ; i++){
				Vector3 OriginalPosition = m_lineRenderer.points [i].pos;
				float tPassed = 0.0f;
				while (Vector3.Distance (m_lineRenderer.points [i].pos, m_lineRenderer.points [i + 1].pos) > 0.1f) {
					tPassed += Time.deltaTime;
					opacityTime += Time.deltaTime;
					m_lineRenderer.points [i].pos = Vector3.Lerp (OriginalPosition, m_lineRenderer.points [i + 1].pos, tPassed * c_speedModifier);
					yield return new WaitForEndOfFrame ();
				}
				m_lineRenderer.points.RemoveAt (i);
				i--;

				Color newAlphaColor = Color.Lerp(m_color,noAlpha,opacityTime * 3.0f);
				foreach (Line2DPoint l in m_lineRenderer.points) {
					l.color = newAlphaColor;
				}
			}
		}
	}
}
