clear ; 
close all; 
clc

input_layer_size = 400
num_labels = %0%; % количество строк в алфавите

fprintf('loading and visualization data ...\n');

data=load('%1%'); % файл с данными
X = data(:, 1:end-1);
y = data(:, end);
m = size(X, 1);

rand_indices = randperm(m);
sel = X(rand_indices(1: 24), :);
displayData(sel);

fprintf('\ntraining one-vs-all logistic regression ... \n');

lambda = 0.5;
[all_theta] = oneVsAll(X, y, num_labels, lambda);

pred = predictOneVsAll(all_theta, X);
fprintf('\ntraining set accuracy: %f\n', mean(double(pred == y)) * 100);
fprintf('exporting model...\n');
save -ascii %2% all_theta; % файл обучения