import re

scene_path = r"d:\GitHubRepos\F1-Balkan-Edition\Assets\Scenes\GameScene.unity"

with open(scene_path, 'r', encoding='utf-8', errors='ignore') as f:
    content = f.read()

# Let's find GameObject blocks and their corresponding names and fileIDs
game_objects = {}
# A GameObject is defined by --- !u!1 &<id> followed by m_Name: <name>
go_matches = re.finditer(r'--- !u!1 &(\d+)\nGameObject:.*?m_Name: (.*?)\n', content, re.DOTALL)
for match in go_matches:
    go_id = match.group(1)
    go_name = match.group(2)
    game_objects[go_id] = go_name

# Let's print the names of the children of MobileUI:
# The MobileUI transform had Father/children. 
# Let's find the component references or transform definitions.
# Let's search for "--- !u!224 &" (RectTransform) or "--- !u!4 &" (Transform) for the children.
children_ids = ["2929252285001833960", "956173343", "1383830440"]
for child_id in children_ids:
    print(f"Child {child_id}:")
    # Let's look for this ID as the &anchor in any block
    block_pattern = r'--- !u!\d+ &' + child_id + r'\n(.*?)(?=\n--- !u!)'
    block_match = re.search(block_pattern, content, re.DOTALL)
    if block_match:
        block_text = block_match.group(1)
        print("  Block found!")
        # Find the m_GameObject field
        go_ref = re.search(r'm_GameObject: {fileID: (\d+)}', block_text)
        if go_ref:
            go_id = go_ref.group(1)
            print(f"  GameObject ID: {go_id}, Name: {game_objects.get(go_id, 'Unknown')}")
        else:
            print("  No m_GameObject ref")
    else:
        # Maybe it's a PrefabInstance modification? Let's check
        print("  Block not found directly in scene (might be prefab child)")
