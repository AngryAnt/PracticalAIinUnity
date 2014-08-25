using UnityEngine;
using System.Collections;


public class SetMaterialOnStart : MonoBehaviour
{
	public Renderer m_Renderer;
	public Material m_Material;
	

	void Start ()
	{
		m_Renderer.material = m_Material;
	}
}
