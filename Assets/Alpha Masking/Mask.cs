using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("Alpha Mask")]
public class Mask : MonoBehaviour
{
	public enum MappingAxis
	{
		X,
		Y,
		Z
	};
	
	[SerializeField]
	private MappingAxis _maskMappingWorldAxis = MappingAxis.Z;
	public MappingAxis maskMappingWorldAxis
	{
		get
		{
			return _maskMappingWorldAxis;
		}
		set
		{
			ChangeMappingAxis(value, _maskMappingWorldAxis, _invertAxis);
			_maskMappingWorldAxis = value;
		}
	}
	
	[SerializeField]
	private bool _invertAxis = false;
	public bool invertAxis
	{
		get
		{
			return _invertAxis;
		}
		set
		{
			ChangeMappingAxis(_maskMappingWorldAxis, _maskMappingWorldAxis, value);
			_invertAxis = value;
		}
	}
	
	[SerializeField]
	private bool _clampAlphaHorizontally = false;
	public bool clampAlphaHorizontally
	{
		get
		{
			return _clampAlphaHorizontally;
		}
		set
		{
			SetMaskBoolValueInMaterials("_ClampHoriz", value);
			_clampAlphaHorizontally = value;
		}
	}
	
	[SerializeField]
	private bool _clampAlphaVertically = false;
	public bool clampAlphaVertically
	{
		get
		{
			return _clampAlphaVertically;
		}
		set
		{
			SetMaskBoolValueInMaterials("_ClampVert", value);
			_clampAlphaVertically = value;
		}
	}
	
	[SerializeField]
	private float _clampingBorder = 0.01f;
	public float clampingBorder
	{
		get
		{
			return _clampingBorder;
		}
		set
		{
			SetMaskFloatValueInMaterials("_ClampBorder", value);
			_clampingBorder = value;
		}
	}
	
	[SerializeField]
	private bool _useMaskAlphaChannel = false;
	public bool useMaskAlphaChannel
	{
		get
		{
			return _useMaskAlphaChannel;
		}
		set
		{
			SetMaskBoolValueInMaterials("_UseAlphaChannel", value);
			_useMaskAlphaChannel = value;
		}
	}


	private Shader _maskedSpriteWorldCoordsShader;
	private Shader _maskedUnlitWorldCoordsShader;


	void Start ()
	{
		_maskedSpriteWorldCoordsShader = Shader.Find("Alpha Masked/Sprites Alpha Masked - World Coords");
		_maskedUnlitWorldCoordsShader = Shader.Find("Alpha Masked/Unlit Alpha Masked - World Coords");

		MeshRenderer maskMeshRenderer = GetComponent<MeshRenderer>();
		MeshFilter maskMeshFilter = GetComponent<MeshFilter>();
		if (Application.isPlaying)
		{
			if (maskMeshRenderer != null)
			{
				maskMeshRenderer.enabled = false;
			}
		}
#if UNITY_EDITOR
		else
		{
			if (maskMeshFilter == null)
			{
				maskMeshFilter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
				maskMeshFilter.sharedMesh = new Mesh();
				CreateAndAssignQuad(maskMeshFilter.sharedMesh);
				maskMeshFilter.sharedMesh.name = "Mask Quad";
			}
			if (maskMeshRenderer == null)
			{
				maskMeshRenderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
				maskMeshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
				maskMeshRenderer.sharedMaterial.name = "Mask Material";
			}

			maskMappingWorldAxis = _maskMappingWorldAxis;
			invertAxis = _invertAxis;
		}
#endif

	}
	
	
	void Update ()
	{
		if (_maskedSpriteWorldCoordsShader == null)
		{
			_maskedSpriteWorldCoordsShader = Shader.Find("Alpha Masked/Sprites Alpha Masked - World Coords");
		}
		if (_maskedUnlitWorldCoordsShader == null)
		{
			_maskedUnlitWorldCoordsShader = Shader.Find("Alpha Masked/Unlit Alpha Masked - World Coords");
		}

		if ((_maskedSpriteWorldCoordsShader == null) || (_maskedUnlitWorldCoordsShader == null))
		{
			Debug.Log("Shaders necessary for masking don't seem to be present in the project.");
			return;
		}

		if (transform.hasChanged == true)
		{
			if ((maskMappingWorldAxis == MappingAxis.X) && ((Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.x, 0)) > 0.01f) || (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, invertAxis ? -90 : 90)) > 0.01f)))
			{
				Debug.Log("You cannot edit X and Y values of the Mask transform rotation!");
				transform.eulerAngles = new Vector3(0, invertAxis ? 270 : 90, transform.eulerAngles.z);
			}
			else if ((maskMappingWorldAxis == MappingAxis.Y) && ((Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.x, invertAxis ? -90 : 90)) > 0.01f) || (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 0)) > 0.01f)))
			{
				Debug.Log("You cannot edit X and Z values of the Mask transform rotation!");
				transform.eulerAngles = new Vector3(invertAxis ? -90 : 90, transform.eulerAngles.y, 0);
			}
			else if ((maskMappingWorldAxis == MappingAxis.Z) && ((Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.x, 0)) > 0.01f) || (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, (invertAxis ? -180 : 0))) > 0.01f)))
			{
				Debug.Log("You cannot edit X and Y values of the Mask transform rotation!");
				transform.eulerAngles = new Vector3(0, invertAxis ? -180 : 0, transform.eulerAngles.z);
			}

			if (transform.parent != null)
			{
				Renderer[] renderers = transform.parent.gameObject.GetComponentsInChildren<Renderer>();
				List<Material> differentMaterials = new List<Material>();

				foreach (Renderer renderer in renderers)
				{
					if (renderer.gameObject != gameObject)
					{
						foreach (Material material in renderer.sharedMaterials)
						{
							if (!differentMaterials.Contains(material))
							{
								differentMaterials.Add(material);

								if ((material.shader.ToString() == _maskedSpriteWorldCoordsShader.ToString()) &&
								    (material.shader.GetInstanceID() != _maskedSpriteWorldCoordsShader.GetInstanceID()))
								{
									Debug.Log("There seems to be more than one masked shader in the project with the same display name, and it's preventing the mask from being properly applied.");
									_maskedSpriteWorldCoordsShader = null;
								}
								if ((material.shader.ToString() == _maskedUnlitWorldCoordsShader.ToString()) &&
								    (material.shader.GetInstanceID() != _maskedUnlitWorldCoordsShader.GetInstanceID()))
								{
									Debug.Log("There seems to be more than one masked shader in the project with the same display name, and it's preventing the mask from being properly applied.");
									_maskedUnlitWorldCoordsShader = null;
								}


								if ((material.shader == _maskedSpriteWorldCoordsShader) ||
								    (material.shader == _maskedUnlitWorldCoordsShader))
								{
									Vector2 scale = new Vector2(1f / transform.lossyScale.x, 1f / transform.lossyScale.y);
									Vector2 offset = Vector2.zero;
									float rotationAngle = 0;
									int sign = 1;
									if (maskMappingWorldAxis == MappingAxis.X)
									{
										sign = (invertAxis ? 1 : -1);
										offset = new Vector2(-transform.position.z, -transform.position.y);
										rotationAngle = sign * transform.eulerAngles.z;
									}
									else if (maskMappingWorldAxis == MappingAxis.Y)
									{
										offset = new Vector2(-transform.position.x, -transform.position.z);
										rotationAngle = -transform.eulerAngles.y;
									}
									else if (maskMappingWorldAxis == MappingAxis.Z)
									{
										sign = (invertAxis ? -1 : 1);
										offset = new Vector2(-transform.position.x, -transform.position.y);
										rotationAngle = sign * transform.eulerAngles.z;
									}

									Vector2 scaleTexture = gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureScale;
									scale.x *= scaleTexture.x;
									scale.y *= scaleTexture.y;
									
									scale.x *= sign;

									Vector2 offsetTemporary = offset;
									float s = Mathf.Sin(-rotationAngle * Mathf.Deg2Rad);
									float c = Mathf.Cos(-rotationAngle * Mathf.Deg2Rad);

									offset.x = (c * offsetTemporary.x - s * offsetTemporary.y) * scale.x + 0.5f * scaleTexture.x;
									offset.y = (s * offsetTemporary.x + c * offsetTemporary.y) * scale.y + 0.5f * scaleTexture.y;

									offset += gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureOffset;

									material.SetTextureOffset ("_AlphaTex", offset);
									material.SetTextureScale ("_AlphaTex", scale);
									material.SetFloat ("_MaskRotation", rotationAngle * Mathf.Deg2Rad);

								}
							}
						}
					}
				}
			}
		}

	}

	private void SetMaskMappingAxisInMaterials (MappingAxis mappingAxis)
	{
		if (transform.parent == null)
		{
			return;
		}

		Renderer[] renderers = transform.parent.gameObject.GetComponentsInChildren<Renderer>();
		List<Material> differentMaterials = new List<Material>();
		
		foreach (Renderer renderer in renderers)
		{
			if (renderer.gameObject != gameObject)
			{
				foreach (Material material in renderer.sharedMaterials)
				{
					if (!differentMaterials.Contains(material))
					{
						differentMaterials.Add(material);

						SetMaskMappingAxisInMaterial(mappingAxis, material);
					}
				}
			}
		}
	}
	
	public void SetMaskMappingAxisInMaterial (MappingAxis mappingAxis, Material material)
	{
		if ((material.shader == _maskedSpriteWorldCoordsShader) ||
		    (material.shader == _maskedUnlitWorldCoordsShader))
		{
			switch (mappingAxis)
			{
			case MappingAxis.X:
				material.SetFloat("_Axis", 0);
				material.EnableKeyword("_AXIS_X");
				material.DisableKeyword("_AXIS_Y");
				material.DisableKeyword("_AXIS_Z");
				break;
			case MappingAxis.Y:
				material.SetFloat("_Axis", 1);
				material.DisableKeyword("_AXIS_X");
				material.EnableKeyword("_AXIS_Y");
				material.DisableKeyword("_AXIS_Z");
				break;
			case MappingAxis.Z:
				material.SetFloat("_Axis", 2);
				material.DisableKeyword("_AXIS_X");
				material.DisableKeyword("_AXIS_Y");
				material.EnableKeyword("_AXIS_Z");
				break;
			}
		}
	}
	
	private void SetMaskFloatValueInMaterials (string variable, float value)
	{
		if (transform.parent == null)
		{
			return;
		}

		Renderer[] renderers = transform.parent.gameObject.GetComponentsInChildren<Renderer>();
		List<Material> differentMaterials = new List<Material>();
		
		foreach (Renderer renderer in renderers)
		{
			if (renderer.gameObject != gameObject)
			{
				foreach (Material material in renderer.sharedMaterials)
				{
					if (!differentMaterials.Contains(material))
					{
						differentMaterials.Add(material);
						
						material.SetFloat(variable, value);
					}
				}
			}
		}
	}
	
	private void SetMaskBoolValueInMaterials (string variable, bool value)
	{
		if (transform.parent == null)
		{
			return;
		}

		Renderer[] renderers = transform.parent.gameObject.GetComponentsInChildren<Renderer>();
		List<Material> differentMaterials = new List<Material>();
		
		foreach (Renderer renderer in renderers)
		{
			if (renderer.gameObject != gameObject)
			{
				foreach (Material material in renderer.sharedMaterials)
				{
					if (!differentMaterials.Contains(material))
					{
						differentMaterials.Add(material);
						
						SetMaskBoolValueInMaterial(variable, value, material);
					}
				}
			}
		}
	}
	
	public void SetMaskBoolValueInMaterial (string variable, bool value, Material material)
	{
		if ((material.shader == _maskedSpriteWorldCoordsShader) ||
		    (material.shader == _maskedUnlitWorldCoordsShader))
		{
			material.SetFloat(variable, (value ? 1 : 0));
			if (value == true)
			{
				material.EnableKeyword(variable.ToUpper() + "_ON");
			}
			else
			{
				material.DisableKeyword(variable.ToUpper() + "_ON");
			}
		}
	}
		
	private void CreateAndAssignQuad (Mesh mesh)
	{
		// assign vertices
		Vector3[] vertices = new Vector3[4];
		
		vertices[0] = new Vector3(-0.5f, -0.5f, 0);
		vertices[1] = new Vector3(0.5f, -0.5f, 0);
		vertices[2] = new Vector3(-0.5f, 0.5f, 0);
		vertices[3] = new Vector3(0.5f, 0.5f, 0);
		
		mesh.vertices = vertices;

		// assign triangles
		int[] tri = new int[6];
		
		//  Lower left triangle.
		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;
		
		//  Upper right triangle.   
		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;
		
		mesh.triangles = tri;

		// assign normals
		Vector3[] normals = new Vector3[4];
		
		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;
		
		mesh.normals = normals;

		// assign UVs
		Vector2[] uv = new Vector2[4];
		
		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);
		
		mesh.uv = uv;
	}


	public void SetMaskRendererActive (bool value)
	{
		if (GetComponent<Renderer>() != null)
		{
			if (value == true)
			{
				GetComponent<Renderer>().enabled = true;
			}
			else
			{
				GetComponent<Renderer>().enabled = false;
			}
		}
	}

	private void ChangeMappingAxis (MappingAxis currMaskMappingWorldAxis, MappingAxis prevMaskMappingWorldAxis, bool currInvertAxis)
	{
		if (currMaskMappingWorldAxis == MappingAxis.X)
		{
			if (prevMaskMappingWorldAxis == MappingAxis.Y)
			{
				transform.eulerAngles = new Vector3(0, currInvertAxis ? -90 : 90, transform.eulerAngles.y);
			}
			else
			{
				transform.eulerAngles = new Vector3(0, currInvertAxis ? -90 : 90, transform.eulerAngles.z);
			}
		}
		else if (currMaskMappingWorldAxis == MappingAxis.Y)
		{
			if (prevMaskMappingWorldAxis == MappingAxis.Y)
			{
				transform.eulerAngles = new Vector3(currInvertAxis ? -90 : 90, transform.eulerAngles.y, 0);
			}
			else
			{
				transform.eulerAngles = new Vector3(currInvertAxis ? -90 : 90, transform.eulerAngles.z, 0);
			}
		}
		else if (currMaskMappingWorldAxis == MappingAxis.Z)
		{
			if (prevMaskMappingWorldAxis == MappingAxis.Y)
			{
				transform.eulerAngles = new Vector3(0, currInvertAxis ? -180 : 0, transform.eulerAngles.y);
			}
			else
			{
				transform.eulerAngles = new Vector3(0, currInvertAxis ? -180 : 0, transform.eulerAngles.z);
			}
		}
		
		SetMaskMappingAxisInMaterials(currMaskMappingWorldAxis);
	}
}
