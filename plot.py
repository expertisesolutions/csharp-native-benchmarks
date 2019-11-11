#!/usr/bin/env python
# coding: utf-8

import sys

import pandas as pd
import seaborn as sns

import matplotlib.pyplot as plt

def main():

    filenames = sys.argv[1:]
    

    df = pd.concat(pd.read_csv(filename) for filename in filenames)

    names = df['name'].unique()

    for name in names:
        plt.clf()
        custom = df[df["name"]==name]
        custom = custom[custom["size"] >= 20]
        plt.title(name)
        chart = sns.pointplot(data=custom, x="size", y="us", hue="domain")
        chart.set_xticklabels(chart.get_xticklabels(), rotation=45)
        plt.savefig(f"{name}.png")



if __name__ == '__main__':
    main()
