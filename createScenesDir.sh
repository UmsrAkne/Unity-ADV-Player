#!/bin/bash

# 現在の日時を yyyyMMdd_hhmm 形式で取得
current_date=$(date +"%Y%m%d_%H%M")

# スクリプトファイルが存在するディレクトリを取得
script_dir=$(dirname "$0")

# ディレクトリ名を現在の日時に置き換える
dir_name="scenes/${current_date}"

# ディレクトリ構造を作成
mkdir -p "$script_dir/$dir_name/bgvs"
mkdir -p "$script_dir/$dir_name/images"
mkdir -p "$script_dir/$dir_name/masks"
mkdir -p "$script_dir/$dir_name/texts"
mkdir -p "$script_dir/$dir_name/voices"

# texts ディレクトリ内に空の XML ファイルを作成
touch "$script_dir/$dir_name/texts/scenario.xml"
touch "$script_dir/$dir_name/texts/setting.xml"

echo "ディレクトリ構造と空のファイルが作成されました。"

